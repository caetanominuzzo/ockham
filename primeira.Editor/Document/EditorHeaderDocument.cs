using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;


namespace primeira.Editor
{
    [DataContract()]
    [DocumentHeader(
                Id = "9B5FF01D-85A2-47DF-8CF0-184EC55A7B52",
                VersionNumber = "1.0",
                Name = "Editors cache",
                DefaultFileName = "editors",
                Description = "Stores the available editors.",
                DefaultFileExtension = ".cache",
                Options = DocumentHeaderOptions.OpenFromTypeDefaultName)]
    internal class EditorHeaderDocument : DocumentBase
    {
        public static string FileName
        {
            get { return "editors.cache"; }
        }

        private List<EditorHeader> _headers = new List<EditorHeader>();

        public void AddEditorHeader(EditorHeader header)
        {
            _headers.Add(header);
        }

        public void Clear()
        {
            _headers.Clear();
        }

        [DataMember()]
        public EditorHeader[] Headers
        {
            get
            {
                if (_headers == null)
                    _headers = new List<EditorHeader>();

                return _headers.ToArray();
            }
            set
            {
                if (_headers == null)
                    _headers = new List<EditorHeader>(value.Length);
                else
                    _headers.Clear();

                _headers.AddRange(value);
            }
        }

        public DateTime LastWriteTime { get; private set; }

        public static EditorHeaderDocument GetInstance()
        {
            EditorHeaderDocument doc = (EditorHeaderDocument)DocumentManager.ToObject(FileName, typeof(EditorHeaderDocument));

            FileInfo f = new FileInfo(FileName);

            if (f.Exists)
                doc.LastWriteTime = f.LastWriteTime;

            if (doc == null)
                throw new InvalidOperationException(
                    string.Format(Message_en.DocumentCreationError, FileName));

            return doc;
        }

        public void ToXml()
        {
            DocumentManager.SaveDocument(this);
        }

    }
}
