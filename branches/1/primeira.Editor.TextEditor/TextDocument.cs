using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Drawing;
using System.IO;
using UndoRedoFramework;
using primeira.Editor;


namespace primeira.Editor
{
    [DataContract()]
    [DocumentHeader(
                Id = "70A84F3A-926E-43B2-BD5E-ED4AFDDC0844",
                VersionNumber = "1.0",
                Name = "Text Editor",
                DefaultFileName = "Noname",
                Description = "A plain text editor.",
                DefaultFileExtension = ".txt",
                Options = DocumentHeaderOptions.UserFile | DocumentHeaderOptions.CustomSerialization)]
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
            string internalFileName = fileName + ".user";

            bool alreadyExists =  File.Exists(internalFileName);
            
            DocumentBase tmp = DocumentBase.ToObject(fileName + ".user", typeof(TextEditorDocument));

            if (!alreadyExists)
                ((TextEditorDocument)tmp).Content = File.ReadAllText(fileName);

            return tmp;
        }

        public void ToXml(string fileName)
        {
            File.WriteAllText(fileName, Content);

            DocumentBase.ToXml(this, fileName + ".user");
        }


    }
}


