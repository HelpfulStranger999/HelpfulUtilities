using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelpfulUtilities.Extensions
{
    public static class EnumerableExtensions
    {
        public static TSource Random<TSource>(this IEnumerable<TSource> source)
            => source.Random(new Random());

        public static TSource Random<TSource>(this IEnumerable<TSource> source, Random random)
        {
            ErrorTests.NullCheck(source, nameof(source));
            ErrorTests.NullCheck(random, nameof(random));

            if (source is IList<TSource> list) return list[random.Next(list.Count)];
            
            return source.ElementAt(random.Next(source.Count()));
        }

        public static IEnumerable<TValue> ToPropertyList<TSource, TValue>(this IEnumerable<TSource> source, Func<TSource, TValue> func)
        {
            ErrorTests.NullCheck(source, nameof(source));
            ErrorTests.NullCheck(func, nameof(func));

            foreach (var element in source)
            {
                yield return func(element);
            }
        }
    }
}
