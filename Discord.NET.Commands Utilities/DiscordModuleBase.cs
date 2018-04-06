using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using System;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using System.Threading.Tasks;
using HelpfulUtilities.Extensions;

namespace HelpfulUtilities.Discord.Commands
{
    /// <summary>Provides an extended module base to create your module.</summary>
    public abstract class DiscordModuleBase<TBot, TCommandContext> : ModuleBase<TCommandContext>
        where TCommandContext : class, ICommandContext
    {
        /// <summary>Your service provider.</summary>
        public IServiceProvider Services { get; set; }

        /// <summary>The instance of your main bot</summary>
        /// <remarks>Remember to add this to your <see cref="IServiceProvider"/></remarks>
        public TBot Bot { get; set; }

        /// <summary>Your instance of the command service</summary>
        /// <remarks>Remember to add this to your <see cref="IServiceProvider"/></remarks>
        public CommandService Commands { get; set; }

        /// <summary>Optional Socket Client.</summary>
        [DontInject]
        protected BaseSocketClient Client { get; }

        /// <summary>Optional Rest Client</summary>
        [DontInject]
        protected DiscordRestClient Rest { get; }

        /// <summary>Constructs a new instance of this module base.</summary>
        public DiscordModuleBase(IServiceProvider services)
        {
            BaseSocketClient client = services.GetService<DiscordSocketClient>();
            if (client == null) client = services.GetService<DiscordShardedClient>() ?? null;
            Client = client;

            Rest = services.GetService<DiscordRestClient>() ?? null;
        }

        /// <summary>Sends a message to the source channel and deletes it after a specified duration in milliseconds.</summary>
        public virtual Task ReplyDeleteAsync(ulong millis, string text = "", bool isTTS = false, Embed embed = null, RequestOptions options = null)
            => Context.Channel.SendDeleteMessageAsync(millis, text, isTTS, embed, options);

        /// <summary>Sends an embed to the source channel.</summary>
        public virtual Task<IUserMessage> ReplyAsync(Embed embed, RequestOptions options = null)
            => Context.Channel.SendMessageAsync("", embed: embed, options: options);

        /// <summary>Sends an embed to the source channel.</summary>
        public virtual Task<IUserMessage> ReplyAsync(EmbedBuilder embed, RequestOptions options = null)
            => Context.Channel.SendEmbedAsync(embed.Build(), options);
    }
}
