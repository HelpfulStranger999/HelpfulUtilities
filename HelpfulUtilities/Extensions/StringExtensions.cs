using System;
using System.Collections.Generic;
using System.Text;

namespace HelpfulUtilities.Extensions
{
    public static partial class Extensions
    {
        private static StringComparison Comparer = StringComparison.CurrentCultureIgnoreCase;

        /// <summary>
        /// The case insensitive <see cref="StringComparison"/> for extension methods.
        /// </summary>
        public static StringComparison IgnoreCaseComparer
        {
            get => Comparer;
            set
            {
                switch (value)
                {
                    case StringComparison.CurrentCultureIgnoreCase:
                        Comparer = value;
                        break;
                    case StringComparison.InvariantCultureIgnoreCase:
                        Comparer = value;
                        break;
                    case StringComparison.OrdinalIgnoreCase:
                        Comparer = value;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Determines whether this string and a specified <see cref="string"/> object have the same value case insensitively.
        /// </summary>
        /// <param name="str">This string instance</param>
        /// <param name="value">The specified <see cref="string"/> object to check against</param>
        /// <returns>Whether the strings have the same value ignoring case.</returns>
        public static bool EqualsIgnoreCase(this string str, string value) => str.Equals(value, Comparer);

        /// <summary>
        /// Determines whether this string contains a specified <see cref="string"/> object case insensitively.
        /// </summary>
        /// <param name="str">This string instance</param>
        /// <param name="value">The specified <see cref="string"/> object to check against</param>
        /// <returns>Whether this string contains <paramref name="value"/> ignoring case.</returns>
        public static bool ContainsIgnoreCase(this string str, string value) => str.IndexOf(value, Comparer) >= 0;

        /// <summary>
        /// Determines whether the beginning of this string matches the specified string case insensitively.
        /// </summary>
        /// <param name="str">This string instance</param>
        /// <param name="value">The specified <see cref="string"/> object to check against</param>
        /// <returns>Whether this string begins with the value of <paramref name="value"/> ignoring case.</returns>
        public static bool StartsWithIgnoreCase(this string str, string value) => str.StartsWith(value, Comparer);



        /// <summary>
        /// Indicates whether this string is null or an <see cref="string"/>.Empty string.
        /// </summary>
        /// <param name="str">This string instance</param>
        /// <returns>Whether this string is null or empty.</returns>
        public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);

        /// <summary>
        /// Indicates whether this string is null, empty, or consists only of whitespace characters.
        /// </summary>
        /// <param name="str">This string instance</param>
        /// <returns>Whether this string is null, empty, or whitespace only.</returns>
        public static bool IsNullOrWhitespace(this string str) => string.IsNullOrWhiteSpace(str);
    }
}
