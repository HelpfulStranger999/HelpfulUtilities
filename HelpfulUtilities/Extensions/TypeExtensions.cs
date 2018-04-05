using System;
using System.Linq;

namespace HelpfulUtilities.Extensions
{
    public static partial class Extensions
    {
        /// <summary>Determines whether <paramref name="baseType"/> extends from <paramref name="type"/></summary>
        /// <param name="type">The type to compare against</param>
        /// <returns>Whether this type extends the specified type.</returns>
        public static bool Extends(this Type baseType, Type type)
            => baseType.GetInterfaces().Contains(type) || type.IsAssignableFrom(baseType);
    }
}
