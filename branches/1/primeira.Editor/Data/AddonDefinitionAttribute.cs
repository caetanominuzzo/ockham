using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace primeira.Editor
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AddonDefinitionAttribute : Attribute
    {
        public AddonDefinitions Definition {get; private set;}

        public AddonDefinitionAttribute(AddonDefinitions definition) 
        {
            Definition = definition;
        }
    }
}
