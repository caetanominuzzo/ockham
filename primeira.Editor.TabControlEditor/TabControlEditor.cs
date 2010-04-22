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
    [EditorDefinition(DocumentType = typeof(TabControlDocument))]
    [AddonDefinition(AddonDefinitions.SystemAddon)]
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

        public TabControlEditor(string filename, DocumentBase data)
            : base(filename, data, typeof(TabControlDocument))
        {
            InitializeComponent();

            MessageManager.SetMessageControl(this);

            ShortcutManager.LoadFromForm(this);

            ((Form)TabControlManager.GetInstance().ParentControl).ResizeEnd += new EventHandler(TabControlEditor_ResizeEnd);
            TabControlManager.GetInstance().ParentControl.SizeChanged += new EventHandler(TabControlEditor_ResizeEnd);
            ((Form)TabControlManager.GetInstance().ParentControl).FormClosing += new FormClosingEventHandler(TabControlEditor_FormClosing);
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
            Control parent = Application.OpenForms[0];

            TabControlManager.GetInstance().ParentControl = parent;

            EditorContainerManager.SetEditorContainer((IEditorContainer)TabControlManager.GetInstance());

            EditorManager.RegisterEditor(typeof(TabControlEditor));

            IEditor tabEditor = EditorManager.LoadEditor(typeof(TabControlDocument));

            TabControlManager.GetInstance().SetTabControl((TabControlEditor)tabEditor);

            parent.Controls.Add((Control)tabEditor);

        }

        public void AddEditor(IEditor editor)
        {


            this.pnTabArea.SuspendLayout();

            this.pnDocArea.Controls.Add((Control)editor);

            TabButton tabbutton = TabButton(editor);

            this.pnTabArea.Controls.Add(tabbutton);

            ToolStripItem t = this.menTabs.Items.Add(tabbutton.TabTitle, DocumentManager.GetDocumentDefinition(editor.Document.GetType()).Icon, toolStripMenuItem_Click);

            //To be removed by RemoveByKey
            t.Name = editor.Filename;

            t.Tag = editor.Filename;

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

            DocumentManager.ToXml(editor.Document, editor.Filename);

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
        
        public void ShowNonModalMessage(string message)
        {
            NonModalMessage.GetInstance(message, pnDocArea);
        }

        #endregion
    }
}
