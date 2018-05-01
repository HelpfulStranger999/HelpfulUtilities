using System;
using System.Collections.Generic;

namespace HelpfulUtilities.Extensions
{

    public static partial class Extensions
    {
        /// <summary>
        /// Adds a set of items to the ICollection<typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The type of the elements in the collection.</typeparam>
        /// <param name="source">A collection of elements.</param>
        /// <param name="elements">A sequence of elements to add to the <paramref name="source"/>.</param>
        /// <exception cref="NotSupportedException">Throws when <paramref name="source"/> is read-only.</exception>
        public static void AddRange<T>(this ICollection<T> source, params T[] elements)
            => source.AddRange((IEnumerable<T>)elements);

        /// <summary>
        /// Adds a set of items to the ICollection<typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The type of the elements in the collection.</typeparam>
        /// <param name="source">A collection of elements.</param>
        /// <param name="elements">A sequence of elements to add to the <paramref name="source"/>.</param>
        /// <exception cref="NotSupportedException">Throws when <paramref name="source"/> is read-only.</exception>
        public static void AddRange<T>(this ICollection<T> source, IEnumerable<T> elements)
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
