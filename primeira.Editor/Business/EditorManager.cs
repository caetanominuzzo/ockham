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
        #region Register & Load

        private static List<Type> _registeredEditors;

        public static void RegisterEditor(Type type)
        {
            if (_registeredEditors == null)
                _registeredEditors = new List<Type>();

            if (!_registeredEditors.Contains(type))
            {
                _registeredEditors.Add(type);

                EditorDefinitionAttribute[] dd = (EditorDefinitionAttribute[])type.GetCustomAttributes(typeof(EditorDefinitionAttribute), false);

                foreach (EditorDefinitionAttribute d in dd)
                    DocumentManager.RegisterDocument(d.DocumentType);
            }
        }

        public static IEditor CreateEditorByFilename(string filename)
        {
            IEditor res = null;
            DocumentBase doc = null;

            FileInfo f = new FileInfo(filename);

            if (!f.Exists)
            {
                f.Create();
                f.Refresh();
            }

            if (f.Length == 0)
            {
                DocumentDefinitionAttribute dd = DocumentManager.GetDocumentDefinitionByFilename(filename);

                if (dd == null)
                {
                    MessageManager.Alert("There is no editor for that file type.");
                    return null;
                }

                res = (IEditor)dd.DefaultEditor.GetConstructor(_defaultEditorCtor).Invoke(new object[2] { filename, doc });
            }
            else
            {
                doc = DocumentManager.ToObject(filename);

                if (doc != null)
                    res = (IEditor)DocumentManager.GetDocumentDefinitionByClrType(doc.GetType()).DefaultEditor.GetConstructor(_defaultEditorCtor).Invoke(new object[2] { filename, doc });
            }

            return res;
        }

        private static Type[] _defaultEditorCtor = new Type[2] { typeof(string), typeof(DocumentBase) };

        /// <summary>
        /// Load the specified file in the registered editor.
        /// If the file is already open its tab is selected.
        /// </summary>
        /// <param name="filename">The file to open.</param>
        /// <returns>The editor with file loaded.</returns>
        public static IEditor LoadEditor(string filename)
        {
            try
            {
                //Verify if the file is already open
                IEditor res = EditorContainerManager.GetOpenEditorByFilename(filename);

                if (res != null)
                {
                    EditorContainerManager.AddEditor(res);
                    return res;
                }

                //Load a new editor
                res = EditorManager.CreateEditorByFilename(filename);

                if (res != null)
                    EditorContainerManager.AddEditor(res);

                return res;
            }
            catch (Exception ex)
            {
                MessageManager.Alert("File ", filename, " cannot be open.\n", ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Load the specified document type with default filename.
        /// The document must has OpenFromTypeByDefaultName option.
        /// </summary>
        /// <param name="documentType">The CLR type of the document to open.</param>
        /// <returns>The editor with the default file loaded.</returns>
        public static IEditor LoadEditor(Type documentType)
        {
            DocumentDefinitionAttribute dd = DocumentManager.GetDocumentDefinitionByClrType(documentType);

            if ((dd.Options & DocumentDefinitionOptions.OpenFromTypeDefaultName) == DocumentDefinitionOptions.OpenFromTypeDefaultName)
            {
                return LoadEditor(dd.DefaultFileName + dd.DefaultFileExtension);
            }

            return null;
        }
    

        #endregion

        #region Get Editor Data

        public static Image GetManifestResourceFileIcon(string extension)
        {
            return GetManifestResourceFileIcon(GetEditorTypeByFileExtension(extension));
        }

        public static Image GetManifestResourceFileIcon(Type type)
        {
            try
            {
                Stream stream = type.Assembly.GetManifestResourceStream(type.Namespace + ".Resources.File.ico");

                if (stream == null)
                    return null;

                return Image.FromStream(stream);
            }

            catch { }

            return null;
        }

        public static Type GetEditorTypeByFileExtension(string extension)
        {
            var x = (from c in _registeredEditors
                     where ((EditorDefinitionAttribute[])c.GetCustomAttributes(typeof(EditorDefinitionAttribute), false)).Count<EditorDefinitionAttribute>(z =>
                         DocumentManager.GetDocumentDefinitionByClrType(z.DocumentType).DefaultFileExtension == extension) > 0
                     select c).First();

            
            return (Type)x;
        }

        #endregion
    }
}
