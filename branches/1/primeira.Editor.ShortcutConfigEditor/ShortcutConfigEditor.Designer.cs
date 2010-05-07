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
            this.btOk = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.btCancel = new System.Windows.Forms.Button();
            this.txtCurrently = new System.Windows.Forms.TextBox();
            this.btRemove = new System.Windows.Forms.Button();
            this.cbShortcut = new System.Windows.Forms.ComboBox();
            this.txtCommand = new System.Windows.Forms.TextBox();
            this.cbEscope = new System.Windows.Forms.ComboBox();
            this.lblCommand1 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lsCommand = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btAssign = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblType = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtShortcut = new primeira.Editor.ShortcutTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblCommand2 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btOk
            // 
            this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btOk.AutoSize = true;
            this.btOk.Location = new System.Drawing.Point(39, 472);
            this.btOk.Margin = new System.Windows.Forms.Padding(9);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(64, 23);
            this.btOk.TabIndex = 8;
            this.btOk.Text = "Ok";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.flowLayoutPanel1.SetFlowBreak(this.label13, true);
            this.label13.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.label13.Location = new System.Drawing.Point(38, 395);
            this.label13.Margin = new System.Windows.Forms.Padding(8);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(406, 13);
            this.label13.TabIndex = 18;
            this.label13.Text = "Removing a shortcut from a command can be undone by hitting the \"Cancel\" button.";
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.AutoSize = true;
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(121, 472);
            this.btCancel.Margin = new System.Windows.Forms.Padding(9);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(64, 23);
            this.btCancel.TabIndex = 9;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // txtCurrently
            // 
            this.txtCurrently.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.SetFlowBreak(this.txtCurrently, true);
            this.txtCurrently.Location = new System.Drawing.Point(38, 321);
            this.txtCurrently.Margin = new System.Windows.Forms.Padding(8);
            this.txtCurrently.Name = "txtCurrently";
            this.txtCurrently.ReadOnly = true;
            this.txtCurrently.Size = new System.Drawing.Size(400, 20);
            this.txtCurrently.TabIndex = 18;
            // 
            // btRemove
            // 
            this.btRemove.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btRemove.AutoSize = true;
            this.flowLayoutPanel1.SetFlowBreak(this.btRemove, true);
            this.btRemove.Location = new System.Drawing.Point(351, 432);
            this.btRemove.Margin = new System.Windows.Forms.Padding(8);
            this.btRemove.Name = "btRemove";
            this.btRemove.Size = new System.Drawing.Size(81, 23);
            this.btRemove.TabIndex = 3;
            this.btRemove.Text = "&Remove";
            this.btRemove.UseVisualStyleBackColor = true;
            this.btRemove.Click += new System.EventHandler(this.btRemove_Click);
            // 
            // cbShortcut
            // 
            this.cbShortcut.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbShortcut.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbShortcut.FormattingEnabled = true;
            this.cbShortcut.Location = new System.Drawing.Point(38, 432);
            this.cbShortcut.Margin = new System.Windows.Forms.Padding(8);
            this.cbShortcut.Name = "cbShortcut";
            this.cbShortcut.Size = new System.Drawing.Size(297, 21);
            this.cbShortcut.TabIndex = 2;
            // 
            // txtCommand
            // 
            this.txtCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCommand.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtCommand.Location = new System.Drawing.Point(0, 29);
            this.txtCommand.Margin = new System.Windows.Forms.Padding(8);
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.Size = new System.Drawing.Size(400, 20);
            this.txtCommand.TabIndex = 0;
            this.txtCommand.TextChanged += new System.EventHandler(this.txtCommand_TextChanged);
            this.txtCommand.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtCommand_KeyUp);
            // 
            // cbEscope
            // 
            this.cbEscope.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbEscope.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.flowLayoutPanel1.SetFlowBreak(this.cbEscope, true);
            this.cbEscope.FormattingEnabled = true;
            this.cbEscope.Location = new System.Drawing.Point(255, 209);
            this.cbEscope.Margin = new System.Windows.Forms.Padding(8);
            this.cbEscope.Name = "cbEscope";
            this.cbEscope.Size = new System.Drawing.Size(182, 21);
            this.cbEscope.TabIndex = 4;
            // 
            // lblCommand1
            // 
            this.lblCommand1.AutoSize = true;
            this.flowLayoutPanel1.SetFlowBreak(this.lblCommand1, true);
            this.lblCommand1.Location = new System.Drawing.Point(165, 151);
            this.lblCommand1.Margin = new System.Windows.Forms.Padding(8, 16, 8, 8);
            this.lblCommand1.Name = "lblCommand1";
            this.lblCommand1.Size = new System.Drawing.Size(132, 13);
            this.lblCommand1.TabIndex = 4;
            this.lblCommand1.Text = "(select a command above)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Commands Search";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.flowLayoutPanel1.SetFlowBreak(this.label5, true);
            this.label5.Location = new System.Drawing.Point(255, 180);
            this.label5.Margin = new System.Windows.Forms.Padding(8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Use this shortcut in";
            // 
            // lsCommand
            // 
            this.lsCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.SetFlowBreak(this.lsCommand, true);
            this.lsCommand.FormattingEnabled = true;
            this.lsCommand.Location = new System.Drawing.Point(38, 79);
            this.lsCommand.Margin = new System.Windows.Forms.Padding(8, 4, 8, 0);
            this.lsCommand.Name = "lsCommand";
            this.lsCommand.Size = new System.Drawing.Size(400, 56);
            this.lsCommand.TabIndex = 1;
            this.lsCommand.SelectedIndexChanged += new System.EventHandler(this.lsCommand_SelectedIndexChanged);
            this.lsCommand.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lsCommand_KeyUp);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(38, 180);
            this.label3.Margin = new System.Windows.Forms.Padding(8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(201, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Press new shortcut key";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.flowLayoutPanel1.SetFlowBreak(this.label4, true);
            this.label4.Location = new System.Drawing.Point(38, 285);
            this.label4.Margin = new System.Windows.Forms.Padding(8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(226, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Setting a new shortcut may cause conflict with";
            // 
            // btAssign
            // 
            this.btAssign.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btAssign.AutoSize = true;
            this.flowLayoutPanel1.SetFlowBreak(this.btAssign, true);
            this.btAssign.Location = new System.Drawing.Point(38, 246);
            this.btAssign.Margin = new System.Windows.Forms.Padding(8);
            this.btAssign.Name = "btAssign";
            this.btAssign.Size = new System.Drawing.Size(81, 23);
            this.btAssign.TabIndex = 6;
            this.btAssign.Text = "Assign";
            this.btAssign.UseVisualStyleBackColor = true;
            this.btAssign.Click += new System.EventHandler(this.btAssign_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Controls.Add(this.lsCommand);
            this.flowLayoutPanel1.Controls.Add(this.label6);
            this.flowLayoutPanel1.Controls.Add(this.lblCommand1);
            this.flowLayoutPanel1.Controls.Add(this.label3);
            this.flowLayoutPanel1.Controls.Add(this.label5);
            this.flowLayoutPanel1.Controls.Add(this.txtShortcut);
            this.flowLayoutPanel1.Controls.Add(this.cbEscope);
            this.flowLayoutPanel1.Controls.Add(this.btAssign);
            this.flowLayoutPanel1.Controls.Add(this.label4);
            this.flowLayoutPanel1.Controls.Add(this.txtCurrently);
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this.lblCommand2);
            this.flowLayoutPanel1.Controls.Add(this.label13);
            this.flowLayoutPanel1.Controls.Add(this.cbShortcut);
            this.flowLayoutPanel1.Controls.Add(this.btRemove);
            this.flowLayoutPanel1.Controls.Add(this.btOk);
            this.flowLayoutPanel1.Controls.Add(this.btCancel);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(30, 15, 0, 0);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(970, 582);
            this.flowLayoutPanel1.TabIndex = 27;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblType);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtCommand);
            this.flowLayoutPanel1.SetFlowBreak(this.panel1, true);
            this.panel1.Location = new System.Drawing.Point(38, 23);
            this.panel1.Margin = new System.Windows.Forms.Padding(8, 8, 8, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(399, 50);
            this.panel1.TabIndex = 19;
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.lblType.Location = new System.Drawing.Point(168, 32);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(226, 13);
            this.lblType.TabIndex = 1;
            this.lblType.Text = "Type to find commands by name or description";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(38, 151);
            this.label6.Margin = new System.Windows.Forms.Padding(8, 16, 8, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(111, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Add New Shorcuts to ";
            // 
            // txtShortcut
            // 
            this.txtShortcut.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtShortcut.Location = new System.Drawing.Point(38, 209);
            this.txtShortcut.Margin = new System.Windows.Forms.Padding(8);
            this.txtShortcut.Name = "txtShortcut";
            this.txtShortcut.Size = new System.Drawing.Size(201, 20);
            this.txtShortcut.TabIndex = 5;
            this.txtShortcut.TextChanged += new System.EventHandler(this.txtShortcut_TextChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 366);
            this.label2.Margin = new System.Windows.Forms.Padding(8, 16, 8, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Remove Shorcuts from ";
            // 
            // lblCommand2
            // 
            this.lblCommand2.AutoSize = true;
            this.flowLayoutPanel1.SetFlowBreak(this.lblCommand2, true);
            this.lblCommand2.Location = new System.Drawing.Point(172, 366);
            this.lblCommand2.Margin = new System.Windows.Forms.Padding(8, 16, 8, 8);
            this.lblCommand2.Name = "lblCommand2";
            this.lblCommand2.Size = new System.Drawing.Size(132, 13);
            this.lblCommand2.TabIndex = 21;
            this.lblCommand2.Text = "(select a command above)";
            // 
            // ShortcutConfigEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ShortcutConfigEditor";
            this.Size = new System.Drawing.Size(970, 582);
            this.Load += new System.EventHandler(this.ShortcutManagerEditor_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtCommand;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lsCommand;
        private System.Windows.Forms.TextBox txtCurrently;
        private System.Windows.Forms.ComboBox cbEscope;
        private System.Windows.Forms.Label lblCommand1;
        private System.Windows.Forms.Label label5;
        private ShortcutTextBox txtShortcut;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btAssign;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btRemove;
        private System.Windows.Forms.ComboBox cbShortcut;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblCommand2;
        private System.Windows.Forms.Label lblType;


    }
}
