using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace primeira.Editor
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class AddonInitializeAttribute : Attribute
    {
    }
}
