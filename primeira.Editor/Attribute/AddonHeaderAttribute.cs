using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace primeira.Editor
{
    /// <summary>
    /// Defines an addons inside an assembly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple=true)]
    public class AddonHeaderAttribute : HeaderAttributeBase
    {
        public AddonOptions Options {get; set;}

        public readonly string ClassName;
        
        public readonly string InitializeMethodName;

        public AddonHeaderAttribute(string className, string initializeMethodName, AddonOptions options)
        {
            ClassName = className;
            InitializeMethodName = initializeMethodName;
            Options = options;
        }

        public AddonHeaderAttribute(string className)
            : this(className, null, AddonOptions.UserAddon)
        { }

        public AddonHeaderAttribute(string className, AddonOptions options)
            : this(className, null, options)
        { }

        public AddonHeaderAttribute(string className, string initializeMethodName)
            : this(className, initializeMethodName, AddonOptions.UserAddon)
        { }
    }

    /// <summary>
    /// Used in AddonAttribute ctor.
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
