using System;

namespace HelpfulUtilities.Discord.Commands.Attributes
{
    /// <summary>Describes whether this command or module is hidden, for help commands.</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Module | AttributeTargets.Parameter,
        AllowMultiple = false, Inherited = true)]
    public class HiddenAttribute : Attribute { }
}
