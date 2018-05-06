using Discord;
using Discord.Commands;
using System;
using System.Linq;

namespace HelpfulUtilities.Discord.Listeners
{
    /// <summary>Marks a method as a listener, enabling <see cref="ListenerService"/> to find it</summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ListenerAttribute : Attribute
    {
        /// <summary>Sets the RunMode of this listener, defaults to <see cref="ListenerServiceConfig.RunMode"/></summary>
        public RunMode RunMode { get; set; } = RunMode.Default;

        /// <summary>The required <see cref="ContextType"/>, optionally ORd together</summary>
        public ContextType RequiredContexts { get; set; } = ContextType.Guild | ContextType.DM | ContextType.Group;

        /// <summary>The required users</summary>
        public ulong[] RequiredUsers { get; set; } = null;

        /// <summary>Instantiates the attribute with a list of users</summary>
        /// <param name="users">Optional array of users</param>
        public ListenerAttribute(params ulong[] users) => RequiredUsers = users;

        internal PreconditionResult HasRequiredContext(ICommandContext context)
        {
            bool isValid = false;

            if ((RequiredContexts & ContextType.Guild) != 0)
                isValid = isValid || context.Channel is IGuildChannel;
            if ((RequiredContexts & ContextType.DM) != 0)
                isValid = isValid || context.Channel is IDMChannel;
            if ((RequiredContexts & ContextType.Group) != 0)
                isValid = isValid || context.Channel is IGroupChannel;

            if (isValid)
                return PreconditionResult.FromSuccess();
            else
                return PreconditionResult.FromError($"Invalid context for command; accepted contexts: {RequiredContexts}");
        }

        internal PreconditionResult IsFromRequiredUser(ICommandContext context)
        {
            if (RequiredUsers == null || RequiredUsers.LongLength <= 0)
                return PreconditionResult.FromSuccess();

            if (RequiredUsers.Contains(context.User.Id))
                return PreconditionResult.FromSuccess();
            else
                return PreconditionResult.FromError($"Command can only be run by users with these ids: {string.Join(", ", RequiredUsers)}");
        }
    }
}
