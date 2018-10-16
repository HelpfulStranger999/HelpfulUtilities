using Discord.Commands;

namespace HelpfulUtilities.Discord.Commands.Results
{
    internal class CommandSearchResult
    {
        public CommandMatch BestMatch { get; private set; }
        public IResult Error { get; private set; }
        public bool IsSuccess => Error == null;

        private CommandSearchResult(CommandMatch match, IResult result = null)
        {
            BestMatch = match;
            Error = result;
        }

        public static CommandSearchResult FromSuccess(CommandMatch match)
            => new CommandSearchResult(match, null);
        public static CommandSearchResult FromError(CommandMatch? bestMatch, IResult result)
            => new CommandSearchResult(bestMatch.GetValueOrDefault(default), result);
    }
}
