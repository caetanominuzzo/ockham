using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace primeira.Editor
{
    public class Shortcut
    {
        internal int AtomID;
        public Keys Key;
        public KeyModifiers KeyModifier;
        public string Escope;
        public ShortcutCommand Command;
        internal KeyEvent Event;

        public new string ToString()
        {
            return string.Format("{0}+ ({1})", this.KeyModifier, this.Key);
        }

        public void Register(IntPtr ParentHandle)
        {
            if(Event == KeyEvent.HotKey)
                ShortcutManager.RegisterHotKey(ParentHandle, AtomID, KeyModifier, Key);
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
        HotKey = 0x0312,
        KeyDown = 0x100,
        KeyUp = 0x101
    }

    public static class BasicEscopes
    {
        public const string Global = "Global";
    }
}
