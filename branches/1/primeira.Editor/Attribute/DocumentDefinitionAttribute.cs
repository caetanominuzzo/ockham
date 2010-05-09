using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace primeira.Editor
{
    /// <summary>
    /// Defines a class as a document.
    /// The class must descend from DocumentBase in order to work properly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DocumentDefinitionAttribute : Attribute
    {
        /// <summary>
        /// The name of the document. Eg.: "Text File", it will appear as "Text File Document".
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The description of the document. Eg.: "Simple plain text."
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The default name of the document file. Eg.: "NoName". Used with DefaultFileExtension.
        /// </summary>
        public string DefaultFileName { get; set; }
        /// <summary>
        /// The default extension of the document file. Eg.: ".txt". Used with DefaultFileExtension.
        /// Must contain the initial dot.
        /// </summary>
        public string DefaultFileExtension { get; set; }
        /// <summary>
        /// Defines the options of the document.
        /// See DocumentDefinitionOptions.
        /// </summary>
        public DocumentDefinitionOptions Options { get; set; }
        /// <summary>
        /// The icon of the document. 
        /// </summary>
        public string IconResourceFile { get; set; }
        /// <summary>
        /// Sets type of the default editor for this document.
        /// </summary>
        public Type DefaultEditor { get; set; }
        /// <summary>
        /// Defines a mask to hide the real file name. Eg.: "Revisions".
        /// To concat the original file name use: "Revisions of %". The percent symbol will be replaced by the real file name becomimg: "Revisions of Noname 1.txt". 
        /// </summary>
        public string FriendlyName { get; set; }
    }


    [Flags()]
    public enum DocumentDefinitionOptions
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
