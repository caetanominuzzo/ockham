using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.IO;
using System.Collections.Generic;
using primeira.Editor;

namespace primeira.Editor
{
    [DataContract()]
    [DocumentDefinition(Name = "File Browser Configuration",
                DefaultFileName = "default",
                Description = "File & Tab Operations",
                DefaultFileExtension = ".filebrowser",
                DefaultEditor = typeof(FileBrowserEditor),
                FriendlyName="File Tab",
                Options = (DocumentDefinitionOptions.DoNotShowLabelAndFixWidth | DocumentDefinitionOptions.TimerSaver | DocumentDefinitionOptions.NeverClose | DocumentDefinitionOptions.OpenFromTypeDefaultName))]
    public class FileBrowserDocument : DocumentBase
    {
        private List<string> _recent = new List<string>();

        public void AddRecent(string fileName)
        {
            if (!_recent.Contains(fileName))
                _recent.Add(fileName);
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
    }
}
