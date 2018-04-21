using Discord;
using Discord.Rest;
using Discord.WebSocket;
using HelpfulUtilities.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace HelpfulUtilities.Discord.Extensions
{
    public static partial class Extensions
    {
        /// <summary>Deletes the message in the specified number of milliseconds.</summary>
        public static async Task DeleteInAsync(this IMessage message, ulong millis, RequestOptions options = null)
        {
            await Operations.DelayAsync(millis);
            await message.DeleteAsync(options);
        }

        /// <summary>Returns a collection of users who reacted to the message with the emote up to <paramref name="limit"/> or all</summary>
        /// <returns>A collection of users who reacted with the specified emote</returns>
        public static Task<IReadOnlyCollection<IUser>> PaginateReactionUsersAsync(this IUserMessage message, IEmote emote, int limit = 1000, RequestOptions options = null)
            => PaginateReactionUsersAsync(message, emote, limit, options);

        /// <summary>Returns a collection of users who reacted to the message with the emote up to <paramref name="limit"/> or all</summary>
        /// <returns>A collection of users who reacted with the specified emote</returns>
        public static async Task<IReadOnlyCollection<IUser>> PaginateReactionUsersAsync(this IUserMessage message, IEmote emote, int? limit = null, RequestOptions options = null)
        {
            var builder = new IUser[limit ?? message.Reactions.Count];
            ulong? lastUserID = null;

            while (builder.Any(u => u == null))
            {
                var count = builder.Count(u => u == null) > 100 ? 100 : builder.Count(u => u == null);
                var users = await message.GetReactionUsersAsync(emote, count, lastUserID);

                lastUserID = users.OrderByDescending(user => user.Id).First().Id;
                builder.AddRange(users);
            }

            return new ReadOnlyCollection<IUser>(builder);
        }

        /// <summary>Returns the guild a message was sent in or null.</summary>
        public static IGuild GetGuild(this IMessage message)
        {
            if (message.Channel is IGuildChannel channel)
                return channel.Guild;
            return null;
        }

        /// <summary>Returns the guild a message was sent in or null.</summary>
        public static SocketGuild GetGuild(this SocketMessage message)
        {
            if (message.Channel is SocketGuildChannel channel)
                return channel.Guild;
            return null;
        }

        /// <summary>Returns the guild a message was sent in or null.</summary>
        /// <remarks>If the <paramref name="client"/> is null, results may be less reliable.</remarks>
        public static async Task<RestGuild> GetGuildAsync(this RestMessage message, DiscordRestClient client = null)
        {
            if (message.Channel is RestGuildChannel channel)
                return (await client.GetGuildAsync(channel.GuildId)) ?? (RestGuild)message.GetGuild();
            return null;
        }
    }
}
