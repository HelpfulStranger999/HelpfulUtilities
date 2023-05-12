using Discord;
using Discord.Commands;
using HelpfulUtilities.Discord.Commands.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace HelpfulUtilities.Discord.Commands.Extensions
{
    public static partial class Extensions
    {
        private static readonly Func<EmbedBuilder, EmbedBuilder> DefaultFormatter = builder => builder;

        /// <summary>Returns the attribute of the type on this module.</summary>
        /// <typeparam name="TAttribute">Type of attribute</typeparam>
        /// <returns>First instance of the attribute or null</returns>
        public static TAttribute GetAttribute<TAttribute>(this ModuleInfo module)
            where TAttribute : Attribute
        {
            return module.GetAttributes<TAttribute>().FirstOrDefault();
        }

        /// <summary>Returns all attributes of the type on this module.</summary>
        /// <typeparam name="TAttribute">Type of attribute</typeparam>
        /// <returns>All instances of the attribute</returns>
        public static IEnumerable<TAttribute> GetAttributes<TAttribute>(this ModuleInfo module)
            where TAttribute : Attribute
        {
            return module.Attributes.Where(attribute => attribute is TAttribute)
                .Select(attribute => attribute as TAttribute);
        }

        /// <summary>Determines whether this module is hidden as determined by <see cref="HiddenAttribute"/></summary>
        /// <returns>Whether this module is hidden</returns>
        public static bool IsHidden(this ModuleInfo module)
        {
            return module.GetAttribute<HiddenAttribute>() != null;
        }

        /// <summary>Returns the name of this module</summary>
        /// <returns>The name of this module</returns>
        public static string GetName(this ModuleInfo module)
        {
            return module.GetAttribute<NameAttribute>()?.Text ??
                CultureInfo.CurrentCulture.TextInfo.ToTitleCase(module.Name);
        }

        /// <summary>Returns the summary of this module or an error <see cref="string"/></summary>
        /// <returns>The summary of this module</returns>
        public static string GetSummary(this ModuleInfo module)
        {
            return module.Summary ?? "No summary found!";
        }

        /// <summary>Returns the remarks of this module or an error <see cref="string"/></summary>
        /// <returns>The remarks of this module</returns>
        public static string GetRemarks(this ModuleInfo module)
        {
            return module.Remarks ?? "No remarks found!";
        }

        /// <summary>Returns a collection of embeds displaying the default help for these modules formatted as directed by <paramref name="formatter"/></summary>
        /// <returns>A collection of default help embeds.</returns>
        public static IList<EmbedBuilder> GetHelp(this IEnumerable<ModuleInfo> modules, string prefix = "", Func<EmbedBuilder, EmbedBuilder> formatter = null)
        {
            formatter ??= DefaultFormatter;
            var list = new List<EmbedBuilder>();
            var embed = formatter(new EmbedBuilder());

            foreach (var module in modules)
            {
                if (module.IsHidden()) { continue; }
                module.AppendHelp(embed, out var total, prefix, formatter);
                if (total.Length >= 1)
                {
                    for (int i = 0; i < total.Length - 1; i++)
                        list.Add(total[i]);
                    embed = formatter(total[^1]);
                }
            }

            list.Add(embed);
            return list;
        }

        /// <summary>Returns a new embed builder with a default command help with an optional prefix
        /// and a function for formatting the embed.</summary>
        /// <returns>The new embed with the provided help for the command</returns>
        public static EmbedBuilder[] GetHelp(this ModuleInfo module, string prefix = "", Func<EmbedBuilder, EmbedBuilder> formatter = null)
        {
            AppendHelp(module, new EmbedBuilder(), out var total, prefix, formatter);
            return total;
        }

        /// <summary>Append to the embed a default command help with an optional prefix
        /// and a function for formatting the embed.</summary>
        /// <returns>The embed modified</returns>
        public static void AppendHelp(this ModuleInfo module, EmbedBuilder embed, out EmbedBuilder[] total, string prefix = "", Func<EmbedBuilder, EmbedBuilder> formatter = null)
        {
            if (module.Commands.Count <= 0) throw new InvalidOperationException($"No commands could be found in {module.GetName()}");
            formatter ??= DefaultFormatter;
            var embeds = new List<EmbedBuilder>();

            var list = new StringBuilder();
            var split = false;
            foreach (var command in module.Commands)
            {
                if (command.IsHidden()) { continue; }
                list.Append(command.GetBriefHelp(prefix));

                // If the list is approaching the field limit or the embed
                // approaching the embed limit after the list is appended
                if (list.Length > 800 || (embed.Length + list.Length) >= 5800)
                {
                    embed.AddField($"{module.GetName()} Commands" + (split ? " (continued)" : ""), list.Length > 0 ? list.ToString() : "No Commands");
                    list.Clear();
                    split = true;
                }

                if (embed.Length >= 5800)
                {
                    embeds.Add(formatter(embed));

                    embed = new EmbedBuilder();
                }
            }

            embed.AddField($"{module.GetName()} Commands" + (split ? " (continued)" : ""), list.Length > 0 ? list.ToString() : "No Commands");
            embeds.Add(formatter(embed));

            total = embeds.ToArray();
        }
    }
}
