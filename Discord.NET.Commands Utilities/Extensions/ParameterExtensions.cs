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
        /// <summary>Returns the attribute of the type on this parameter.</summary>
        /// <typeparam name="TAttribute">Type of attribute</typeparam>
        /// <returns>First instance of the attribute or null</returns>
        public static TAttribute GetAttribute<TAttribute>(this ParameterInfo parameter)
            where TAttribute : Attribute
        {
            return parameter.GetAttributes<TAttribute>().FirstOrDefault();
        }

        /// <summary>Returns all attributes of the type on this command.</summary>
        /// <typeparam name="TAttribute">Type of attribute</typeparam>
        /// <returns>All instances of the attribute</returns>
        public static IEnumerable<TAttribute> GetAttributes<TAttribute>(this ParameterInfo parameter)
        {
            return (IEnumerable<TAttribute>)parameter.Attributes.Where(attribute =>
            {
                return attribute is TAttribute;
            });
        }

        /// <summary>Determines whether this module is hidden as determined by <see cref="HiddenAttribute"/></summary>
        /// <returns>Whether this command is hidden</returns>
        public static bool IsHidden(this ParameterInfo parameter)
        {
            return parameter.GetAttribute<HiddenAttribute>() != null;
        }

        /// <summary>Returns the name of this parameter</summary>
        /// <returns>The name of this parameter</returns>
        public static string GetName(this ParameterInfo parameter)
        {
            return parameter.GetAttribute<NameAttribute>()?.Text ?? 
                CultureInfo.CurrentCulture.TextInfo.ToTitleCase(parameter.Name);
        }

        /// <summary>Returns the summary of this parameter or an error <see cref="string"/></summary>
        /// <returns>The summary of this parameter</returns>
        public static string GetSummary(this ParameterInfo parameter)
        {
            return parameter.Summary ?? "No summary found!";
        }

        /// <summary>Generates an optional help format for this parameter.</summary>
        /// <returns>An optional help for this parameter</returns>
        public static EmbedFieldBuilder GetHelp(this ParameterInfo parameter)
        {
            return new EmbedFieldBuilder
            {
                Name = parameter.GetName(),
                Value = parameter.GetSummary(),
                IsInline = true
            };
        }
    }
}
