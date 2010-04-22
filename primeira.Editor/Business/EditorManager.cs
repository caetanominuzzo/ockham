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
        private static List<Type> _knownEditors;

        #region Register & Load

        public static void RegisterEditor(Type type)
        {
            if (_knownEditors == null)
                _knownEditors = new List<Type>();

            if (!_knownEditors.Contains(type))
            {
                _knownEditors.Add(type);

                EditorDefinitionAttribute[] dd = (EditorDefinitionAttribute[])type.GetCustomAttributes(typeof(EditorDefinitionAttribute), false);

                foreach (EditorDefinitionAttribute d in dd)
                    DocumentManager.RegisterDocument(d.DocumentType);
            }
        }

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
                Type documentType = DocumentManager.GetDocumentType(filename);

                return LoadEditor(documentType, filename);

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
            DocumentDefinitionAttribute dd = DocumentManager.GetDocumentDefinition(documentType);

            if ((dd.Options & DocumentDefinitionOptions.OpenFromTypeDefaultName) > 0)
            {
                return LoadEditor(documentType, dd.DefaultFileName + dd.DefaultFileExtension);
            }

            return null;
        }

        private static IEditor CreateEditorByFilename(Type documentType, string filename)
        {
            IEditor res = null;

            DocumentDefinitionAttribute def = DocumentManager.GetDocumentDefinition(documentType);

            if (def == null)
            {
                throw new Exception("There is no editor for that file type.");
            }

            DocumentBase doc = DocumentManager.LoadDocument(documentType, filename);

            res = (IEditor)def.DefaultEditor.GetConstructor(_defaultEditorCtor).Invoke(new object[2] { filename, doc });

            return res;
        }

        private static Type[] _defaultEditorCtor = new Type[2] { typeof(string), typeof(DocumentBase) };
        
        private static IEditor LoadEditor(Type documentType, string filename)
        {
            //Verify if the file is already open
            IEditor res = EditorContainerManager.GetOpenEditorByFilename(filename);

            if (res != null)
            {
                EditorContainerManager.AddEditor(res);
                return res;
            }

            res = EditorManager.CreateEditorByFilename(documentType, filename);

            if (res != null)
                EditorContainerManager.AddEditor(res);

            return res;
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
            var x = (from c in _knownEditors
                     where ((EditorDefinitionAttribute[])c.GetCustomAttributes(typeof(EditorDefinitionAttribute), false)).Count<EditorDefinitionAttribute>(z =>
                         DocumentManager.GetDocumentDefinition(z.DocumentType).DefaultFileExtension == extension) > 0
                     select c).First();

            
            return (Type)x;
        }

        #endregion
    }
}
