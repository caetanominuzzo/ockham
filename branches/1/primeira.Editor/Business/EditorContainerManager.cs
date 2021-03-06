﻿using System;
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
            if(editor.HasOption(DocumentHeaderOptions.ShowInRecents))
            {
                if (FileManager.Recent != null)
                    FileManager.Recent.AddRecent(editor.FileName);
            }

            _editorContainer.LoadEditor(editor);
        }

        public static IEditor GetOpenEditor(string fileName)
        {
            return _editorContainer == null ? null : _editorContainer.GetOpenEditor(fileName);
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

        public static bool IsActive(IEditor editor)
        {
            return _editorContainer.IsActive(editor);
        }

        public static System.Windows.Forms.Form MainMForm
        {
            get { return _editorContainer.MainForm; }
        }
    }
}
