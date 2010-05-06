﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using primeira.Editor.Components;
using primeira.Editor;


namespace primeira.Editor
{
    public partial class fmMain : Form, IShorcutEscopeProvider, IMessageControl
    {
        private WindowPositionDocument cache;

        private FormWindowState lastWindowState;

        public fmMain()
        {
            InitializeComponent();
        }

        private void fmMain_Load(object sender, EventArgs e)
        {
            //try to set windows 7 style
            DwmHelper.SeventishIt(this);

            MessageManager.SetMessageControl(this);

            AddonManager.Discovery();

            ShortcutManager.InitializePreFilter();

            ShortcutManager.LoadFromType(this.GetType());

            ShortcutManager.LoadFromForm(this);

            if (Controls.Count == 0)
                showNonAddonsLabel();

            cache = WindowPositionDocument.GetInstance(Application.ProductName + ".Main");

            this.Location = cache.Location;

            this.Size = cache.Size;

            this.WindowState = cache.WindowState;

            lastWindowState = this.WindowState;
            
            this.ClientSizeChanged += new System.EventHandler(this.fmMain_SizeChanged);
        }

        private System.Windows.Forms.Label lblNoAddons;

        private void showNonAddonsLabel()
        {
            this.lblNoAddons = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // 
            // lblNoAddons
            // 
            this.lblNoAddons.BackColor = Color.White;
            this.lblNoAddons.Location = new Point(0, 33);
            this.lblNoAddons.Name = "lblNoAddons";
            this.lblNoAddons.Size = new Size(Width, Height- 50);
            this.lblNoAddons.TabIndex = 0;
            this.lblNoAddons.Text = "We can't find any addon. Try reinstalling the product.";
            this.lblNoAddons.TextAlign = ContentAlignment.MiddleCenter;
            this.lblNoAddons.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Left;

            this.Controls.Add(this.lblNoAddons);
            this.ResumeLayout(false);
        }

        #region IShorcutEscopeProvider Members

        public bool IsAtiveByEscope(string escope)
        {
            return false;
        }

        #endregion

        #region IMessageControl Members

        public void Send(MessageSeverity severity, string message)
        {
            switch (severity)
            {
                case MessageSeverity.Information:
                    MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    break;
                case MessageSeverity.Alert:
                    MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;

                case MessageSeverity.Error:
                    MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                case MessageSeverity.Fatal:
                    MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Application.Exit();
                    break;
            }
        }

        #endregion

        private void fmMain_SizeChanged(object sender, EventArgs e)
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
