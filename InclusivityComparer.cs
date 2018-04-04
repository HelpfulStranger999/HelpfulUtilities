using System;
using System.Collections.Generic;
using System.Text;

namespace HelpfulUtilities
{
    public struct InclusivityComparer
    {
        public static InclusivityComparer Base = new InclusivityComparer(true, false);
        public bool MinimumInclusive { get; }
        public bool MaximumInclusive { get; }

        public InclusivityComparer(bool minInclusive = true, bool maxInclusive = false)
        {
            MinimumInclusive = minInclusive;
            MaximumInclusive = maxInclusive;
        }

        public bool CompareMinimum(IComparable @base, IComparable minimum)
        {
            // If the dates are equal, then the result depends on inclusivity.
            // Otherwise, the result depends on the base being greater than the minimum
            var result = @base.CompareTo(minimum);
            if (result == 0) return MinimumInclusive;
            return result < 0;
        }

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
