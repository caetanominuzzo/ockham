using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace primeira.Editor
{
    public interface IEditorContainer
    {
        void LoadEditor(IEditor editor);

        bool CloseEditor(IEditor editor);

        IEditor GetOpenEditor(string fileName);

        bool IsActive(IEditor editor);

        System.Windows.Forms.Form MainForm { get; }
    }
}
