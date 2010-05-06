using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace primeira.Editor
{
    /// <summary>
    /// Defines a class as an Editor.
    /// 
    /// To be correctly handled the class must have an static method with AddonInitializeAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class EditorDefinitionAttribute : Attribute
    {
        public Type DocumentType { get; set; }
        public bool DefaultDocumentType { get; set; }
    }
}
