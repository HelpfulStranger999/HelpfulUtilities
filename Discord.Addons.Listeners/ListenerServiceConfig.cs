using Discord.Commands;
using System;

namespace HelpfulUtilities.Discord.Listeners
{
    /// <summary>Represents configuration settings for <see cref="ListenerService"/></summary>
    public class ListenerServiceConfig
    {
        private RunMode _runMode = RunMode.Sync;

        /// <summary>Sets the default runmode for listeners</summary>
        /// <exception cref="InvalidOperationException">Throws if set to <see cref="RunMode.Default"/></exception>
        public RunMode RunMode
        {
            get => _runMode;
            set
            {
                if (value == RunMode.Default)
                    throw new InvalidOperationException($"Cannot set default runmode to {nameof(RunMode.Default)}");
                _runMode = value;
            }
        }
        
        /// <summary>Sets the severity of logs for <see cref="ListenerService.Log"/></summary>
        public LogSeverity LogLevel { get; set; }

        /// <summary>Sets the service provider for use in <see cref="ListenerService.ListenerAsync(IMessage)"/></summary>
        public IServiceProvider ServiceProvider { get; set; }

        /// <summary>Sets the service provider factory for use in <see cref="ListenerService.ListenerAsync(IMessage)"/></summary>
        public Func<ICommandContext, IServiceProvider> ServiceProviderFactory { get; set; }

        /// <summary>Sets the context factor for use in <see cref="ListenerService.ListenerAsync(IMessage)"/></summary>
        public Func<IUserMessage, ICommandContext> ContextFactory { get; set; }
    }
}
