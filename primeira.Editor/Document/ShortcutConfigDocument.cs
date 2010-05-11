using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace primeira.Editor
{
    [DataContract()]
    [DocumentHeader(
                Id = "0E284BCE-E907-414F-9C4D-241546052626",
                VersionNumber = "1.0",                 
                Name = "Shortcut Manager",
                DefaultFileName = "default",
                Description = "Shortcut Manager",
                DefaultFileExtension = ".shortcut",
                FriendlyNameMask= "Shortcuts"
                //, DefaultEditor = typeof(ShortcutConfigEditor)
                , Options=DocumentHeaderOptions.OpenFromTypeDefaultName
                )]
    public class ShortcutConfigDocument : DocumentBase
    {
        
        #region Data

        [DataMember()]
        public object o { get; set; }

        //[DataMember()]
        //List<Shortcut> Shortcuts

        #endregion



    }
}


