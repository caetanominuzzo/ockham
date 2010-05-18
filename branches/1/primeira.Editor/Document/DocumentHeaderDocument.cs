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
                Id = "2F2C28E5-2919-497D-B482-FAD820E2B6CA",
                VersionNumber = "1.0",
                Name = "Documents cache",
                DefaultFileName = "documents",
                Description = "Stores the available documents.",
                DefaultFileExtension = ".cache",
                Options = DocumentHeaderOptions.OpenFromTypeDefaultName)]
    internal class DocumentHeaderDocument : DocumentBase
    {
        public static string FileName
        {
            get { return "documents.cache"; }
        }

        private List<DocumentHeader> _headers = null;

        public void AddDocumentHeader(DocumentHeader header)
        {
            _headers.Add(header);
        }

        public void Clear()
        {
            _headers.Clear();
        }

        [DataMember()]
        public DocumentHeader[] Headers
        {
            get
            {
                if (_headers == null)
                    _headers = new List<DocumentHeader>();

                return _headers.ToArray();
            }
            set
            {
                if (_headers == null)
                    _headers = new List<DocumentHeader>(value.Length);
                else
                    _headers.Clear();

                _headers.AddRange(value);
            }
        }

        public DateTime LastWriteTime { get; private set; }

        public static DocumentHeaderDocument GetInstance()
        {
            DocumentHeader header = new DocumentHeader();
            header.BaseType = typeof(DocumentHeaderDocument);
            header.Options = DocumentHeaderOptions.OpenFromTypeDefaultName;
            header.DefaultFileName = "documents";
            header.DefaultFileExtension = ".cache";
            
            //DocumentHeaderDocument can not be registered by type becouse it stores
            DocumentHeaderDocument doc = (DocumentHeaderDocument)DocumentManager.LoadDocument(header);

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
