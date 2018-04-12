using Discord;

namespace HelpfulUtilities.Discord.Extensions
{
    public static partial class Extensions
    {
        /// <summary>Returns the url for this emoji.</summary>
        public static string GetUrl(this Emoji emoji)
            => $"https://raw.githubusercontent.com/HenrikJoreteg/emoji-images/master/pngs/{emoji.Name}.png";
    }
}
