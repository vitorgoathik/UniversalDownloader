namespace UniversalDownloader
{
    partial class MainWindow
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
            this.components = new System.ComponentModel.Container();
            this.validationLabel = new System.Windows.Forms.Label();
            this.DownloadButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.downloadPanel = new System.Windows.Forms.Panel();
            this.overallEstimatedTime = new System.Windows.Forms.Label();
            this.estimatedTimeLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.fileNameLabel = new System.Windows.Forms.Label();
            this.OverallProgressLabel = new System.Windows.Forms.Label();
            this.OverallProgressPercentLabel = new System.Windows.Forms.Label();
            this.OverallProgressProgressBar = new System.Windows.Forms.ProgressBar();
            this.DownloadingFileLabel = new System.Windows.Forms.Label();
            this.ProgressLabel = new System.Windows.Forms.Label();
            this.DownloadSpeedLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.DownloadProgressBar = new System.Windows.Forms.ProgressBar();
            this.label5 = new System.Windows.Forms.Label();
            this.fieldsPanel = new System.Windows.Forms.Panel();
            this.SaveAsButton = new System.Windows.Forms.Button();
            this.DestinationTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.urlTextBox = new System.Windows.Forms.TextBox();
            this.EnterUrlLabel = new System.Windows.Forms.Label();
            this.downloadsGridView = new System.Windows.Forms.DataGridView();
            this.DeleteSelectedButton = new System.Windows.Forms.Button();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.UsernameTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.secondTicker = new System.Windows.Forms.Timer(this.components);
            this.FileNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileSizeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ElapsedTimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MessageColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DestinationColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UrlColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.downloadPanel.SuspendLayout();
            this.fieldsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.downloadsGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // validationLabel
            // 
            this.validationLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.validationLabel.ForeColor = System.Drawing.Color.Red;
            this.validationLabel.Location = new System.Drawing.Point(25, 250);
            this.validationLabel.Name = "validationLabel";
            this.validationLabel.Size = new System.Drawing.Size(435, 46);
            this.validationLabel.TabIndex = 3;
            // 
            // DownloadButton
            // 
            this.DownloadButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DownloadButton.Location = new System.Drawing.Point(341, 184);
            this.DownloadButton.Name = "DownloadButton";
            this.DownloadButton.Size = new System.Drawing.Size(122, 61);
            this.DownloadButton.TabIndex = 5;
            this.DownloadButton.Text = "Download";
            this.DownloadButton.UseVisualStyleBackColor = true;
            this.DownloadButton.Click += new System.EventHandler(this.DownloadButton_Click);
            // 
            // downloadPanel
            // 
            this.downloadPanel.Controls.Add(this.overallEstimatedTime);
            this.downloadPanel.Controls.Add(this.estimatedTimeLabel);
            this.downloadPanel.Controls.Add(this.label6);
            this.downloadPanel.Controls.Add(this.fileNameLabel);
            this.downloadPanel.Controls.Add(this.OverallProgressLabel);
            this.downloadPanel.Controls.Add(this.OverallProgressPercentLabel);
            this.downloadPanel.Controls.Add(this.OverallProgressProgressBar);
            this.downloadPanel.Controls.Add(this.DownloadingFileLabel);
            this.downloadPanel.Controls.Add(this.ProgressLabel);
            this.downloadPanel.Controls.Add(this.DownloadSpeedLabel);
            this.downloadPanel.Controls.Add(this.label1);
            this.downloadPanel.Controls.Add(this.DownloadProgressBar);
            this.downloadPanel.Location = new System.Drawing.Point(12, 299);
            this.downloadPanel.Name = "downloadPanel";
            this.downloadPanel.Size = new System.Drawing.Size(463, 204);
            this.downloadPanel.TabIndex = 18;
            this.downloadPanel.Visible = false;
            // 
            // overallEstimatedTime
            // 
            this.overallEstimatedTime.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overallEstimatedTime.ForeColor = System.Drawing.Color.Green;
            this.overallEstimatedTime.Location = new System.Drawing.Point(204, 107);
            this.overallEstimatedTime.Name = "overallEstimatedTime";
            this.overallEstimatedTime.Size = new System.Drawing.Size(227, 58);
            this.overallEstimatedTime.TabIndex = 29;
            this.overallEstimatedTime.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // estimatedTimeLabel
            // 
            this.estimatedTimeLabel.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.estimatedTimeLabel.ForeColor = System.Drawing.Color.Green;
            this.estimatedTimeLabel.Location = new System.Drawing.Point(224, 22);
            this.estimatedTimeLabel.Name = "estimatedTimeLabel";
            this.estimatedTimeLabel.Size = new System.Drawing.Size(226, 56);
            this.estimatedTimeLabel.TabIndex = 27;
            this.estimatedTimeLabel.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(378, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 19);
            this.label6.TabIndex = 26;
            this.label6.Text = "Est. time";
            // 
            // fileNameLabel
            // 
            this.fileNameLabel.AutoSize = true;
            this.fileNameLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileNameLabel.Location = new System.Drawing.Point(132, 5);
            this.fileNameLabel.Name = "fileNameLabel";
            this.fileNameLabel.Size = new System.Drawing.Size(0, 19);
            this.fileNameLabel.TabIndex = 25;
            // 
            // OverallProgressLabel
            // 
            this.OverallProgressLabel.AutoSize = true;
            this.OverallProgressLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OverallProgressLabel.Location = new System.Drawing.Point(3, 146);
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
            this.OverallProgressPercentLabel.Location = new System.Drawing.Point(210, 170);
            this.OverallProgressPercentLabel.Name = "OverallProgressPercentLabel";
            this.OverallProgressPercentLabel.Size = new System.Drawing.Size(29, 19);
            this.OverallProgressPercentLabel.TabIndex = 23;
            this.OverallProgressPercentLabel.Text = "0%";
            // 
            // OverallProgressProgressBar
            // 
            this.OverallProgressProgressBar.Location = new System.Drawing.Point(14, 168);
            this.OverallProgressProgressBar.Name = "OverallProgressProgressBar";
            this.OverallProgressProgressBar.Size = new System.Drawing.Size(427, 23);
            this.OverallProgressProgressBar.TabIndex = 22;
            // 
            // DownloadingFileLabel
            // 
            this.DownloadingFileLabel.AutoSize = true;
            this.DownloadingFileLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DownloadingFileLabel.Location = new System.Drawing.Point(3, 4);
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
            this.ProgressLabel.Location = new System.Drawing.Point(210, 83);
            this.ProgressLabel.Name = "ProgressLabel";
            this.ProgressLabel.Size = new System.Drawing.Size(29, 19);
            this.ProgressLabel.TabIndex = 20;
            this.ProgressLabel.Text = "0%";
            // 
            // DownloadSpeedLabel
            // 
            this.DownloadSpeedLabel.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DownloadSpeedLabel.ForeColor = System.Drawing.Color.Green;
            this.DownloadSpeedLabel.Location = new System.Drawing.Point(128, 31);
            this.DownloadSpeedLabel.Name = "DownloadSpeedLabel";
            this.DownloadSpeedLabel.Size = new System.Drawing.Size(97, 47);
            this.DownloadSpeedLabel.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 19);
            this.label1.TabIndex = 17;
            this.label1.Text = "Download speed:";
            // 
            // DownloadProgressBar
            // 
            this.DownloadProgressBar.Location = new System.Drawing.Point(14, 81);
            this.DownloadProgressBar.Name = "DownloadProgressBar";
            this.DownloadProgressBar.Size = new System.Drawing.Size(427, 23);
            this.DownloadProgressBar.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(312, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(316, 39);
            this.label5.TabIndex = 19;
            this.label5.Text = "Batch File Downloader";
            // 
            // fieldsPanel
            // 
            this.fieldsPanel.Controls.Add(this.SaveAsButton);
            this.fieldsPanel.Controls.Add(this.DestinationTextBox);
            this.fieldsPanel.Controls.Add(this.label3);
            this.fieldsPanel.Controls.Add(this.urlTextBox);
            this.fieldsPanel.Controls.Add(this.EnterUrlLabel);
            this.fieldsPanel.Location = new System.Drawing.Point(29, 51);
            this.fieldsPanel.Name = "fieldsPanel";
            this.fieldsPanel.Size = new System.Drawing.Size(434, 122);
            this.fieldsPanel.TabIndex = 20;
            // 
            // SaveAsButton
            // 
            this.SaveAsButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SaveAsButton.Image = global::UniversalDownloader.Properties.Resources.saveonfolder;
            this.SaveAsButton.Location = new System.Drawing.Point(386, 76);
            this.SaveAsButton.Name = "SaveAsButton";
            this.SaveAsButton.Size = new System.Drawing.Size(37, 31);
            this.SaveAsButton.TabIndex = 22;
            this.SaveAsButton.UseVisualStyleBackColor = true;
            this.SaveAsButton.Click += new System.EventHandler(this.SaveAsButton_Click);
            // 
            // DestinationTextBox
            // 
            this.DestinationTextBox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DestinationTextBox.Location = new System.Drawing.Point(102, 74);
            this.DestinationTextBox.Multiline = true;
            this.DestinationTextBox.Name = "DestinationTextBox";
            this.DestinationTextBox.Size = new System.Drawing.Size(278, 34);
            this.DestinationTextBox.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(-2, 81);
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
            this.urlTextBox.Size = new System.Drawing.Size(321, 43);
            this.urlTextBox.TabIndex = 1;
            // 
            // EnterUrlLabel
            // 
            this.EnterUrlLabel.AutoSize = true;
            this.EnterUrlLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EnterUrlLabel.Location = new System.Drawing.Point(-2, 22);
            this.EnterUrlLabel.Name = "EnterUrlLabel";
            this.EnterUrlLabel.Size = new System.Drawing.Size(39, 19);
            this.EnterUrlLabel.TabIndex = 18;
            this.EnterUrlLabel.Text = "URL:";
            // 
            // downloadsGridView
            // 
            this.downloadsGridView.AllowUserToAddRows = false;
            this.downloadsGridView.AllowUserToDeleteRows = false;
            this.downloadsGridView.BackgroundColor = System.Drawing.Color.Snow;
            this.downloadsGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.downloadsGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.downloadsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.downloadsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FileNameColumn,
            this.FileSizeColumn,
            this.ElapsedTimeColumn,
            this.StateColumn,
            this.MessageColumn,
            this.DestinationColumn,
            this.UrlColumn});
            this.downloadsGridView.Location = new System.Drawing.Point(481, 51);
            this.downloadsGridView.Name = "downloadsGridView";
            this.downloadsGridView.ReadOnly = true;
            this.downloadsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.downloadsGridView.Size = new System.Drawing.Size(404, 389);
            this.downloadsGridView.TabIndex = 21;
            // 
            // DeleteSelectedButton
            // 
            this.DeleteSelectedButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteSelectedButton.Location = new System.Drawing.Point(763, 450);
            this.DeleteSelectedButton.Name = "DeleteSelectedButton";
            this.DeleteSelectedButton.Size = new System.Drawing.Size(122, 44);
            this.DeleteSelectedButton.TabIndex = 23;
            this.DeleteSelectedButton.Text = "Delete Selected";
            this.DeleteSelectedButton.UseVisualStyleBackColor = true;
            this.DeleteSelectedButton.Click += new System.EventHandler(this.DeleteSelectedButton_Click);
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passwordTextBox.Location = new System.Drawing.Point(131, 222);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(181, 23);
            this.passwordTextBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(32, 222);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 19);
            this.label2.TabIndex = 28;
            this.label2.Text = "Password:";
            // 
            // UsernameTextBox
            // 
            this.UsernameTextBox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UsernameTextBox.Location = new System.Drawing.Point(131, 184);
            this.UsernameTextBox.Name = "UsernameTextBox";
            this.UsernameTextBox.Size = new System.Drawing.Size(181, 23);
            this.UsernameTextBox.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(32, 184);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 19);
            this.label4.TabIndex = 26;
            this.label4.Text = "Username:";
            // 
            // secondTicker
            // 
            this.secondTicker.Enabled = true;
            this.secondTicker.Interval = 2000;
            this.secondTicker.Tick += new System.EventHandler(this.secondTicker_Tick);
            // 
            // FileNameColumn
            // 
            this.FileNameColumn.DataPropertyName = "FileName";
            this.FileNameColumn.HeaderText = "File Name";
            this.FileNameColumn.Name = "FileNameColumn";
            this.FileNameColumn.ReadOnly = true;
            // 
            // FileSizeColumn
            // 
            this.FileSizeColumn.DataPropertyName = "FileSize";
            this.FileSizeColumn.HeaderText = "File Size";
            this.FileSizeColumn.Name = "FileSizeColumn";
            this.FileSizeColumn.ReadOnly = true;
            this.FileSizeColumn.Width = 80;
            // 
            // ElapsedTimeColumn
            // 
            this.ElapsedTimeColumn.DataPropertyName = "ElapsedTime";
            this.ElapsedTimeColumn.HeaderText = "Elapsed Time";
            this.ElapsedTimeColumn.Name = "ElapsedTimeColumn";
            this.ElapsedTimeColumn.ReadOnly = true;
            // 
            // StateColumn
            // 
            this.StateColumn.DataPropertyName = "DownloadState";
            this.StateColumn.HeaderText = "State";
            this.StateColumn.Name = "StateColumn";
            this.StateColumn.ReadOnly = true;
            this.StateColumn.Width = 80;
            // 
            // MessageColumn
            // 
            this.MessageColumn.DataPropertyName = "Message";
            this.MessageColumn.HeaderText = "Message";
            this.MessageColumn.Name = "MessageColumn";
            this.MessageColumn.ReadOnly = true;
            // 
            // DestinationColumn
            // 
            this.DestinationColumn.DataPropertyName = "Destination";
            this.DestinationColumn.HeaderText = "Destination Folder";
            this.DestinationColumn.Name = "DestinationColumn";
            this.DestinationColumn.ReadOnly = true;
            this.DestinationColumn.Width = 200;
            // 
            // UrlColumn
            // 
            this.UrlColumn.DataPropertyName = "Url";
            this.UrlColumn.HeaderText = "Url";
            this.UrlColumn.Name = "UrlColumn";
            this.UrlColumn.ReadOnly = true;
            this.UrlColumn.Width = 300;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Snow;
            this.ClientSize = new System.Drawing.Size(912, 506);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.UsernameTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.DeleteSelectedButton);
            this.Controls.Add(this.downloadsGridView);
            this.Controls.Add(this.fieldsPanel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.downloadPanel);
            this.Controls.Add(this.DownloadButton);
            this.Controls.Add(this.validationLabel);
            this.Name = "Form1";
            this.Text = "Universal Downloader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.downloadPanel.ResumeLayout(false);
            this.downloadPanel.PerformLayout();
            this.fieldsPanel.ResumeLayout(false);
            this.fieldsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.downloadsGridView)).EndInit();
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
        private System.Windows.Forms.Label DownloadSpeedLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar DownloadProgressBar;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel fieldsPanel;
        private System.Windows.Forms.Button SaveAsButton;
        private System.Windows.Forms.TextBox DestinationTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox urlTextBox;
        private System.Windows.Forms.Label EnterUrlLabel;
        private System.Windows.Forms.Label fileNameLabel;
        private System.Windows.Forms.Label overallEstimatedTime;
        private System.Windows.Forms.Label estimatedTimeLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView downloadsGridView;
        private System.Windows.Forms.Button DeleteSelectedButton;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox UsernameTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer secondTicker;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileSizeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ElapsedTimeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn StateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MessageColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DestinationColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn UrlColumn;
    }
}

