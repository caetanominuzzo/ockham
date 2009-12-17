using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using primeira.Editor.Components;
using primeira.Editor.Business;


namespace primeira.Editor
{
    public partial class fmMain : Form, IShorcutEscopeProvider
    {


        TabControlEditor _tabControl;

        FileBrowserEditor _fileBrowser;

        public fmMain()
        {
            InitializeComponent();
        }

        private void fmMain_Load(object sender, EventArgs e)
        {
            //try to set windows 7 style
            DwmHelper.SeventishIt(this);

            EditorManager.RegisterEditors();

            #region The "Tabs Editor", also: Message Control

            //Loads a file with tabs default configuration
            IEditor tmp = EditorManager.LoadEditor("default.tabcontrol");

            _tabControl = (TabControlEditor)tmp;

            this.Controls.Add(_tabControl);

            //Define the _tabControl as the currente instance tab control & message control
            TabManager.GetInstance().SetTabControl(_tabControl);
            MessageManager.SetMessageControl(_tabControl);

            #endregion

            //Loads a file with file browser default configuration
            _fileBrowser = (FileBrowserEditor)EditorManager.LoadEditor("default.filebrowser");

            //Define the _fileBrowser as the currente instance file browser control
            FileManager.SetRecentManager(_fileBrowser);

            ShortcutManager.LoadFromForm(this);

            ShortcutManager.EscopeProvider = this;
        }

        [ShortCutVisibility("Nome", "", "", Keys.A, KeyModifiers.Alt)]
        public void show()
        {
            ShortcutManager.ShowConfig();
          //  MessageManager.Alert("asd");
        }

        #region IShorcutEscopeProvider Members

        public bool IsAtiveByControl(string controlName)
        {
            throw new NotImplementedException();
        }

        public bool IsAtiveByEscope(string escope)
        {
            return (escope == string.Empty);
        }

        #endregion
    }
}
