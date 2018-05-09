using System;

namespace HelpfulUtilities.Discord.Commands
{
    /// <summary>Represents a <see cref="IServiceProvider"/> that is empty.</summary>
    public class EmptyServiceProvider : IServiceProvider
    {
        /// <summary>Provides a <see langword="static"/> <see cref="EmptyServiceProvider"/></summary>
        public static readonly EmptyServiceProvider Instance = new EmptyServiceProvider();
        /// <inheritdoc />
        public object GetService(Type serviceType) => null;
    }
}
