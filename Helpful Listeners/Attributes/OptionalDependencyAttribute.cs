using System;

namespace Discord.Addons.Listeners
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    class OptionalDependencyAttribute : Attribute { }
}
