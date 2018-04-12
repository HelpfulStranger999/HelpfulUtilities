using System;
using System.Reflection;

namespace HelpfulUtilities.Extensions
{
    public static partial class Extensions
    {
        /// <summary>
        /// Returns whether this object has the given attribute and outputs the attribute to <paramref name="attribute"/>
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute</typeparam>
        /// <returns>Whether this object has the given attribute</returns>
        public static bool TryGetAttribute<TAttribute>(this MemberInfo memberInfo, out TAttribute attribute) where TAttribute : Attribute
        {
            attribute = memberInfo.GetAttribute<TAttribute>();
            return memberInfo.HasAttribute<TAttribute>();
        }

        /// <summary>
        /// Returns the first instance of <typeparamref name="TAttribute"/> on this object
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns>The first instance of <typeparamref name="TAttribute"/> on this object</returns>
        public static TAttribute GetAttribute<TAttribute>(this MemberInfo memberInfo) where TAttribute : Attribute
            => memberInfo.HasAttribute<TAttribute>() ? memberInfo.GetAttribute<TAttribute>() :
            throw new ArgumentException($"{typeof(TAttribute).Name} not found");

        /// <summary>
        /// Returns whether this instance has the specified attribute
        /// </summary>
        /// <typeparam name="TAttribute">Type of attribute</typeparam>
        /// <returns>Whether this <see cref="MemberInfo"/> has the attribute</returns>
        public static bool HasAttribute<TAttribute>(this MemberInfo memberInfo) where TAttribute : Attribute
        {
            return memberInfo.GetCustomAttribute<TAttribute>() != null;
        }
    }
}
