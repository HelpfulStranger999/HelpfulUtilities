using Discord.Commands;
using HelpfulUtilities.Discord.Commands.Extensions;
using System;
using System.Threading.Tasks;

namespace HelpfulUtilities.Discord.Commands.Preconditions
{
    /// <summary>Checks whether a bot can perform the command.</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Module | AttributeTargets.Parameter,
        AllowMultiple = false, Inherited = true)]
    public class BotsCommandAttribute : PreconditionAttribute
    {
        /// <summary>Whether bots can perform the command.</summary>
        public bool Value { get; set; } = false;

        /// <summary>Constructs the precondition with whether bots can perform the command as an optional parameter.</summary>
        public BotsCommandAttribute(bool value = false) => Value = value;

        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            if (context.User.IsBot)
            {
                if (!Value)
                {
                    return Task.FromResult(PreconditionResult.FromError(
                        $"Bots cannot perform {command.GetName()} command."));
                }
            }

            return Task.FromResult(PreconditionResult.FromSuccess());
        }
    }
}
