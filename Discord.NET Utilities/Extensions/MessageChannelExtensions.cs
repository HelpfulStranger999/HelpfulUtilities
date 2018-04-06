using Discord;
using System.Threading.Tasks;

namespace HelpfulUtilities.Discord.Extensions
{
    public static partial class Extensions
    {
        /// <summary>Sends a message to this message channel and deletes it after a specified duration in milliseconds.</summary>
        public static async Task SendDeleteMessageAsync(this IMessageChannel channel, ulong millis, string text = "", bool isTTS = false, Embed embed = null, RequestOptions options = null)
        {
            var message = await channel.SendMessageAsync(text, isTTS, embed, options);
            await message.DeleteInAsync(millis, options);
        }

        /// <summary>Sends a message to this message channel.</summary>
        public static Task<IUserMessage> SendMessageAsync(this IMessageChannel channel, string text = "", bool isTTS = false, Embed embed = null, RequestOptions options = null)
            => channel.SendMessageAsync(text, isTTS, embed, options);

        /// <summary>Sends an embed to this message channel.</summary>
        public static Task<IUserMessage> SendEmbedAsync(this IMessageChannel channel, Embed embed, RequestOptions options = null)
            => channel.SendMessageAsync("", embed: embed, options: options);

        /// <summary>Sends an embed to this message channel.</summary>
        public static Task<IUserMessage> SendEmbedAsync(this IMessageChannel channel, EmbedBuilder embed, RequestOptions options = null)
            => channel.SendEmbedAsync(embed.Build(), options);
    }
}
