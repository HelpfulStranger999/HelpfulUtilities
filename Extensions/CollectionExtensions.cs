using System.Collections.Generic;

namespace HelpfulUtilities.Extensions
{
    public static class CollectionExtensions
    {
        public static void AddRange<TSource>(this ICollection<TSource> source, params TSource[] elements)
            => source.AddRange(elements);

        public static void AddRange<TSource>(this ICollection<TSource> source, IEnumerable<TSource> elements)
        {
            ErrorTests.NullCheck(source);
            ErrorTests.NullCheck(elements);

            foreach (var item in elements)
            {
                source.Add(item);
            }
        }
    }
}
