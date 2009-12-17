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

        private bool _showCloseButton = true;

        #endregion

        #region Properties

        public string Filename { get; set; }

        public ITabButton TabButton { get; private set; }

        public bool ShowCloseButton
        {
            get { return _showCloseButton; }
            protected set { _showCloseButton = value; }
        }

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

                if (TabButton != null)
                    this.TabButton.Invalidate();

                if (_selected && OnSelected != null)
                    OnSelected(this);
            }
        }

        #endregion

        #region Ctor

        public EditorBase()
        {
        }
      
        public EditorBase(string filename, DocumentBase data, Type documentType)
        {
            if (data == null)
                this.Document = (DocumentBase)documentType.GetConstructor(System.Type.EmptyTypes).Invoke(System.Type.EmptyTypes);
            else
                this.Document = data;

            this.Filename = filename;

            this.Dock = DockStyle.Fill;

            this.BorderStyle = BorderStyle.None;

            this.AutoScroll = true;

            this.Font = new Font("SegoeUI", 9);

            this.BackColor = Color.White;

            this.OnChanged += new ChangedDelegate(EditorBase_OnChanged);

            InitializeComponent();

        }

        private void InitializeComponent()
        {
            if ((Document.Definition.Options & DocumentDefinitionOptions.TimerSaver) == DocumentDefinitionOptions.TimerSaver)
            {
                _saveTimer = new Timer();
                _saveTimer.Interval = 1000;
                _saveTimer.Tick += new EventHandler(_saveTimer_Tick);
            }



            if (TabManager.GetInstance().TabControl != null)
            {
                TabButton = (TabButton)TabManager.GetInstance().CreateTabButton(this);

                if ((this.Document.Definition.Options & DocumentDefinitionOptions.DontShowLabel) == DocumentDefinitionOptions.DontShowLabel)
                {
                    this.TabButton.SetWidth(40);
                }

                this.TabButton.SetText(this.Filename);

                TabButton.Click += new EventHandler(TabButton_Click);
            }
        }

        #endregion

        #region Methods

        public void PrepareToClose()
        {

            //Since if !DocumentDefinitionOptions.Virtual there is not _timer set;
            if (_saveTimer != null)
                _saveTimer.Stop();

            DocumentManager.ToXml(this.Document, this.Filename);
        }

        #endregion

        #region Event Handlers

        private void _saveTimer_Tick(object sender, EventArgs e)
        {
            //Since if !DocumentDefinitionOptions.Virtual there is not _timer set;
            if (_saveTimer != null)
                _saveTimer.Stop();

            DocumentManager.ToXml(this.Document, this.Filename);
        }

        private void TabButton_Click(object sender, EventArgs e)
        {
            this.Selected = true;
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
