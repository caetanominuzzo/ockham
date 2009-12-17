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


namespace primeira.Editor.MetaEditor
{
    public partial class MetaEditor : EditorBase
    {
        public MetaEditor(string filename, DocumentBase data)
            : base(filename, data, typeof(MetaEditorDocument))
        {
            InitializeComponent();

            txtName.DataBindings.Add("Text", this.Document, "EditorName");
            txtDescription.DataBindings.Add("Text", this.Document, "EditorDescription");
            txtFilename.DataBindings.Add("Text", this.Document, "DefaultFileName");
            txtExtension.DataBindings.Add("Text", this.Document, "DefaultFileExtension");
            txtGuid.DataBindings.Add("Text", this.Document, "EditorGuid");
            txtIcon.DataBindings.Add("Text", this.Document, "Icon");

        }

        private void btnNewGuid_Click(object sender, EventArgs e)
        {
            txtGuid.Text = Guid.NewGuid().ToString();
        }

        private void btnBrowseIcon_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog o = new OpenFileDialog())
            {
                o.Filter = "Image Files|*.jpg;*.gif;*.bmp;*.png;*.jpeg|All Files|*.*";
                o.FilterIndex = 1;

                if (o.ShowDialog() == DialogResult.OK)
                {
                    txtIcon.Text = o.FileName;
                    pctIcon.Image = Image.FromFile(o.FileName);
                }
            }
        }

        private void btnCreateEditor_Click(object sender, EventArgs e)
        {
            CompilerParameters param = new CompilerParameters();
            param.CompilerOptions = @"out:d:\";
            CSharpCodeProvider code = new CSharpCodeProvider(new Dictionary<String, String> { { "CompilerVersion", "v3.5" } });
            code.CompileAssemblyFromFile(param, new string[] { @"D:\Desenv\Ockham\branches\1\primeira.Editor.MetaEditor\primeira.Editor.MetaEditor.csproj" });

        }
    }
}
