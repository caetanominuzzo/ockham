﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace primeira.Editor
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class EditorDefinitionAttribute : Attribute
    {
        public Type DocumentType { get; set; }
    }
}
