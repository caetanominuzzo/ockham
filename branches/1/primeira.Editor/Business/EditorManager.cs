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
                //Open a new
                res = EditorManager.CreateEditor(fileName);
            }
            catch (TargetInvocationException ex)
            {
                LogFileManager.Log(
                    string.Format(
                        Message_us.EditorCreationError,
                        fileName),
                        "\n",
                        ex.InnerException.ToString());
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
                        Message_us.EditorCreationError,
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
        public static IEditor LoadEditor(MemberInfo documentType)
        {
            DocumentDefinitionAttribute dd = DocumentManager.GetDocumentDefinition(documentType);

            if (dd == null)
            {
                MessageManager.Send(
                    MessageSeverity.Error,
                    string.Format(
                        Message_us.DocumentMissingDocumentDefinitionAttribute,
                        documentType.Name));

                return null;
            }

            if ((dd.Options & DocumentDefinitionOptions.OpenFromTypeDefaultName) > 0)
            {
                return LoadEditor(dd.DefaultFileName + dd.DefaultFileExtension);
            }
            else
            {
                MessageManager.Send(
                    MessageSeverity.Error,
                    string.Format(
                        Message_us.DocumentMissingOpenFromTypeDefaultName,
                        documentType.Name));

                return null;
            }
        }

        private static IEditor CreateEditor(string fileName)
        {
            IEditor res = null;

            DocumentDefinitionAttribute def = DocumentManager.GetDocumentDefinition(fileName);

            res = (IEditor)def.DefaultEditor.GetConstructor(_defaultEditorCtor).Invoke(new object[1] { fileName });

            return res;
        }

        private static Type[] _defaultEditorCtor = new Type[1] { typeof(string) };

        #region Get Editor Data

        public static Image GetManifestResourceFileIcon(string extension)
        {
            return GetManifestResourceFileIcon(GetEditorTypeByFileExtension(extension));
        }

        public static Image GetManifestResourceFileIcon(Type type)
        {
            Stream stream = type.Assembly.GetManifestResourceStream(type.Namespace + ".Resources.File.ico");

            if (stream == null)
                return null;

            return Image.FromStream(stream);
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
