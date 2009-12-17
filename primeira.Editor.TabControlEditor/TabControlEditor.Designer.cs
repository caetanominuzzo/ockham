using primeira.Editor.Components;
namespace primeira.Editor
{
    partial class TabControlEditor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TabControlEditor));
            this.menTabs = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pnDocArea = new System.Windows.Forms.Panel();
            this.lblNonModalMessage = new System.Windows.Forms.Label();
            this.pnTop = new System.Windows.Forms.Panel();
            this.pnTabArea = new System.Windows.Forms.FlowLayoutPanel();
            this.pnTabOptions = new System.Windows.Forms.Panel();
            this.btnShowTabs = new primeira.Editor.Components.EditorBaseButton();
            this.btnClose = new primeira.Editor.Components.EditorBaseButton();
            this.pnDocArea.SuspendLayout();
            this.pnTop.SuspendLayout();
            this.pnTabOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // menTabs
            // 
            this.menTabs.Name = "menTabs";
            this.menTabs.Size = new System.Drawing.Size(61, 4);
            // 
            // pnDocArea
            // 
            this.pnDocArea.Controls.Add(this.lblNonModalMessage);
            this.pnDocArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnDocArea.Location = new System.Drawing.Point(0, 28);
            this.pnDocArea.Name = "pnDocArea";
            this.pnDocArea.Size = new System.Drawing.Size(400, 358);
            this.pnDocArea.TabIndex = 3;
            // 
            // lblNonModalMessage
            // 
            this.lblNonModalMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNonModalMessage.BackColor = System.Drawing.SystemColors.Info;
            this.lblNonModalMessage.Location = new System.Drawing.Point(41, 104);
            this.lblNonModalMessage.Name = "lblNonModalMessage";
            this.lblNonModalMessage.Size = new System.Drawing.Size(209, 30);
            this.lblNonModalMessage.TabIndex = 0;
            this.lblNonModalMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnTop
            // 
            this.pnTop.BackColor = System.Drawing.Color.Black;
            this.pnTop.Controls.Add(this.pnTabArea);
            this.pnTop.Controls.Add(this.pnTabOptions);
            this.pnTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnTop.Location = new System.Drawing.Point(0, 0);
            this.pnTop.Name = "pnTop";
            this.pnTop.Size = new System.Drawing.Size(400, 28);
            this.pnTop.TabIndex = 4;
            // 
            // pnTabArea
            // 
            this.pnTabArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnTabArea.Location = new System.Drawing.Point(0, 0);
            this.pnTabArea.Name = "pnTabArea";
            this.pnTabArea.Size = new System.Drawing.Size(356, 28);
            this.pnTabArea.TabIndex = 2;
            this.pnTabArea.WrapContents = false;
            // 
            // pnTabOptions
            // 
            this.pnTabOptions.Controls.Add(this.btnShowTabs);
            this.pnTabOptions.Controls.Add(this.btnClose);
            this.pnTabOptions.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnTabOptions.Location = new System.Drawing.Point(356, 0);
            this.pnTabOptions.Name = "pnTabOptions";
            this.pnTabOptions.Size = new System.Drawing.Size(44, 28);
            this.pnTabOptions.TabIndex = 2;
            // 
            // btnShowTabs
            // 
            this.btnShowTabs.BackColor = System.Drawing.Color.Transparent;
            this.btnShowTabs.Image = ((System.Drawing.Image)(resources.GetObject("btnShowTabs.Image")));
            this.btnShowTabs.Location = new System.Drawing.Point(0, 2);
            this.btnShowTabs.Name = "btnShowTabs";
            this.btnShowTabs.ShowFocus = true;
            this.btnShowTabs.ShowLabel = false;
            this.btnShowTabs.Size = new System.Drawing.Size(22, 22);
            this.btnShowTabs.TabIndex = 1;
            this.btnShowTabs.Text = "editorBaseButton1";
            this.btnShowTabs.UseVisualStyleBackColor = false;
            this.btnShowTabs.Click += new System.EventHandler(this.btnShowTabs_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Enabled = false;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(22, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.ShowFocus = true;
            this.btnClose.ShowLabel = false;
            this.btnClose.Size = new System.Drawing.Size(22, 22);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "editorBaseButton1";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // TabControlEditor
            // 
            this.Controls.Add(this.pnDocArea);
            this.Controls.Add(this.pnTop);
            this.Name = "TabControlEditor";
            this.Size = new System.Drawing.Size(400, 386);
            this.Load += new System.EventHandler(this.TabControlEditor_Load);
            this.pnDocArea.ResumeLayout(false);
            this.pnTop.ResumeLayout(false);
            this.pnTabOptions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel pnTabArea;
        private System.Windows.Forms.Panel pnDocArea;
        private System.Windows.Forms.Panel pnTop;
        private System.Windows.Forms.Panel pnTabOptions;
        private System.Windows.Forms.Label lblNonModalMessage;
        private EditorBaseButton btnClose;
        private EditorBaseButton btnShowTabs;
        private System.Windows.Forms.ContextMenuStrip menTabs;
    }
}
