﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace primeira.Editor.Business
{

    public static class DocumentManager
    {
        #region Fields

        private static List<Type> _knownDocumentType = new List<Type>();
        
        private static List<DocumentDefinition> _knownDocumentDefinition = new List<DocumentDefinition>();

        #endregion

        #region DocumentDefinition & DocumentType

        private static DocumentDefinition GetDocumentDefinitionByClrType(Type documentType)
        {
            PropertyInfo pDef = documentType.GetProperty("DocumentDefinition");

            if (pDef == null)
            {
                MessageManager.Alert("Missing DocumentDefinition Property in ", documentType.Name, " Class.");
                return null;
            }

            return (DocumentDefinition)pDef.GetValue(null, null);
        }

        private static DocumentDefinition GetDocumentDefinitionByEditorType(Type editorType)
        {
            try
            {
                return (from x in _knownDocumentDefinition where x.DefaultEditor == editorType select (DocumentDefinition)x).FirstOrDefault();
            }
            catch
            {
                MessageManager.Alert("No DocumentDefinition found for ", editorType.Name, " Class");
                return null;
            }
        }

        internal static DocumentDefinition GetDocumentDefinitionByFileExtension(string extension)
        {
            try
            {
                return (from x in _knownDocumentDefinition where x.DefaultFileExtension == extension select (DocumentDefinition)x).FirstOrDefault();
            }
            catch
            {
                MessageManager.Alert("No Editor Class found for *", extension, " files.");
                return null;
            }
        }

        internal static Type[] GetKnownDocumentTypes()
        {
            return _knownDocumentType.ToArray();
        }

        public static DocumentDefinition GetDocumentDefinitionByFilename(string filename)
        {
            string ext = Path.GetExtension(filename);

            return GetDocumentDefinitionByFileExtension(ext);
        }

        public static DocumentDefinition[] GetKnowDocumentDefinition()
        {
            return _knownDocumentDefinition.ToArray();
        }

        /// <summary>
        /// Add the document CLR type and its DocumentDefinition to registered documents list.
        /// </summary>
        /// <param name="documentType">The document CLR type.</param>
        public static void RegisterDocument(Type documentType)
        {
            DocumentDefinition def = GetDocumentDefinitionByClrType(documentType);

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

            foreach (DocumentDefinition d in _knownDocumentDefinition)
            {
                if ((d.Options & DocumentDefinitionOptions.ShowIQuickLauchnOpen) == DocumentDefinitionOptions.ShowIQuickLauchnOpen)
                    sb.Append(string.Format("{0} (*{1})|*{1}|", d.Name, d.DefaultFileExtension));
            }

            sb.Append("All files (*.*)|*.*");

            return sb.ToString();
        }

        public static int GetDialogFilterIndex(DocumentDefinition FileVersion)
        {
            int i = 0;
            foreach (DocumentDefinition d in _knownDocumentDefinition)
            {
                if ((d.Options & DocumentDefinitionOptions.ShowIQuickLauchnOpen) == DocumentDefinitionOptions.ShowIQuickLauchnOpen)
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
            return DocumentBase.ToObject(filename, null);
        }

        public static void ToXml(DocumentBase document, string filename)
        {
            document.ToXml(filename);
        }

        #endregion

        #region New, Open & Save Document

        internal static void AddDocument(IEditor editor)
        {
            if(TabManager.GetInstance().TabControl != null)
                TabManager.GetInstance().TabControl.AddTab(editor);

            if ((editor.Document.Definition.Options & DocumentDefinitionOptions.ShowInRecents) == DocumentDefinitionOptions.ShowInRecents)
            {
                    if(FileManager.Recent != null)
                        FileManager.Recent.AddRecent(editor.Filename);
            }

            editor.OnSelected += new SelectedDelegate(TabControl_OnSelected);

            editor.Selected = true;
        }

        static void TabControl_OnSelected(IEditor sender)
        {
            TabManager.GetInstance().ActiveEditor = (IEditor)sender;
        }


        public static void OpenOrCreateDocument(bool NewFile, DocumentDefinition FileVersion)
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

                LoadDocument(ss);
            }
        }

        private static string _baseDir = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public static string BaseDir
        {
            get { return _baseDir; }
            private set { _baseDir = value; }
        }

        internal static DocumentBase LoadDocument(string filename)
        {
            return EditorManager.LoadEditor(filename).Document;
        }

        #endregion

    }
}