using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;

namespace primeira.Editor
{
    [DataContract()]
    public class AddonHeader : HeaderBase
    {
        [DataMember()]
        public string AssemblyFile { get; set; }

        [DataMember()]
        public string Type { get; set; }

        [DataMember()]
        public AddonOptions Options { get; set; }

        public MethodInfo InitializeMethod { get; set; }

        public AddonHeader(Type addonType)
        {
            AssemblyFile = addonType.Assembly.CodeBase;
            Type = addonType.Name;
            BaseType = addonType;
        }


    }
}
