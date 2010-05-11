using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Drawing;
using UndoRedoFramework;
using primeira.Editor;


namespace primeira.Editor
{
    [DataContract()]
    [DocumentHeader(
                Id = "6225BC45-218C-4E1E-BE24-745CF827EABD",
                VersionNumber = "1.0",
                Name = "Meta Editor",
                DefaultFileName = "Editor",
                Description = "An editor for editors.",
                DefaultFileExtension = ".metaeditor",
                Options = DocumentHeaderOptions.UserFile)]
    public class MetaEditorDocument : DocumentBase
    {
        UndoRedo<string> _editorName = new UndoRedo<string>(string.Empty);
        UndoRedo<string> _defaultFileName = new UndoRedo<string>();
        UndoRedo<string> _editorDescription = new UndoRedo<string>();
        UndoRedo<string> _defaultFileExtension = new UndoRedo<string>();

        [DataMember()]
        public string EditorName
        {
            get { return _editorName.Value; }
            set
            {
                using (UndoRedoManager.Start("Edit Editor Name"))
                {
                    if (_editorName == null)
                        _editorName = new UndoRedo<string>(string.Empty);

                    _editorName.Value = value;

                    UndoRedoManager.Commit();

                    
                }
            }
        }

        //[DataMember()]
        //public string EditorName { get; set; }

        [DataMember()]
        public string DefaultFileName { get; set; }

        [DataMember()]
        public string EditorDescription { get; set; }

        [DataMember()]
        public string DefaultFileExtension { get; set; }

    }
}


