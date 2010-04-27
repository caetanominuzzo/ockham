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
        public Keys Modifiers;


     

        protected override void OnKeyDown(KeyEventArgs e)
        {
            Key = e.KeyCode;
            Modifiers = Keys.None;

            switch (e.Modifiers)
            {
                case Keys.Control: Modifiers = Modifiers | Keys.Control;
                    break;
                case Keys.Alt: Modifiers = Modifiers | Keys.Alt;
                    break;
                case Keys.Shift: Modifiers = Modifiers | Keys.Shift;
                    break;
            }

            this.Text = (Modifiers == Keys.None ? "" : Modifiers.ToString() + "+") + e.KeyCode.ToString();
            e.SuppressKeyPress = true;
        }

    }
}
