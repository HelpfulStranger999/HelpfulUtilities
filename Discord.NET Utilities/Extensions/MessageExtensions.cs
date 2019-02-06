using Discord;
using Discord.Rest;
using Discord.WebSocket;
using HelpfulUtilities.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
