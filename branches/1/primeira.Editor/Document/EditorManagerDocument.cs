using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace primeira.Editor
{
    [DataContract()]
    [DocumentDefinition(Name = "Editors cache",
                DefaultFileName = "editors",
                Description = "Stores the available editors.",
                DefaultFileExtension = ".cache",
                Options = DocumentDefinitionOptions.OpenFromTypeDefaultName)]
    internal class EditorManagerDocument : DocumentBase
    {
        public static string FileName
        {
            get { return "editors.cache"; }
        }

        private List<EditorDetail> _editors = new List<EditorDetail>();

        public void AddEditor(EditorDetail editor)
        {
            _editors.Add(editor);
        }

        public void Clear()
        {
            _editors.Clear();
        }

        [DataMember()]
        public EditorDetail[] Editors
        {
            get { return _editors.ToArray(); }
            set
            {
                if (_editors == null)
                    _editors = new List<EditorDetail>(value.Length);
                else
                    _editors.Clear();

                _editors.AddRange(value);
            }
        }

        public static EditorManagerDocument GetInstance()
        {
            DocumentDetail detail = DocumentManager.RegisterDocument(typeof(EditorManagerDocument));

            EditorManagerDocument doc = (EditorManagerDocument)DocumentManager.LoadDocument(detail);

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
