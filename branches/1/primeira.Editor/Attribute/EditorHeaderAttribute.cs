using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace primeira.Editor
{
    /// <summary>
    /// Defines an addons (explicitly an editor) inside an assembly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple=true)]
    public sealed class EditorHeaderAttribute : AddonHeaderAttribute
    {
        public EditorHeaderAttribute(string typeName, string initializeMethodName, AddonOptions options)
            : base(typeName, initializeMethodName, options)
        { }

        public EditorHeaderAttribute(string typeName)
            : this(typeName, null, AddonOptions.UserAddon)
        { }

        public EditorHeaderAttribute(string typeName, AddonOptions options)
            : this(typeName, null, options)
        { }
    }
}
