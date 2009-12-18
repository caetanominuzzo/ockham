using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using primeira.Editor;
using primeira.Editor.Components;

namespace primeira.Editor
{
    public partial class TabControlEditor : EditorBase, ITabControl, IMessageControl, IShorcutEscopeProvider
    {
        
        public TabControlEditor(string filename, DocumentBase data)
            : base(filename, data, typeof(TabControlDocument))
        {
            InitializeComponent();

            ShortcutManager.LoadFromForm(this);
        }

        public void AddTab(IEditor editor)
        {
            this.pnTabArea.SuspendLayout();

            this.pnDocArea.Controls.Add((Control)editor);

            this.pnTabArea.Controls.Add((TabButton)editor.TabButton);

            ToolStripItem t = this.menTabs.Items.Add(editor.TabButton.TabTitle, editor.Document.Definition.Icon, toolStripMenuItem_Click);
            t.Tag = editor.Filename;

            editor.OnSelected += new SelectedDelegate(editor_OnSelected);

            this.pnTabArea.ResumeLayout(true);
        }

        void editor_OnSelected(IEditor sender)
        {
            MessageManager.Alert("open: " + sender.Filename);

            btnClose.Enabled = sender.ShowCloseButton;

            if (sender.ShowCloseButton)
                btnClose.Image = Image.FromFile(@"D:\Desenv\Ockham\branches\1\primeira.Editor.TabControlEditor\close5.gif");
            else
                btnClose.Image = Image.FromFile(@"D:\Desenv\Ockham\branches\1\primeira.Editor.TabControlEditor\close5_disabled.gif");
            btnClose.Refresh();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            TabManager.GetInstance().CloseEditor();
        }

        public void HideTab(IEditor tab)
        {
            ((Control)tab).Visible = false;
            ((TabButton)tab.TabButton).Visible = false;
            menTabs.Items.RemoveByKey(tab.TabButton.TabTitle);
        }

        public void CloseHideTabs()
        {
            Control c;
            for(int i = 0; i < pnTabArea.Controls.Count; i++)
            {
                c = pnTabArea.Controls[i];
                if (!c.Visible)
                {
                    pnTabArea.Controls.Remove(c);
                    menTabs.Items.RemoveAt(i);
                    c.Dispose();
                    break;
                }
            }
        }

        public ITabButton CreateTabButton(IEditor editor)
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
            if(selectedTab!=null)
                EditorManager.LoadEditor(selectedTab);
        }

        public void ShowNonModalMessage(string message)
        {
            NonModalMessage.GetInstance(message, pnDocArea);
        }

        private void btnShowTabs_Click(object sender, EventArgs e)
        {
            menTabs.Show((Control)sender, 0, 20);
        }

        private void toolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditorManager.LoadEditor((string)((ToolStripItem)sender).Tag);
        }
    }
}
