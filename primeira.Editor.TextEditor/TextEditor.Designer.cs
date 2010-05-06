namespace primeira.Editor
{
    partial class TextEditor
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
            this.txtMain = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtMain
            // 
            this.txtMain.AcceptsReturn = true;
            this.txtMain.AcceptsTab = true;
            this.txtMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMain.HideSelection = false;
            this.txtMain.Location = new System.Drawing.Point(0, 0);
            this.txtMain.Margin = new System.Windows.Forms.Padding(10);
            this.txtMain.Multiline = true;
            this.txtMain.Name = "txtMain";
            this.txtMain.Size = new System.Drawing.Size(1226, 582);
            this.txtMain.TabIndex = 10;
            // 
            // TextEditor
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.Controls.Add(this.txtMain);
            this.Name = "TextEditor";
            this.Size = new System.Drawing.Size(1226, 582);
            this.Load += new System.EventHandler(this.TextEditor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtMain;



    }
}
