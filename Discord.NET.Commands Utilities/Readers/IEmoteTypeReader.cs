using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace HelpfulUtilities.Discord.Commands.Readers
{
    /// <summary>This <see cref="TypeReader"/> parses <see cref="IEmote"/>.</summary>
    public class IEmoteTypeReader : TypeReader
    {
        private EmojiTypeReader Emoji = new EmojiTypeReader();

        /// <summary>Provides a default static error message</summary>
        public static TypeReaderResult Error { get; } = TypeReaderResult.FromError(CommandError.ParseFailed, "IEmote could not be parsed from input.");

        /// <summary>Parses the <see cref="IEmote"/></summary>
        public async override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        {
            var result = await Emoji.ReadAsync(context, input, services);
            if (result.IsSuccess) return result;

            if (Emote.TryParse(input, out var emote))
                return TypeReaderResult.FromSuccess(emote);

            return Error;
        }

    }
}
