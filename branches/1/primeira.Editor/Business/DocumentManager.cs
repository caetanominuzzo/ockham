using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace primeira.Editor
{
    public static class DocumentManager
    {
        #region Fields

        private static string _baseDir = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        private static List<DocumentDefinition> _definitions = new List<DocumentDefinition>();

        #endregion

        #region Documents & Definitions

        public static DocumentDefinition[] Definitions
        {
            get
            {
                return _definitions.ToArray();
            }
        }

        /// <summary>
        /// Gets the System.Type of the document of a given file.
        /// </summary>
        /// <param name="fileName">Path to a file</param>
        /// <returns>The System.Type of the document of a given file</returns>
        public static DocumentDefinition GetDocumentDefinition(string fileName)
        {
            DocumentDefinition[] document = GetDocumentDefinitionByFileExtension(Path.GetExtension(fileName));

            if (document.Length == 1)
                return document[0];
            else
            {
                throw new NotImplementedException();

                //TODO: Read document definition from file.
            }
        }

        /// <summary>
        /// Gets the System.Type of the document of a given file extension.
        /// </summary>
        /// <param name="extension">File extesion</param>
        /// <returns>The System.Type of the document of a given file</returns>
        public static DocumentDefinition[] GetDocumentDefinitionByFileExtension(string extension)
        {
            DocumentDefinition[] document = (from a in Definitions
                                   where a.Attributes.DefaultFileExtension.Equals(extension, StringComparison.InvariantCultureIgnoreCase)
                                   select a).ToArray();

            if (document.Length > 0)
                return document;

            throw new InvalidOperationException(
                string.Format(Message_en.ThereIsNoEditorForType, "*" + extension));
        }

        /// <summary>
        /// Registers a System.Type as a document.
        /// </summary>
        /// <param name="documentType"></param>
        /// <returns></returns>
        public static DocumentDefinition RegisterDocument(Type documentType)
        {
            DocumentDefinition doc = GetDocumentDefinition(documentType);

            if (doc == null)
            {
                doc = DocumentManager.GetDocumentDefinitionFromReflection(documentType);

                _definitions.Add(doc);
            }

            return doc;
        }

        private static DocumentDefinition GetDocumentDefinitionFromReflection(Type documentType)
        {
            DocumentDefinition doc = new DocumentDefinition();

            doc.DocumentType = documentType;

            object[] attribs = doc.DocumentType.GetCustomAttributes(typeof(DocumentDefinitionAttribute), false);

            if (attribs.Length != 0)
                doc.Attributes = (DocumentDefinitionAttribute)attribs[0];
            else
                throw new InvalidOperationException(string.Format(Message_en.DocumentMissingDocumentDefinitionAttribute,
                    doc.DocumentType.Name));

            doc.Icon = DocumentManager.GetIcon(documentType, doc.Attributes.IconResourceFile);

            return doc;
        }

        /// <summary>
        /// Gets the DocumentDefinitionAttribute of a given document System.Type.
        /// </summary>
        /// <param name="documentType">The type of a document</param>
        /// <returns>The DocumentDefinitionAttribute of the given System.Type</returns>
        private static DocumentDefinition GetDocumentDefinition(Type documentType)
        {
            DocumentDefinition doc = (from a in Definitions.AsParallel()
                                      where a.DocumentType == documentType
                                      select a).First();
            return doc;
        }

        private static Image GetIcon(Type type, string iconResourceFile)
        {
            if (iconResourceFile == null || iconResourceFile.Length == 0)
                iconResourceFile = "File.ico";

            Stream stream = type.Assembly.GetManifestResourceStream(
                string.Concat(type.Namespace, ".Resources.", iconResourceFile));

            if (stream == null)
                return null;

            return Image.FromStream(stream);
        }

        #endregion

        #region Dialogs

        /// <summary>
        /// Renders a dialog filter string with all registered document types.
        /// </summary>
        /// <returns></returns>
        public static string RenderDialogFilterString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (DocumentDefinition doc in Definitions)
            {
                if (doc.Attributes.Options.HasFlag(DocumentDefinitionOptions.ShowIQuickLauchnOpen))
                    sb.Append(string.Format("{0} (*{1})|*{1}|", doc.Attributes.Name, doc.Attributes.DefaultFileExtension));
            }

            sb.Append("All files (*.*)|*.*");

            return sb.ToString();
        }

        /// <summary>
        /// Gets the dialog filter string index of a given file type.
        /// </summary>
        /// <param name="FileVersion"></param>
        /// <returns></returns>
        public static int GetDialogFilterIndex(DocumentDefinition FileVersion)
        {
            int i = 0;
            foreach (DocumentDefinition doc in Definitions)
            {
                if (doc.Attributes.Options.HasFlag(DocumentDefinitionOptions.ShowIQuickLauchnOpen))
                    continue;
                else i++;

                if (doc == FileVersion)
                    return i;
            }

            return 1;
        }

        #endregion

        #region Serialization

        /// <summary>
        /// Deserializes a given file in an specified document type.
        /// </summary>
        /// <param name="fileName">A file path</param>
        /// <param name="type">The type of the document</param>
        /// <returns></returns>
        internal static DocumentBase ToObject(string fileName, Type type)
        {
            FileInfo f = new FileInfo(fileName);

            DocumentBase doc = null;

            if (!f.Exists || f.Length == 0)
            {
                doc = (DocumentBase)type.GetConstructor(System.Type.EmptyTypes).Invoke(System.Type.EmptyTypes);
            }
            else
            {
                Stream sm = File.OpenRead(fileName);

                Type[] knownTypes = (from a in Definitions
                                     select a.DocumentType).ToArray();

                Array.Resize(ref knownTypes, knownTypes.Length + 1);

                knownTypes[knownTypes.Length - 1] = type;

                DataContractSerializer ser = new DataContractSerializer(typeof(DocumentBase),
                    knownTypes,
                    10000000, false, true, null);

                doc = (DocumentBase)ser.ReadObject(sm);

                UndoRedoFramework.UndoRedoManager.FlushHistory();

                sm.Close();
            }

            return doc;
        }

        /// <summary>
        /// Serializes a given document in its default file name.
        /// </summary>
        /// <param name="document">The document to serialize</param>
        internal static void ToXml(DocumentBase document)
        {
            DocumentDefinition doc = (from a in Definitions.AsParallel()
                                               where a.DocumentType == document.GetType()
                                               select a).First();

            if (doc.Attributes.Options.HasFlag(DocumentDefinitionOptions.OpenFromTypeDefaultName))
            {
                string fileName = doc.Attributes.DefaultFileName + doc.Attributes.DefaultFileExtension;

                DocumentManager.ToXml(document, fileName);
            }
            else
                throw new InvalidOperationException(
                    string.Format(
                        Message_en.DocumentMissingOpenFromTypeDefaultName,
                        document.GetType().Name));

        }

        /// <summary>
        /// Serializes a given document in an specified file.
        /// </summary>
        /// <param name="document">The document to serialize</param>
        /// <param name="fileName">The file to serialize</param>
        internal static void ToXml(DocumentBase document, string fileName)
        {

            Stream sm = File.Create(fileName);

            Type[] knownTypes = (from a in Definitions
                                 select a.DocumentType).ToArray();

            Array.Resize(ref knownTypes, 1);

            knownTypes[knownTypes.Length - 1] = document.GetType();

            DataContractSerializer ser = new DataContractSerializer(typeof(DocumentBase),
                knownTypes,
                10000000, false, true, null);
            ser.WriteObject(sm, document);

            sm.Close();
        }

        #endregion

        #region New, Open & Save Document

        /// <summary>
        /// Loads a document by its default name.
        /// </summary>
        /// <param name="documentType">The System.Type of the document to open</param>
        /// <returns>A loaded document</returns>
        public static DocumentBase LoadDocument(DocumentDefinition document)
        {
            if (document.Attributes.Options.HasFlag(DocumentDefinitionOptions.OpenFromTypeDefaultName))
            {
                return DocumentManager.LoadDocument(document, document.Attributes.DefaultFileName + document.Attributes.DefaultFileExtension);
            }
            else
            {
                throw new InvalidOperationException(
                    string.Format(
                        Message_en.DocumentMissingOpenFromTypeDefaultName,
                         document.Attributes.Name));
            }
        }

        /// <summary>
        /// Loads a document by its default name.
        /// </summary>
        /// <param name="documentType">The System.Type of the document to open</param>
        /// <param name="fileName">The file to load</param>
        /// <returns>A loaded document</returns>
        public static DocumentBase LoadDocument(DocumentDefinition document, string fileName)
        {
            if (document.Attributes.Options.HasFlag(DocumentDefinitionOptions.CustomSerializationRead))
            {
                MethodInfo m = document.DocumentType.GetMethod("ToObject", new Type[] { typeof(string) });

                if (m == null)
                    throw new InvalidOperationException(
                        Message_en.DocumentCustomSerializationReadMustHaveToObjectMethod);

                try
                {
                    return (DocumentBase)m.Invoke(null, new object[] { fileName });
                }
                catch (Exception ex)
                {
                    LogFileManager.Log(
                        string.Format(
                            Message_en.DocumentCustomSerializatinoError, document.Attributes.Name), Environment.NewLine, ex.ToString());

                    throw new InvalidOperationException(
                        string.Format(
                            Message_en.DocumentCustomSerializatinoError, document.Attributes.Name), ex);
                }
            }

            return DocumentManager.ToObject(fileName, document.DocumentType);
        }

        /// <summary>
        /// Saves a document by its default name.
        /// </summary>
        /// <param name="documentType">The System.Type of the document to open</param>
        public static void SaveDocument(DocumentBase document)
        {
            if (document.Definition.Attributes.Options.HasFlag(DocumentDefinitionOptions.OpenFromTypeDefaultName))
            {
                DocumentManager.SaveDocument(document, document.Definition.Attributes.DefaultFileName + document.Definition.Attributes.DefaultFileExtension);
            }
            else
            {
                throw new InvalidOperationException(
                    string.Format(
                        Message_en.DocumentMissingOpenFromTypeDefaultName,
                        document.GetType().Name));
            }
        }

        /// <summary>
        /// Saves a document by its default name.
        /// </summary>
        /// <param name="documentType">The System.Type of the document to open</param>
        /// <param name="fileName">The file to load</param>
        public static void SaveDocument(DocumentBase document, string fileName)
        {
            if (document.Definition.Attributes.Options.HasFlag(DocumentDefinitionOptions.CustomSerializationWrite))
            {
                MethodInfo m = document.GetType().GetMethod("ToXml", new Type[] { typeof(string) });

                if (m == null)
                    throw new InvalidOperationException(
                        Message_en.DocumentCustomSerializationWriteMustHaveToObjectMethod);

                try
                {
                    m.Invoke(document, new object[] { fileName });
                }
                catch (Exception ex)
                {
                    LogFileManager.Log(Message_en.DocumentCustomSerializatinoError);

                    throw new InvalidOperationException(
                        Message_en.DocumentCustomSerializatinoError, ex);
                }

                return;
            }

            ToXml(document, fileName);

        }

        public static string BaseDir
        {
            get { return _baseDir; }
            set { _baseDir = value; }
        }

        #endregion

    }
}
