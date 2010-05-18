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
using System.Runtime.InteropServices;

namespace primeira.Editor
{
    public class TabButton : UserControl
    {

        private IEditor _editor;

        private Image[,] _imgs;


        public string TabTitle { get; private set; }
        public string FileName { get; private set; }
        public string FriendlyName { get; private set; }

        public void SetBounds(Rectangle bounds)
        {
            if (TabControlManager.GetInstance().MainForm.Width - 60 < bounds.Left + bounds.Width)
                bounds = new Rectangle(0, 0, 0, 0);

            if (bounds != this.Bounds)
            {
                this.Bounds = bounds;

                _hideLabel = _editor.HasOption(DocumentHeaderOptions.DoNotShowLabelAndFixWidth);

                if(!_hideLabel)
                    _printLabel = MeasureFromIDC();

                _hideLabel = _hideLabel || this.Width == 50 || _printLabel == string.Empty;

                this.Invalidate();
            }
        }

        public TabButton(IEditor editor)
            : base()
        {

            #region Load Image Sprites

            _imgs = new Image[2, 3];

            Image img_unselected = Image.FromFile(@"D:\Desenv\Ockham\branches\1\primeira.Editor.Components\tab\unselected_full.png");
            Image img_selected = Image.FromFile(@"D:\Desenv\Ockham\branches\1\primeira.Editor.Components\tab\selected_full.png");

            int width = (img_selected.Width + 1) / 2;
            int height = img_selected.Height;

            _imgs[0, 0] = new Bitmap(width, height);
            _imgs[0, 1] = new Bitmap(TabControlManager.MAX_SIZE_TABBUTTON_WIDTH, height);
            _imgs[0, 2] = new Bitmap(width, height);

            _imgs[1, 0] = new Bitmap(width, height);
            _imgs[1, 1] = new Bitmap(TabControlManager.MAX_SIZE_TABBUTTON_WIDTH, height);
            _imgs[1, 2] = new Bitmap(width, height);

            Rectangle r = new Rectangle(0, 0, width, height);
            Graphics g = Graphics.FromImage(_imgs[0, 0]);
            g.DrawImage(img_unselected, r, r, GraphicsUnit.Pixel);

            g = Graphics.FromImage(_imgs[1, 0]);
            g.DrawImage(img_selected, r, r, GraphicsUnit.Pixel);

            g = Graphics.FromImage(_imgs[0, 2]);
            g.DrawImage(img_unselected, r, new Rectangle(img_selected.Width - width, 0, width, height), GraphicsUnit.Pixel);

            g = Graphics.FromImage(_imgs[1, 2]);
            g.DrawImage(img_selected, r, new Rectangle(img_selected.Width - width, 0, width, height), GraphicsUnit.Pixel);

            g = Graphics.FromImage(_imgs[0, 1]);
            g.DrawImage(img_unselected, new Rectangle(0, 0, _imgs[0, 1].Width, height), new Rectangle(width-1, 0, 1, height), GraphicsUnit.Pixel);

            g = Graphics.FromImage(_imgs[1, 1]);
            g.DrawImage(img_selected, new Rectangle(0, 0, _imgs[1, 1].Width, height), new Rectangle(width-1, 0, 1, height), GraphicsUnit.Pixel);

            #endregion

            this.Font = new System.Drawing.Font("Segoe UI", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            this._editor = editor;

            _icon = editor.Header.Icon;

            this._toolTip = new System.Windows.Forms.ToolTip();

            this.Cursor = Cursors.Hand;

            this.TabStop = true;

            SetText(editor.FileName, editor.Header.FriendlyNameMask);

            this.Click += new EventHandler(TabButton_Click);

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, false);
            this.SetStyle(ControlStyles.UserPaint, true);
        }

        void TabButton_Click(object sender, EventArgs e)
        {
            TabControlManager.GetInstance().ActiveEditor = _editor;
        }

        private int _iSelectedOffSet = 0;
        private bool _hideLabel;
        private Image _icon;
        private string _printLabel = string.Empty;

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            _iSelectedOffSet = Convert.ToInt16(this._editor.Selected);

            e.Graphics.DrawImageUnscaledAndClipped(_imgs[_iSelectedOffSet, 0], new Rectangle(0, 0, _imgs[_iSelectedOffSet, 0].Width, _imgs[_iSelectedOffSet, 0].Height));

            e.Graphics.DrawImageUnscaledAndClipped(_imgs[_iSelectedOffSet, 1], new Rectangle(_imgs[_iSelectedOffSet, 0].Width, 0, this.Width - _imgs[_iSelectedOffSet, 0].Width * 2, _imgs[_iSelectedOffSet, 0].Height));

            e.Graphics.DrawImageUnscaledAndClipped(_imgs[_iSelectedOffSet, 2], new Rectangle(this.Width - _imgs[_iSelectedOffSet, 2].Width, 0, _imgs[_iSelectedOffSet, 2].Width, _imgs[_iSelectedOffSet, 2].Height));

        }

        protected override void OnPaint(PaintEventArgs e)
        {

            if (_icon != null)
                e.Graphics.DrawImageUnscaled(_icon, _hideLabel ? 6 + this.Width/4 : 6, 4);
            
            if(!_hideLabel)
                e.Graphics.DrawString(_printLabel, this.Font, SystemBrushes.ControlText, 21, 5);

        }

        private ToolTip _toolTip;

        public void SetText(string fileName)
        {
            SetText(fileName, null);
        }

        public void SetText(string fileName, string friendlyName)
        {
            if (friendlyName == null)
                friendlyName = "%";

            this.FriendlyName = friendlyName;

            this.TabTitle = friendlyName.Replace("%", fileName);

            _toolTip.SetToolTip(this, this.TabTitle);
        }

        //To avoid creating graphics dynamically. Used in MeasureFromIDC below.
        private static Control _control;
        private static Graphics _graphics;
        private string _value;
        private Size _size;
        private char[] _tmpValue;
        private string _ext;
        private TextFormatFlags _textFormatFlags = TextFormatFlags.ModifyString | TextFormatFlags.PathEllipsis | TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine | TextFormatFlags.TextBoxControl | TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
      
        public string MeasureFromIDC()
        {
            if (_control == null)
            {
                _control = new Control();
                _graphics = _control.CreateGraphics();
            }

            _value = TabTitle;

            _size = new Size(
                        this.Width - 20,
                        this.Height); 

             _tmpValue = new char[_value.Length];

            _value.CopyTo(0, _tmpValue, 0, _value.Length);

            _value = new string(_tmpValue);
            _ext = Path.GetExtension(_value);
            if (_ext.Length > 0)
                _value = _value.Replace(_ext, "");

            TextRenderer.MeasureText(_graphics, _value, this.Font, _size, _textFormatFlags);
            int i = _value.IndexOf("\0");
            if (i > -1)
                _value = _value.Substring(0, i);

            if (_value == "...\\...")
                return string.Empty;

            return _value;
        }

    }
}
