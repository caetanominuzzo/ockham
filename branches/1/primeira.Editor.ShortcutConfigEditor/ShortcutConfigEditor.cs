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

[assembly: EditorHeader("primeira.Editor.ShortcutConfigEditor",
    AddonOptions.SystemAddon,
    Name = "ShortcutConfigEditor",
    Id = "{328F4FE6-7D48-44EB-AC09-7B9F2E8FFB33}",
    VersionNumber="3.1.a")]

namespace primeira.Editor
{
    [EditorDocument(DocumentType = typeof(ShortcutConfigDocument))]
    public partial class ShortcutConfigEditor : EditorBase
    {
        public ShortcutConfigEditor(string fileName)
            : base(fileName)
        {
            InitializeComponent();
        }

        private void ShortcutManagerEditor_Load(object sender, EventArgs e)
        {
            Shortcut[] shortcuts = (from a in ShortcutManager.Shorcuts
                             group a by a.Method into g
                             orderby g.First().CommandCaption
                             select g.First()
                      ).ToArray();

            lsCommand.DataSource = shortcuts;
            lsCommand.DisplayMember = "CommandCaption";

            string[] escopes = (from a in ShortcutManager.Shorcuts
                             group a by a.Escope into g
                             select g.First().Escope
                      ).ToArray();

            cbEscope.DataSource = escopes;

            txtCommand.Focus();
        }

        private void lsCommand_SelectedIndexChanged(object sender, EventArgs e)
        {
            Shortcut[] shortcuts = (from a in ShortcutManager.Shorcuts
                                    where a.Method == ((Shortcut)lsCommand.SelectedItem).Method
                                    select a).ToArray();

            cbShortcut.DataSource = shortcuts;
            cbShortcut.DisplayMember = "KeyCaption";

            lblCommand1.Text = lblCommand2.Text = lsCommand.SelectedItem.ToString();

            if (cbShortcut.Items.Count > 0)
                cbShortcut.SelectedIndex = 0;
        }

        private void btRemove_Click(object sender, EventArgs e)
        {
            if (cbShortcut.SelectedItem != null)
            {
                ShortcutManager.Remove((Shortcut)cbShortcut.SelectedItem);
            }

            lsCommand_SelectedIndexChanged(null, null);
        }

        private void btAssign_Click(object sender, EventArgs e)
        {
            ShortcutManager.Assign((Shortcut)lsCommand.SelectedItem, cbEscope.Text, txtShortcut.Key, txtShortcut.Modifiers);
            lsCommand_SelectedIndexChanged(null, null);
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtShortcut_TextChanged(object sender, EventArgs e)
        {
            btAssign.Enabled = txtShortcut.IsValid;

            if (txtShortcut.IsValid)
            {
                Shortcut shortcut = (from a in ShortcutManager.Shorcuts
                                     where a.Key == txtShortcut.Key && a.Modifiers == txtShortcut.Modifiers
                                     select a).FirstOrDefault();

                if (shortcut != null)
                    txtCurrently.Text = shortcut.CommandCaption;
                else
                    txtCurrently.Text = Message_en.ShortcutNoConflictsDetected;
            }
            else
            {
                txtCurrently.Text = string.Empty;
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCommand_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Down ||
                (e.KeyCode == Keys.Right && txtCommand.SelectionStart == txtCommand.Text.Length))
                && lsCommand.Items.Count > 0)
            {
                e.Handled = true;

                lsCommand.Focus();

                lsCommand.SelectedIndex = 0;

            }
        }

        private void lsCommand_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Up || e.KeyCode == Keys.Left) && lsCommand.SelectedItem != null && lsCommand.SelectedIndex == 0) 
            {
                e.Handled = true;

                txtCommand.Focus();

                txtCommand.SelectAll();
            }
        }

        private int lastTextLength = 0;

        private void txtCommand_TextChanged(object sender, EventArgs e)
        {
            if ((lastTextLength == 0 && txtCommand.Text.Length == 0) ||
                (lastTextLength > 0 && txtCommand.Text.Length > 0))
            {
                lastTextLength = txtCommand.Text.Length;
            }
            else if ((lastTextLength == 0 && txtCommand.Text.Length > 0) || (lastTextLength > 0 && txtCommand.Text.Length == 0))
            {
                DelayedExecutionManager.AbortTask(this);

                DelayedExecutionManager.AddTask(this, 500, delegate()
                {
                    if (this != null && !this.IsDisposed && this.IsHandleCreated)
                        this.Invoke((MethodInvoker)delegate
                        {
                            if (!this.IsDisposed && this.IsHandleCreated)
                                lblType.Visible = txtCommand.Text.Length == 0;
                        });

                });

                
            }
           
            lsCommand.DataSource = (from a in ShortcutManager.Shorcuts
                                    group a by a.Method into g
                                    orderby g.First().CommandCaption
                                    select g.First()).ToArray();

            lastTextLength = txtCommand.Text.Length;
        }
    }
}
