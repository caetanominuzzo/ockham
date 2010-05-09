using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Drawing;

namespace primeira.Editor
{
    public delegate void SelectedDelegate(IEditor sender);

    public static class EditorManager
    {
        private static EditorManagerDocument cache = EditorManagerDocument.GetInstance();

        /// <summary>
        /// Registers a System.Type as an editor.
        /// </summary>
        /// <param name="type">The System.Type to register</param>
        public static EditorDefinition RegisterEditor(Type type)
        {
            EditorDefinition editor = cache.Editors.AsParallel().Where(x => x.EditorType == type).FirstOrDefault();

            if(editor == null)
            {
                editor = new EditorDefinition();
                
                editor.EditorType = type;

                EditorDocumentAttribute[] dd = (EditorDocumentAttribute[])type.GetCustomAttributes(typeof(EditorDocumentAttribute), false);

                List<DocumentDefinition> docs = new List<DocumentDefinition>();

                DocumentDefinition doc = null;

                foreach (EditorDocumentAttribute d in dd)
                {
                    doc = DocumentManager.RegisterDocument(d.DocumentType); 

                    docs.Add(doc);
                }

                editor.Documents = docs.ToArray();

                cache.AddEditor(editor);

                ShortcutManager.LoadFromType(type);
            }

            return editor;
        }

        /// <summary>
        /// Load the specified file in the registered editor.
        /// If the file is already open its tab is selected.
        /// </summary>
        /// <param name="fileName">The file to open.</param>
        /// <returns>The editor with file loaded.</returns>
        public static IEditor LoadEditor(string fileName)
        {
            //Verify if the file is already open
            IEditor res = EditorContainerManager.GetOpenEditor(fileName);
            if (res != null)
            {
                EditorContainerManager.AddEditor(res);
                return res;
            }

            try
            {
                res = EditorManager.CreateEditor(fileName);
            }
            catch (TargetInvocationException ex)
            {
                LogFileManager.Log(
                    string.Format(
                        Message_en.EditorCreationError,
                        fileName),
                        Environment.NewLine,
                        ex.ToString());

                throw;
            }

            if (res != null)
            {
                EditorContainerManager.AddEditor(res);
                return res;
            }
            else
            {
                MessageManager.Send(
                    MessageSeverity.Alert,
                    string.Format(
                        Message_en.EditorCreationError,
                        fileName));

                return null;
            }
        }

        /// <summary>
        /// Load the specified document type with default fileName.
        /// The document must has OpenFromTypeByDefaultName option.
        /// </summary>
        /// <param name="documentType">The CLR type of the document to open.</param>
        /// <returns>The editor with the default file loaded.</returns>
        public static IEditor LoadEditor(DocumentDefinition document)
        {
            if (document.Attributes.Options.HasFlag(DocumentDefinitionOptions.OpenFromTypeDefaultName))
                return LoadEditor(document.Attributes.DefaultFileName + document.Attributes.DefaultFileExtension);
            
            throw new InvalidOperationException(Message_en.DocumentMissingOpenFromTypeDefaultName);
        }

        private static IEditor CreateEditor(string fileName)
        {
            IEditor res = null;

            Type editorType;

            DocumentDefinition doc = DocumentManager.GetDocumentDefinition(fileName);

            editorType = (from a in EditorManager.Editors.AsParallel()
                            where a.Documents.Contains(doc)
                            select a.EditorType).FirstOrDefault();

            if (editorType == null)
                throw new InvalidOperationException(
                    string.Format(Message_en.ThereIsNoEditorForType, fileName));
            

            res = (IEditor)editorType.GetConstructor(_defaultEditorCtor).Invoke(new object[1] { fileName });

            return res;

        }

        private static Type[] _defaultEditorCtor = new Type[1] { typeof(string) };

        #region Editors

        public static EditorDefinition[] Editors
        {
            get
            {
                return cache.Editors;
            }
        }

        

        #endregion
    }
}
