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


        TabControlEditor _tabControl;

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
            IEditor tmp = EditorManager.LoadEditor(typeof(TabControlDocument));

            _tabControl = (TabControlEditor)tmp;

            this.Controls.Add(_tabControl);

            //Define the _tabControl as the currente instance tab control & message control
            TabManager.GetInstance().SetTabControl(_tabControl);
            MessageManager.SetMessageControl(_tabControl);

            #endregion

            //Loads a file with file browser default configuration
            tmp = (FileBrowserEditor)EditorManager.LoadEditor(typeof(FileBrowserDocument));

            //Define the _fileBrowser as the currente instance recent files browser control
            FileManager.SetRecentManager((IRecentFileControl)tmp);

            ShortcutManager.LoadFromForm(this);

            ShortcutManager.ParentEscopeProvider = this;

            ShortcutManager.SetShortcutConfigDocumentType(typeof(ShortcutConfigDocument));
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
