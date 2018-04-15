using System;

namespace HelpfulUtilities.Discord.Listeners
{
    /// <summary>Marks a property or parameter as an optional dependency.</summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    class OptionalDependencyAttribute : Attribute { }
}
