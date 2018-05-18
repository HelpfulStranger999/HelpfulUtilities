using Discord;
using System;

namespace HelpfulUtilities.Discord.Extensions
{
    public static partial class Extensions
    {
        /// <summary>Returns the url for this emoji.</summary>
        /// <remarks>Original (if simple) source at <see href="https://github.com/discord-csharp/MODiX/blob/master/Modix/Modules/FunModule.cs#L29-L32">this GitHub project.</see></remarks>
        public static string GetUrl(this Emoji emoji)
        {
            var codepoint = Char.ConvertToUtf32(emoji.Name, 0).ToString("X").ToLower();
            return $"https://raw.githubusercontent.com/twitter/twemoji/gh-pages/2/72x72/{codepoint}.png"; ;
        }
    }
}
