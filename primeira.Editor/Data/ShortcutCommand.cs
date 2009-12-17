using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace primeira.Editor
{
    internal class ShortcutCommand
    {
        internal string Name;
        internal string Description;
        internal MethodInfo Method;
        internal object Object;
        internal string Escope;
    }
}
