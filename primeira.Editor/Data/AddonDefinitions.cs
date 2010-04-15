using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace primeira.Editor
{
    [Flags()]
    public enum AddonDefinitions
    {
        None = 0,
        SystemAddon = 1,
        UserAddon = 2,
        WaitEditorContainer = 4,
        SystemDelayedInitializationAddon = 8,
    }
}
