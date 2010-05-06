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
    [DocumentDefinition(Name = "Meta Editor",
                DefaultFileName = "Editor",
                Description = "An editor for editors.",
                DefaultFileExtension = ".metaeditor",
                DefaultEditor = typeof(MetaEditor),
                Options = DocumentDefinitionOptions.UserFile)]
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


