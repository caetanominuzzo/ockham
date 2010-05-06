using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace primeira.Editor
{
    partial class fmMain
    {
        [ShortcutVisibility("Manage Shortcuts", "", BasicEscopes.Global, Keys.S, Keys.Control | Keys.Shift | Keys.Alt, Event=KeyEvent.KeyUp)]
        public static void show()
        {
            ShortcutManager.ShowConfig();
        }


    }
}
