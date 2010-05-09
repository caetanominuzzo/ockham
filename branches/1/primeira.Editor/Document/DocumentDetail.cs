﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Drawing;

namespace primeira.Editor
{
    [DataContract()]
    public class DocumentDetail
    {
        [DataMember()]
        public Type DocumentType { get; set; }

        [DataMember()]
        public DocumentDefinitionAttribute Definition { get; set; }

        [DataMember()]
        public bool DefaultEditor { get; set; }

        public Image Icon { get; internal set; }
    }
}