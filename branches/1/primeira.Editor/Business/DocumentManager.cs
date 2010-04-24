using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

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
            DocumentDefinitionAttribute tmp = GetDocumentDefinitionByFileExtension(extension);

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
                string.Format(Message_us.DocumentMissingDocumentDefinitionAttribute,
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
            DocumentDefinitionAttribute tmp = (from x in _knownDocumentDefinition where x.DefaultFileExtension == extension select (DocumentDefinitionAttribute)x).FirstOrDefault();

            if (tmp == null)
                MessageManager.Send("No Editor found for ", extension, " files.");

            return tmp;
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

        public static string GetDialogFilterString()
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

        public static DocumentBase ToObject(string fileName)
        {
            return DocumentManager.ToObject(fileName, null);
        }

        public static DocumentBase ToObject(string fileName, Type type)
        {
            try
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
            catch
            {
                MessageManager.Send("There is an error while deserializing file ", fileName, ".");
            }

            return null;
        }

        public static void ToXml(DocumentBase document)
        {
            DocumentDefinitionAttribute def = DocumentManager.GetDocumentDefinition(document.GetType());

            if ((def.Options & DocumentDefinitionOptions.OpenFromTypeDefaultName) > 0)
            {
                string fileName = def.DefaultFileName + def.DefaultFileExtension;

                DocumentManager.ToXml(document, fileName);
            }
            else
                throw new InvalidOperationException("This document don't uses a default name, you must especify one.");
        }

        public static void ToXml(DocumentBase document, string fileName)
        {
            Stream sm = File.Create(fileName);

            Type[] knownTypes = DocumentManager.GetDocumentTypes();

            Array.Resize(ref knownTypes, knownTypes.Length + 1);

            knownTypes[knownTypes.Length - 1] = document.GetType();

            DataContractSerializer ser = new DataContractSerializer(typeof(DocumentBase),
                knownTypes,
                10000000, false, true, null);
            ser.WriteObject(sm, document);

            sm.Close();
        }

        #endregion

        #region New, Open & Save Document

        public static DocumentBase GetInstance(Type documentType)
        {
            DocumentDefinitionAttribute def = DocumentManager.GetDocumentDefinition(documentType);

            DocumentBase doc = null;

            if ((def.Options & DocumentDefinitionOptions.OpenFromTypeDefaultName) > 0)
            {
                doc = DocumentManager.LoadDocument(documentType, def.DefaultFileName + def.DefaultFileExtension);
            }
            else
                doc = (DocumentBase)documentType.GetConstructor(System.Type.EmptyTypes).Invoke(System.Type.EmptyTypes);

            return doc;
        }

        public static DocumentBase LoadDocument(Type documentType, string fileName)
        {
            DocumentDefinitionAttribute def = DocumentManager.GetDocumentDefinition(documentType);

            if (def.Options.HasFlag(DocumentDefinitionOptions.CustomSerialization))
            {
                MethodInfo m = documentType.GetMethod("ToObject", new Type[] { typeof(string) });

                if (m == null)
                    MessageManager.Send(MessageSeverity.Fatal, Message_us.DocumentCustomSerializationMustHaveToObjectMethod);

                Object o = m.Invoke(null, new object[] { fileName });

                if (o is DocumentBase)
                    return (DocumentBase)o;
                else
                    MessageManager.Send(MessageSeverity.Error, Message_us.DocumentCustomSerializatinoError);

                return null;
            }

            FileInfo f = new FileInfo(fileName);

            if (!f.Exists || f.Length == 0)
            {
                return (DocumentBase)documentType.GetConstructor(System.Type.EmptyTypes).Invoke(System.Type.EmptyTypes);
            }
            else
                return DocumentManager.ToObject(fileName, documentType);
        }

        public static void OpenOrCreateDocument(bool NewFile, DocumentDefinitionAttribute FileVersion)
        {
            OpenFileDialog s = new OpenFileDialog();

            s.CheckFileExists = false;

            if (NewFile)
                s.FileName = FileManager.GetNewFile(FileVersion, BaseDir);

            s.Filter = DocumentManager.GetDialogFilterString();

            s.DefaultExt = FileVersion.DefaultFileExtension;

            s.FilterIndex = DocumentManager.GetDialogFilterIndex(FileVersion);

            s.InitialDirectory = BaseDir;

            if (s.ShowDialog() == DialogResult.OK)
            {
                BaseDir = s.InitialDirectory;

                string ss = Path.Combine(BaseDir, s.FileName);

                if (!File.Exists(ss))
                    File.Create(ss).Close();

                EditorManager.LoadEditor(ss);
            }
        }

        public static string BaseDir
        {
            get { return _baseDir; }
            private set { _baseDir = value; }
        }

        #endregion

    }
}
