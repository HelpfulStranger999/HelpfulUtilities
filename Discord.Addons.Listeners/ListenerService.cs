using Discord;
using Discord.Commands;
using HelpfulUtilities.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HelpfulUtilities.Discord.Listeners
{
    /// <summary>Offers a service that streamlines non-command message handling to complement <see cref="CommandService"/></summary>
    public class ListenerService
    {
        private const string DefaultSource = "Listeners";
        internal static Type IModuleBase = Assembly.GetAssembly(typeof(CommandService)).GetType("Discord.Commands.IModuleBase", true);

        internal static Func<Type, bool> TypeFilter = ((type) =>
        {
            return (type.IsClass && !type.IsAbstract)
                && type.Extends(IModuleBase);
        });

        /// <summary>An immutable collection of listeners</summary>
        public IReadOnlyCollection<ListenerInfo> Listeners => _listeners.ToImmutableList();
        private IList<ListenerInfo> _listeners = new List<ListenerInfo>();

        /// <summary>Subscribe to this event and receive logs.</summary>
        public event Func<LogMessage, Task> Log;

        internal RunMode _runMode = RunMode.Sync;
        internal LogSeverity _logLevel = LogSeverity.Info;
        internal Func<IUserMessage, ICommandContext> _context;
        internal Func<ICommandContext, IServiceProvider> _services;

        /// <summary>Instantiates a <see cref="ListenerService"/> with an optional config</summary>
        public ListenerService(ListenerServiceConfig config = null)
        {
            config = config ?? new ListenerServiceConfig();

            _runMode = config.RunMode;
            _logLevel = config.LogLevel;

            _context = config.ContextFactory;
            _services = config.ServiceProvider == null ? config.ServiceProviderFactory : _ => config.ServiceProvider;

            VerboseAsync($"{nameof(ListenerService)} initialized").Await(false);
        }

        /// <summary>Adds listeners found in specified types.</summary>
        /// <returns>An enumerable of created listeners.</returns>
        public Task<IReadOnlyCollection<ListenerInfo>> AddModulesAsync(params Type[] types) => AddModulesAsync((IEnumerable<Type>)types);

        /// <summary>Adds listeners found in specified types.</summary>
        /// <returns>An enumerable of created listeners.</returns>
        public async Task<IReadOnlyCollection<ListenerInfo>> AddModulesAsync(IEnumerable<Type> types)
        {
            var builder = ImmutableArray.CreateBuilder<ListenerInfo>();

            foreach (var type in types)
            {
                builder.AddRange(await AddModuleAsync(type).ConfigureAwait(false));
            }

            return builder.ToImmutable();
        }

        /// <summary>Adds listeners found in optional specified assembly</summary>
        /// <returns>An enumerable of created listeners.</returns>
        public async Task<IReadOnlyCollection<ListenerInfo>> AddModulesAsync(Assembly assembly = null)
        {
            assembly = assembly ?? Assembly.GetCallingAssembly();
            await VerboseAsync($"Searching for listeners in {assembly.FullName} assembly.").ConfigureAwait(false);
            return await AddModulesAsync(assembly.GetTypes().Where(TypeFilter)).ConfigureAwait(false);
        }

        /// <summary>Adds listeners found in <typeparamref name="T"/></summary>
        /// <returns>An enumerable of created listeners.</returns>
        public Task<IReadOnlyCollection<ListenerInfo>> AddModuleAsync<T>() => AddModuleAsync(typeof(T));

        /// <summary>Adds listeners found in specified type.</summary>
        /// <returns>An enumerable of created listeners.</returns>
        public async Task<IReadOnlyCollection<ListenerInfo>> AddModuleAsync(Type type)
        {
            var builder = ImmutableArray.CreateBuilder<ListenerInfo>();

            await DebugAsync($"Searching for listeners in {type.Name}").ConfigureAwait(false);

            foreach (var method in type.GetMethods().Where(x => x.GetCustomAttribute<ListenerAttribute>() != null))
            {
                builder.Add(new ListenerInfo(this, method));
            }

            await DebugAsync($"Found {builder.Count} listeners").ConfigureAwait(false);
            await VerboseAsync($"Preparing to add {builder.Count} listeners").ConfigureAwait(false);

            _listeners.AddRange(builder);
            return builder.ToImmutable();
        }


        /// <summary>Removes specified listeners.</summary>
        /// <returns>Whether remove operation was successful</returns>
        public bool RemoveModules(params ListenerInfo[] listeners) => RemoveModules(listeners);

        /// <summary>Removes specified listeners.</summary>
        /// <returns>Whether remove operation was successful</returns>
        public async Task<bool> RemoveModulesAsync(IEnumerable<ListenerInfo> listeners)
        {
            await VerboseAsync($"Preparing to remove {listeners.Count()} listeners").ConfigureAwait(false);
            foreach (var listener in listeners)
            {
                if (!await RemoveModuleAsync(listener).ConfigureAwait(false))
                    return false;
            }
            await DebugAsync($"Removed {listeners.Count()} listeners").ConfigureAwait(false);
            return true;
        }

        /// <summary>Removes specified listener.</summary>
        /// <returns>Whether remove operation was successful</returns>
        public async Task<bool> RemoveModuleAsync(ListenerInfo listener)
        {
            await VerboseAsync($"Preparing to remove {listener.Name}").ConfigureAwait(false);
            var result = _listeners.Remove(listener);
            if (result)
                await DebugAsync($"Removed listener {listener.Name}").ConfigureAwait(false);
            else
                await WarnAsync($"Failed to remove listener {listener.Name}").ConfigureAwait(false);
            return result;
        }

        /// <summary>Executes the listeners with the provided context and service provider.</summary>
        /// <param name="context">The context of the message</param>
        /// <param name="services">The service provider to inject dependencies from.</param>
        /// <returns>An enumerable of the results of the listeners.</returns>
        public async Task<IEnumerable<IResult>> ExecuteAsync<TCommandContext>(TCommandContext context, IServiceProvider services)
            where TCommandContext : class, ICommandContext

        {
            await VerboseAsync("Preparing to execute").ConfigureAwait(false);
            return await Task.WhenAll(_listeners.Select(async x =>
            {
                var result = x.CheckPreconditions(context);
                if (!result.IsSuccess)
                {
                    await DebugAsync($"Preconditions on listener {x.Name} failed").ConfigureAwait(false);
                    return result;
                }

                result = x.Execute(context, services);
                if (result.IsSuccess)
                    await VerboseAsync($"Executed {x.Name}").ConfigureAwait(false);
                else
                    await ErrorAsync($"{x.Name} failed to execute: {result.ErrorReason}", Enum.GetName(typeof(CommandError), result.Error)).ConfigureAwait(false);

                return result;
            })).ConfigureAwait(false);
        }

        /// <summary>Offers a default message handler you can subscribe to the message received handler.</summary>
        /// <remarks>Remember to set <see cref="ListenerServiceConfig.ContextFactory"/> and either 
        /// <see cref="ListenerServiceConfig.ServiceProvider"/> or <see cref="ListenerServiceConfig.ServiceProviderFactory"/></remarks>
        public async Task ListenerAsync(IMessage msg)
        {
            if (msg is IUserMessage message)
            {
                await VerboseAsync("Constructing context").ConfigureAwait(false);
                var context = _context(message);
                await VerboseAsync("Constructing service provider").ConfigureAwait(false);
                var services = _services(context);

                await DebugAsync("Executing message").ConfigureAwait(false);
                await ExecuteAsync(context, services).ConfigureAwait(false);
            }
        }

        private async Task LogInternalAsync(LogSeverity severity, string message, string source, Exception exception)
        {
            if (severity <= _logLevel)
                await Log(new LogMessage(severity, source, message, exception)).ConfigureAwait(false);
        }

        internal Task CriticalAsync(string message, string source = DefaultSource, Exception exception = null)
            => LogInternalAsync(LogSeverity.Critical, message, source, exception);

        internal Task ErrorAsync(string message, string source = DefaultSource, Exception exception = null)
            => LogInternalAsync(LogSeverity.Error, message, source, exception);

        internal Task WarnAsync(string message, string source = DefaultSource, Exception exception = null)
            => LogInternalAsync(LogSeverity.Warning, message, source, exception);

        internal Task LogAsync(string message, string source = DefaultSource, Exception exception = null)
            => LogInternalAsync(LogSeverity.Info, message, source, exception);

        internal Task VerboseAsync(string message, string source = DefaultSource, Exception exception = null)
            => LogInternalAsync(LogSeverity.Verbose, message, source, exception);

        internal Task DebugAsync(string message, string source = DefaultSource, Exception exception = null)
            => LogInternalAsync(LogSeverity.Debug, message, source, exception);

    }
}
