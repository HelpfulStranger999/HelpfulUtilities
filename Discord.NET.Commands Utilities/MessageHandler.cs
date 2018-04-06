using Discord;
using Discord.Commands;
using Discord.WebSocket;
using HelpfulUtilities.Discord.Commands.Extensions;
using HelpfulUtilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GlobalPreconditions = System.Collections.Generic.IEnumerable<System.Func<Discord.Commands.ICommandContext, bool>>;
using ContextCreator = System.Func<Discord.IUserMessage, Discord.Commands.ICommandContext>;
using CommandResultHandler = System.Func<Discord.IUserMessage, Discord.Commands.IResult, System.Threading.Tasks.Task>;

namespace HelpfulUtilities.Discord.Commands
{
    /// <summary>This class offers either a static function that handles messages or builder for the former.</summary>
    public class MessageHandler<TCommandContext> where TCommandContext : ICommandContext
    {
        /// <summary>The message prefix</summary>
        public string Prefix { get; set; } = null;

        /// <summary>The service provider</summary>
        public IServiceProvider Services { get; set; }

        /// <summary>The command service</summary>
        public CommandService Commands { get; set; }

        /// <summary>The listener types</summary>
        public IEnumerable<Type> Listeners { get; set; } = new List<Type>();

        /// <summary>The context creator</summary>
        public ContextCreator ContextCreator { get; set; }

        /// <summary>The user to mentions</summary>
        public IUser MentionUser { get; set; } = null;

        /// <summary>The global preconditions</summary>
        public GlobalPreconditions GlobalPreconditions { get; set; } = new List<Func<ICommandContext, bool>>();

        /// <summary>The global command preconditions</summary>
        public GlobalPreconditions GlobalCommandPreconditions { get; set; } = new List<Func<ICommandContext, bool>>();

        /// <summary>The global message preconditions</summary>
        public GlobalPreconditions GlobalMessagePreconditions { get; set; } = new List<Func<ICommandContext, bool>>();

        /// <summary>A function that handles the result of command execution.</summary>
        public CommandResultHandler CommandResultHandler { get; set; } = (user, result) => { return Task.CompletedTask; };

        /// <summary>Constructs an instance of <see cref="MessageHandler{TCommandContext}"/></summary>
        /// <remarks>Remember to set <see cref="Prefix"/>, <see cref="Services"/>, <see cref="Commands"/>, 
        /// and <see cref="ContextCreator"/> for use.</remarks>
        public MessageHandler() { }

        /// <summary>Constructs an instance of <see cref="MessageHandler{TCommandContext}"/></summary>
        public MessageHandler(string prefix, IServiceProvider services, CommandService commands, ContextCreator contextCreator,
            IEnumerable<Type> listeners = null, IUser user = null, GlobalPreconditions globalPreconditions = null,
            GlobalPreconditions globalCommandPreconditions = null, GlobalPreconditions globalMessagePreconditions = null, CommandResultHandler handler = null)
        {
            Prefix = prefix;
            Services = services;
            Commands = commands;
            ContextCreator = contextCreator;

            Listeners = listeners ?? Listeners;
            MentionUser = user;
            GlobalPreconditions = globalPreconditions ?? GlobalPreconditions;
            GlobalCommandPreconditions = globalCommandPreconditions ?? GlobalCommandPreconditions;
            GlobalMessagePreconditions = globalMessagePreconditions ?? GlobalMessagePreconditions;
            CommandResultHandler = handler ?? CommandResultHandler;
        }

        /// <summary>Sets the prefix and returns itself.</summary>
        public MessageHandler<TCommandContext> WithPrefix(string prefix)
        {
            Prefix = prefix;
            return this;
        }

        /// <summary>Sets the <see cref="IServiceProvider"/> and returns itself.</summary>
        public MessageHandler<TCommandContext> WithServiceProvider(IServiceProvider services)
        {
            Services = services;
            return this;
        }

        /// <summary>Sets the <see cref="CommandService"/> and returns itself.</summary>
        public MessageHandler<TCommandContext> WithCommandService(CommandService commands)
        {
            Commands = commands;
            return this;
        }

        /// <summary>Sets the mention user and returns itself.</summary>
        public MessageHandler<TCommandContext> WithUser(IUser user)
        {
            MentionUser = user;
            return this;
        }

        /// <summary>Sets the context creator function and returns itself.</summary>
        public MessageHandler<TCommandContext> WithContextCreator(ContextCreator creator)
        {
            ContextCreator = creator;
            return this;
        }

        /// <summary>Sets the listener types and returns itself.</summary>
        public MessageHandler<TCommandContext> WithListeners(IEnumerable<Type> listeners)
        {
            Listeners = listeners;
            return this;
        }

        /// <summary>Searches for and sets the listener types and returns itself.</summary>
        public MessageHandler<TCommandContext> SearchForListeners(Assembly assembly = null)
        {
            Listeners = GetListeners(assembly);
            return this;
        }

        /// <summary>Sets the global preconditions and returns itself.</summary>
        public MessageHandler<TCommandContext> WithGlobalPreconditions(GlobalPreconditions globalPreconditions)
        {
            GlobalPreconditions = globalPreconditions;
            return this;
        }

        /// <summary>Sets the global command preconditions and returns itself.</summary>
        public MessageHandler<TCommandContext> WithGlobalCommandPreconditions(GlobalPreconditions globalCommandPreconditions)
        {
            GlobalCommandPreconditions = globalCommandPreconditions;
            return this;
        }

        /// <summary>Sets the global message preconditions and returns itself.</summary>
        public MessageHandler<TCommandContext> WithGlobalMessagePreconditions(GlobalPreconditions globalMessagePreconditions)
        {
            GlobalMessagePreconditions = globalMessagePreconditions;
            return this;
        }

