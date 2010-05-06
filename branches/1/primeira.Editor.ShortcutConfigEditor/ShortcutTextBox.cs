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

            StringBuilder sb = new StringBuilder();

            if (e.Modifiers.HasFlag(Keys.Alt))
            {
                sb.Append("Alt");

                if (Key.HasFlag(Keys.Menu))
                    Key &= ~Keys.Menu;
            }

            if (e.Modifiers.HasFlag(Keys.Control))
            {
                if (sb.Length > 0)
                    sb.Append(" + ");

                sb.Append("Control");

                if (Key.HasFlag(Keys.ControlKey))
                    Key &= ~Keys.ControlKey;
            }

            if (e.Modifiers.HasFlag(Keys.Shift))
            {
                if (sb.Length > 0)
                    sb.Append(" + ");

                sb.Append("Shift");

                if (Key.HasFlag(Keys.ShiftKey))
                    Key &= ~Keys.ShiftKey;
            }

            this.Text = (e.Modifiers == Keys.None ? "" : sb.ToString() + (Key == Keys.None? "" : " + ")) + (Key == Keys.None? "" : Key.ToString());
            e.SuppressKeyPress = true;
            e.Handled = true;
        }

    }
}
