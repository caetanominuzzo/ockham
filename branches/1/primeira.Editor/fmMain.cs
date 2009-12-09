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
    public partial class fmMain : Form
    {
        TabControlEditor.TabControlEditor _tabControl;

        FileBrowserEditor _fileBrowser;

        public fmMain()
        {
            InitializeComponent();

            //MessageManager.AddMessageControl(new MessageControl());

            EditorManager.RegisterEditors();

            _tabControl = (TabControlEditor.TabControlEditor)EditorManager.LoadEditor("default.tabcontrol");

            MessageManager.OnAlert += new MessageManager.OnAlertDelegate(_tabControl.ShowNonModalMessage);

            this.Controls.Add(_tabControl);
            TabManager.GetInstance().SetTabControl(_tabControl);

            _fileBrowser = (FileBrowserEditor)EditorManager.LoadEditor("default.filebrowser");
            FileManager.SetRecentManager(_fileBrowser);

        }
    }
}
