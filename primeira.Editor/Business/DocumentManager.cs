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
        
        public static Type[] GetDocumentTypes()
        {
            return _knownDocumentType.ToArray();
        }

        public static Type GetDocumentType(string filename)
        {
            return GetDocumentTypeByFileExtension(Path.GetExtension(filename));
        }

        public static Type GetDocumentTypeByFileExtension(string extension)
        {
            DocumentDefinitionAttribute tmp = GetDocumentDefinitionByFileExtension(extension);

            Type documentType = (from x in _knownDocumentType where ((DocumentDefinitionAttribute[])x.GetCustomAttributes(typeof(DocumentDefinitionAttribute), false))[0].DefaultFileExtension == extension select x).First();

            if (documentType != null)
                return documentType;

            throw new Exception(string.Format("No Editor found for *{0} files.", extension));
        }

        public static DocumentDefinitionAttribute[] GetDocumentDefinition()
        {
            return _knownDocumentDefinition.ToArray();
        }
        
        public static DocumentDefinitionAttribute GetDocumentDefinition(Type documentType)
        {
            object[] attribs = documentType.GetCustomAttributes(typeof(DocumentDefinitionAttribute), false);

            if (attribs.Length == 0)
            {
                MessageManager.Alert("Missing DocumentDefinitionAttribute Definition property in ", documentType.Name, " Class.");
                return null;
            }

            return (DocumentDefinitionAttribute)attribs[0];
        }

        public static DocumentDefinitionAttribute GetDocumentDefinition(string filename)
        {
            string ext = Path.GetExtension(filename);

            return GetDocumentDefinitionByFileExtension(ext);
        }

        private static DocumentDefinitionAttribute GetDocumentDefinitionByFileExtension(string extension)
        {
            try
            {
                DocumentDefinitionAttribute tmp = (from x in _knownDocumentDefinition where x.DefaultFileExtension == extension select (DocumentDefinitionAttribute)x).FirstOrDefault();
                
                if(tmp != null)
                    return tmp;
                throw new Exception();
            }
            catch(Exception)
            {
                MessageManager.Alert("No Editor found for *", extension, " files.");
                return null;
            }
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

        public static DocumentBase ToObject(string filename)
        {
            return DocumentManager.ToObject(filename, null);
        }

        public static DocumentBase ToObject(string filename, Type type)
        {
            try
            {
                Stream sm = File.OpenRead(filename);

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
            catch (Exception ex)
            {
                MessageManager.Alert("There is an error while deserializing file ", filename, ".\n", ex.Message);
            }

            return null;
        }

        public static void ToXml(DocumentBase document)
        {
            DocumentDefinitionAttribute def = DocumentManager.GetDocumentDefinition(document.GetType());

            if ((def.Options & DocumentDefinitionOptions.OpenFromTypeDefaultName) > 0)
            {
                string filename = def.DefaultFileName + def.DefaultFileExtension;

                DocumentManager.ToXml(document, filename);
            }
            else
                throw new InvalidOperationException("This document don't uses a default name, you must especify one.");
        }

        public static void ToXml(DocumentBase document, string filename)
        {
            Stream sm = File.Create(filename);

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

        public static DocumentBase LoadDocument(Type documentType, string filename)
        {
            FileInfo f = new FileInfo(filename);

            if (!f.Exists || f.Length == 0)
            {
                return (DocumentBase)documentType.GetConstructor(System.Type.EmptyTypes).Invoke(System.Type.EmptyTypes);
            }

            return DocumentManager.ToObject(filename, documentType);
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
