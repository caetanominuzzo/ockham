using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace primeira.Editor.Business
{
    public class TabManager
    {
        #region Singleton

        private TabManager() { }

        private static TabManager _tabManager;

        public static TabManager GetInstance()
        {
            if (_tabManager == null)
                _tabManager = new TabManager();

            return _tabManager;
        }

        #endregion

        #region TabControl

        private ITabControl _tabcontrol;

        public void SetTabControl(ITabControl tabcontrol)
        {
            _tabcontrol = tabcontrol;
        }

        public ITabControl TabControl
        {
            get { return _tabcontrol; }
        }

        public ITabButton CreateTabButton(IEditor editor)
        {
            if (_tabcontrol != null)
            {
                return _tabcontrol.CreateTabButton(editor);
            }

            return null;
        }

        #endregion

        #region ActiveDocument

        private IEditor _activeEditor = null;

        private List<IEditor> _openEditors = new List<IEditor>();

        int iChangeEditorIndex = -1;

        public IEditor ActiveEditor
        {
            get
            {
                return _activeEditor;
            }
            internal set
            {
                if (!_openEditors.Contains(value))
                    _openEditors.Insert(0, value);
                else
                {
                    //To control z-order
                    _openEditors.Remove(value);
                    _openEditors.Insert(0, value);
                }

                iChangeEditorIndex = _openEditors.IndexOf(ActiveEditor);

                _activeEditor = value;

                if (iChangeEditorIndex != -1)
                    _openEditors[iChangeEditorIndex].Selected = false;
                
            }
        }

        #endregion

        #region Open, Close & Select

        public void CloseEditor()
        {
            if (ActiveEditor != null && ActiveEditor.ShowCloseButton)
            {
                _tabcontrol.HideTab(ActiveEditor);
                DocumentManager.ToXml(ActiveEditor.Document, ActiveEditor.Filename);
                _tabcontrol.CloseHideTabs();

                _openEditors.Remove(ActiveEditor);

                //One more than filebrowser
                if (_openEditors.Count > 1)
                    _openEditors[1].Selected = true;
                else
                    _openEditors[0].Selected = true;
            }
        }

        public void SelectNext()
        {
            if (_openEditors.Count > 1)
                _openEditors[1].Selected = true;
        }
        
        public void SelectPrior()
        {
            if (_openEditors.Count > 1)
                _openEditors[_openEditors.Count-1].Selected = true;
        }

        /// <summary>
        /// Get documents eventually already open in any open editor.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public IEditor GetDocumentByFilename(string filename)
        {
            foreach (IEditor d in _openEditors)
            {
                if (d.Filename.Equals(filename))
                    return d;
            }

            return null;
        }

        public IEnumerable<IEditor> GetEditorByOptions(DocumentDefinitionOptions options)
        {
            var x = (from c in this._openEditors where (c.Document.Definition.Options & options) == options select (IEditor)c); 
            return x;
        }

        #endregion



    }
}
