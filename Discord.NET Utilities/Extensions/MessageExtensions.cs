using Discord;
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
    }
}
