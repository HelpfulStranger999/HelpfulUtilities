using System;
using System.Collections.Generic;
using System.Text;

namespace HelpfulUtilities.Extensions
{
    public static class StringExtensions
    {
        internal static StringComparison Comparer = StringComparison.CurrentCultureIgnoreCase;
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

        public static bool EqualsIgnoreCase(this string str, string value) => str.Equals(value, Comparer);
        public static bool ContainsIgnoreCase(this string str, string value) => str.IndexOf(value, Comparer) >= 0;
        public static bool StartsWithIgnoreCase(this string str, string value) => str.StartsWith(value, Comparer);

        public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);
        public static bool IsNullOrWhitespace(this string str) => string.IsNullOrWhiteSpace(str);
    }
}
