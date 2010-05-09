using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace primeira.Editor
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EditorDefinitionAttribute : Attribute
    {
        [DataMember()]
        public DocumentDefinitionAttribute[] Documents { get; internal set; }

        [DataMember()]
        public Type EditorType { get; internal set; }
    }
}