        /// <summary>Sets the Command Result Handler and returns itself.</summary>
        public MessageHandler<TCommandContext> WithHandler(CommandResultHandler handler)
            => WithCommandResultHandler(handler);

        /// <summary>Sets the Command Result Handler and returns itself.</summary>
        public MessageHandler<TCommandContext> WithCommandResultHandler(CommandResultHandler handler)
        {
            CommandResultHandler = handler;
            return this;
        }

        /// <summary>Return whether any critical components have not been set.</summary>
        public bool Check()
        {
            if (Prefix == null) { return false; }
            if (Services == null) { return false; }
            if (Commands == null) { return false; }
            if (ContextCreator == null) { return false; }

            return true;
        }

        /// <summary>Handles message using <see cref="CommandResultHandler"/></summary>
        /// <remarks>Supports being hooked onto <see cref="BaseSocketClient.MessageReceived"/></remarks>
        public async Task MessageReceivedAsync(SocketMessage message)
        {
            await CommandResultHandler(message as IUserMessage, await Handle(message));
        }

        /// <summary>Handles command and non commands implementing <see cref="IListener{TCommandContext}"/></summary>
        /// <param name="message">The message you wish to parse as commands and non-commands</param>
        /// <returns>The result of the handler.</returns>
        public Task<IResult> Handle(SocketMessage message)
        {
            if (!Check())
            {
                return Task.FromResult((IResult)ExecuteResult.FromError(
                    new NullReferenceException("A critical component of the message handler was not set!")));
            }
            return Handle(message, Prefix, Services, Commands, 
                ContextCreator, Listeners, MentionUser, GlobalPreconditions, GlobalCommandPreconditions, GlobalMessagePreconditions);
        }

        /// <summary>Returns a collection of types extending <see cref="IListener{TCommandContext}"/>. For use in <see cref="Handle(IMessage, string,
        /// IServiceProvider, CommandService, ContextCreator, IEnumerable{Type},
        /// IUser, GlobalPreconditions, GlobalPreconditions, GlobalPreconditions)"/></summary>
        /// <param name="assembly">The assembly to search in - defaults to the calling assembly.</param>
        /// <returns>A collection of types that extend <see cref="IListener{TCommandContext}"/></returns>
        public static IEnumerable<Type> GetListeners(Assembly assembly = null)
        {
            var listener = typeof(IListener<TCommandContext>);
            return from type in (assembly ?? Assembly.GetCallingAssembly()).GetTypes()
                   where type.Extends(listener) && !type.IsAbstract
                   select type;
        }

        /// <summary>Handles command and non commands implementing <see cref="IListener{TCommandContext}"/></summary>
        /// <param name="message">The message you wish to parse as commands and non-commands</param>
        /// <param name="prefix">The prefix of the bot.</param>
        /// <param name="services">The <see cref="IServiceProvider"/> to be used</param>
        /// <param name="commands">The <see cref="CommandService"/> to be used.</param>
        /// <param name="createContextFunc">Function that takes a <see cref="IUserMessage"/> and returns a command context.</param>
        /// <param name="listeners">The collection of <see cref="IListener{TCommandContext}"/> to use.</param>
        /// <param name="user">The optional user for enabling mention prefix</param>
        /// <param name="globalPreconditions">A collection of functions that take a command context and return a boolean for global preconditions.</param>
        /// <param name="globalCommandPreconditions">A collection of functions that take a command context and return a boolean for global command preconditions.</param>
        /// <param name="globalMessagePreconditions">A collection of functions that take a command context and return a boolean for global message preconditions.</param>
        /// <returns>The result of the handler.</returns>
        public static async Task<IResult> Handle(IMessage message, string prefix, IServiceProvider services,
            CommandService commands, ContextCreator createContextFunc, IEnumerable<Type> listeners,
            IUser user = null, GlobalPreconditions globalPreconditions = null, GlobalPreconditions globalCommandPreconditions = null,
            GlobalPreconditions globalMessagePreconditions = null)
        {
            if (message is IUserMessage msg)
            {
                var context = createContextFunc(msg);

                foreach (var func in globalPreconditions)
                {
                    if (!func(context))
                    {
                        return PreconditionResult.FromError("Global preconditions failed.");
                    }
                }

                var pos = 0;

                var result = user != null ? msg.HasPrefix(prefix, user, ref pos) : msg.HasPrefix(prefix, ref pos);

                if (result)
                {
                    foreach (var func in globalCommandPreconditions)
                    {
                        if (!func(context))
                        {
                            return PreconditionResult.FromError("Global command preconditions failed.");
                        }
                    }

                    return await commands.ExecuteAsync(context, pos, services);
                }
                else
                {
                    if (listeners == null) { return ExecuteResult.FromSuccess(); }

                    foreach (var func in globalMessagePreconditions)
                    {
                        if (!func(context))
                        {
                            return PreconditionResult.FromError("Global message preconditions failed.");
                        }
                    }

                    foreach (var type in listeners)
                    {
                        var listener = DependencyInjection.CreateInjected<IListener<TCommandContext>>(type, services, context);
                        IResult output = null;

                        if (listener.GetRunMode() == RunMode.Async)
                        {
                            await Task.Run(async () =>
                            {
                                try
                                {
                                    await listener.ExecuteAsync();
                                }
                                catch (Exception e)
                                {
                                    output = ExecuteResult.FromError(e);
                                }
                            });
                        }
                        else
                        {
                            try
                            {
                                await listener.ExecuteAsync();
                            }
                            catch (Exception e)
                            {
                                output = ExecuteResult.FromError(e);
                            }
                        }

                        return output;
                    }
                }
            }

            return ExecuteResult.FromSuccess();
        }
    }
}
