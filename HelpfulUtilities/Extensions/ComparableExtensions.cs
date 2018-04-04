using System;

namespace HelpfulUtilities.Extensions
{
    public static partial class Extensions
    {
        /// <summary>
        /// Determines whether <paramref name="base"/> is between the <paramref name="minimum"/> inclusively and the <paramref name="maximum"/> exclusively.
        /// </summary>
        /// <typeparam name="TValue">Type of <see cref="IComparable"/></typeparam>
        /// <param name="base">The base element to check</param>
        /// <param name="minimum">The minimum element it can be</param>
        /// <param name="maximum">The maximum element it can be</param>
        /// <returns>Whether <paramref name="base"/> is between <paramref name="minimum"/> and <paramref name="maximum"/></returns>
        public static bool IsBetween<TValue>(this TValue @base, TValue minimum, TValue maximum)
            where TValue : IComparable
            => IsBetween(@base, minimum, maximum, InclusivityComparer.Default);

        /// <summary>
        /// Determines whether <paramref name="base"/> is between the <paramref name="minimum"/> and 
        /// the <paramref name="maximum"/> with inclusivity and exclusitivity as determined by <paramref name="comparer"/>.
        /// </summary>
        /// <typeparam name="TValue">Type of <see cref="IComparable"/></typeparam>
        /// <param name="base">The base element to check</param>
        /// <param name="minimum">The minimum element it can be</param>
        /// <param name="maximum">The maximum element it can be</param>
        /// <param name="comparer">The <see cref="InclusivityComparer"/> that determines the exclusivity and inclusivity
        /// of the minimum and the maximum in comparison to <paramref name="base"/></param>
        /// <returns>Whether <paramref name="base"/> is between <paramref name="minimum"/> and <paramref name="maximum"/></returns>
        public static bool IsBetween<TValue>(this TValue @base, TValue minimum, TValue maximum, InclusivityComparer comparer)
            where TValue : IComparable
        {
            return comparer.CompareMaximum(@base, maximum) && comparer.CompareMinimum(@base, minimum);
        }

    }

}
