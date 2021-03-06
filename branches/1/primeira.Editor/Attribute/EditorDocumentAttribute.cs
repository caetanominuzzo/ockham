﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace primeira.Editor
{
    /// <summary>
    /// Defines a class as an Editor for an specific document.
    /// 
    /// To be correctly handled the class must be registered as an editor using EditorHeaderAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class EditorDocumentAttribute : Attribute
    {
        public Type DocumentType { get; set; }
    }
}

