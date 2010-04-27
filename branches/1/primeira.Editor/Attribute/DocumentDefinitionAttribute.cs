using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace primeira.Editor
{

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DocumentDefinitionAttribute : Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string DefaultFileName { get; set; }
        public string DefaultFileExtension { get; set; }
        public DocumentDefinitionOptions Options { get; set; }
        public Image Icon { get; set; }
        public Type DefaultEditor { get; set; }
        public string TabTitle { get; set; }
    }


    [Flags()]
    public enum DocumentDefinitionOptions
    {
        None = 0,
        //Hide the Tab Title and fix the width
        DoNotShowLabelAndFixWidth =1,
        //Enables autosave feature
        TimerSaver = 2,
        //Never close tab
        NeverClose = 4, 
        //Shown in recents list
        ShowInRecents = 8,
        //Shown in quick launch draft list
        ShowInQuickLauchDraft = 16,
        //Shown in quick launch ppen list
        ShowIQuickLauchnOpen = 32,
        //Used for open default.tabcontrol and default.filebrowser
        OpenFromTypeDefaultName = 128,
        //The document must implement is own ToObject & ToXml methods
        CustomSerialization = 256,
        //Default option for users editors
        UserFile = TimerSaver | ShowInRecents | ShowInQuickLauchDraft | ShowIQuickLauchnOpen,
    }
}
