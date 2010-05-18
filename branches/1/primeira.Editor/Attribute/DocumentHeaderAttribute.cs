using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace primeira.Editor
{
    /// <summary>
    /// Defines a class as a document.
    /// The class must descend from DocumentBase in order to work properly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DocumentHeaderAttribute : HeaderAttributeBase
    {
        /// <summary>
        /// Gets or sets the default name of the document file. Eg.: "NoName". Used with DefaultFileExtension.
        /// </summary>
        public string DefaultFileName { get; set; }
        /// <summary>
        /// Gets or sets the default extension of the document file. Eg.: ".txt". Used with DefaultFileExtension.
        /// Must contain the initial dot.
        /// </summary>
        public string DefaultFileExtension { get; set; }
        /// <summary>
        /// Gets or sets the options of the document.
        /// See DocumenHeaderOptions.
        /// </summary>
        public DocumentHeaderOptions Options { get; set; }
        /// <summary>
        /// Gets or sets a mask to hide the real file name. Eg.: "Revisions".
        /// To concat the original file name use: "Revisions of %". The percent symbol will be replaced by the real file name becomimg: "Revisions of Noname 1.txt". 
        /// </summary>
        public string FriendlyNameMask { get; set; }
        /// <summary>
        /// Gets or sets the name of the icon in the assembly resources.
        /// </summary>
        public string IconResourceFile { get; set; }
    }


    [Flags()]
    public enum DocumentHeaderOptions
    {
        None = 0,
        /// <summary>
        /// Hide the Tab Title and fix the width 
        /// </summary>
        DoNotShowLabelAndFixWidth =1,
        /// <summary>
        /// Enables autosave feature
        /// </summary>
        TimerSaver = 2,
        /// <summary>
        /// Never close tab
        /// </summary>
        NeverClose = 4,
        /// <summary>
        /// Shown in recents list
        /// </summary>
        ShowInRecents = 8,
        /// <summary>
        /// Shown in quick launch draft list
        /// </summary>
        ShowInQuickLauchDraft = 16,
        /// <summary>
        /// Shown in quick launch ppen list
        /// </summary>
        ShowIQuickLauchnOpen = 32,
        /// <summary>
        /// Used for open default.tabcontrol and default.filebrowser
        /// </summary>
        OpenFromTypeDefaultName = 128,
        /// <summary>
        /// The document must implement is own ToObject method
        /// </summary>
        CustomSerializationRead = 256,
        /// <summary>
        /// The document must implement is own ToXml method
        /// </summary>
        CustomSerializationWrite = 512,
        /// <summary>
        /// The document must implement is own ToObject & ToXml methods
        /// </summary>
        CustomSerialization = CustomSerializationRead | CustomSerializationWrite,
        /// <summary>
        /// Default option for users editors
        /// </summary>
        UserFile = TimerSaver | ShowInRecents | ShowInQuickLauchDraft | ShowIQuickLauchnOpen,
    }
}
