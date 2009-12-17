using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Drawing;
using primeira.Editor;

namespace primeira.Editor.MetaEditor
{
    [DataContract()]
    public class MetaEditorDocument : DocumentBase
    {
        private static DocumentDefinition _definition =
            new DocumentDefinition()
            {
                Name = "Meta Editor",
                DefaultFileName = "Editor {0}",
                Description = "An editor for editors.",
                DefaultFileExtension = ".metaeditor",
                Id = new Guid("513ff96c-0d23-44f4-82ab-0dea5a62dcd3"),
                DefaultEditor = typeof(MetaEditor),
                Options = DocumentDefinitionOptions.UserFile,
                Icon = Image.FromFile(@"D:\Desenv\Ockham\branches\1\primeira.Editor.TabControlEditor\file1.gif")
            };

        public static DocumentDefinition DocumentDefinition
        {
            get { return _definition; }
        }

        public override DocumentDefinition Definition
        {
            get { return _definition; }
        }

        #region Data

        [DataMember()]
        public string EditorName { get; set; }

        [DataMember()]
        public string DefaultFileName { get; set; }

        [DataMember()]
        public string EditorDescription { get; set; }

        [DataMember()]
        public string DefaultFileExtension { get; set; }

        [DataMember()]
        public Guid EditorGuid { get; set; }

        [DataMember()]
        public string Icon { get; set; }

        #endregion

        public static DocumentBase ToObject(string filename)
        {
            return DocumentBase.ToObject(filename, typeof(MetaEditorDocument));
        }
    }
}


