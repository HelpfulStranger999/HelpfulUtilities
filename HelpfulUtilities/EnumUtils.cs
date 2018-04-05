using HelpfulUtilities.Extensions;
using System;
using System.Linq;

namespace HelpfulUtilities
{
    /// <summary>
    /// This class provides a host of methods for operating on Enums.
    /// </summary>
    public static class EnumUtils
    {

        /// <summary>Returns a random value of the <paramref name="enum"/></summary>
        /// <typeparam name="T">Type of enum</typeparam><param name="enum">The enum</param>
        /// <returns>A random enum value of type <typeparamref name="T"/></returns>
        public static T Random<T>(Enum @enum) where T : struct, IConvertible
            => Random<T>(@enum.GetType());

        /// <summary>Returns a random value of the <paramref name="obj"/> enum</summary>
        /// <typeparam name="T">Type of enum</typeparam><param name="obj">An object of the enum</param>
        /// <returns>A random enum value of type <typeparamref name="T"/></returns>
        public static T Random<T>(T obj) where T : struct, IConvertible
            => Random<T>(typeof(T));

        /// <summary>Returns a random value of the <paramref name="type"/> enum</summary>
        /// <typeparam name="T">Type of enum</typeparam><param name="enum">The enum type</param>
        /// <returns>A random enum value of type <typeparamref name="T"/></returns>
        public static T Random<T>(Type type) where T : struct, IConvertible
            => Enum.GetValues(type).Cast<T>().Random();
    }
}
