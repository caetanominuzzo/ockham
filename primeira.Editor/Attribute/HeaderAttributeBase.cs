using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace primeira.Editor
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class HeaderAttributeBase : Attribute
    {
        /// <summary>
        /// Gets or sets the id of the document. 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the version of the document. 
        /// </summary>
        public string VersionNumber { get; set; }
        /// <summary>
        /// Gets or sets the author of the document header
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// Gets or sets an info of the document header. 
        /// </summary>
        public string Info { get; set; }
        /// <summary>
        /// Gets or sets the email of the author of the document header.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the website of the document header.
        /// </summary>
        public string WebSite { get; set; }
        /// <summary>
        /// Gets or sets the auto update url of the document header.
        /// </summary>
        public string AutoUpdateUrl { get; set; }
        /// <summary>
        /// Gets or sets the name of the document. Eg.: "Text File", it will appear as "Text File Document".
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the description of the document. Eg.: "Simple plain text."
        /// </summary>
        public string Description { get; set; }
    }
}
