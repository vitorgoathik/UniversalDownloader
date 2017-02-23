namespace UniversalDownloader
{
    partial class Form1
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
            this.validationLabel = new System.Windows.Forms.Label();
            this.DownloadButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.downloadPanel = new System.Windows.Forms.Panel();
            this.OverallProgressLabel = new System.Windows.Forms.Label();
            this.OverallProgressPercentLabel = new System.Windows.Forms.Label();
            this.OverallProgressProgressBar = new System.Windows.Forms.ProgressBar();
            this.DownloadingFileLabel = new System.Windows.Forms.Label();
            this.ProgressLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.DownloadSpeedLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.DownloadProgressBar = new System.Windows.Forms.ProgressBar();
            this.label5 = new System.Windows.Forms.Label();
            this.fieldsPanel = new System.Windows.Forms.Panel();
            this.splittingCharComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SaveAsButton = new System.Windows.Forms.Button();
            this.DestinationTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.urlTextBox = new System.Windows.Forms.TextBox();
            this.EnterUrlLabel = new System.Windows.Forms.Label();
            this.BackButton = new System.Windows.Forms.Button();
            this.downloadPanel.SuspendLayout();
            this.fieldsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // validationLabel
            // 
            this.validationLabel.AutoSize = true;
            this.validationLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.validationLabel.ForeColor = System.Drawing.Color.Red;
            this.validationLabel.Location = new System.Drawing.Point(33, 295);
            this.validationLabel.Name = "validationLabel";
            this.validationLabel.Size = new System.Drawing.Size(0, 19);
            this.validationLabel.TabIndex = 3;
            // 
            // DownloadButton
            // 
            this.DownloadButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DownloadButton.Location = new System.Drawing.Point(362, 249);
            this.DownloadButton.Name = "DownloadButton";
            this.DownloadButton.Size = new System.Drawing.Size(98, 35);
            this.DownloadButton.TabIndex = 4;
            this.DownloadButton.Text = "Validate";
            this.DownloadButton.UseVisualStyleBackColor = true;
            this.DownloadButton.Click += new System.EventHandler(this.DownloadButton_Click);
            // 
            // downloadPanel
            // 
            this.downloadPanel.Controls.Add(this.OverallProgressLabel);
            this.downloadPanel.Controls.Add(this.OverallProgressPercentLabel);
            this.downloadPanel.Controls.Add(this.OverallProgressProgressBar);
            this.downloadPanel.Controls.Add(this.DownloadingFileLabel);
            this.downloadPanel.Controls.Add(this.ProgressLabel);
            this.downloadPanel.Controls.Add(this.label2);
            this.downloadPanel.Controls.Add(this.DownloadSpeedLabel);
            this.downloadPanel.Controls.Add(this.label1);
            this.downloadPanel.Controls.Add(this.DownloadProgressBar);
            this.downloadPanel.Location = new System.Drawing.Point(29, 339);
            this.downloadPanel.Name = "downloadPanel";
            this.downloadPanel.Size = new System.Drawing.Size(434, 151);
            this.downloadPanel.TabIndex = 18;
            this.downloadPanel.Visible = false;
            // 
            // OverallProgressLabel
            // 
            this.OverallProgressLabel.AutoSize = true;
            this.OverallProgressLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OverallProgressLabel.Location = new System.Drawing.Point(30, 103);
            this.OverallProgressLabel.Name = "OverallProgressLabel";
            this.OverallProgressLabel.Size = new System.Drawing.Size(124, 19);
            this.OverallProgressLabel.TabIndex = 24;
            this.OverallProgressLabel.Text = "Overall Progress:";
            // 
            // OverallProgressPercentLabel
            // 
            this.OverallProgressPercentLabel.AutoSize = true;
            this.OverallProgressPercentLabel.BackColor = System.Drawing.Color.Transparent;
            this.OverallProgressPercentLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OverallProgressPercentLabel.Location = new System.Drawing.Point(200, 127);
            this.OverallProgressPercentLabel.Name = "OverallProgressPercentLabel";
            this.OverallProgressPercentLabel.Size = new System.Drawing.Size(29, 19);
            this.OverallProgressPercentLabel.TabIndex = 23;
            this.OverallProgressPercentLabel.Text = "0%";
            // 
            // OverallProgressProgressBar
            // 
            this.OverallProgressProgressBar.Location = new System.Drawing.Point(4, 125);
            this.OverallProgressProgressBar.Name = "OverallProgressProgressBar";
            this.OverallProgressProgressBar.Size = new System.Drawing.Size(427, 23);
            this.OverallProgressProgressBar.TabIndex = 22;
            // 
            // DownloadingFileLabel
            // 
            this.DownloadingFileLabel.AutoSize = true;
            this.DownloadingFileLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DownloadingFileLabel.Location = new System.Drawing.Point(30, 4);
            this.DownloadingFileLabel.Name = "DownloadingFileLabel";
            this.DownloadingFileLabel.Size = new System.Drawing.Size(129, 19);
            this.DownloadingFileLabel.TabIndex = 21;
            this.DownloadingFileLabel.Text = "Downloading file:";
            // 
            // ProgressLabel
            // 
            this.ProgressLabel.AutoSize = true;
            this.ProgressLabel.BackColor = System.Drawing.Color.Transparent;
            this.ProgressLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProgressLabel.Location = new System.Drawing.Point(200, 71);
            this.ProgressLabel.Name = "ProgressLabel";
            this.ProgressLabel.Size = new System.Drawing.Size(29, 19);
            this.ProgressLabel.TabIndex = 20;
            this.ProgressLabel.Text = "0%";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "* green = fast / blue = slow";
            // 
            // DownloadSpeedLabel
            // 
            this.DownloadSpeedLabel.AutoSize = true;
            this.DownloadSpeedLabel.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DownloadSpeedLabel.ForeColor = System.Drawing.Color.Green;
            this.DownloadSpeedLabel.Location = new System.Drawing.Point(179, 30);
            this.DownloadSpeedLabel.Name = "DownloadSpeedLabel";
            this.DownloadSpeedLabel.Size = new System.Drawing.Size(0, 26);
            this.DownloadSpeedLabel.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(32, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 19);
            this.label1.TabIndex = 17;
            this.label1.Text = "Download speed:";
            // 
            // DownloadProgressBar
            // 
            this.DownloadProgressBar.Location = new System.Drawing.Point(4, 69);
            this.DownloadProgressBar.Name = "DownloadProgressBar";
            this.DownloadProgressBar.Size = new System.Drawing.Size(427, 23);
            this.DownloadProgressBar.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(93, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(316, 39);
            this.label5.TabIndex = 19;
            this.label5.Text = "Batch File Downloader";
            // 
            // fieldsPanel
            // 
            this.fieldsPanel.Controls.Add(this.splittingCharComboBox);
            this.fieldsPanel.Controls.Add(this.label4);
            this.fieldsPanel.Controls.Add(this.SaveAsButton);
            this.fieldsPanel.Controls.Add(this.DestinationTextBox);
            this.fieldsPanel.Controls.Add(this.label3);
            this.fieldsPanel.Controls.Add(this.urlTextBox);
            this.fieldsPanel.Controls.Add(this.EnterUrlLabel);
            this.fieldsPanel.Location = new System.Drawing.Point(29, 51);
            this.fieldsPanel.Name = "fieldsPanel";
            this.fieldsPanel.Size = new System.Drawing.Size(434, 192);
            this.fieldsPanel.TabIndex = 20;
            // 
            // splittingCharComboBox
            // 
            this.splittingCharComboBox.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.splittingCharComboBox.FormattingEnabled = true;
            this.splittingCharComboBox.Items.AddRange(new object[] {
            ";",
            ","});
            this.splittingCharComboBox.Location = new System.Drawing.Point(102, 107);
            this.splittingCharComboBox.Name = "splittingCharComboBox";
            this.splittingCharComboBox.Size = new System.Drawing.Size(31, 31);
            this.splittingCharComboBox.TabIndex = 24;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(-2, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 19);
            this.label4.TabIndex = 23;
            this.label4.Text = "Splitting Char:";
            // 
            // SaveAsButton
            // 
            this.SaveAsButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SaveAsButton.Image = global::UniversalDownloader.Properties.Resources.saveonfolder;
            this.SaveAsButton.Location = new System.Drawing.Point(399, 146);
            this.SaveAsButton.Name = "SaveAsButton";
            this.SaveAsButton.Size = new System.Drawing.Size(37, 31);
            this.SaveAsButton.TabIndex = 22;
            this.SaveAsButton.UseVisualStyleBackColor = true;
            this.SaveAsButton.Click += new System.EventHandler(this.SaveAsButton_Click);
            // 
            // DestinationTextBox
            // 
            this.DestinationTextBox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DestinationTextBox.Location = new System.Drawing.Point(102, 144);
            this.DestinationTextBox.Multiline = true;
            this.DestinationTextBox.Name = "DestinationTextBox";
            this.DestinationTextBox.Size = new System.Drawing.Size(291, 34);
            this.DestinationTextBox.TabIndex = 21;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(-2, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 19);
            this.label3.TabIndex = 20;
            this.label3.Text = "Destination:";
            // 
            // urlTextBox
            // 
            this.urlTextBox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.urlTextBox.Location = new System.Drawing.Point(102, 14);
            this.urlTextBox.Multiline = true;
            this.urlTextBox.Name = "urlTextBox";
            this.urlTextBox.Size = new System.Drawing.Size(334, 88);
            this.urlTextBox.TabIndex = 19;
            // 
            // EnterUrlLabel
            // 
            this.EnterUrlLabel.AutoSize = true;
            this.EnterUrlLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EnterUrlLabel.Location = new System.Drawing.Point(-2, 22);
            this.EnterUrlLabel.Name = "EnterUrlLabel";
            this.EnterUrlLabel.Size = new System.Drawing.Size(45, 19);
            this.EnterUrlLabel.TabIndex = 18;
            this.EnterUrlLabel.Text = "URLs:";
            // 
            // BackButton
            // 
            this.BackButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BackButton.Location = new System.Drawing.Point(29, 249);
            this.BackButton.Name = "BackButton";
            this.BackButton.Size = new System.Drawing.Size(98, 35);
            this.BackButton.TabIndex = 21;
            this.BackButton.Text = "Back";
            this.BackButton.UseVisualStyleBackColor = true;
            this.BackButton.Visible = false;
            this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 506);
            this.Controls.Add(this.BackButton);
            this.Controls.Add(this.fieldsPanel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.downloadPanel);
            this.Controls.Add(this.DownloadButton);
            this.Controls.Add(this.validationLabel);
            this.Name = "Form1";
            this.Text = "Universal Downloader";
            this.downloadPanel.ResumeLayout(false);
            this.downloadPanel.PerformLayout();
            this.fieldsPanel.ResumeLayout(false);
            this.fieldsPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label validationLabel;
        private System.Windows.Forms.Button DownloadButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Panel downloadPanel;
        private System.Windows.Forms.Label OverallProgressLabel;
        private System.Windows.Forms.Label OverallProgressPercentLabel;
        private System.Windows.Forms.ProgressBar OverallProgressProgressBar;
        private System.Windows.Forms.Label DownloadingFileLabel;
        private System.Windows.Forms.Label ProgressLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label DownloadSpeedLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar DownloadProgressBar;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel fieldsPanel;
        private System.Windows.Forms.ComboBox splittingCharComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button SaveAsButton;
        private System.Windows.Forms.TextBox DestinationTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox urlTextBox;
        private System.Windows.Forms.Label EnterUrlLabel;
        private System.Windows.Forms.Button BackButton;
    }
}

