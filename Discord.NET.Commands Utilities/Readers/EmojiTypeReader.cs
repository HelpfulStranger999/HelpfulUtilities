using Discord;
using Discord.Addons.EmojiTools;
using Discord.Commands;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace HelpfulUtilities.Discord.Commands.Readers
{
    /// <summary>This <see cref="TypeReader"/> parses emoji.</summary>
    public class EmojiTypeReader : TypeReader
    {
        /// <summary>Provides a default static error message</summary>
        public static TypeReaderResult Error { get; } = TypeReaderResult.FromError(CommandError.ParseFailed, "Input was not an emoji");

        /// <summary>Parses the emoji</summary>
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        {
            if (EmojiMap.Map.Values.Contains(input.Trim(':')))
                return Task.FromResult(TypeReaderResult.FromSuccess(new Emoji(input)));
            return Task.FromResult(Error);
        }
    }
}