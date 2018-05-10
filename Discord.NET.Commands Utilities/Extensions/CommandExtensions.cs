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
        /// <summary>Returns the attribute of the type on this command.</summary>
        /// <typeparam name="TAttribute">Type of attribute</typeparam>
        /// <returns>First instance of the attribute or null</returns>
        public static TAttribute GetAttribute<TAttribute>(this CommandInfo command)
            where TAttribute : Attribute
        {
            return command.GetAttributes<TAttribute>().FirstOrDefault();
        }

        /// <summary>Returns all attributes of the type on this command.</summary>
        /// <typeparam name="TAttribute">Type of attribute</typeparam>
        /// <returns>All instances of the attribute</returns>
        public static IEnumerable<TAttribute> GetAttributes<TAttribute>(this CommandInfo command)
            where TAttribute : Attribute
        {
            return command.Attributes.Where(attribute => attribute is TAttribute)
                .Select(attribute => attribute as TAttribute);
        }

        /// <summary>Determines whether this module is hidden as determined by <see cref="HiddenAttribute"/></summary>
        /// <returns>Whether this command is hidden</returns>
        public static bool IsHidden(this CommandInfo command)
        {
            return command.Module.IsHidden() || command.GetAttribute<HiddenAttribute>() != null;
        }

        /// <summary>Returns the name of this command</summary>
        /// <returns>The name of this command</returns>
        public static string GetName(this CommandInfo command)
        {
            return command.GetAttribute<NameAttribute>()?.Text ??
                CultureInfo.CurrentCulture.TextInfo.ToTitleCase(command.Name);
        }

        /// <summary>Returns the summary of this command or an error <see cref="string"/></summary>
        /// <returns>The summary of this command</returns>
        public static string GetSummary(this CommandInfo command)
        {
            return command.Summary ?? "No summary found!";
        }

        /// <summary>Returns the remarks of this command or an error <see cref="string"/></summary>
        /// <returns>The remarks of this command</returns>
        public static string GetRemarks(this CommandInfo command)
        {
            return command.Remarks ?? "No remarks found!";
        }

        /// <summary>Returns a brief default help of this command</summary>
        /// <returns>The brief help of this command</returns>
        public static string GetBriefHelp(this CommandInfo command, string prefix)
        {
            var builder = new StringBuilder($"`{prefix}{command.GetName()}` - {command.GetRemarks()}\n");

            foreach (var alias in command.Aliases)
            {
                builder.AppendLine($"`{prefix}{alias}` - {command.GetRemarks()}");
            }

            return builder.ToString();
        }
        
        /// <summary>Returns a new embed builder with a default command help with an optional invite to the support server
        /// and a function for formatting the embed.</summary>
        /// <returns>The new embed with the provided help for the command</returns>
        public static EmbedBuilder GetHelp(this CommandInfo command, string invite = null, Func<EmbedBuilder, EmbedBuilder> formatter = null)
            => command.AppendHelp(new EmbedBuilder(), invite);

        /// <summary>Append to the embed a default command help with an optional invite to the support server
        /// and a function for formatting the embed.</summary>
        /// <returns>The embed modified</returns>
        public static EmbedBuilder AppendHelp(this CommandInfo command, EmbedBuilder embed, string invite = null, Func<EmbedBuilder, EmbedBuilder> formatter = null)
        {
            formatter = formatter ?? DefaultFormatter;
            embed.WithTitle($"`{command.GetName()}` Command Help")
                 .WithDescription(command.GetSummary());

            if (invite != null)
            {
                embed.Description += $" Join the [Support Server]{invite} for additional help.";
            }

            foreach (var parameter in command.Parameters)
            {
                if(!parameter.IsHidden())
                    embed.AddField(parameter.GetHelp());
            }

            return formatter(embed);
        }
    }
}
