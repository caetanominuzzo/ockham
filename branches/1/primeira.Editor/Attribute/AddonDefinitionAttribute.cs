using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace primeira.Editor
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AddonDefinitionAttribute : Attribute
    {
        public AddonOptions Options {get; private set;}

        public AddonDefinitionAttribute(AddonOptions options) 
        {
            Options = options;
        }
    }

    [Flags()]
    public enum AddonOptions
    {
        None = 0,
        SystemAddon = 1,
        UserAddon = 2,
        WaitEditorContainer = 4,
        SystemDelayedInitializationAddon = 8,
    }
}
