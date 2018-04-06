using Discord;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace HelpfulUtilities.Discord.Extensions
{
    public static partial class Extensions
    {
        /// <summary>Returns this guild's roles as ordered by position in ascending order.</summary>
        /// <returns>This guild's roles ordered ascending.</returns>
        public static IReadOnlyCollection<IRole> GetOrderedRoles(this IGuild guild)
        {
            return guild.Roles.OrderBy(role =>
            {
                return role.Position;
            }).ToImmutableArray();
        }

        /// <summary>Gets a collection of users in this guild, downloading if incomplete.</summary>
        /// <returns>A collection of users in this guild</returns>
        public static async Task<IReadOnlyCollection<IGuildUser>> FetchUsersAsync(this IGuild guild, CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            await guild.DownloadUsersAsync();
            return await guild.GetUsersAsync(mode, options);
        }
    }
}
