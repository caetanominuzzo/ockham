using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace primeira.Editor
{
    partial class TabControlEditor
    {
        
        [ShortCutVisibility("File Browser", "Shows the File Browser tab", BasicEscopes.Global, Keys.T, KeyModifiers.Control)]
        public void show()
        {
            EditorManager.LoadEditor(typeof(FileBrowserDocument));
        }

        #region IShorcutEscopeProvider Members

        public bool IsAtiveByControl(string controlName)
        {
            throw new NotImplementedException();
        }

        public bool IsAtiveByEscope(string escope)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
