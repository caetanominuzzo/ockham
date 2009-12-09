using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using primeira.Editor.Business;
using primeira.Editor.Components;


namespace primeira.Editor.MetaEditor
{
    public partial class MetaEditor : EditorBase
    {
        public MetaEditor(string filename, DocumentBase data)
            : base(filename, data, typeof(MetaEditorDocument))
        {
            InitializeComponent();
        }
    }
}
