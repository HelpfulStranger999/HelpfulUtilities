using System;
using System.Collections.Generic;
using System.Linq;

namespace HelpfulUtilities.Extensions
{
    public static partial class Extensions
    {
        /// <summary>
        /// Returns a random element of the <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the sequence.</typeparam>
        /// <param name="source">A sequence of elements.</param>
        /// <param name="random">The psuedo-random number generator used to randomly choose an element.</param>
        /// <returns>A random element of the <paramref name="source"/></returns>
        public static TSource Random<TSource>(this IEnumerable<TSource> source, Random random = null)
        {
            ErrorTests.NullCheck(source, nameof(source));
            random = random ?? new Random();

            if (source is IList<TSource> list) return list[random.Next(list.Count)];
            
            return source.ElementAt(random.Next(source.Count()));
        }
    }
}
