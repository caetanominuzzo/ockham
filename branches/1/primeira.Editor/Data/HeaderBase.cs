using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace primeira.Editor
{
    [DataContract()]
    public class HeaderBase : IExtensibleDataObject
    {
        /// <summary>
        /// Gets or sets the version of the document.
        /// </summary>
        [DataMember()]
        public VersionData Version { get; set; }
        /// <summary>
        /// Gets or sets the name of the document. Eg.: "Text File", it will appear as "Text File Document".
        /// </summary>
        [DataMember()]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the description of the document. Eg.: "Simple plain text."
        /// </summary>
        [DataMember()]
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the auto update url of the document header.
        /// </summary>
        [DataMember()]
        public string AutoUpdateUrl { get; set; }

        public Type BaseType { get; internal set; }

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

        public HeaderBase GetHeaderBaseFromReflection(HeaderAttributeBase attrib)
        {
            this.Name = attrib.Name;

            this.Description = attrib.Description;

            this.AutoUpdateUrl = attrib.AutoUpdateUrl;

            this.Version = new VersionData(attrib.Id, attrib.VersionNumber, attrib.Author, attrib.Info, attrib.Email, attrib.WebSite);

            return this;
        }
    }
}
