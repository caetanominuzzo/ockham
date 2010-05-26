using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace primeira.Editor
{
    [DataContract()]
    public class VersionFilter
    {
        [DataMember()]
        public Guid Target { get; set; }

        [DataMember()]
        public string[] Numbers { get; set; }

        [DataMember()]
        public string MinNumber { get; set; }

        [DataMember()]
        public string MaxNumber { get; set; }
    }
}
