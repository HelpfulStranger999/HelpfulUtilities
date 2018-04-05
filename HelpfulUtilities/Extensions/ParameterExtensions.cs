using System;
using System.Reflection;

namespace HelpfulUtilities.Extensions
{
    public static partial class Extensions
    {
        /// <summary>Determines whether this <paramref name="parameter"/> extends from <paramref name="type"/></summary>
        /// <param name="type">The type to compare against</param>
        /// <returns>Whether this parameter extends the specified type.</returns>
        public static bool Extends(this ParameterInfo parameter, Type type)
            => parameter.ParameterType.Extends(type);
    }
}
