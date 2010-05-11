using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Drawing;

namespace primeira.Editor
{
    [DataContract()]
    public class DocumentHeader : IExtensibleDataObject
    {
        [DataMember()]
        public Version DocumentVersion { get; set; }

        [DataMember()]
        public DocumentHeaderAttribute Attributes { get; set; }

        [DataMember()]
        public VersionFilter DefaultEditorVersion { get; set; }

        public Type DocumentType { get; set; }

        public EditorHeader DefaultEditor { get; set; }
        
        public Image Icon { get; internal set; }

        #region IExtensibleDataObject Members

        private ExtensionDataObject _extensionDataObject;

        public ExtensionDataObject ExtensionData
        {
            get
            {
                return _extensionDataObject;
            }
            set
            {
                _extensionDataObject = value;
            }
        }

        #endregion
    }
}
