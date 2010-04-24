using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace primeira.Editor
{
    internal class TabControlManager : IEditorContainer
    {
        #region Singleton

        private TabControlManager() { }

        private static TabControlManager _tabControlManager;

        public static TabControlManager GetInstance()
        {
            if (_tabControlManager == null)
                _tabControlManager = new TabControlManager();

            return _tabControlManager;
        }

        #endregion

        #region Parent Control

        public Control ParentControl
        {
            get;
            internal set;
        }

        #endregion

        #region TabControl

        private TabControlEditor _tabcontrol;

        internal void SetTabControl(TabControlEditor tabcontrol)
        {
            if (_tabcontrol != null)
                MessageManager.Send(MessageSeverity.Fatal,
                    Message_us.TabControlAlreadySet);

            _tabcontrol = tabcontrol;
        }

        public TabControlEditor TabControl
        {
            get { return _tabcontrol; }
        }

        public TabButton CreateTabButton(IEditor editor)
        {
            if (_tabcontrol != null)
            {
                return _tabcontrol.CreateTabButton(editor);
            }

            return null;
        }

        public const int FIXED_SIZE_TABBUTTON_WIDTH = 50;
        public const int MAX_SIZE_TABBUTTON_WIDTH = 200;

        public void RefreshTabButtonSize()
        {
            int iiFixedSizeCount = 0, iiFixedSizeWidth = 0;

            int iiLeftMargin = -1, iiAbsoluteLeftMargin = 4;

            int iiDinamicSizeCount = 0, iiAvalaibleSize = 0, iiDinamicSizeWidth = 0, iiLeftButtons = 0;

            foreach (IEditor editor in _openEditorsDisplayOrder)
            {
                iiFixedSizeWidth = iiFixedSizeCount * (FIXED_SIZE_TABBUTTON_WIDTH + iiLeftMargin);

                //if the document is open it must have a doc definition, so no exception handling here.
                if (DocumentManager.GetDocumentDefinition(editor.FileName).Options.HasFlag(DocumentDefinitionOptions.DontShowLabelAndFixWidth)
                {
                    _tabcontrol.TabButton(editor).SetBounds(new Rectangle(iiAbsoluteLeftMargin + iiFixedSizeWidth + iiLeftMargin, 3, FIXED_SIZE_TABBUTTON_WIDTH, 25));

                    iiFixedSizeCount++;

                    continue;
                }

                iiDinamicSizeCount = _openEditors.Count() - iiFixedSizeCount;

                iiAvalaibleSize = this.ParentControl.Width - 70 - iiFixedSizeWidth;

                iiDinamicSizeWidth = Math.Max(FIXED_SIZE_TABBUTTON_WIDTH, Math.Min(MAX_SIZE_TABBUTTON_WIDTH, iiAvalaibleSize / iiDinamicSizeCount));

                _tabcontrol.TabButton(editor).SetBounds(new Rectangle(iiAbsoluteLeftMargin + iiFixedSizeWidth + iiLeftMargin + iiLeftButtons * (iiDinamicSizeWidth + iiLeftMargin), 3, iiDinamicSizeWidth, 25));

                iiLeftButtons++;
            }

        }

        #endregion

        #region ActiveDocument

        private IEditor _activeEditor = null;

        private List<IEditor> _openEditors = new List<IEditor>();
        private List<IEditor> _openEditorsTabOrder = new List<IEditor>();
        private List<IEditor> _openEditorsDisplayOrder = new List<IEditor>();

        private int ipreviousSelectedEditorIndex = -1;

        public IEditor ActiveEditor
        {
            get
            {
                return _activeEditor;
            }
            internal set
            {

                if (ActiveEditor == value)
                    return;

                if (!_openEditors.Contains(value))
                {
                    _openEditors.Add(value);
                    _openEditorsTabOrder.Insert(0, value);

                    if(value.HasOption(DocumentDefinitionOptions.DontShowLabelAndFixWidth))
                        _openEditorsDisplayOrder.Insert(0, value);
                    else
                        _openEditorsDisplayOrder.Add(value);

                    RefreshTabButtonSize();
                }

                //Gets the index of previous active editor
                ipreviousSelectedEditorIndex = _openEditors.IndexOf(ActiveEditor);

                //Sets the new active editor
                _activeEditor = value;

                //unselect previous active editor
                if (ipreviousSelectedEditorIndex != -1)
                    _openEditors[ipreviousSelectedEditorIndex].Selected = false;

                ActiveEditor.Selected = true;

                if (!_delayedZOrderControl)
                    ReleaseDelayedZOrderControl();

                if (ipreviousSelectedEditorIndex != -1)
                    TabControl.TabButton(_openEditors[ipreviousSelectedEditorIndex]).Invalidate();

                TabControl.TabButton(ActiveEditor).BringToFront();

                TabControl.TabButton(ActiveEditor).Invalidate();
            }
        }

        private bool _delayedZOrderControl = false;

        public void ReleaseDelayedZOrderControl()
        {
            //To control z-order
            _openEditorsTabOrder.Remove(ActiveEditor);
            _openEditorsTabOrder.Insert(0, ActiveEditor);
            _delayedZOrderControl = false;
        }

        public void StartDelayedZOrderControl()
        {
            _delayedZOrderControl = true;
        }

        public bool DelayedZOrderControl
        {
            get { return _delayedZOrderControl; }
        }

        #endregion

        #region Open, Close & Select

        internal void CloseEditor()
        {
            if (_openEditors.Count == 0)
                return;

            int i = 0;

            IEditor editor;

            while(true)
            {
                editor = _openEditors[i];

                if(editor.HasOption(DocumentDefinitionOptions.NeverClose))
                    i++;
                else
                    CloseEditor(editor);

                if(i == _openEditors.Count)
                    break;
            }
        }

        public bool CloseEditor(IEditor editor)
        {

            if (editor != null && !editor.HasOption(DocumentDefinitionOptions.NeverClose))
            {
                _tabcontrol.SaveCloseEditor(editor);

                if (editor == ActiveEditor)
                {
                    SelectNext();
                    ReleaseDelayedZOrderControl();
                }

                _openEditors.Remove(editor);
                _openEditorsDisplayOrder.Remove(editor);
                _openEditorsTabOrder.Remove(editor);

                RefreshTabButtonSize();
            }

            return true;
        }

        public void SelectNext()
        {
            StartDelayedZOrderControl();

            int i = _openEditorsTabOrder.IndexOf(ActiveEditor);

            if (i == _openEditorsTabOrder.Count - 1)
                i = 0;
            else i++;

            ActiveEditor = _openEditorsTabOrder[i];
        }

        public void SelectPrior()
        {
            StartDelayedZOrderControl();
            int i = _openEditorsTabOrder.IndexOf(ActiveEditor);

            if (i == 0)
                i = _openEditorsTabOrder.Count - 1;
            else i--;

            ActiveEditor = _openEditorsTabOrder[i];
        }

        public IEditor GetOpenEditor(string fileName)
        {
            foreach (IEditor d in _openEditors)
            {
                if (d.FileName.Equals(fileName))
                    return d;
            }

            return null;
        }

        #endregion

        public IEditor GetEditor(DocumentBase document)
        {
            return (from c in this._openEditors where c.Document == document select c).First();
        }

        public void LoadEditor(IEditor editor)
        {
            //Dont loads itself
            if (editor is TabControlEditor)
                return;

            //Verify if the file is already open
            IEditor res = EditorContainerManager.GetOpenEditor(editor.FileName);

            if (res != null)
                TabControlManager.GetInstance().ActiveEditor = editor;
            else
                _tabcontrol.AddEditor(editor);
        }
    }
}
