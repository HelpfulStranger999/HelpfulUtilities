using Discord.Commands;
using System.Threading.Tasks;

namespace HelpfulUtilities.Discord.Commands
{
    /// <summary>A Listener interface to implement to handle non messages.</summary>
    public interface IListener<TCommandContext>
        where TCommandContext : ICommandContext
    {
        /// <summary>The context for this message.</summary>
        /// <remarks>Warning: implementation must be able to adapt to non-command context</remarks>
        TCommandContext Context { get; }

        /// <summary>Returns the <see cref="RunMode"/> to use for execution.</summary>
        RunMode GetRunMode();

        /// <summary>Handle non-commands.</summary>
        Task ExecuteAsync();
    }
}
