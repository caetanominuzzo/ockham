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
        private static List<Type> _knownEditors = new List<Type>();

        /// <summary>
        /// Registers a System.Type as an editor.
        /// </summary>
        /// <param name="type">The System.Type to register</param>
        public static void RegisterEditor(Type type)
        {
            if (!_knownEditors.Contains(type))
            {
                _knownEditors.Add(type);

                ShortcutManager.LoadFromType(type);

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
        public static IEditor LoadEditor(MemberInfo documentType)
        {
            DocumentDefinitionAttribute dd = DocumentManager.GetDocumentDefinition(documentType);

            if (dd == null)
            {
                LogFileManager.Log(string.Format(
                        Message_en.DocumentMissingDocumentDefinitionAttribute,
                        documentType.Name));

                throw new InvalidOperationException(
                    string.Format(
                        Message_en.DocumentMissingDocumentDefinitionAttribute,
                        documentType.Name));
            }

            if (dd.Options.HasFlag(DocumentDefinitionOptions.OpenFromTypeDefaultName))
                return LoadEditor(dd.DefaultFileName + dd.DefaultFileExtension);
            
            throw new InvalidOperationException(Message_en.DocumentMissingOpenFromTypeDefaultName);
        }

        private static IEditor CreateEditor(string fileName)
        {
            IEditor res = null;

            Type editorType;

            DocumentDefinitionAttribute def = DocumentManager.GetDocumentDefinition(fileName);

            if (def.DefaultEditor != null)
            {
                editorType = def.DefaultEditor;
            }
            else
            {
                Type documentType = DocumentManager.GetDocumentType(fileName);

                editorType = EditorManager.GetEditorByDocumentType(documentType);

                if (editorType == null)
                    throw new InvalidOperationException(
                        string.Format(Message_en.ThereIsNoEditorForType, fileName));
            }

            res = (IEditor)editorType.GetConstructor(_defaultEditorCtor).Invoke(new object[1] { fileName });

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

        public static Type GetDocumentTypeByEditorType(Type editorType)
        {
            return ((EditorDefinitionAttribute)editorType.GetCustomAttributes(typeof(EditorDefinitionAttribute), false)[0]).DocumentType;
        }

        public static Type GetEditorByDocumentType(Type documentType)
        {
            return (from x in _knownEditors where ((EditorDefinitionAttribute[])x.GetCustomAttributes(typeof(EditorDefinitionAttribute), false))[0].DocumentType.Equals(documentType)
                        select x).First();
        }

        #endregion
    }
}
