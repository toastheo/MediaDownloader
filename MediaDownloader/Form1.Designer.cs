namespace MediaDownloader
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Headline = new System.Windows.Forms.Label();
            this.InputBox = new System.Windows.Forms.TextBox();
            this.ConvertButton = new System.Windows.Forms.Button();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.ProgressLabel = new System.Windows.Forms.Label();
            this.QualitySettings = new System.Windows.Forms.GroupBox();
            this.Button4K = new System.Windows.Forms.RadioButton();
            this.Button360p = new System.Windows.Forms.RadioButton();
            this.Button480p = new System.Windows.Forms.RadioButton();
            this.Button720p = new System.Windows.Forms.RadioButton();
            this.Button1080p = new System.Windows.Forms.RadioButton();
            this.Button1440p = new System.Windows.Forms.RadioButton();
            this.ButtonMax = new System.Windows.Forms.RadioButton();
            this.TargetDirLabel = new System.Windows.Forms.Label();
            this.ChooseButton = new System.Windows.Forms.Button();
            this.ResetButton = new System.Windows.Forms.Button();
            this.PathTextBox = new System.Windows.Forms.TextBox();
            this.StepLabel = new System.Windows.Forms.Label();
            this.ProgressTextBox = new System.Windows.Forms.RichTextBox();
            this.FormatLabel = new System.Windows.Forms.Label();
            this.FormatBox = new System.Windows.Forms.ComboBox();
            this.ASettingsLabel = new System.Windows.Forms.Label();
            this.ReencodeVideoCheck = new System.Windows.Forms.CheckBox();
            this.ReencodeAudioCheck = new System.Windows.Forms.CheckBox();
            this.ReencodeTip = new System.Windows.Forms.ToolTip(this.components);
            this.AdvancedInformationsCheck = new System.Windows.Forms.CheckBox();
            this.AdvancedInformationsTextBox = new System.Windows.Forms.RichTextBox();
            this.QualitySettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // Headline
            // 
            this.Headline.AutoSize = true;
            this.Headline.Font = new System.Drawing.Font("Montserrat Medium", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Headline.Location = new System.Drawing.Point(29, 29);
            this.Headline.Name = "Headline";
            this.Headline.Size = new System.Drawing.Size(161, 29);
            this.Headline.TabIndex = 0;
            this.Headline.Text = "Youtube-Link:";
            // 
            // InputBox
            // 
            this.InputBox.Location = new System.Drawing.Point(34, 71);
            this.InputBox.Name = "InputBox";
            this.InputBox.Size = new System.Drawing.Size(519, 20);
            this.InputBox.TabIndex = 1;
            // 
            // ConvertButton
            // 
            this.ConvertButton.Font = new System.Drawing.Font("Montserrat Medium", 9.749999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConvertButton.Location = new System.Drawing.Point(35, 112);
            this.ConvertButton.Name = "ConvertButton";
            this.ConvertButton.Size = new System.Drawing.Size(134, 35);
            this.ConvertButton.TabIndex = 2;
            this.ConvertButton.Text = "Start Download";
            this.ConvertButton.UseVisualStyleBackColor = true;
            this.ConvertButton.Click += new System.EventHandler(this.ConvertButton_Click);
            // 
            // ProgressBar
            // 
            this.ProgressBar.Location = new System.Drawing.Point(199, 112);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(354, 35);
            this.ProgressBar.TabIndex = 3;
            // 
            // ProgressLabel
            // 
            this.ProgressLabel.AutoSize = true;
            this.ProgressLabel.Font = new System.Drawing.Font("Montserrat Medium", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProgressLabel.Location = new System.Drawing.Point(570, 112);
            this.ProgressLabel.Name = "ProgressLabel";
            this.ProgressLabel.Size = new System.Drawing.Size(0, 21);
            this.ProgressLabel.TabIndex = 4;
            // 
            // QualitySettings
            // 
            this.QualitySettings.Controls.Add(this.Button4K);
            this.QualitySettings.Controls.Add(this.Button360p);
            this.QualitySettings.Controls.Add(this.Button480p);
            this.QualitySettings.Controls.Add(this.Button720p);
            this.QualitySettings.Controls.Add(this.Button1080p);
            this.QualitySettings.Controls.Add(this.Button1440p);
            this.QualitySettings.Controls.Add(this.ButtonMax);
            this.QualitySettings.Font = new System.Drawing.Font("Montserrat Medium", 8.249999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QualitySettings.Location = new System.Drawing.Point(35, 165);
            this.QualitySettings.Name = "QualitySettings";
            this.QualitySettings.Size = new System.Drawing.Size(154, 225);
            this.QualitySettings.TabIndex = 6;
            this.QualitySettings.TabStop = false;
            this.QualitySettings.Text = "Quality Settings";
            // 
            // Button4K
            // 
            this.Button4K.AutoSize = true;
            this.Button4K.Location = new System.Drawing.Point(17, 57);
            this.Button4K.Name = "Button4K";
            this.Button4K.Size = new System.Drawing.Size(112, 19);
            this.Button4K.TabIndex = 6;
            this.Button4K.Text = "Max. 2160p (4K)";
            this.Button4K.UseVisualStyleBackColor = true;
            // 
            // Button360p
            // 
            this.Button360p.AutoSize = true;
            this.Button360p.Location = new System.Drawing.Point(17, 186);
            this.Button360p.Name = "Button360p";
            this.Button360p.Size = new System.Drawing.Size(82, 19);
            this.Button360p.TabIndex = 5;
            this.Button360p.Text = "Max. 360p";
            this.Button360p.UseVisualStyleBackColor = true;
            // 
            // Button480p
            // 
            this.Button480p.AutoSize = true;
            this.Button480p.Location = new System.Drawing.Point(17, 160);
            this.Button480p.Name = "Button480p";
            this.Button480p.Size = new System.Drawing.Size(83, 19);
            this.Button480p.TabIndex = 4;
            this.Button480p.Text = "Max. 480p";
            this.Button480p.UseVisualStyleBackColor = true;
            // 
            // Button720p
            // 
            this.Button720p.AutoSize = true;
            this.Button720p.Location = new System.Drawing.Point(17, 134);
            this.Button720p.Name = "Button720p";
            this.Button720p.Size = new System.Drawing.Size(82, 19);
            this.Button720p.TabIndex = 3;
            this.Button720p.Text = "Max. 720p";
            this.Button720p.UseVisualStyleBackColor = true;
            // 
            // Button1080p
            // 
            this.Button1080p.AutoSize = true;
            this.Button1080p.Location = new System.Drawing.Point(17, 108);
            this.Button1080p.Name = "Button1080p";
            this.Button1080p.Size = new System.Drawing.Size(87, 19);
            this.Button1080p.TabIndex = 2;
            this.Button1080p.Text = "Max. 1080p";
            this.Button1080p.UseVisualStyleBackColor = true;
            // 
            // Button1440p
            // 
            this.Button1440p.AutoSize = true;
            this.Button1440p.Location = new System.Drawing.Point(17, 82);
            this.Button1440p.Name = "Button1440p";
            this.Button1440p.Size = new System.Drawing.Size(87, 19);
            this.Button1440p.TabIndex = 1;
            this.Button1440p.Text = "Max. 1440p";
            this.Button1440p.UseVisualStyleBackColor = true;
            // 
            // ButtonMax
            // 
            this.ButtonMax.AutoSize = true;
            this.ButtonMax.Checked = true;
            this.ButtonMax.Location = new System.Drawing.Point(17, 32);
            this.ButtonMax.Name = "ButtonMax";
            this.ButtonMax.Size = new System.Drawing.Size(121, 19);
            this.ButtonMax.TabIndex = 0;
            this.ButtonMax.TabStop = true;
            this.ButtonMax.Text = "Highest Possible";
            this.ButtonMax.UseVisualStyleBackColor = true;
            // 
            // TargetDirLabel
            // 
            this.TargetDirLabel.AutoSize = true;
            this.TargetDirLabel.Font = new System.Drawing.Font("Montserrat Medium", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TargetDirLabel.Location = new System.Drawing.Point(570, 175);
            this.TargetDirLabel.Name = "TargetDirLabel";
            this.TargetDirLabel.Size = new System.Drawing.Size(136, 21);
            this.TargetDirLabel.TabIndex = 8;
            this.TargetDirLabel.Text = "Target Directory:";
            // 
            // ChooseButton
            // 
            this.ChooseButton.Font = new System.Drawing.Font("Montserrat Medium", 9.749999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChooseButton.Location = new System.Drawing.Point(573, 202);
            this.ChooseButton.Name = "ChooseButton";
            this.ChooseButton.Size = new System.Drawing.Size(75, 35);
            this.ChooseButton.TabIndex = 9;
            this.ChooseButton.Text = "Choose";
            this.ChooseButton.UseVisualStyleBackColor = true;
            this.ChooseButton.Click += new System.EventHandler(this.ChooseButton_Click);
            // 
            // ResetButton
            // 
            this.ResetButton.Font = new System.Drawing.Font("Montserrat Medium", 9.749999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResetButton.Location = new System.Drawing.Point(654, 202);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(75, 35);
            this.ResetButton.TabIndex = 10;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // PathTextBox
            // 
            this.PathTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.PathTextBox.Enabled = false;
            this.PathTextBox.Font = new System.Drawing.Font("Montserrat Medium", 8.249999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PathTextBox.Location = new System.Drawing.Point(573, 244);
            this.PathTextBox.Multiline = true;
            this.PathTextBox.Name = "PathTextBox";
            this.PathTextBox.ReadOnly = true;
            this.PathTextBox.Size = new System.Drawing.Size(190, 63);
            this.PathTextBox.TabIndex = 11;
            this.PathTextBox.Text = "Standart Download Folder";
            // 
            // StepLabel
            // 
            this.StepLabel.AutoSize = true;
            this.StepLabel.Font = new System.Drawing.Font("Montserrat Medium", 8.249999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StepLabel.Location = new System.Drawing.Point(571, 133);
            this.StepLabel.Name = "StepLabel";
            this.StepLabel.Size = new System.Drawing.Size(0, 15);
            this.StepLabel.TabIndex = 12;
            // 
            // ProgressTextBox
            // 
            this.ProgressTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ProgressTextBox.Enabled = false;
            this.ProgressTextBox.Location = new System.Drawing.Point(199, 175);
            this.ProgressTextBox.Name = "ProgressTextBox";
            this.ProgressTextBox.ReadOnly = true;
            this.ProgressTextBox.Size = new System.Drawing.Size(354, 215);
            this.ProgressTextBox.TabIndex = 13;
            this.ProgressTextBox.Text = "";
            // 
            // FormatLabel
            // 
            this.FormatLabel.AutoSize = true;
            this.FormatLabel.Font = new System.Drawing.Font("Montserrat Medium", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormatLabel.Location = new System.Drawing.Point(570, 35);
            this.FormatLabel.Name = "FormatLabel";
            this.FormatLabel.Size = new System.Drawing.Size(132, 21);
            this.FormatLabel.TabIndex = 14;
            this.FormatLabel.Text = "Choose Format:";
            // 
            // FormatBox
            // 
            this.FormatBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FormatBox.FormattingEnabled = true;
            this.FormatBox.Items.AddRange(new object[] {
            ".mp4",
            ".mkv",
            ".webm",
            ".flv",
            ".mp3",
            ".wav",
            ".oga ",
            ".m4a",
            ".aac"});
            this.FormatBox.Location = new System.Drawing.Point(574, 69);
            this.FormatBox.MaxDropDownItems = 2;
            this.FormatBox.Name = "FormatBox";
            this.FormatBox.Size = new System.Drawing.Size(155, 21);
            this.FormatBox.TabIndex = 15;
            this.FormatBox.SelectedIndexChanged += new System.EventHandler(this.FormatBox_SelectedIndexChanged);
            // 
            // ASettingsLabel
            // 
            this.ASettingsLabel.AutoSize = true;
            this.ASettingsLabel.Font = new System.Drawing.Font("Montserrat Medium", 8.249999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ASettingsLabel.Location = new System.Drawing.Point(35, 404);
            this.ASettingsLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ASettingsLabel.Name = "ASettingsLabel";
            this.ASettingsLabel.Size = new System.Drawing.Size(117, 15);
            this.ASettingsLabel.TabIndex = 16;
            this.ASettingsLabel.Text = "Advanced Settings:";
            // 
            // ReencodeVideoCheck
            // 
            this.ReencodeVideoCheck.AutoSize = true;
            this.ReencodeVideoCheck.Font = new System.Drawing.Font("Montserrat Medium", 8.249999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReencodeVideoCheck.Location = new System.Drawing.Point(38, 427);
            this.ReencodeVideoCheck.Margin = new System.Windows.Forms.Padding(2);
            this.ReencodeVideoCheck.Name = "ReencodeVideoCheck";
            this.ReencodeVideoCheck.Size = new System.Drawing.Size(123, 19);
            this.ReencodeVideoCheck.TabIndex = 17;
            this.ReencodeVideoCheck.Text = "Re-encode Video";
            this.ReencodeTip.SetToolTip(this.ReencodeVideoCheck, "Enable this if you have problems playing the video.\r\nIt will take longer because " +
        "the video will be re-encoded.");
            this.ReencodeVideoCheck.UseVisualStyleBackColor = true;
            // 
            // ReencodeAudioCheck
            // 
            this.ReencodeAudioCheck.AutoSize = true;
            this.ReencodeAudioCheck.Checked = true;
            this.ReencodeAudioCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ReencodeAudioCheck.Font = new System.Drawing.Font("Montserrat Medium", 8.249999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReencodeAudioCheck.Location = new System.Drawing.Point(38, 447);
            this.ReencodeAudioCheck.Margin = new System.Windows.Forms.Padding(2);
            this.ReencodeAudioCheck.Name = "ReencodeAudioCheck";
            this.ReencodeAudioCheck.Size = new System.Drawing.Size(123, 19);
            this.ReencodeAudioCheck.TabIndex = 18;
            this.ReencodeAudioCheck.Text = "Re-encode Audio";
            this.ReencodeTip.SetToolTip(this.ReencodeAudioCheck, "Enable this if you have problems playing the audio.\r\nIt will take longer because " +
        "the audio will be re-encoded.");
            this.ReencodeAudioCheck.UseVisualStyleBackColor = true;
            // 
            // AdvancedInformationsCheck
            // 
            this.AdvancedInformationsCheck.AutoSize = true;
            this.AdvancedInformationsCheck.Font = new System.Drawing.Font("Montserrat Medium", 8.249999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AdvancedInformationsCheck.Location = new System.Drawing.Point(38, 470);
            this.AdvancedInformationsCheck.Margin = new System.Windows.Forms.Padding(2);
            this.AdvancedInformationsCheck.Name = "AdvancedInformationsCheck";
            this.AdvancedInformationsCheck.Size = new System.Drawing.Size(193, 19);
            this.AdvancedInformationsCheck.TabIndex = 19;
            this.AdvancedInformationsCheck.Text = "Show Advanced Informations";
            this.ReencodeTip.SetToolTip(this.AdvancedInformationsCheck, "Displays extended information about video, download and conversion");
            this.AdvancedInformationsCheck.UseVisualStyleBackColor = true;
            this.AdvancedInformationsCheck.CheckedChanged += new System.EventHandler(this.AdvancedInformationsCheck_CheckedChanged);
            // 
            // AdvancedInformationsTextBox
            // 
            this.AdvancedInformationsTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.AdvancedInformationsTextBox.Enabled = false;
            this.AdvancedInformationsTextBox.Font = new System.Drawing.Font("Montserrat", 9.7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AdvancedInformationsTextBox.Location = new System.Drawing.Point(38, 507);
            this.AdvancedInformationsTextBox.Name = "AdvancedInformationsTextBox";
            this.AdvancedInformationsTextBox.ReadOnly = true;
            this.AdvancedInformationsTextBox.Size = new System.Drawing.Size(699, 262);
            this.AdvancedInformationsTextBox.TabIndex = 20;
            this.AdvancedInformationsTextBox.Text = "Waiting for Download...";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(776, 781);
            this.Controls.Add(this.AdvancedInformationsTextBox);
            this.Controls.Add(this.AdvancedInformationsCheck);
            this.Controls.Add(this.ReencodeAudioCheck);
            this.Controls.Add(this.ReencodeVideoCheck);
            this.Controls.Add(this.ASettingsLabel);
            this.Controls.Add(this.FormatBox);
            this.Controls.Add(this.FormatLabel);
            this.Controls.Add(this.ProgressTextBox);
            this.Controls.Add(this.StepLabel);
            this.Controls.Add(this.PathTextBox);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.ChooseButton);
            this.Controls.Add(this.TargetDirLabel);
            this.Controls.Add(this.QualitySettings);
            this.Controls.Add(this.ProgressLabel);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.ConvertButton);
            this.Controls.Add(this.InputBox);
            this.Controls.Add(this.Headline);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Media Downloader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.QualitySettings.ResumeLayout(false);
            this.QualitySettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Headline;
        private System.Windows.Forms.TextBox InputBox;
        private System.Windows.Forms.Button ConvertButton;
        private System.Windows.Forms.ProgressBar ProgressBar;
        private System.Windows.Forms.Label ProgressLabel;
        private System.Windows.Forms.GroupBox QualitySettings;
        private System.Windows.Forms.RadioButton ButtonMax;
        private System.Windows.Forms.RadioButton Button360p;
        private System.Windows.Forms.RadioButton Button480p;
        private System.Windows.Forms.RadioButton Button720p;
        private System.Windows.Forms.RadioButton Button1080p;
        private System.Windows.Forms.RadioButton Button1440p;
        private System.Windows.Forms.Label TargetDirLabel;
        private System.Windows.Forms.Button ChooseButton;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.TextBox PathTextBox;
        private System.Windows.Forms.RadioButton Button4K;
        private System.Windows.Forms.Label StepLabel;
        private System.Windows.Forms.RichTextBox ProgressTextBox;
        private System.Windows.Forms.Label FormatLabel;
        private System.Windows.Forms.ComboBox FormatBox;
        private System.Windows.Forms.Label ASettingsLabel;
        private System.Windows.Forms.CheckBox ReencodeVideoCheck;
        private System.Windows.Forms.CheckBox ReencodeAudioCheck;
        private System.Windows.Forms.ToolTip ReencodeTip;
        private System.Windows.Forms.CheckBox AdvancedInformationsCheck;
        private System.Windows.Forms.RichTextBox AdvancedInformationsTextBox;
    }
}

