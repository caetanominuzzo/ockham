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
        public bool IsValid;

     

        protected override void OnKeyDown(KeyEventArgs e)
        {
            Key = e.KeyCode;
            Modifiers = e.Modifiers;

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
            else Key = Keys.None;

            Text =  sb.ToString();

            IsValid = (Key != Keys.None);

            this.OnTextChanged(e);

            e.SuppressKeyPress = true;
            e.Handled = true;
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);

            ShortcutManager.PausePreFilter();
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);

            ShortcutManager.ResumePreFilter();
        }

    }
}
