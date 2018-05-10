﻿using System;
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

        /// <summary>
        /// Returns a sequences of <typeparamref name="TValue"/> from an <see cref="IEnumerable{TValue}"/> of <see cref="IEnumerable{TValue}"/>
        /// </summary>
        /// <typeparam name="TValue">The type of elements in the sequence.</typeparam>
        /// <param name="source">A sequence of elements</param>
        /// <returns>The flattened <see cref="IEnumerable{TValue}"/></returns>
        public static IEnumerable<TValue> Flatten<TValue>(this IEnumerable<IEnumerable<TValue>> source)
        {
            ErrorTests.NullCheck(source, nameof(source));

            foreach (var enumerable in source)
            {
                ErrorTests.NullCheck(enumerable, nameof(enumerable));
                foreach (var element in enumerable)
                {
                    yield return element;
                }
            }
        }
    }
}
