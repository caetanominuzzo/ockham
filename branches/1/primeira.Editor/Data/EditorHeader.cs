﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace primeira.Editor
{
    [DataContract()]
    public class EditorHeader : AddonHeader
    {
        [DataMember()]
        public VersionData[] DocumentVersions { get; set; }

        public EditorHeader(Type editorType)
            : base(editorType)
        { }
    }
}
