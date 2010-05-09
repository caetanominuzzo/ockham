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

        private List<EditorDefinition> _editors = new List<EditorDefinition>();

        public void AddEditor(EditorDefinition editor)
        {
            _editors.Add(editor);
        }

        public void Clear()
        {
            _editors.Clear();
        }

        [DataMember()]
        public EditorDefinition[] Editors
        {
            get { return _editors.ToArray(); }
            set
            {
                if (_editors == null)
                    _editors = new List<EditorDefinition>(value.Length);
                else
                    _editors.Clear();

                _editors.AddRange(value);
            }
        }

        public static EditorManagerDocument GetInstance()
        {
            DocumentDefinition def = DocumentManager.RegisterDocument(typeof(EditorManagerDocument));

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
