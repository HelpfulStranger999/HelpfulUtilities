using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Discord.Addons.Listeners
{
    public class ListenerServiceConfig
    {
        private RunMode _runMode = RunMode.Sync;
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

        public LogSeverity LogLevel { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        public Func<ICommandContext, IServiceProvider> ServiceProviderFactory { get; set; }
        public Func<IUserMessage, ICommandContext> ContextFactory { get; set; }
    }
}
