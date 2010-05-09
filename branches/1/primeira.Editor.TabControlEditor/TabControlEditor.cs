using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using primeira.Editor;
using primeira.Editor.Components;

namespace primeira.Editor
{
    [EditorDocument(DocumentType = typeof(TabControlDocument))]
    [AddonDefinition(AddonOptions.SystemDelayedInitializationAddon)]
    public partial class TabControlEditor : EditorBase, IMessageControl, IShorcutEscopeProvider
    {

        #region Interop

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        #endregion

        public TabControlEditor(string fileName)
            : base(fileName)
        {
            InitializeComponent();

            MessageManager.SetMessageControl(this);

            ((Form)TabControlManager.GetInstance().MainForm).ResizeEnd += new EventHandler(TabControlEditor_ResizeEnd);
            TabControlManager.GetInstance().MainForm.SizeChanged += new EventHandler(TabControlEditor_ResizeEnd);
            ((Form)TabControlManager.GetInstance().MainForm).FormClosing += new FormClosingEventHandler(TabControlEditor_FormClosing);
        }

        void TabControlEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            TabControlManager.GetInstance().CloseEditor();
        }

        void TabControlEditor_ResizeEnd(object sender, EventArgs e)
        {
            TabControlManager.GetInstance().RefreshTabButtonSize();
        }

        [AddonInitialize()]
        public static void AddonInitialize()
        {
            FormBase fmMain = new FormBase();

            //try to set windows 7 style
            DwmHelper.SeventishIt(fmMain);

            TabControlManager.GetInstance().MainForm = fmMain;

            EditorContainerManager.SetEditorContainer((IEditorContainer)TabControlManager.GetInstance());

            EditorDefinition editor = EditorManager.RegisterEditor(typeof(TabControlEditor));

            DocumentDefinition doc = editor.Documents[0];

            IEditor tabEditor = EditorManager.LoadEditor(doc);

            TabControlManager.GetInstance().SetTabControl((TabControlEditor)tabEditor);

            fmMain.Controls.Add((Control)tabEditor);
        }

        public void AddEditor(IEditor editor)
        {
            this.pnTabArea.SuspendLayout();

            this.pnDocArea.Controls.Add((Control)editor);

            TabButton tabbutton = TabButton(editor);

            this.pnTabArea.Controls.Add(tabbutton);

            ToolStripItem t = this.menTabs.Items.Add(tabbutton.TabTitle, editor.Definition.Icon, toolStripMenuItem_Click);

            //To be removed by RemoveByKey
            t.Name = editor.FileName;

            t.Tag = editor.FileName;

            editor.OnSelected += new SelectedDelegate(editor_OnSelected);

            TabControlManager.GetInstance().ActiveEditor = editor;

            this.pnTabArea.ResumeLayout(true);
        }

        void editor_OnSelected(IEditor sender)
        {
            btnClose.Enabled = !sender.HasOption(DocumentDefinitionOptions.NeverClose);

            if (btnClose.Enabled)
                btnClose.Image = Image.FromFile(@"D:\Desenv\Ockham\branches\1\primeira.Editor.TabControlEditor\close5.gif");
            else
                btnClose.Image = Image.FromFile(@"D:\Desenv\Ockham\branches\1\primeira.Editor.TabControlEditor\close5_disabled.gif");

            btnClose.Refresh();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            TabControlManager.GetInstance().CloseEditor(TabControlManager.GetInstance().ActiveEditor);
        }

        public void SaveCloseEditor(IEditor editor)
        {
            Control c = (Control)editor;

            TabButton(editor).Visible = false;
            c.Visible = false;

            DocumentManager.SaveDocument(editor.Document, editor.FileName);

            pnTabArea.Controls.Remove(c);

            menTabs.Items.RemoveByKey(TabButton(editor).TabTitle);

            c.Dispose();
        }

        public TabButton CreateTabButton(IEditor editor)
        {
            return new TabButton(editor);
        }

        private void TabControlEditor_Load(object sender, EventArgs e)
        {
            MessageManager.SetMessageControl(this);

            //ShortcutManager.LoadFromType(this.GetType());

            //ShortcutManager.LoadFromForm(this);

            foreach (string file in ((TabControlDocument)Document).GetOpenTabsFilename())
            {
                EditorManager.LoadEditor(file);
            }

            string selectedTab = ((TabControlDocument)Document).GetSelectedTab();
            if (selectedTab != null)
                EditorManager.LoadEditor(selectedTab);
        }

        private void btnShowTabs_Click(object sender, EventArgs e)
        {
            menTabs.Show((Control)sender, 0, 20);
        }

        private void toolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditorManager.LoadEditor((string)((ToolStripItem)sender).Name);
        }

        private Dictionary<IEditor, TabButton> _tabButtons = new Dictionary<IEditor, TabButton>();

        public TabButton TabButton(IEditor editor)
        {
            if (!_tabButtons.Keys.Contains(editor))
                _tabButtons.Add(editor, CreateTabButton(editor));

            return _tabButtons[editor];
        }

        private void pnTop_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.ParentForm.WindowState == FormWindowState.Maximized)
                this.ParentForm.WindowState = FormWindowState.Normal;
            else if (this.ParentForm.WindowState == FormWindowState.Normal)
                this.ParentForm.WindowState = FormWindowState.Maximized;
        }

        private void pnTop_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.ParentForm.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        #region IMessageControl Members

        public void Send(MessageSeverity severity, string message)
        {
            switch (severity)
            {
                case MessageSeverity.Information: 
                case MessageSeverity.Alert:
                        NonModalMessage.GetInstance(message, pnDocArea);
                    break;

                case MessageSeverity.Error:
                        MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                case MessageSeverity.Fatal:
                        MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        Application.Exit();
                    break;
            }
        }

        #endregion
    }
}
