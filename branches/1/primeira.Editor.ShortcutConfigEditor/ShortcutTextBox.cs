using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace primeira.Editor
{
    internal class ShortcutTextBox : TextBox
    {
        public Keys Key;
        public KeyModifiers Modifiers;


        [Flags()]
        public enum KeyModifiers
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            Windows = 8

        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            Key = e.KeyCode;
            Modifiers = KeyModifiers.None;

            switch (e.Modifiers)
            {
                case Keys.Control: Modifiers = Modifiers | KeyModifiers.Control;
                    break;
                case Keys.Alt: Modifiers = Modifiers | KeyModifiers.Alt;
                    break;
                case Keys.Shift: Modifiers = Modifiers | KeyModifiers.Shift;
                    break;
            }

            this.Text = (Modifiers == KeyModifiers.None ? "" : Modifiers.ToString() + "+") + e.KeyCode.ToString();
            e.SuppressKeyPress = true;
        }

    }
}
