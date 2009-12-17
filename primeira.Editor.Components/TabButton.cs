using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.IO;
using primeira.Editor;

namespace primeira.Editor.Components
{
    public class TabButton : UserControl, ITabButton
    {

        private IEditor _editor;

        private IContainer components;
        private string _tabTitle;
        private Image[,] _imgs;
       

        public string TabTitle
        {
            get { return _tabTitle; }
        }

        public void SetWidth(int width)
        {
            if(Parent != null)
                this.Parent.SuspendLayout();

            this.Width = width;

            if (Parent != null) 
                this.Parent.ResumeLayout(false);
        }

        public TabButton(IEditor editor)
            : base()
        {
            
            _imgs = new Image[2, 3];
            _imgs[0, 0] = Image.FromFile(@"D:\Desenv\Ockham\branches\1\primeira.Editor.Components\tab\left_unselected.png");
            _imgs[0, 1] = Image.FromFile(@"D:\Desenv\Ockham\branches\1\primeira.Editor.Components\tab\unselected.png");
            _imgs[0, 2] = Image.FromFile(@"D:\Desenv\Ockham\branches\1\primeira.Editor.Components\tab\right_unselected.png");
            _imgs[1, 0] = Image.FromFile(@"D:\Desenv\Ockham\branches\1\primeira.Editor.Components\tab\left_selected.png");
            _imgs[1, 1] = Image.FromFile(@"D:\Desenv\Ockham\branches\1\primeira.Editor.Components\tab\selected.png");
            _imgs[1, 2] = Image.FromFile(@"D:\Desenv\Ockham\branches\1\primeira.Editor.Components\tab\right_selected.png");
            
            InitializeComponent();

            this._editor = editor;

            this.Cursor = Cursors.Hand;

            this.Width = 150;
            this.MaximumSize = new Size(150, 25);
            this.MinimumSize = new Size(40, 25);
            this.Margin = new Padding(2, this.Margin.Top, 0, this.Margin.Bottom);
            this.TabStop = true;
            
        }

        void childs_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // TabButton
            // 
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Name = "TabButton";
            this.Size = new System.Drawing.Size(140, 110);
            this.ResumeLayout(false);

        }

        private int _iSelectedOffSet = 0;
        private bool _hideLabel;
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;

            _iSelectedOffSet = Convert.ToInt16(this._editor.Selected);

            e.Graphics.DrawImage(_imgs[_iSelectedOffSet, 0], 0, 0, 5, _imgs[_iSelectedOffSet, 0].Height);
            e.Graphics.DrawImage(_imgs[_iSelectedOffSet, 1], 5, 0, (int)((this.Width - 10) * 1.33), _imgs[_iSelectedOffSet, 1].Height);
            e.Graphics.DrawImage(_imgs[_iSelectedOffSet, 2], this.Width - 5, 0, 5, _imgs[_iSelectedOffSet, 2].Height);

            _hideLabel = ((this._editor.Document.Definition.Options & DocumentDefinitionOptions.DontShowLabel) == DocumentDefinitionOptions.DontShowLabel) || this.Width == 49;

            e.Graphics.DrawImageUnscaled(this._editor.Document.Definition.Icon, _hideLabel? 12 : 6, 4);
            
            if(!this._hideLabel)
                e.Graphics.DrawString(MeasureFromIDC(), _font, SystemBrushes.ControlText, 21, 5);
        }

        private ToolTip _toolTip;

        public void SetText(string filename)
        {
            _toolTip.SetToolTip(this, filename);
            this._tabTitle = filename;
        }

        //Needed to avoid creating graphics dynamically. Used in MeasureFromIDC below.
        private static Control _control;
        private static Graphics _graphics;
        private static Font _font;
        public string MeasureFromIDC()
        {
            if (_control == null)
            {
                _control = new Control();
                _font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                _graphics = _control.CreateGraphics();
            }

            string value = _tabTitle;

            Size Size = new Size(
                        this.Width - 20,
                        this.Height); 

            char[] ss = new char[value.Length];

            value.CopyTo(0, ss, 0, value.Length);

            string s = new string(ss);
            s = s.Replace(Path.GetExtension(s), "");

            TextFormatFlags t = TextFormatFlags.ModifyString | TextFormatFlags.PathEllipsis | TextFormatFlags.SingleLine | TextFormatFlags.TextBoxControl | TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;

            TextRenderer.MeasureText(_graphics, s, _font, Size, t);
            int i = s.IndexOf("\0");
            if (i > -1)
                s = s.Substring(0, i);

            return s;
        }

    }
}
