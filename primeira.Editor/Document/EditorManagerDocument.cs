using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

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
    internal class EditorManagerDocument : DocumentBase
    {
        public static string FileName
        {
            get { return "editors.cache"; }
        }

        private List<EditorHeader> _editors = new List<EditorHeader>();

        public void AddEditor(EditorHeader editor)
        {
            _editors.Add(editor);
        }

        public void Clear()
        {
            _editors.Clear();
        }

        [DataMember()]
        public EditorHeader[] Editors
        {
            get { return _editors.ToArray(); }
            set
            {
                if (_editors == null)
                    _editors = new List<EditorHeader>(value.Length);
                else
                    _editors.Clear();

                _editors.AddRange(value);
            }
        }

        public static EditorManagerDocument GetInstance()
        {
            DocumentHeader def = DocumentManager.RegisterDocument(typeof(EditorManagerDocument));

            EditorManagerDocument doc = (EditorManagerDocument)DocumentManager.LoadDocument(def);

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
