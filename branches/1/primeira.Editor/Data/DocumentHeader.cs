using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Drawing;

namespace primeira.Editor
{
    [DataContract()]
    public class DocumentHeader : HeaderBase
    {
        /// <summary>
        /// Gets or sets the version of the default editor for the document.
        /// </summary>
        [DataMember()]
        public VersionFilter DefaultEditorVersion { get; set; }
        /// <summary>
        /// Gets or sets the default name of the document file. Eg.: "NoName". Used with DefaultFileExtension.
        /// </summary>
        [DataMember()]
        public string DefaultFileName { get; set; }
        /// <summary>
        /// Gets or sets the default extension of the document file. Eg.: ".txt". Used with DefaultFileExtension.
        /// Must contain the initial dot.
        /// </summary>
        [DataMember()]
        public string DefaultFileExtension { get; set; }
        /// <summary>
        /// Gets or sets the options of the document.
        /// See DocumenHeaderOptions.
        /// </summary>
        [DataMember()]
        public DocumentHeaderOptions Options { get; set; }
        /// <summary>
        /// Gets or sets a mask to hide the real file name. Eg.: "Revisions".
        /// To concat the original file name use: "Revisions of %". The percent symbol will be replaced by the real file name becomimg: "Revisions of Noname 1.txt". 
        /// </summary>
        [DataMember()]
        public string FriendlyNameMask { get; set; }

        public EditorHeader DefaultEditor { get; internal set; }

        public Image Icon { get; internal set; }
    }
}
