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

        /// <summary>Determines whether an instance of a specified type can be assigned to an instance of the current type.</summary>
        /// <returns>Whether the specified type can be assigned to an instance of the current type.</returns>
        /// <remarks>Base source code from which this was created can be found at
        /// <see href="https://stackoverflow.com/questions/74616/how-to-detect-if-type-is-another-generic-type/1075059#1075059">this StackOverflow question</see></remarks>
        public static bool IsAssignableToGenericType(this Type baseType, Type type)
        {
            bool CheckGenericConditions(Type t)
            {
                return t.IsGenericType && baseType.GetGenericTypeDefinition() == type;
            }

            if (baseType.GetInterfaces().Any(CheckGenericConditions)) return true;
            if (CheckGenericConditions(baseType)) return true;

            Type typeBase = baseType.BaseType;
            if (typeBase == null) return false;

            return IsAssignableToGenericType(typeBase, type);
        }
    }
}
