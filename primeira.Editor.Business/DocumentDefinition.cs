using System;
using System.Drawing;

namespace primeira.Editor.Business
{
        
    public class DocumentDefinition
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DefaultName { get; set; }
        public string Extension { get; set; }
        public DocumentDefinitionOptions Options { get; set; }
        public Image Icon { get; set; }
        public Type DefaultEditor { get; set; }

        public DocumentDefinition() { }
    }

    [Flags()]
    public enum DocumentDefinitionOptions
    {
        None = 0,
        DontShowLabel =1,
        TimerSaver = 2,
        KeepOnCloseTabs = 4,
        ShowInRecents = 8,
        ShowInQuickLauchDraft = 16,
        ShowIQuickLauchnOpen = 32,
        TabControl = 64,
        UserFile = KeepOnCloseTabs | TimerSaver | ShowInRecents | ShowInQuickLauchDraft | ShowIQuickLauchnOpen,
    }
}
