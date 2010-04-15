using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace primeira.Editor
{
    partial class TabControlEditor
    {
        #region IShorcutEscopeProvider Members

        bool IShorcutEscopeProvider.IsAtiveByEscope(string escope)
        {
            if (escope == "DelayedTabSelectEscope")
                return TabControlManager.GetInstance().DelayedZOrderControl;

            return false;
        }

        [ShortcutVisibility("Close Tab", "", BasicEscopes.Global, Keys.F4, KeyModifiers.Control)]
        public void CloseActiveTab()
        {
            TabControlManager.GetInstance().CloseEditor(TabControlManager.GetInstance().ActiveEditor);
        }

        [ShortcutVisibility("Select Next Tab", "", BasicEscopes.Global, Keys.Tab, KeyModifiers.Control)]
        public void SelectNextTab()
        {
            TabControlManager.GetInstance().SelectNext();
        }

        [ShortcutVisibility("Select Prior Tab", "", BasicEscopes.Global, Keys.Tab, KeyModifiers.Control | KeyModifiers.Shift)]
        public void SelectPriorTab()
        {
            TabControlManager.GetInstance().SelectPrior();
        }

        [ShortcutVisibility(Name = "Control Tab Released", DefaultKey = Keys.ControlKey, Escope = "DelayedTabSelectEscope", Event = KeyEvent.KeyUp)]
        public void ReleaseControl()
        {
            TabControlManager.GetInstance().ReleaseDelayedZOrderControl();
        }


        #endregion

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
