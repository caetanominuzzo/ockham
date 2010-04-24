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
    public class EditorBase : UserControl, IEditor
    {
        #region Fields

        private bool _selected = false;

        private Timer _saveTimer;

        #endregion

        #region Properties

        public string FileName { get; set; }

        public DocumentBase Document { get; private set; }

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

        public Type DocumentType { get; private set; }

        public string DefaultFileName
        {
            get
            {
                DocumentDefinitionAttribute dd = DocumentManager.GetDocumentDefinition(this.Document.GetType());
                return dd.DefaultFileName + dd.DefaultFileExtension;
            }
        }

        #endregion

        #region Ctor

        public EditorBase()
        {
            this.Dock = DockStyle.Fill;

            this.BorderStyle = BorderStyle.None;

            this.AutoScroll = true;

            this.Font = new Font("SegoeUI", 9);

            this.BackColor = Color.White;

            this.OnChanged += new ChangedDelegate(EditorBase_OnChanged);
        }

        public EditorBase(string fileName) : this()
        {
            this.DocumentType = DocumentManager.GetDocumentType(fileName);

            this.Document = DocumentManager.LoadDocument(this.DocumentType, fileName);

            this.FileName = fileName;

            if (HasOption(DocumentDefinitionOptions.TimerSaver))
            {
                _saveTimer = new Timer();
                _saveTimer.Interval = 1000;
                _saveTimer.Tick += new EventHandler(_saveTimer_Tick);
            }
        }
        
        #endregion

        #region Methods

        protected void Close()
        {
            EditorContainerManager.CloseEditor(this);
        }

        public void PrepareToClose()
        {
            //if !DocumentDefinitionOptions.Virtual there is not _timer set;
            if (_saveTimer != null)
                _saveTimer.Dispose();

            DocumentManager.ToXml(this.Document, this.FileName);
        }

        public bool HasOption(DocumentDefinitionOptions Option)
        {
            return (DocumentManager.GetDocumentDefinition(this.Document.GetType()).Options & Option) > 0;
        }

        #endregion

        #region Event Handlers

        private void _saveTimer_Tick(object sender, EventArgs e)
        {
            //Since if !DocumentDefinitionOptions.Virtual there is not _timer set;
            if (_saveTimer != null)
                _saveTimer.Stop();

            DocumentManager.ToXml(this.Document, this.FileName);
        }

        private void EditorBase_OnChanged()
        {
            //Since if !DocumentDefinitionOptions.Virtual there is not _timer set;
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
    }
}
