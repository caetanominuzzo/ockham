using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace primeira.Editor
{
    internal class Shortcut
    {
        internal int AtomID;
        internal Keys Key;
        internal KeyModifiers KeyModifier;
        internal string Escope;
        internal ShortcutCommand Command;

        public new string ToString()
        {
            return string.Format("{0}+ ({1})", this.KeyModifier, this.Key);

            //StringBuilder sb = new StringBuilder();
            //sb.Append(this.KeyModifier.ToString());
            //sb.Append("+");
            //sb.Append(this.Key.ToString());
            //sb.Append(" (");
            //sb.Append(Escope);
            //sb.Append(")");
            //return sb.ToString();
        }

        public void Register(IntPtr ParentHandle)
        {
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

    public static class BasicEscopes
    {
        public const string Global = "Global";
    }
}
