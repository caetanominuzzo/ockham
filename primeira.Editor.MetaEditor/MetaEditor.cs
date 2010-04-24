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
    [EditorDefinition(DocumentType=typeof(MetaEditorDocument))]
    [AddonDefinition(AddonOptions.WaitEditorContainer | AddonOptions.UserAddon)]
    public partial class MetaEditor : EditorBase, IShorcutEscopeProvider
    {
        public MetaEditor(string fileName)
            : base(fileName)
        {
            InitializeComponent();

            txtName.DataBindings.Add("Text", this.Document, "EditorName", false, DataSourceUpdateMode.OnPropertyChanged);
            txtDescription.DataBindings.Add("Text", this.Document, "EditorDescription");
            txtFilename.DataBindings.Add("Text", this.Document, "DefaultFileName");
            txtExtension.DataBindings.Add("Text", this.Document, "DefaultFileExtension");
            UndoRedoFramework.UndoRedoManager.CommandDone += new EventHandler<UndoRedoFramework.CommandDoneEventArgs>(UndoRedoManager_CommandDone);

            ShortcutManager.LoadFromForm(this);

        }

        void UndoRedoManager_CommandDone(object sender, UndoRedoFramework.CommandDoneEventArgs e)
        {
            if(txtName.DataBindings.Count > 0)
                txtName.DataBindings[0].ReadValue();
        }

        [ShortcutVisibility("Nome1", "", BasicEscopes.Global, Keys.Z, KeyModifiers.Control)]
        public void Undo()
        {
            ((MetaEditorDocument)EditorContainerManager.GetOpenEditor("C:\\Users\\caetano\\Documents\\Editor 11.metaeditor").Document).EditorName = "asd";
            

          //  ((MetaEditor)EditorContainerManager.GetOpenEditorByFilename("C:\\Users\\caetano\\Documents\\Editor 11.metaeditor")).Refresh();

            //foreach (BindingManagerBase ctl in (((MetaEditor)EditorContainerManager.GetOpenEditorByFilename("C:\\Users\\caetano\\Documents\\Editor 11.metaeditor")).BindingContext))
            //{
            //    if (ctl is CurrencyManager)
            //    {
            //        CurrencyManager cm = (CurrencyManager)ctl;
            //        cm.Refresh();
            //    }
            //}




            //((primeira.Editor.MetaEditorDocument)(((primeira.Editor.MetaEditor)((primeira.Editor.TabControlManager)(primeira.Editor.EditorContainerManager._))._activeEditor).Document)).EditorName = "asd";

        }

        [AddonInitialize()]
        public static void AddonInitialize()
        {
            EditorManager.RegisterEditor(typeof(MetaEditor));
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

        private void txtName_Enter(object sender, EventArgs e)
        {
        }

        private void txtName_Leave(object sender, EventArgs e)
        {
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            
        }

        #region IShorcutEscopeProvider Members

        public bool IsAtiveByEscope(string escope)
        {
            return true;
        }

        #endregion
    }
}
