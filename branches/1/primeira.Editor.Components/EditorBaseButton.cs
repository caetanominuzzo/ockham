﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.ComponentModel;

namespace primeira.Editor.Components
{
    public class EditorBaseButton : Button
    {
        private Image m_image = Image.FromFile(@"D:\ockham\branches\1\primeira.Editor.Components\tab\ball.png");
        private bool m_showLabel = false;
        private bool m_showFocus = true;

        [Browsable(true)]
        public bool ShowFocus
        {
            get { return m_showFocus; }
            set { m_showFocus = value; }
        }

        [Browsable(true)]
        public bool ShowLabel
        {
            get { return m_showLabel; }
            set
            {
                m_showLabel = value;
                this.Invalidate();
            }
        }

        [Browsable(true)]
        public new Image Image
        {
            get { return m_image; }
            set { m_image = value; }
        }

        private static Color? _parentFormBackColor = null;

        private Form getParentForm(Control c)
        {
            if (c == null)
                return null;
            else if (c is Form)
                return (Form)c;
            else return getParentForm(c.Parent);
        }

        private Color getParentFormBackColor(Control c)
        {
            if (!_parentFormBackColor.HasValue)
            {
                Control tmp = getParentForm(c);
                if (tmp == null)
                    _parentFormBackColor = this.BackColor;
                else
                    _parentFormBackColor = tmp.BackColor;

                //Windows 7 transparency
                if (_parentFormBackColor == Color.Black)
                    _parentFormBackColor = Color.Transparent;
            }

            return _parentFormBackColor.Value;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {

            Rectangle r = new Rectangle(0, 0, this.Width, this.Height);
            
            pevent.Graphics.Clear(getParentFormBackColor(this));

            if (m_image != null)
            {
                Size = m_image.Size;

                //pevent.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                pevent.Graphics.DrawImage((Bitmap)m_image, r, 0, m_image.Height / 2 - this.Height / 2, this.Width, this.Height, GraphicsUnit.Pixel);//, GraphicsUnit.Pixel, attr);
            }

            if (ShowLabel)
                pevent.Graphics.DrawString(Text, this.Font, new SolidBrush(this.ForeColor), new Point(10, this.Height / 2 - 20 / 2));

            if (this.ShowFocus && this.Focused)
            {
                r = pevent.ClipRectangle;
                r.Inflate(-1, -1);
                ControlPaint.DrawFocusRectangle(pevent.Graphics, r);
            }

        }
    }

}
