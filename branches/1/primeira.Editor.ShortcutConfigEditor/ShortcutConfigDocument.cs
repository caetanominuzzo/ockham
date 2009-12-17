using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace primeira.Editor
{
    [DataContract()]
    public class ShortcutConfigDocument : DocumentBase
    {
        private static DocumentDefinition _definition =
            new DocumentDefinition()
            {
                Name = "ShortCut Manager",
                DefaultFileName = "default",
                Description = "ShortCut Manager",
                DefaultFileExtension = ".shortcut",
                Id = new Guid("513ff96c-0d23-44f4-82ab-0dea5a62dcd3"),
                DefaultEditor = typeof(ShortcutConfigDocument),
                Options = DocumentDefinitionOptions.DontShowLabel,
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
        public object o { get; set; }

        #endregion

        public static DocumentBase ToObject(string filename)
        {
            return DocumentBase.ToObject(filename, typeof(ShortcutConfigDocument));
        }
    }
}


