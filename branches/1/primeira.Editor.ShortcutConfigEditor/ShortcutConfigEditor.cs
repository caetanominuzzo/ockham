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
    public partial class ShortcutConfigEditor : EditorBase
    {
        
        public ShortcutConfigEditor(string filename, DocumentBase data)
            : base(filename, data, typeof(ShortcutConfigDocument))
        {
            InitializeComponent();
        }

    }
}
