using System;
using System.Drawing;

namespace primeira.Editor
{
        
    public class DocumentDefinition
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DefaultFileName { get; set; }
        public string DefaultFileExtension { get; set; }
        public DocumentDefinitionOptions Options { get; set; }
        public Image Icon { get; set; }
        public Type DefaultEditor { get; set; }
    }


    [Flags()]
    public enum DocumentDefinitionOptions
    {
        None = 0,
        //Hide the Tab Title and fix the width
        DontShowLabel =1,
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
        //Indicate this is the odd TabControl editor
        TabControl = 64,
        //Default option for users editors
        UserFile = NeverClose | TimerSaver | ShowInRecents | ShowInQuickLauchDraft | ShowIQuickLauchnOpen,
    }
}
