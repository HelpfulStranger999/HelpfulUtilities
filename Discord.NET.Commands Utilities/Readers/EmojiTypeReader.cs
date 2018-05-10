#if EMOJI
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

        /// <summary>Parses the emoji</summary>
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        {
            if (EmojiMap.Map.Values.Contains(input.Trim(':')))
                return Task.FromResult(TypeReaderResult.FromSuccess(new Emoji(input)));
            return Task.FromResult(TypeReaderResult.FromError(CommandError.ParseFailed, "Input was not an emoji"));
        }
    }
}
#endif