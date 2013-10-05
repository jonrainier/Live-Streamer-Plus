namespace Live_Streamer_Plus
{
    partial class LiveStreamerPlus
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LiveStreamerPlus));
            this.lbl_Stream = new System.Windows.Forms.Label();
            this.tb_DoStreamer = new System.Windows.Forms.TextBox();
            this.btn_Go = new System.Windows.Forms.Button();
            this.cb_Quality = new System.Windows.Forms.ComboBox();
            this.cb_SelectedStream = new System.Windows.Forms.ComboBox();
            this.rtb_Console = new System.Windows.Forms.RichTextBox();
            this.tc_MainForm = new System.Windows.Forms.TabControl();
            this.tp_Stream = new System.Windows.Forms.TabPage();
            this.tp_EditConfig = new System.Windows.Forms.TabPage();
            this.btn_SaveConfig = new System.Windows.Forms.Button();
            this.rtb_ConfigEditor = new System.Windows.Forms.RichTextBox();
            this.tp_ChangeLog = new System.Windows.Forms.TabPage();
            this.rtb_ChangeLog = new System.Windows.Forms.RichTextBox();
            this.tc_MainForm.SuspendLayout();
            this.tp_Stream.SuspendLayout();
            this.tp_EditConfig.SuspendLayout();
            this.tp_ChangeLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_Stream
            // 
            this.lbl_Stream.AutoSize = true;
            this.lbl_Stream.Location = new System.Drawing.Point(6, 9);
            this.lbl_Stream.Name = "lbl_Stream";
            this.lbl_Stream.Size = new System.Drawing.Size(48, 15);
            this.lbl_Stream.TabIndex = 6;
            this.lbl_Stream.Text = "Stream:";
            // 
            // tb_DoStreamer
            // 
            this.tb_DoStreamer.Location = new System.Drawing.Point(184, 6);
            this.tb_DoStreamer.Name = "tb_DoStreamer";
            this.tb_DoStreamer.Size = new System.Drawing.Size(239, 24);
            this.tb_DoStreamer.TabIndex = 1;
            // 
            // btn_Go
            // 
            this.btn_Go.Location = new System.Drawing.Point(556, 7);
            this.btn_Go.Name = "btn_Go";
            this.btn_Go.Size = new System.Drawing.Size(87, 23);
            this.btn_Go.TabIndex = 3;
            this.btn_Go.Text = "Go!";
            this.btn_Go.UseVisualStyleBackColor = true;
            this.btn_Go.Click += new System.EventHandler(this.btn_Go_Click);
            // 
            // cb_Quality
            // 
            this.cb_Quality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Quality.FormattingEnabled = true;
            this.cb_Quality.Items.AddRange(new object[] {
            "source",
            "high",
            "medium",
            "low"});
            this.cb_Quality.Location = new System.Drawing.Point(429, 7);
            this.cb_Quality.Name = "cb_Quality";
            this.cb_Quality.Size = new System.Drawing.Size(121, 23);
            this.cb_Quality.TabIndex = 2;
            // 
            // cb_SelectedStream
            // 
            this.cb_SelectedStream.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_SelectedStream.FormattingEnabled = true;
            this.cb_SelectedStream.Items.AddRange(new object[] {
            "twitch.tv"});
            this.cb_SelectedStream.Location = new System.Drawing.Point(57, 6);
            this.cb_SelectedStream.Name = "cb_SelectedStream";
            this.cb_SelectedStream.Size = new System.Drawing.Size(121, 23);
            this.cb_SelectedStream.TabIndex = 0;
            // 
            // rtb_Console
            // 
            this.rtb_Console.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtb_Console.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rtb_Console.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtb_Console.Location = new System.Drawing.Point(3, 41);
            this.rtb_Console.Name = "rtb_Console";
            this.rtb_Console.ReadOnly = true;
            this.rtb_Console.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtb_Console.Size = new System.Drawing.Size(645, 345);
            this.rtb_Console.TabIndex = 4;
            this.rtb_Console.Text = "";
            // 
            // tc_MainForm
            // 
            this.tc_MainForm.Controls.Add(this.tp_Stream);
            this.tc_MainForm.Controls.Add(this.tp_EditConfig);
            this.tc_MainForm.Controls.Add(this.tp_ChangeLog);
            this.tc_MainForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tc_MainForm.Location = new System.Drawing.Point(0, 0);
            this.tc_MainForm.Name = "tc_MainForm";
            this.tc_MainForm.SelectedIndex = 0;
            this.tc_MainForm.Size = new System.Drawing.Size(659, 417);
            this.tc_MainForm.TabIndex = 6;
            // 
            // tp_Stream
            // 
            this.tp_Stream.Controls.Add(this.rtb_Console);
            this.tp_Stream.Controls.Add(this.lbl_Stream);
            this.tp_Stream.Controls.Add(this.btn_Go);
            this.tp_Stream.Controls.Add(this.cb_Quality);
            this.tp_Stream.Controls.Add(this.cb_SelectedStream);
            this.tp_Stream.Controls.Add(this.tb_DoStreamer);
            this.tp_Stream.Location = new System.Drawing.Point(4, 24);
            this.tp_Stream.Name = "tp_Stream";
            this.tp_Stream.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Stream.Size = new System.Drawing.Size(651, 389);
            this.tp_Stream.TabIndex = 0;
            this.tp_Stream.Text = "Stream";
            this.tp_Stream.UseVisualStyleBackColor = true;
            // 
            // tp_EditConfig
            // 
            this.tp_EditConfig.Controls.Add(this.btn_SaveConfig);
            this.tp_EditConfig.Controls.Add(this.rtb_ConfigEditor);
            this.tp_EditConfig.Location = new System.Drawing.Point(4, 24);
            this.tp_EditConfig.Name = "tp_EditConfig";
            this.tp_EditConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tp_EditConfig.Size = new System.Drawing.Size(651, 389);
            this.tp_EditConfig.TabIndex = 1;
            this.tp_EditConfig.Text = "Config";
            this.tp_EditConfig.UseVisualStyleBackColor = true;
            // 
            // btn_SaveConfig
            // 
            this.btn_SaveConfig.Location = new System.Drawing.Point(557, 3);
            this.btn_SaveConfig.Name = "btn_SaveConfig";
            this.btn_SaveConfig.Size = new System.Drawing.Size(87, 23);
            this.btn_SaveConfig.TabIndex = 0;
            this.btn_SaveConfig.Text = "Save";
            this.btn_SaveConfig.UseVisualStyleBackColor = true;
            this.btn_SaveConfig.Click += new System.EventHandler(this.btn_SaveConfig_Click);
            // 
            // rtb_ConfigEditor
            // 
            this.rtb_ConfigEditor.Dock = System.Windows.Forms.DockStyle.Left;
            this.rtb_ConfigEditor.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtb_ConfigEditor.Location = new System.Drawing.Point(3, 3);
            this.rtb_ConfigEditor.Name = "rtb_ConfigEditor";
            this.rtb_ConfigEditor.Size = new System.Drawing.Size(548, 383);
            this.rtb_ConfigEditor.TabIndex = 0;
            this.rtb_ConfigEditor.Text = "";
            // 
            // tp_ChangeLog
            // 
            this.tp_ChangeLog.Controls.Add(this.rtb_ChangeLog);
            this.tp_ChangeLog.Location = new System.Drawing.Point(4, 24);
            this.tp_ChangeLog.Name = "tp_ChangeLog";
            this.tp_ChangeLog.Size = new System.Drawing.Size(651, 389);
            this.tp_ChangeLog.TabIndex = 2;
            this.tp_ChangeLog.Text = "Change Log";
            this.tp_ChangeLog.UseVisualStyleBackColor = true;
            // 
            // rtb_ChangeLog
            // 
            this.rtb_ChangeLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_ChangeLog.Location = new System.Drawing.Point(0, 0);
            this.rtb_ChangeLog.Name = "rtb_ChangeLog";
            this.rtb_ChangeLog.ReadOnly = true;
            this.rtb_ChangeLog.Size = new System.Drawing.Size(651, 389);
            this.rtb_ChangeLog.TabIndex = 0;
            this.rtb_ChangeLog.Text = "CHANGELOG_TEXT";
            // 
            // LiveStreamerPlus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 417);
            this.Controls.Add(this.tc_MainForm);
            this.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "LiveStreamerPlus";
            this.Text = "Live Streamer Plus :: ";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tc_MainForm.ResumeLayout(false);
            this.tp_Stream.ResumeLayout(false);
            this.tp_Stream.PerformLayout();
            this.tp_EditConfig.ResumeLayout(false);
            this.tp_ChangeLog.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_Stream;
        private System.Windows.Forms.TextBox tb_DoStreamer;
        private System.Windows.Forms.Button btn_Go;
        private System.Windows.Forms.ComboBox cb_Quality;
        private System.Windows.Forms.ComboBox cb_SelectedStream;
        private System.Windows.Forms.RichTextBox rtb_Console;
        private System.Windows.Forms.TabControl tc_MainForm;
        private System.Windows.Forms.TabPage tp_Stream;
        private System.Windows.Forms.TabPage tp_EditConfig;
        private System.Windows.Forms.RichTextBox rtb_ConfigEditor;
        private System.Windows.Forms.Button btn_SaveConfig;
        private System.Windows.Forms.TabPage tp_ChangeLog;
        private System.Windows.Forms.RichTextBox rtb_ChangeLog;
    }
}

