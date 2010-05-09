using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Drawing;

namespace primeira.Editor
{
    [DataContract()]
    public class DocumentDefinition
    {
        [DataMember()]
        public Type DocumentType { get; set; }

        [DataMember()]
        public DocumentDefinitionAttribute Attributes { get; set; }

        [DataMember()]
        public EditorDefinition DefaultEditor { get; set; }

        public Image Icon { get; internal set; }
    }
}
