﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Drawing;

namespace primeira.Editor
{
    [DataContract()]
    [DocumentDefinition(Name = "Shortcut Manager",
                DefaultFileName = "default",
                Description = "Shortcut Manager",
                DefaultFileExtension = ".shortcut",
                DefaultEditor = typeof(ShortcutConfigEditor),
                Options = DocumentDefinitionOptions.DoNotShowLabelAndFixWidth | DocumentDefinitionOptions.OpenFromTypeDefaultName)]
    public class ShortcutConfigDocument : DocumentBase
    {
        #region Data

        [DataMember()]
        public object o { get; set; }

        #endregion

        //public static DocumentBase ToObject(string fileName)
        //{
        //    return DocumentBase.ToObject(fileName, typeof(ShortcutConfigDocument));
        //}
    }
}


