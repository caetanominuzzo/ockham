using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace primeira.Editor
{
    [DataContract()]
    [DocumentDefinition(Name = "Shortcut Manager",
                DefaultFileName = "default",
                Description = "Shortcut Manager",
                DefaultFileExtension = ".shortcut",
                FriendlyName= "Shortcuts",
                DefaultEditor = typeof(ShortcutConfigEditor))]
    public class ShortcutConfigDocument : DocumentBase
    {
        #region Data

        [DataMember()]
        public object o { get; set; }

        #endregion

    }
}


