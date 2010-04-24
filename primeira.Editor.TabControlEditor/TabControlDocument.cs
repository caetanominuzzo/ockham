using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

namespace primeira.Editor
{
    [DataContract()]
    [DocumentDefinition(Name = "Tab Control",
                DefaultFileName = "default",
                Description = "Tab Control",
                DefaultFileExtension = ".tabcontrol",
                DefaultEditor = typeof(TabControlEditor),
                Options = DocumentDefinitionOptions.OpenFromTypeDefaultName)]
    public class TabControlDocument : DocumentBase
    {
        private IEnumerable<string> _openEditors;
        private string _selectedTab;

        //TODO:Serialize members
        //[DataMember()]
        //public string[] OpenEditors
        //{
        //    get { return (from x in TabManager.GetInstance().GetOpenEditorsByOptions(DocumentDefinitionOptions.NeverClose) select x.FileName).ToArray(); }
        //    set { _openEditors = value; }
        //}

        //[DataMember()]
        //public string SelectedTab
        //{
        //    get { return TabManager.GetInstance().ActiveEditor.FileName; }
        //    set { _selectedTab = value; }
        //}

        public string GetSelectedTab()
        {
            return _selectedTab;
        }

        public IEnumerable<string> GetOpenTabsFilename()
        {
            if(_openEditors == null)
                _openEditors = new string[0];

            return _openEditors.Cast<string>();
        }
   }
}


