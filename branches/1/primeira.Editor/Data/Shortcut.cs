using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace primeira.Editor
{
    public class Shortcut
    {
        public Keys Key;
        public KeyModifiers KeyModifier;
        public string Escope;
        public ShortcutCommand Command;
        internal KeyEvent Event;

        public new string ToString()
        {
            return string.Format("{0}+ ({1})", this.KeyModifier, this.Key);
        }

    }

    [Flags()]
    public enum KeyModifiers
    {
        None = 0,
        Alt = 1,
        Control = 2,
        Shift = 4,
        Windows = 8
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
