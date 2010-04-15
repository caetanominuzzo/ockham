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
                TabTitle="File Tab",
                Options = (DocumentDefinitionOptions.DontShowLabelAndFixWidth | DocumentDefinitionOptions.TimerSaver | DocumentDefinitionOptions.NeverClose | DocumentDefinitionOptions.OpenFromTypeDefaultName))]
    public class FileBrowserDocument : DocumentBase
    {
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
    }
}
