using Discord;
using Discord.Commands;
using HelpfulUtilities.Discord.Commands.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace HelpfulUtilities.Discord.Commands.Readers
{
    /// <summary>This <see cref="TypeReader"/> parses emoji.</summary>
    public class EmojiTypeReader : TypeReader
    {
        /// <summary>A dictionary containing all emojis with the name as the key and the emoji as value</summary>
        public static IReadOnlyDictionary<string, string> EmojiSet { get; }

        static EmojiTypeReader()
        {
            var emojis = JsonConvert.DeserializeObject<EmojiLoader>(Resources.Emoji);
            EmojiSet = emojis.Generate();
        }

        /// <summary>Parses the emoji</summary>
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        {
            if (EmojiSet.Values.Contains(input.Trim()))
                return Task.FromResult(TypeReaderResult.FromSuccess(new Emoji(input)));
            return Task.FromResult(TypeReaderResult.FromError(CommandError.ParseFailed, "Input was not an emoji"));
        }
    }
    
    internal class EmojiLoader
    {
        [JsonProperty("people")]
        public EmojiSet[] People { get; set; }
        [JsonProperty("nature")]
        public EmojiSet[] Nature { get; set; }
        [JsonProperty("food")]
        public EmojiSet[] Food { get; set; }
        [JsonProperty("activity")]
        public EmojiSet[] Activity { get; set; }
        [JsonProperty("travel")]
        public EmojiSet[] Travel { get; set; }
        [JsonProperty("objects")]
        public EmojiSet[] Objects { get; set; }
        [JsonProperty("symbols")]
        public EmojiSet[] Symbols { get; set; }
        [JsonProperty("flags")]
        public EmojiSet[] Flags { get; set; }

        public IReadOnlyDictionary<string, string> Generate()
        {
            var dict = new Dictionary<string, string>();

            foreach (var set in People)
                set.Generate(ref dict);
            foreach (var set in Nature)
                set.Generate(ref dict);
            foreach (var set in Food)
                set.Generate(ref dict);
            foreach (var set in Activity)
                set.Generate(ref dict);
            foreach (var set in Travel)
                set.Generate(ref dict);
            foreach (var set in Objects)
                set.Generate(ref dict);
            foreach (var set in Symbols)
                set.Generate(ref dict);
            foreach (var set in Flags)
                set.Generate(ref dict);

            return dict.ToImmutableDictionary();
        }

        internal class EmojiSet
        {
            [JsonProperty("names")]
            public string[] Names { get; set; }

            [JsonProperty("surrogates")]
            public string Emoji { get; set; }

            [JsonProperty("hasDiversity", Required = Required.Default)]
            public bool Diversity { get; set; } = false;

            public void Generate(ref Dictionary<string, string> dict)
            {
                foreach (var name in Names)
                {
                    dict.Add(name, Emoji);
                }
            }
        }
    }
}
