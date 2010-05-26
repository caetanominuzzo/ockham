using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using primeira.Editor;

namespace primeira.Editor.Components
{
    public partial class EditorBase : UserControl, IEditor, IShorcutEscopeProvider
    {
        #region Fields

        private bool _selected = false;

        private Timer _saveTimer;

        #endregion

        #region Properties

        public string FileName { get; set; }

        public DocumentBase Document { get; protected set; }

        public bool Selected
        {
            get { return _selected; }
            set
            {
                if (_selected == value)
                    return;

                _selected = value;

                if (value)
                    BringToFront();
                
                Invalidate();

                if (_selected && OnSelected != null)
                    OnSelected(this);
            }
        }

        public string DefaultFileName
        {
            get
            {
                return Header.DefaultFileName + Header.DefaultFileExtension;
            }
        }

        public DocumentHeader Header { get; protected set; }

        #endregion

        #region Ctor

        public EditorBase()
        {
            InitializeComponent();

            this.Dock = DockStyle.Fill;

            this.BorderStyle = BorderStyle.None;

            this.AutoScroll = true;

            this.BackColor = Color.White;

            this.OnChanged += new ChangedDelegate(EditorBase_OnChanged);


        }

        public EditorBase(string fileName)
            : this()
        {
            this.FileName = fileName;

            this.Header = DocumentManager.GetDocumentHeader(fileName);

            this.Document = DocumentManager.LoadDocument(this.Header, this.FileName);
        }

        #endregion

        #region Methods

        protected void Close()
        {
            EditorContainerManager.CloseEditor(this);
        }

        public void PrepareToClose()
        {
            if (_saveTimer != null)
                _saveTimer.Dispose();

            DocumentManager.SaveDocument(this.Document, this.FileName);
        }

        public bool HasOption(DocumentHeaderOptions Option)
        {
            return this.Header.Options.HasFlag(Option);
        }

        #endregion

        #region Event Handlers

        private void EditorBase_Load(object sender, EventArgs e)
        {
            try //DesignTime problem
            {
                if (HasOption(DocumentHeaderOptions.TimerSaver))
                {
                    _saveTimer = new Timer();
                    _saveTimer.Interval = 1000;
                    _saveTimer.Tick += new EventHandler(_saveTimer_Tick);
                }

                this.SelectNextControl(this, true, true, true, false);


                ShortcutManager.LoadFromForm(this);
            }
            catch { }
        }

        private void _saveTimer_Tick(object sender, EventArgs e)
        {
            if (_saveTimer != null)
                _saveTimer.Stop();

            DocumentManager.SaveDocument(this.Document, this.FileName);
        }

        private void EditorBase_OnChanged()
        {
            if (_saveTimer != null)
            {
                _saveTimer.Stop();
                _saveTimer.Start();
            }
        }

        #endregion

        #region Events

        public event SelectedDelegate OnSelected;
        
        public delegate void ChangedDelegate();
        public event ChangedDelegate OnChanged;

        protected virtual void Changed()
        {
            if (OnChanged != null)
                OnChanged();
        }

        #endregion

        #region IShorcutEscopeProvider Members

        public bool IsAtiveByEscope(string escope)
        {
            if (escope.Equals(BasicEscopes.Active))
                return (EditorContainerManager.IsActive(this));

            else return false;
        }

        #endregion
    }
}
