using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace primeira.Editor
{
    public class Shortcut
    {
        public string Name;
        public string Description;
        public MethodInfo Method;
        internal List<object> Target = new List<object>();
        
        public Keys Key;
        public Keys Modifiers;
        public string Escope;
        internal KeyEvent Event;

        public string CommandCaption
        {
            get
            {
                return string.Format("{0} \t {1}", this.Name, this.Description);
            }
        }

        public string KeyCaption
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(Modifiers == Keys.None ? string.Empty : Modifiers.ToString());

                if (Key != Keys.Menu &&
                    Key != Keys.ShiftKey &&
                    Key != Keys.ControlKey)
                {
                    if (sb.Length > 0)
                        sb.Append(" + ");

                    sb.Append(Key.ToString());
                }

                return sb.ToString();
            }
        }

        public new string ToString()
        {
            return string.Format("{0} \t {1}", this.Name, this.Description);
        }

        public Shortcut New(string escope, Keys key, Keys modifiers)
        {
            Shortcut res = new Shortcut();
            res.Name = this.Name;
            res.Description = this.Description;
            res.Method = this.Method;
            res.Target = this.Target;
            res.Event = this.Event;
            res.Escope = escope;
            res.Key = key;
            res.Modifiers = modifiers;

            return res;
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
        public const string Active = "Active";
        public const string Chord = "Chord";
    }
}
