using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace primeira.Editor
{
    partial class TextEditor
    {
        #region IShorcutEscopeProvider Members

        public bool IsAtiveByEscope(string escope)
        {
            return escope.Equals(Escopes.Editing) ? base.IsAtiveByEscope(BasicEscopes.Active) : false;
        }

        public static class Escopes
        {
            public const string Editing = "Editing";
        }

        #endregion

        [ShortcutVisibility("Select All", "Selects all text", Escopes.Editing, Keys.A, Keys.Control, Event = KeyEvent.KeyUp)]
        public void SelectAll()
        {
            this.txtMain.SelectAll();
        }

    }
}
