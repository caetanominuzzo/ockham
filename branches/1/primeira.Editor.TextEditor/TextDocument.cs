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
    [DocumentDefinition(Name = "Text Editor",
                DefaultFileName = "Noname",
                Description = "A plain text editor.",
                DefaultFileExtension = ".txt",
                Options = DocumentDefinitionOptions.UserFile | DocumentDefinitionOptions.CustomSerialization)]
    public class TextEditorDocument : DocumentBase
    {
        [DataMember()]
        public string Content { get; set; }

        [DataMember()]
        public int SelectionLength { get; set; }

        [DataMember()]
        public int SelectionStart
        {

            get { return _caret; }
            set { _caret = value; }
       }

        private int _caret;

        public static DocumentBase ToObject(string fileName)
        {
            return DocumentBase.ToObject(fileName + ".user", typeof(TextEditorDocument));
        }

        public void ToXml(string fileName)
        {
            System.IO.File.WriteAllText(fileName, Content);

            DocumentBase.ToXml(this, fileName + ".user");
        }


    }
}


