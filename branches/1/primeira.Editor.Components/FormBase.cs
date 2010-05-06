using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace primeira.Editor.Components
{
    public partial class FormBase : Form
    {
        private WindowPositionDocument cache;

        private FormWindowState lastWindowState;

        public FormBase()
        {
            InitializeComponent();
        }

        private void FormBase_Load(object sender, EventArgs e)
        {
            cache = WindowPositionDocument.GetInstance(Application.ProductName + ".Main");

            this.Location = cache.Location;

            this.Size = cache.Size;

            this.WindowState = cache.WindowState;

            lastWindowState = this.WindowState;

            this.ClientSizeChanged += new EventHandler(FormBase_ClientSizeChanged);
        }

        void FormBase_ClientSizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                cache.Location = this.Location;
                cache.Size = this.Size;
            }

            if (this.WindowState != lastWindowState)
            {
                cache.WindowState = this.WindowState;
                lastWindowState = this.WindowState;
            }

            cache.Save(Application.ProductName + ".Main");
        }
    }
}
