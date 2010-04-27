using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace primeira.Editor
{
    public class Shortcut
    {
        public Keys Key;
        public Keys KeyModifier;
        public string Escope;
        public ShortcutCommand Command;
        internal KeyEvent Event;

        public new string ToString()
        {
            return string.Format("{0}+ ({1})", this.KeyModifier, this.Key);
        }

    }

    public enum KeyEvent
    {
        KeyDown = 0x100,
        KeyUp = 0x101
    }

    public static class BasicEscopes
    {
        public const string Global = "Global";
    }
}
