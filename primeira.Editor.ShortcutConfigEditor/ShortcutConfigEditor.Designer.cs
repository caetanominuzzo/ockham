using primeira.Editor.Components;
namespace primeira.Editor
{
    partial class ShortcutConfigEditor
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
            this.cbEscope = new System.Windows.Forms.ComboBox();
            this.btCancel = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.lbDescription = new System.Windows.Forms.Label();
            this.btOk = new System.Windows.Forms.Button();
            this.cbCurrently = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btAssign = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btRemove = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cbShortcut = new System.Windows.Forms.ComboBox();
            this.lsCommand = new System.Windows.Forms.ListBox();
            this.txtCommand = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.txtShortcut = new primeira.Editor.ShortcutTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbEscope
            // 
            this.cbEscope.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbEscope.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEscope.FormattingEnabled = true;
            this.cbEscope.Location = new System.Drawing.Point(33, 106);
            this.cbEscope.Margin = new System.Windows.Forms.Padding(10);
            this.cbEscope.Name = "cbEscope";
            this.cbEscope.Size = new System.Drawing.Size(182, 21);
            this.cbEscope.TabIndex = 15;
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(452, 22);
            this.btCancel.Margin = new System.Windows.Forms.Padding(10);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(39, 84);
            this.label5.Margin = new System.Windows.Forms.Padding(10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Use this shortcut in";
            // 
            // lbDescription
            // 
            this.lbDescription.AutoSize = true;
            this.lbDescription.Location = new System.Drawing.Point(29, 140);
            this.lbDescription.Margin = new System.Windows.Forms.Padding(10);
            this.lbDescription.Name = "lbDescription";
            this.lbDescription.Size = new System.Drawing.Size(60, 13);
            this.lbDescription.TabIndex = 13;
            this.lbDescription.Text = "Description";
            // 
            // btOk
            // 
            this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btOk.Location = new System.Drawing.Point(357, 22);
            this.btOk.Margin = new System.Windows.Forms.Padding(10);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(75, 23);
            this.btOk.TabIndex = 0;
            this.btOk.Text = "Ok";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // cbCurrently
            // 
            this.cbCurrently.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbCurrently.FormattingEnabled = true;
            this.cbCurrently.Location = new System.Drawing.Point(33, 180);
            this.cbCurrently.Margin = new System.Windows.Forms.Padding(10);
            this.cbCurrently.Name = "cbCurrently";
            this.cbCurrently.Size = new System.Drawing.Size(494, 21);
            this.cbCurrently.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(39, 147);
            this.label4.Margin = new System.Windows.Forms.Padding(10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(130, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Shortcut currently used by";
            // 
            // btAssign
            // 
            this.btAssign.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btAssign.Location = new System.Drawing.Point(432, 104);
            this.btAssign.Margin = new System.Windows.Forms.Padding(10);
            this.btAssign.Name = "btAssign";
            this.btAssign.Size = new System.Drawing.Size(95, 23);
            this.btAssign.TabIndex = 8;
            this.btAssign.Text = "Assign";
            this.btAssign.UseVisualStyleBackColor = true;
            this.btAssign.Click += new System.EventHandler(this.btAssign_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(232, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Press new shortcut key";
            // 
            // btRemove
            // 
            this.btRemove.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btRemove.Location = new System.Drawing.Point(432, 41);
            this.btRemove.Margin = new System.Windows.Forms.Padding(10);
            this.btRemove.Name = "btRemove";
            this.btRemove.Size = new System.Drawing.Size(95, 23);
            this.btRemove.TabIndex = 5;
            this.btRemove.Text = "&Remove";
            this.btRemove.UseVisualStyleBackColor = true;
            this.btRemove.Click += new System.EventHandler(this.btRemove_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 10);
            this.label2.Margin = new System.Windows.Forms.Padding(10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(151, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Shorcut for selected command";
            // 
            // cbShortcut
            // 
            this.cbShortcut.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbShortcut.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbShortcut.FormattingEnabled = true;
            this.cbShortcut.Location = new System.Drawing.Point(32, 43);
            this.cbShortcut.Margin = new System.Windows.Forms.Padding(10);
            this.cbShortcut.Name = "cbShortcut";
            this.cbShortcut.Size = new System.Drawing.Size(380, 21);
            this.cbShortcut.TabIndex = 3;
            // 
            // lsCommand
            // 
            this.lsCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lsCommand.FormattingEnabled = true;
            this.lsCommand.Location = new System.Drawing.Point(32, 65);
            this.lsCommand.Margin = new System.Windows.Forms.Padding(10);
            this.lsCommand.Name = "lsCommand";
            this.lsCommand.Size = new System.Drawing.Size(495, 69);
            this.lsCommand.TabIndex = 2;
            this.lsCommand.SelectedIndexChanged += new System.EventHandler(this.lsCommand_SelectedIndexChanged);
            // 
            // txtCommand
            // 
            this.txtCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCommand.Location = new System.Drawing.Point(32, 42);
            this.txtCommand.Margin = new System.Windows.Forms.Padding(10);
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.Size = new System.Drawing.Size(495, 20);
            this.txtCommand.TabIndex = 1;
            this.txtCommand.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCommand_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Show commands containing";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.panel2);
            this.flowLayoutPanel1.Controls.Add(this.panel3);
            this.flowLayoutPanel1.Controls.Add(this.panel4);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1127, 518);
            this.flowLayoutPanel1.TabIndex = 22;
            // 
            // txtShortcut
            // 
            this.txtShortcut.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtShortcut.Location = new System.Drawing.Point(235, 107);
            this.txtShortcut.Margin = new System.Windows.Forms.Padding(10);
            this.txtShortcut.Name = "txtShortcut";
            this.txtShortcut.Size = new System.Drawing.Size(177, 20);
            this.txtShortcut.TabIndex = 12;
            this.txtShortcut.TextChanged += new System.EventHandler(this.txtShortcut_TextChanged);
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.Controls.Add(this.txtCommand);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.lsCommand);
            this.panel2.Controls.Add(this.lbDescription);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(552, 163);
            this.panel2.TabIndex = 22;
            // 
            // panel3
            // 
            this.panel3.AutoSize = true;
            this.panel3.Controls.Add(this.cbEscope);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.btRemove);
            this.panel3.Controls.Add(this.txtShortcut);
            this.panel3.Controls.Add(this.cbShortcut);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.cbCurrently);
            this.panel3.Controls.Add(this.btAssign);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(561, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(557, 211);
            this.panel3.TabIndex = 23;
            // 
            // panel4
            // 
            this.panel4.AutoSize = true;
            this.panel4.Controls.Add(this.btOk);
            this.panel4.Controls.Add(this.btCancel);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(3, 220);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(552, 55);
            this.panel4.TabIndex = 24;
            // 
            // ShortcutConfigEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "ShortcutConfigEditor";
            this.Size = new System.Drawing.Size(1127, 518);
            this.Load += new System.EventHandler(this.ShortcutManagerEditor_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion


        private System.Windows.Forms.ComboBox cbEscope;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbDescription;
        private System.Windows.Forms.Button btOk;
        private ShortcutTextBox txtShortcut;
        private System.Windows.Forms.ComboBox cbCurrently;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btAssign;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btRemove;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbShortcut;
        private System.Windows.Forms.ListBox lsCommand;
        private System.Windows.Forms.TextBox txtCommand;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;

    }
}
