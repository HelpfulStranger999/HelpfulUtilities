using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Discord.Addons.Listeners
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ListenerAttribute : Attribute
    {
        public RunMode RunMode { get; set; } = RunMode.Default;
        public ContextType? RequiredContexts { get; set; } = null;
        public ulong[] RequiredUsers { get; set; } = null;

        public ListenerAttribute(params ulong[] users) => RequiredUsers = users;

        public PreconditionResult HasRequiredContext(ICommandContext context)
        {
            if (!RequiredContexts.HasValue)
                return PreconditionResult.FromSuccess();

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

        public PreconditionResult IsFromRequiredUser(ICommandContext context)
        {
            if (RequiredUsers == null)
                return PreconditionResult.FromSuccess();

            if (RequiredUsers.Contains(context.User.Id))
                return PreconditionResult.FromSuccess();
            else
                return PreconditionResult.FromError($"Command can only be run by users with these ids: {string.Join(", ", RequiredUsers)}");
        }
    }
}
