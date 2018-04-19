namespace HelpfulUtilities.Discord.Commands.Extensions
{
    public static partial class Extensions
    {
        /// <summary>Determines whether the beginning of this message starts with the prefix or the mention of the current user.</summary>
        /// <returns>Whether this message starts with the prefix or a mention of the current user</returns>
        public static bool HasPrefix(this IUserMessage message, string prefix, BaseDiscordClient client, ref int position)
            => HasPrefix(message, prefix, client.CurrentUser, ref position);

        /// <summary>Determines whether the beginning of this message starts with the mention of the current user.</summary>
        /// <returns>Whether this message starts with a mention of the current user</returns>
        public static bool HasPrefix(this IUserMessage message, BaseDiscordClient client, ref int position)
            => HasMentionPrefix(message, client.CurrentUser, ref position);

        /// <summary>Determines whether the beginning of this message starts with the prefix or the mention.</summary>
        /// <returns>Whether this message starts with the prefix or a mention of <paramref name="user"/></returns>
        public static bool HasPrefix(this IUserMessage message, string prefix, IUser user, ref int position)
        {
            return message.HasPrefix(prefix, ref position) || message.HasMentionPrefix(user, ref position);
        }

        /// <summary>Determines whether the beginning of this message starts with the mention of <paramref name="user"/></summary>
        /// <returns>Whether this message starts with the mention of <paramref name="user"/></returns>
        public static bool HasMentionPrefix(this IUserMessage message, IUser user, ref int position)
        {
            var msg = message.Content.TrimStart();
            if (msg.EqualsIgnoreCase($"<@{user.Id}>"))
            {
                position = $"<@{user.Id}>".Length;
                return true;
            }

            if (msg.EqualsIgnoreCase($"<@!{user.Id}>"))
            {
                position = $"<@!{user.Id}>".Length;
                return true;
            }

            return false;
        }

        /// <summary>Determines whether the beginning of this message matches the prefix.</summary>
        /// <returns>Whether this message begins with the prefix.</returns>
        public static bool HasPrefix(this IUserMessage message, string prefix, ref int position)
        {
            if (message.Content.StartsWithIgnoreCase(prefix))
            {
                position = prefix.Length;
                return true;
            }
            return false;
        }
    }
}
