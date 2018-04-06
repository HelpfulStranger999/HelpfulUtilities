using Discord;
using System.Linq;

namespace HelpfulUtilities.Discord.Extensions
{
    public static partial class Extensions
    {
        /// <summary>Return the effective name of this user.</summary>
        /// <returns>The nickname of this user or, if null, the username</returns>
        public static string GetEffectiveName(this IGuildUser user)
        {
            return user.Nickname ?? user.Username;
        }

        /// <summary>Returns the highest role this user has.</summary>
        /// <returns>The highest role this user has.</returns>
        public static IRole GetTopRole(this IGuildUser user)
        {
            return user.Guild.GetOrderedRoles().First(role =>
            {
                return user.RoleIds.Contains(role.Id);
            });
        }

        /// <summary>Returns the color their username appears in while chatting.</summary>
        /// <returns>The <see cref="Color"/> of this user's name in the guild.</returns>
        public static Color GetEffectiveRoleColor(this IGuildUser user)
        {
            return user.Guild.GetOrderedRoles().FirstOrDefault(role =>
            {
                return user.RoleIds.Contains(role.Id) && role.Color.RawValue != Color.Default.RawValue;
            }).Color;
        }

    }
}
