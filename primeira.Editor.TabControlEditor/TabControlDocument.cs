using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace primeira.Editor
{
    [DataContract()]
    public class TabControlDocument : DocumentBase
    {
        private static DocumentDefinition _definition =
            new DocumentDefinition()
            {
                Name = "Tab Control",
                DefaultFileName = "default",
                Description = "Tab Control",
                DefaultFileExtension = ".tabcontrol",
                Id = new Guid("513ff96c-0d23-44f4-82ab-0dea5a62dcd3"),
                DefaultEditor = typeof(TabControlEditor),
                Options = DocumentDefinitionOptions.TabControl,
            };

        public static DocumentDefinition DocumentDefinition
        {
            get { return _definition; }
        }

        public override DocumentDefinition Definition
        {
            get { return _definition; }
        }

        #region Data

        private IEnumerable<string> _openEditors;
        private string _selectedTab;

        [DataMember()]
        public string[] OpenEditors
        {
            get { return (from x in TabManager.GetInstance().GetEditorByOptions(DocumentDefinitionOptions.NeverClose) select x.Filename).ToArray(); }
            set { _openEditors = value; }
        }

        [DataMember()]
        public string SelectedTab
        {
            get { return TabManager.GetInstance().ActiveEditor.Filename; }
            set { _selectedTab = value; }
        }

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

        #endregion

        public static DocumentBase ToObject(string filename)
        {
            return DocumentBase.ToObject(filename, typeof(TabControlDocument));
        }
    }
}


