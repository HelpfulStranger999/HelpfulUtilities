using Discord;
#if EMOJI
using Discord.Addons.EmojiTools;
#endif

namespace HelpfulUtilities.Discord.Extensions
{
    public static partial class Extensions
    {
#if EMOJI
        /// <summary>Returns the url for this emoji.</summary>
        public static string GetUrl(this Emoji emoji)
            => $"https://raw.githubusercontent.com/HenrikJoreteg/emoji-images/master/pngs/{emoji.GetShorthand()}.png";
#endif
    }
}
