using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace primeira.Editor
{
    [DataContract()]
    public class EditorHeader
    {
        public Type EditorType { get; set; }

        [DataMember()]
        public Version EditorVersion { get; set; }

        public DocumentHeader[] Documents { get; set; }

        [DataMember()]
        public Version[] DocumentsVersions { get; set; }

    }
}
