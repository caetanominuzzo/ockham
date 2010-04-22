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
    [EditorDefinition(DocumentType=typeof(ShortcutConfigDocument))]
    public partial class ShortcutConfigEditor : EditorBase
    {
        public ShortcutConfigEditor(string filename, DocumentBase data)
            : base(filename, data, typeof(ShortcutConfigDocument))
        {
            InitializeComponent();
        }

        [AddonInitialize()]
        public static void RegisterEditor()
        {
            EditorManager.RegisterEditor(typeof(ShortcutConfigEditor));
            ShortcutManager.ParentEscopeProvider = Application.OpenForms[0];
            ShortcutManager.SetShortcutConfigDocumentType(typeof(ShortcutConfigDocument));
        }

        private void ShortcutManagerEditor_Load(object sender, EventArgs e)
        {
            foreach (ShortcutCommand p in ShortcutManager.Commands)
            {
                if (!lsCommand.Items.Contains(p.Name))
                    lsCommand.Items.Add(p.Name);
            }

            foreach (ShortcutCommand p in ShortcutManager.Commands)
            {
                if (!cbEscope.Items.Contains(p.Escope))
                    cbEscope.Items.Add(p.Escope);
            }
        }

        private void lsCommand_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbShortcut.Items.Clear();
            foreach (ShortcutCommand p in ShortcutManager.Commands)
            {
                if (p.Name == lsCommand.SelectedItem.ToString())
                {
                    lbDescription.Text = p.Description;
                    foreach (Shortcut pp in ShortcutManager.Shorcuts)
                    {
                        if (pp.Command == p)
                            cbShortcut.Items.Add(pp.ToString());
                    }
                }
            }

            if (cbShortcut.Items.Count > 0)
                cbShortcut.SelectedIndex = 0;
        }

        private void btRemove_Click(object sender, EventArgs e)
        {
            if (cbShortcut.SelectedItem != null)
                foreach (Shortcut p in ShortcutManager.Shorcuts)
                {
                    if (p.ToString() == cbShortcut.SelectedItem.ToString())
                    {
                        ShortcutManager.Unassign(p);
                        break;
                    }
                }

            lsCommand_SelectedIndexChanged(null, null);
        }

        private void btAssign_Click(object sender, EventArgs e)
        {
            ShortcutManager.Assign(lsCommand.SelectedItem.ToString(), cbEscope.Text, txtShortcut.Key, txtShortcut.Modifiers);
            lsCommand_SelectedIndexChanged(null, null);
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void txtShortcut_TextChanged(object sender, EventArgs e)
        {
            cbCurrently.Items.Clear();
            foreach (Shortcut p in ShortcutManager.Shorcuts)
            {
                if (p.Key == txtShortcut.Key && p.KeyModifier == txtShortcut.Modifiers && p.Escope == cbEscope.Text)
                {
                    cbCurrently.Items.Add(p.Command.Name);
                }
            }

            if (cbCurrently.Items.Count > 0)
                cbCurrently.SelectedIndex = 0;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCommand_KeyPress(object sender, KeyPressEventArgs e)
        {
            lsCommand.Items.Clear();

            foreach (ShortcutCommand p in ShortcutManager.Commands)
            {
                if (!lsCommand.Items.Contains(p.Name) && p.Name.ToUpper().Contains(txtCommand.Text.ToUpper()))
                    lsCommand.Items.Add(p.Name);
            }

        }


    }
}
