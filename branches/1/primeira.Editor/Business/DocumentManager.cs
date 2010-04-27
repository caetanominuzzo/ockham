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

        private static List<Type> _knownDocumentType = new List<Type>();

        private static List<DocumentDefinitionAttribute> _knownDocumentDefinition = new List<DocumentDefinitionAttribute>();

        private static string _baseDir = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        #endregion

        #region DocumentDefinitionAttribute & DocumentType

        /// <summary>
        /// Gets a list of all registered document types.
        /// </summary>
        /// <returns>Registered document types.</returns>
        public static Type[] GetDocumentTypes()
        {
            return _knownDocumentType.ToArray();
        }

        /// <summary>
        /// Gets the System.Type of the document of a given file.
        /// </summary>
        /// <param name="fileName">Path to a file</param>
        /// <returns>The System.Type of the document of a given file</returns>
        public static Type GetDocumentType(string fileName)
        {
            return GetDocumentTypeByFileExtension(Path.GetExtension(fileName));
        }

        /// <summary>
        /// Gets the System.Type of the document of a given file extension.
        /// </summary>
        /// <param name="extension">File extesion</param>
        /// <returns>The System.Type of the document of a given file</returns>
        public static Type GetDocumentTypeByFileExtension(string extension)
        {
            Type documentType = (from x in _knownDocumentType where ((DocumentDefinitionAttribute[])x.GetCustomAttributes(typeof(DocumentDefinitionAttribute), false))[0].DefaultFileExtension == extension select x).First();

            if (documentType != null)
                return documentType;

            MessageManager.Send(
                string.Concat("No Editor found for ", extension, " files."));

            return null;
        }

        /// <summary>
        /// Gets a list of DocumentDefinitionAttribute for all registered document types.
        /// </summary>
        /// <returns>Registered DocumentDefinitionAttribute.</returns>
        public static DocumentDefinitionAttribute[] GetDocumentDefinition()
        {
            return _knownDocumentDefinition.ToArray();
        }

        /// <summary>
        /// Gets the DocumentDefinitionAttribute of a given document System.Type.
        /// </summary>
        /// <param name="documentType">The type of a document</param>
        /// <returns>The DocumentDefinitionAttribute of the given System.Type</returns>
        public static DocumentDefinitionAttribute GetDocumentDefinition(MemberInfo documentType)
        {
            object[] attribs = documentType.GetCustomAttributes(typeof(DocumentDefinitionAttribute), false);

            if (attribs.Length != 0)
                return (DocumentDefinitionAttribute)attribs[0];

            MessageManager.Send(MessageSeverity.Fatal,
                string.Format(Message_en.DocumentMissingDocumentDefinitionAttribute,
                documentType.Name));

            return null;
        }

        /// <summary>
        /// Gets the DocumentDefinitionAttribute of a given file.
        /// </summary>
        /// <param name="documentType">Path to a file</param>
        /// <returns>The DocumentDefinitionAttribute of the given file</returns>
        public static DocumentDefinitionAttribute GetDocumentDefinition(string fileName)
        {
            string ext = Path.GetExtension(fileName);

            return GetDocumentDefinitionByFileExtension(ext);
        }

        /// <summary>
        /// Gets the DocumentDefinitionAttribute of a given file extesion.
        /// </summary>
        /// <param name="documentType">A file extension</param>
        /// <returns>The DocumentDefinitionAttribute of the given file extension</returns>
        private static DocumentDefinitionAttribute GetDocumentDefinitionByFileExtension(string extension)
        {
            return (from x in _knownDocumentDefinition where x.DefaultFileExtension == extension select (DocumentDefinitionAttribute)x).FirstOrDefault();
        }

        /// <summary>
        /// Adds the document CLR type and it DocumentDefinitionAttribute to registered documents list.
        /// </summary>
        /// <param name="documentType">The document CLR type.</param>
        internal static void RegisterDocument(Type documentType)
        {
            DocumentDefinitionAttribute def = GetDocumentDefinition(documentType);

            if (def != null)
            {
                _knownDocumentDefinition.Add(def);
                _knownDocumentType.Add(documentType);
            }
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

            foreach (DocumentDefinitionAttribute d in _knownDocumentDefinition)
            {
                if ((d.Options & DocumentDefinitionOptions.ShowIQuickLauchnOpen) > 0)
                    sb.Append(string.Format("{0} (*{1})|*{1}|", d.Name, d.DefaultFileExtension));
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
            foreach (DocumentDefinitionAttribute d in _knownDocumentDefinition)
            {
                if ((d.Options & DocumentDefinitionOptions.ShowIQuickLauchnOpen) > 0)
                    continue;
                else i++;

                if (d == FileVersion)
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
        private static DocumentBase ToObject(string fileName, Type type)
        {
            Stream sm = File.OpenRead(fileName);

            Type[] knownTypes = DocumentManager.GetDocumentTypes();

            Array.Resize(ref knownTypes, knownTypes.Length + 1);

            knownTypes[knownTypes.Length - 1] = type;

            DataContractSerializer ser = new DataContractSerializer(typeof(DocumentBase),
                knownTypes,
                10000000, false, true, null);

            DocumentBase res = (DocumentBase)ser.ReadObject(sm);

            UndoRedoFramework.UndoRedoManager.FlushHistory();

            sm.Close();

            return res;
        }

        /// <summary>
        /// Serializes a given document in its default file name.
        /// </summary>
        /// <param name="document">The document to serialize</param>
        public static void ToXml(DocumentBase document)
        {
            DocumentDefinitionAttribute def = DocumentManager.GetDocumentDefinition(document.GetType());

            if (def.Options.HasFlag(DocumentDefinitionOptions.OpenFromTypeDefaultName))
            {
                string fileName = def.DefaultFileName + def.DefaultFileExtension;

                DocumentManager.ToXml(document, fileName);
            }
            else
            {
                MessageManager.Send(
                    MessageSeverity.Fatal,
                    string.Format(
                        Message_en.DocumentMissingOpenFromTypeDefaultName,
                        document.GetType().Name));

                return;
            }
                
        }

        /// <summary>
        /// Serializes a given document in an specified file.
        /// </summary>
        /// <param name="document">The document to serialize</param>
        /// <param name="fileName">The file to serialize</param>
        public static void ToXml(DocumentBase document, string fileName)
        {
            Stream sm = File.Create(fileName);

            Type[] knownTypes = DocumentManager.GetDocumentTypes();

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
        public static DocumentBase LoadDocument(Type documentType)
        {
            DocumentDefinitionAttribute def = DocumentManager.GetDocumentDefinition(documentType);

            if (def.Options.HasFlag(DocumentDefinitionOptions.OpenFromTypeDefaultName))
            {
                return DocumentManager.LoadDocument(documentType, def.DefaultFileName + def.DefaultFileExtension);
            }
            else
            {
                MessageManager.Send(
                    MessageSeverity.Fatal,
                    string.Format(
                        Message_en.DocumentMissingOpenFromTypeDefaultName,
                        documentType.Name));

                return null;
            }
        }

        /// <summary>
        /// Loads a document by its default name.
        /// </summary>
        /// <param name="documentType">The System.Type of the document to open</param>
        /// <param name="fileName">The file to load</param>
        /// <returns>A loaded document</returns>
        public static DocumentBase LoadDocument(Type documentType, string fileName)
        {
            DocumentDefinitionAttribute def = DocumentManager.GetDocumentDefinition(documentType);

            if (def.Options.HasFlag(DocumentDefinitionOptions.CustomSerialization))
            {
                MethodInfo m = documentType.GetMethod("ToObject", new Type[] { typeof(string) });

                if (m == null)
                    MessageManager.Send(MessageSeverity.Fatal, Message_en.DocumentCustomSerializationMustHaveToObjectMethod);

                try
                {
                    return (DocumentBase)m.Invoke(null, new object[] { fileName });
                }
                catch
                {
                    MessageManager.Send(MessageSeverity.Error, Message_en.DocumentCustomSerializatinoError);
                }


                return null;
            }

            FileInfo f = new FileInfo(fileName);

            DocumentBase doc = null;

            if (!f.Exists || f.Length == 0)
            {
                doc = (DocumentBase)documentType.GetConstructor(System.Type.EmptyTypes).Invoke(System.Type.EmptyTypes);
            }
            else
                doc = DocumentManager.ToObject(fileName, documentType);


            //if(doc == null)
            //    MessageManager.Send(
            //        MessageSeverity.Alert, 

            return doc;

        }

        public static string BaseDir
        {
            get { return _baseDir; }
            set { _baseDir = value; }
        }

        #endregion

    }
}
