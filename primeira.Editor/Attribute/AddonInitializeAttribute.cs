using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace primeira.Editor
{
    /// <summary>
    /// Defines a method as entry point of an addon.
    /// To be correctly handled the method must be:
    /// 
    ///     1. Static &
    ///     
    ///     2. Member of a class with AddonInitializeAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class AddonInitializeAttribute : Attribute
    {
    }
}
