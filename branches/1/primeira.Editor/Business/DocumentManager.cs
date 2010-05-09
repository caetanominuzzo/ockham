using System;
using System.Collections.Generic;
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

        private static List<DocumentDetail> _documents = new List<DocumentDetail>();

        #endregion

        #region Documents

        public static DocumentDetail[] Documents
        {
            get
            {
                return _documents.ToArray();
            }
        }

        /// <summary>
        /// Gets the System.Type of the document of a given file.
        /// </summary>
        /// <param name="fileName">Path to a file</param>
        /// <returns>The System.Type of the document of a given file</returns>
        public static DocumentDetail GetDocumentDetail(string fileName)
        {
            DocumentDetail[] document = GetDocumenttDetailByFileExtension(Path.GetExtension(fileName));

            if (document.Length == 1)
                return document[0];
            else
            {
                throw new NotImplementedException();

                //TODO: Read document details from file.
            }
        }

        /// <summary>
        /// Gets the System.Type of the document of a given file extension.
        /// </summary>
        /// <param name="extension">File extesion</param>
        /// <returns>The System.Type of the document of a given file</returns>
        public static DocumentDetail[] GetDocumenttDetailByFileExtension(string extension)
        {
            DocumentDetail[] document = (from a in Documents
                                   where a.Definition.DefaultFileExtension.Equals(extension, StringComparison.InvariantCultureIgnoreCase)
                                   select a).ToArray();

            if (document.Length > 0)
                return document;

            throw new InvalidOperationException(
                string.Format(Message_en.ThereIsNoEditorForType, "*" + extension));
        }

        /// <summary>
        /// Gets the DocumentDefinitionAttribute of a given document System.Type.
        /// </summary>
        /// <param name="documentType">The type of a document</param>
        /// <returns>The DocumentDefinitionAttribute of the given System.Type</returns>
        public static DocumentDetail GetDocumentDetail(Type documentType)
        {
            DocumentDetail doc = (from a in Documents.AsParallel()
                            where a.DocumentType == documentType
                            select a).FirstOrDefault();
            return doc;
        }

        private static DocumentDetail GetDocumentDetailFromReflection(Type documentType)
        {
            DocumentDetail doc = new DocumentDetail();

            doc.DocumentType = documentType;

            object[] attribs = doc.DocumentType.GetCustomAttributes(typeof(DocumentDefinitionAttribute), false);

            if (attribs.Length != 0)
                doc.Definition = (DocumentDefinitionAttribute)attribs[0];
            else
                throw new InvalidOperationException(string.Format(Message_en.DocumentMissingDocumentDefinitionAttribute,
                    doc.DocumentType.Name));

            return doc;
        }

        public static DocumentDetail RegisterDocument(Type documentType)
        {
            DocumentDetail doc = (from a in _documents
                                  where a.DocumentType == documentType
                                  select a).FirstOrDefault();

            if (doc == null)
            {
                doc = DocumentManager.GetDocumentDetailFromReflection(documentType);

                _documents.Add(doc);
            }

            return doc;
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


            foreach (DocumentDetail doc in Documents)
            {
                DocumentDefinitionAttribute def = doc.Definition;

                if (def.Options.HasFlag(DocumentDefinitionOptions.ShowIQuickLauchnOpen))
                    sb.Append(string.Format("{0} (*{1})|*{1}|", def.Name, def.DefaultFileExtension));
            }

            sb.Append("All files (*.*)|*.*");

            return sb.ToString();
        }

        /// <summary>
        /// Gets the dialog filter string index of a given file type.
        /// </summary>
        /// <param name="FileVersion"></param>
        /// <returns></returns>
        public static int GetDialogFilterIndex(DocumentDefinitionAttribute FileVersion)
        {
            int i = 0;
            foreach (DocumentDetail doc in Documents)
            {
                DocumentDefinitionAttribute def = doc.Definition;
                if (def.Options.HasFlag(DocumentDefinitionOptions.ShowIQuickLauchnOpen))
                    continue;
                else i++;

                if (def == FileVersion)
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

                Type[] knownTypes = (from a in Documents
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
            DocumentDefinitionAttribute def = (from a in Documents.AsParallel()
                                               where a.DocumentType == document.GetType()
                                               select a.Definition).First();

            if (def.Options.HasFlag(DocumentDefinitionOptions.OpenFromTypeDefaultName))
            {
                string fileName = def.DefaultFileName + def.DefaultFileExtension;

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

            Type[] knownTypes = (from a in Documents
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
        public static DocumentBase LoadDocument(DocumentDetail document)
        {
            DocumentDefinitionAttribute def = document.Definition;

            if (def.Options.HasFlag(DocumentDefinitionOptions.OpenFromTypeDefaultName))
            {
                return DocumentManager.LoadDocument(document, def.DefaultFileName + def.DefaultFileExtension);
            }
            else
            {
                throw new InvalidOperationException(
                    string.Format(
                        Message_en.DocumentMissingOpenFromTypeDefaultName,
                        def.Name));
            }
        }

        /// <summary>
        /// Loads a document by its default name.
        /// </summary>
        /// <param name="documentType">The System.Type of the document to open</param>
        /// <param name="fileName">The file to load</param>
        /// <returns>A loaded document</returns>
        public static DocumentBase LoadDocument(DocumentDetail document, string fileName)
        {
            DocumentDefinitionAttribute def = document.Definition;

            if (def.Options.HasFlag(DocumentDefinitionOptions.CustomSerializationRead))
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
                            Message_en.DocumentCustomSerializatinoError, def.DefaultEditor.Name), Environment.NewLine, ex.ToString());

                    throw new InvalidOperationException(
                        string.Format(
                            Message_en.DocumentCustomSerializatinoError, def.DefaultEditor.Name), ex);
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
            DocumentDefinitionAttribute def = document.DocumentDetail.Definition;

            if (def.Options.HasFlag(DocumentDefinitionOptions.OpenFromTypeDefaultName))
            {
                DocumentManager.SaveDocument(document, def.DefaultFileName + def.DefaultFileExtension);
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
            DocumentDefinitionAttribute def = document.DocumentDetail.Definition;

            if (def.Options.HasFlag(DocumentDefinitionOptions.CustomSerializationWrite))
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
