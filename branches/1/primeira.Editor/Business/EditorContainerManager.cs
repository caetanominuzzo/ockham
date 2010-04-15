using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace primeira.Editor
{
    public class EditorContainerManager
    {
        private static IEditorContainer _editorContainer;

        public static void AddEditor(IEditor editor)
        {
            if(editor.HasOption(DocumentDefinitionOptions.ShowInRecents))
            {
                if (FileManager.Recent != null)
                    FileManager.Recent.AddRecent(editor.Filename);
            }

            _editorContainer.LoadEditor(editor);
        }

        public static IEditor GetOpenEditorByFilename(string filename)
        {
            return _editorContainer == null ? null : _editorContainer.GetOpenEditorByFilename(filename);
        }

        public static void CloseEditor(IEditor editor)
        {
            _editorContainer.CloseEditor(editor);
        }

        internal static bool IsInitialized()
        {
            return _editorContainer != null;
        }

        public static void SetEditorContainer(IEditorContainer container)
        {
            _editorContainer = container;
        }

    }
}
