using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using primeira.Editor;
using primeira.Editor.Components;
using Microsoft.CSharp;
using System.CodeDom.Compiler;


namespace primeira.Editor
{
    [EditorDocument(DocumentType=typeof(TextEditorDocument))]
    [AddonDefinition(AddonOptions.UserAddon)]
    public partial class TextEditor : EditorBase, IShorcutEscopeProvider
    {
        public TextEditor(string fileName)
            : base(fileName)
        {
            InitializeComponent();
        }

        private void TextEditor_Load(object sender, EventArgs e)
        {
            txtMain.Focus();

            List<TextEditorDocument> doc = new TextEditorDocument[] { (TextEditorDocument)this.Document }.ToList<TextEditorDocument>();

            txtMain.DataBindings.Add("SelectionStart", doc, "SelectionStart");
            txtMain.DataBindings.Add("SelectionLength", doc, "SelectionLength");
            txtMain.DataBindings.Add("Text", doc, "Content");

            foreach (Binding b in txtMain.DataBindings)
                b.ReadValue();

            this.txtMain.TextChanged += new System.EventHandler(this.txtMain_TextChanged);
            this.txtMain.Click += new System.EventHandler(this.txtMain_TextChanged);
            this.txtMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.txtMain_TextChanged);
            this.txtMain.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMain_TextChanged);

        }

        [AddonInitialize()]
        public static void AddonInitialize()
        {
            EditorManager.RegisterEditor(typeof(TextEditor));
        }

        private void txtMain_TextChanged(object sender, EventArgs e)
        {
            foreach (Binding b in txtMain.DataBindings)
                b.WriteValue();

            this.Changed();
        }
    }
}
