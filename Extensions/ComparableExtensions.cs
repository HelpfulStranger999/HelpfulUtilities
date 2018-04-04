using System;

namespace HelpfulUtilities.Extensions
{
    public static class ComparableExtensions
    {
        public static bool IsBetween<TValue>(this TValue @base, TValue minimum, TValue maximum)
            where TValue : IComparable
            => IsBetween(@base, minimum, maximum, InclusivityComparer.Base);

        public static bool IsBetween<TValue>(this TValue @base, TValue minimum, TValue maximum, InclusivityComparer comparer)
            where TValue : IComparable
        {
            return comparer.CompareMaximum(@base, maximum) && comparer.CompareMinimum(@base, minimum);
        }

    }

}
