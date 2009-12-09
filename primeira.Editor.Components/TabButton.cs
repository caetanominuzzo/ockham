using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.ComponentModel;
using primeira.Editor.Business;

namespace primeira.Editor.Components
{
    public class TabButton : Button, ITabButton
    {
        private Image m_image = Image.FromFile(@"D:\Desenv\Neural Network\Imgs.png");

        private static Image _defaultSelectedImage = Image.FromFile(@"D:\Desenv\Neural Network\Imgs.png");

        private static Image _defaultUnselectedImage = Image.FromFile(@"D:\Desenv\Neural Network\Imgs.png");

        private Image _selectedImage = _defaultSelectedImage;

        private Image _unselectedImage = _defaultUnselectedImage;

        public Image SelectedImage
        {
            get { return _selectedImage; }
            set { _selectedImage = value; }
        }

        public Image UnselectedImage
        {
            get { return _unselectedImage; }
            set { _unselectedImage = value; }
        }

        public TabButton() : base()
        {
            InitializeComponent();

            this.Cursor = Cursors.Hand;
            this.Dock = DockStyle.Left;
            this.Width = 150;
            this.MaximumSize = new Size(150, 40);
            this.BackgroundImage = Unselectedimage;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.Font = new Font(SystemFonts.CaptionFont.FontFamily, 10);
            this.ForeColor = Color.Gray;
            this.UseCompatibleTextRendering = false;
        }

        public static Image Selectedimage = Image.FromFile(@"D:\Desenv\Neural Network\Imgs.png");

        public static Image Unselectedimage = Image.FromFile(@"D:\Desenv\Neural Network\Imgs.png");

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // TabButton
            // 
            this.FlatAppearance.BorderSize = 0;
            this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
            this.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.TextAlign = ContentAlignment.MiddleCenter;
            this.AutoEllipsis = false;
            this.ResumeLayout(false);

            _toolTip = new ToolTip();
        }

        private ToolTip _toolTip;

        public void SetToolTip(string tooltip)
        {
            _toolTip.SetToolTip(this, tooltip);

        }

    }
}
