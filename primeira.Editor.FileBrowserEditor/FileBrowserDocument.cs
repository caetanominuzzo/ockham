using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.IO;
using System.Collections.Generic;
using primeira.Editor;

namespace primeira.Editor
{
    [DataContract()]
    public class FileBrowserDocument : DocumentBase
    {
        private static DocumentDefinition _definition =
            new DocumentDefinition()
            {
                Name = "File Browser Configuration",
                DefaultFileName = "default",
                Description = "File & Tab Operations",
                DefaultFileExtension = ".filebrowser",
                Id = new Guid("513ff96c-0d23-44f4-82ab-0dea5a62dcd3"),
                Icon = Image.FromFile(@"D:\Desenv\Ockham\branches\1\primeira.Editor.FileBrowserEditor\Resources\folder_noborder.gif"),
                DefaultEditor = typeof(FileBrowserEditor),
                Options = (DocumentDefinitionOptions.DontShowLabel | DocumentDefinitionOptions.TimerSaver | DocumentDefinitionOptions.NeverClose | DocumentDefinitionOptions.OpenFromTypeByDefaultName)
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

        private List<string> _recent = new List<string>();

        public void AddRecent(string filename)
        {
            if (!_recent.Contains(filename))
                _recent.Add(filename);
        }

        [DataMember()]
        public string[] Recent
        {
            get { return _recent.ToArray(); }
            set
            {
                if (_recent == null)
                    _recent = new List<string>(value.Length);
                else
                    _recent.Clear();

                _recent.AddRange(value);
            }

        }

        #endregion

        public static DocumentBase ToObject(string filename)
        {
            return DocumentBase.ToObject(filename, typeof(FileBrowserDocument));
        }

    }
}
