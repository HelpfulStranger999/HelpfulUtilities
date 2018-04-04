using System;
using System.Collections.Generic;
using System.Text;

namespace HelpfulUtilities
{
    /// <summary>
    /// This struct is used for comparing <see cref="IComparable"/>s with a minimum and maximum inclusivity.
    /// </summary>
    public struct InclusivityComparer
    {
        /// <summary>
        /// The base <see cref="InclusivityComparer"/> to be used by default.
        /// </summary>
        public static InclusivityComparer Default = new InclusivityComparer(true, false);

        /// <summary>
        /// Whether comparison should be inclusive or exclusive of the minimum.
        /// </summary>
        public bool MinimumInclusive { get; }

        /// <summary>
        /// Whether comparison should be inclusive or exclusive of the maximum.
        /// </summary>
        public bool MaximumInclusive { get; }


        /// <summary>
        /// This creates an instance of a <see cref="InclusivityComparer"/>
        /// </summary>
        /// <param name="minInclusive">Whether comparison should be inclusive or exclusive of the minimum</param>
        /// <param name="maxInclusive">Whether comparison should be inclusive or exclusive of the maximum.</param>
        public InclusivityComparer(bool minInclusive = true, bool maxInclusive = false)
        {
            
            MinimumInclusive = minInclusive;
            MaximumInclusive = maxInclusive;
        }

        /// <summary>
        /// Determines whether <paramref name="base"/> is greater than or, is inclusive of the minimum, equal to the minimum.
        /// See <seealso cref="MinimumInclusive"/>
        /// </summary>
        /// <param name="base">The <see cref="IComparable"/> to compare against the <paramref name="minimum"/></param>
        /// <param name="minimum">The minimum value possible</param>
        /// <returns>Whether <paramref name="base"/> is greater than or equal to the <paramref name="minimum"/></returns>
        public bool CompareMinimum(IComparable @base, IComparable minimum)
        {
            // If the dates are equal, then the result depends on inclusivity.
            // Otherwise, the result depends on the base being greater than the minimum
            var result = @base.CompareTo(minimum);
            if (result == 0) return MinimumInclusive;
            return result < 0;
        }

        /// <summary>
        /// Determines whether <paramref name="base"/> is less than or, is inclusive of the maximum, equal to the maximum.
        /// See <seealso cref="MaximumInclusive"/>
        /// </summary>
        /// <param name="base">The <see cref="IComparable"/> to compare against the <paramref name="maximum"/></param>
        /// <param name="maximum">The maximum value possible</param>
        /// <returns>Whether <paramref name="base"/> is less than or equal to the <paramref name="maximum"/></returns>
        public bool CompareMaximum(IComparable @base, IComparable maximum)
        {
            // If the dates are equal, then the result depends on inclusivity.
            // Otherwise, the result depends on the base being less than the maximum.
            var result = @base.CompareTo(maximum);
            if (result == 0) return MaximumInclusive;
            return result > 0;
        }

    }
}
