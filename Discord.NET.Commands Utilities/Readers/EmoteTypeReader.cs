namespace HelpfulUtilities.Discord.Commands.Readers
{
    /// <summary>This <see cref="TypeReader"/> parses <see cref="Emote"/>.</summary>
    public class EmoteTypeReader : TypeReader
    {
        /// <summary>Parses the <see cref="Emote"/></summary>
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        {
            if (Emote.TryParse(input, out var emote))
            {
                return Task.FromResult(TypeReaderResult.FromSuccess(emote));
            }

            return Task.FromResult(TypeReaderResult.FromError(CommandError.ParseFailed, "Emote not found"));
        }
    }
}
