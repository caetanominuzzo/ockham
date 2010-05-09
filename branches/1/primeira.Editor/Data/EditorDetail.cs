using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace primeira.Editor
{
    [DataContract()]
    public class EditorDetail
    {
        [DataMember()]
        public Type EditorType { get; set; }

        [DataMember()]
        public DocumentDetail[] Documents { get; set; }
    }
}
