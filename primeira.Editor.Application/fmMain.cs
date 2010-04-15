using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using primeira.Editor.Components;
using primeira.Editor;


namespace primeira.Editor
{
    public partial class fmMain : Form, IShorcutEscopeProvider
    {
        public fmMain()
        {
            InitializeComponent();
        }

        private void fmMain_Load(object sender, EventArgs e)
        {
            //try to set windows 7 style
            DwmHelper.SeventishIt(this);

            AddonManager.Discovery();

            //TODO:Remove Shortcut manager dependecy. (Maybe using InitializeAddon attribute.)
            ShortcutManager.LoadFromForm(this);
        }

        #region IShorcutEscopeProvider Members

        public bool IsAtiveByEscope(string escope)
        {
            return false;
        }

        #endregion
    }
}
