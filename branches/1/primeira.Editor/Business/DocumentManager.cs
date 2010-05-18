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

        private static List<DocumentHeader> _headers = new List<DocumentHeader>();

        #endregion

        #region Documents & Headers

        public static DocumentHeader[] Headers
        {
            get
            {
                return _headers.ToArray();
            }
        }

        /// <summary>
        /// Registers a System.Type as a document.
        /// </summary>
        /// <param name="documentType"></param>
        /// <returns></returns>
        public static DocumentHeader RegisterDocument(Type documentType)
        {
            DocumentHeader header = DocumentManager.GetDocumentHeader(documentType);

            if (header == null)
            {
                header = DocumentManager.GetDocumentHeaderFromReflection(documentType);

                if (_headers == null)
                    _headers = new List<DocumentHeader>();

                _headers.Add(header);
            }

            return header;
        }

        /// <summary>
        /// Gets the System.Type of the document of a given file.
        /// </summary>
        /// <param name="fileName">Path to a file</param>
        /// <returns>The System.Type of the document of a given file</returns>
        public static DocumentHeader GetDocumentHeader(string fileName)
        {
            DocumentHeader[] document = GetDocumentHeaderByFileExtension(Path.GetExtension(fileName));

            if (document.Length == 1)
                return document[0];
            else
            {
                throw new NotImplementedException();

                //TODO: Read document header from file.
                //Read the begining (a chunk of bytes) of the given file.
            }
        }

        /// <summary>
        /// Gets the System.Type of the document of a given file extension.
        /// </summary>
        /// <param name="extension">File extesion</param>
        /// <returns>The System.Type of the document of a given file</returns>
        public static DocumentHeader[] GetDocumentHeaderByFileExtension(string extension)
        {
            DocumentHeader[] document = ( from a in Headers
                                          where a.DefaultFileExtension.Equals(extension, StringComparison.InvariantCultureIgnoreCase)
                                          select a ).ToArray();

            if (document.Length > 0)
                return document;

            throw new InvalidOperationException(
                string.Format(Message_en.ThereIsNoEditorForType, "*" + extension));
        }

        /// <summary>
        /// Gets the DocumentHeaderAttribute of a given document System.Type.
        /// </summary>
        /// <param name="documentType">The type of a document</param>
        /// <returns>The DocumentHeaderAttribute of the given System.Type</returns>
        public static DocumentHeader GetDocumentHeader(Type documentType)
        {
            return ( from a in Headers.AsParallel()
                     where a.BaseType == documentType
                     select a ).FirstOrDefault();
        }
        /// <summary>
        /// Gets the DocumentHeaderAttribute of a given document System.Type.
        /// </summary>
        /// <param name="documentType">The type of a document</param>
        /// <returns>The DocumentHeaderAttribute of the given System.Type</returns>
        public static DocumentHeader GetDocumentHeader(VersionData version)
        {
            return ( from a in Headers.AsParallel()
                     where a.Version == version
                     select a ).FirstOrDefault();
        }


        private static DocumentHeader GetDocumentHeaderFromReflection(Type documentType)
        {
            object[] attribs = documentType.GetCustomAttributes(typeof(DocumentHeaderAttribute), false);

            if (attribs.Length != 0)
            {
                DocumentHeaderAttribute attr       = (DocumentHeaderAttribute)attribs[0];

                DocumentHeader header              = new DocumentHeader();

                header.GetHeaderBaseFromReflection(attr);

                header.BaseType = documentType;

                header.Options = attr.Options;
                header.DefaultFileName = attr.DefaultFileName;
                header.DefaultFileExtension = attr.DefaultFileExtension;
                header.FriendlyNameMask = header.FriendlyNameMask;

                header.Icon = DocumentManager.GetIcon(documentType, attr.IconResourceFile);

                header.Version = new VersionData(attr.Id, attr.VersionNumber, attr.Author, attr.Info, attr.Email, attr.WebSite);

                if (header.DefaultEditorVersion != null)
                {
                    EditorHeader[] editor = EditorManager.GetEditorsHeader(header.DefaultEditorVersion);

                    if (editor.Count() > 0)
                        header.DefaultEditor = editor[0];
                }

                return header;
            }
            else
                throw new InvalidOperationException(string.Format(Message_en.DocumentMissingDocumentHeaderAttribute,
                    documentType.Name));
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

            foreach (DocumentHeader doc in Headers)
            {
                if (doc.Options.HasFlag(DocumentHeaderOptions.ShowIQuickLauchnOpen))
                    sb.Append(string.Format("{0} (*{1})|*{1}|", doc.Name, doc.DefaultFileExtension));
            }

            sb.Append("All files (*.*)|*.*");

            return sb.ToString();
        }

        /// <summary>
        /// Gets the dialog filter string index of a given file type.
        /// </summary>
        /// <param name="FileVersion"></param>
        /// <returns></returns>
        public static int GetDialogFilterIndex(DocumentHeader FileVersion)
        {
            int i = 0;
            foreach (DocumentHeader doc in Headers)
            {
                if (doc.Options.HasFlag(DocumentHeaderOptions.ShowIQuickLauchnOpen))
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

                Type[] knownTypes = null;
                if (_headers != null && Headers.Length > 0)
                    knownTypes = ( from a in Headers
                                   where a.BaseType != null
                                   select a.BaseType ).ToArray();
                else
                    knownTypes = new Type[2] { typeof(DocumentBase),
                                                type};

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
            DocumentHeader doc = ( from a in Headers.AsParallel()
                                   where a.BaseType == document.GetType()
                                   select a ).First();

            if (doc.Options.HasFlag(DocumentHeaderOptions.OpenFromTypeDefaultName))
            {
                string fileName = doc.DefaultFileName + doc.DefaultFileExtension;

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

            Type[] knownTypes = ( from a in Headers
                                  where a.BaseType != null
                                  select a.BaseType ).ToArray();

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
        public static DocumentBase LoadDocument(DocumentHeader document)
        {
            if (document.Options.HasFlag(DocumentHeaderOptions.OpenFromTypeDefaultName))
            {
                return DocumentManager.LoadDocument(document, document.DefaultFileName + document.DefaultFileExtension);
            }
            else
            {
                throw new InvalidOperationException(
                    string.Format(
                        Message_en.DocumentMissingOpenFromTypeDefaultName,
                         document.Name));
            }
        }

        /// <summary>
        /// Loads a document by its default name.
        /// </summary>
        /// <param name="documentType">The System.Type of the document to open</param>
        /// <param name="fileName">The file to load</param>
        /// <returns>A loaded document</returns>
        public static DocumentBase LoadDocument(DocumentHeader document, string fileName)
        {
            if (document.Options.HasFlag(DocumentHeaderOptions.CustomSerializationRead))
            {
                MethodInfo m = document.BaseType.GetMethod("ToObject", new Type[] { typeof(string) });

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
                            Message_en.DocumentCustomSerializatinoError, document.Name), Environment.NewLine, ex.ToString());

                    throw new InvalidOperationException(
                        string.Format(
                            Message_en.DocumentCustomSerializatinoError, document.Name), ex);
                }
            }

            return DocumentManager.ToObject(fileName, document.BaseType);
        }

        /// <summary>
        /// Saves a document by its default name.
        /// </summary>
        /// <param name="documentType">The System.Type of the document to open</param>
        public static void SaveDocument(DocumentBase document)
        {
            if (document.Header.Options.HasFlag(DocumentHeaderOptions.OpenFromTypeDefaultName))
            {
                DocumentManager.SaveDocument(document, document.Header.DefaultFileName + document.Header.DefaultFileExtension);
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
            if (document.Header.Options.HasFlag(DocumentHeaderOptions.CustomSerializationWrite))
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
