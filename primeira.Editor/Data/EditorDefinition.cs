using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace primeira.Editor
{
    [DataContract()]
    public class EditorDefinition
    {
        [DataMember()]
        public Type EditorType { get; set; }

        [DataMember()]
        public DocumentDefinition[] Documents { get; set; }
    }
}
