using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.IO;
using System.Collections.Generic;
using primeira.Editor;

namespace primeira.Editor
{
    [DataContract()]
    [DocumentHeader(
                Id = "A9ED003E-992E-4952-A5D1-39EF2766A48F",
                VersionNumber = "1.0",
                Name = "File Browser Configuration",
                DefaultFileName = "default",
                Description = "File & Tab Operations",
                DefaultFileExtension = ".filebrowser",
                FriendlyNameMask="File Tab",
                Options = (DocumentHeaderOptions.DoNotShowLabelAndFixWidth | DocumentHeaderOptions.TimerSaver | DocumentHeaderOptions.NeverClose | DocumentHeaderOptions.OpenFromTypeDefaultName))]
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
