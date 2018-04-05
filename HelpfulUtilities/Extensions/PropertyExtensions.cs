using System;
using System.Reflection;

namespace HelpfulUtilities.Extensions
{
    public static partial class Extensions
    {
        /// <summary>Determines whether <paramref name="property"/> extends from <paramref name="type"/></summary>
        /// <param name="type">The type to compare against</param>
        /// <returns>Whether this property extends the specified type.</returns>
        public static bool Extends(this PropertyInfo property, Type type)
            => property.PropertyType.Extends(type);
    }
}
