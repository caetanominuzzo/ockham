using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace primeira.Editor
{
    /// <summary>
    /// Defines a class as an Addon.
    /// 
    /// To be correctly handled the class must have an static method with AddonInitializeAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AddonDefinitionAttribute : Attribute
    {
        public AddonOptions Options {get; private set;}

        public AddonDefinitionAttribute(AddonOptions options) 
        {
            Options = options;
        }
    }

    /// <summary>
    /// Used in AddonDefinitionAttribute ctor.
    /// This enum defines the addons load order. 
    /// 
    /// First are loaded SystemAddon,
    ///             then SystemDelayedInitializationAddon,
    ///             then UserAddon 
    ///      and finally LastInitilizedAddon.
    /// 
    /// </summary>
    public enum AddonOptions
    {
        None = 0,
        UserAddon = 1,
        SystemAddon = 2,
        SystemDelayedInitializationAddon = 4,
        LastInitilizedAddon = 8,
    }
}
