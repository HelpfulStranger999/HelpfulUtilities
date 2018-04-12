using Discord;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace HelpfulUtilities.Discord.Extensions
{
    public static partial class Extensions
    {
        /// <summary>Gets a collection of all the users in all the guilds.</summary>
        /// <returns>A collection of all users</returns>
        public static async Task<IReadOnlyCollection<IUser>> GetUsersAsync(this IDiscordClient client, CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            var builder = ImmutableList.CreateBuilder<IUser>();

            foreach (var guild in await client.GetGuildsAsync(mode, options))
            {
                if (mode == CacheMode.AllowDownload && guild is SocketGuild socketGuild)
                    builder.AddRange(await socketGuild.FetchUsersAsync(mode, options));
                else
                    builder.AddRange(await guild.GetUsersAsync(mode, options));
            }

            return builder.ToImmutable();
        }
    }
}
