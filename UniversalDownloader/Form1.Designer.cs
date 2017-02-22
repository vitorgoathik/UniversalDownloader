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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.DownloadProgressBar = new System.Windows.Forms.ProgressBar();
            this.EnterUrlLabel = new System.Windows.Forms.Label();
            this.urlTextBox = new System.Windows.Forms.TextBox();
            this.ErrorLabel = new System.Windows.Forms.Label();
            this.DownloadButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.DownloadSpeedLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ProgressLabel = new System.Windows.Forms.Label();
            this.DestinationTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SaveAsButton = new System.Windows.Forms.Button();
            this.DownloadingFileLabel = new System.Windows.Forms.Label();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // DownloadProgressBar
            // 
            this.DownloadProgressBar.Location = new System.Drawing.Point(29, 250);
            this.DownloadProgressBar.Name = "DownloadProgressBar";
            this.DownloadProgressBar.Size = new System.Drawing.Size(427, 23);
            this.DownloadProgressBar.TabIndex = 0;
            // 
            // EnterUrlLabel
            // 
            this.EnterUrlLabel.AutoSize = true;
            this.EnterUrlLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EnterUrlLabel.Location = new System.Drawing.Point(25, 35);
            this.EnterUrlLabel.Name = "EnterUrlLabel";
            this.EnterUrlLabel.Size = new System.Drawing.Size(45, 19);
            this.EnterUrlLabel.TabIndex = 1;
            this.EnterUrlLabel.Text = "URLs:";
            // 
            // urlTextBox
            // 
            this.urlTextBox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.urlTextBox.Location = new System.Drawing.Point(122, 27);
            this.urlTextBox.Multiline = true;
            this.urlTextBox.Name = "urlTextBox";
            this.urlTextBox.Size = new System.Drawing.Size(334, 88);
            this.urlTextBox.TabIndex = 2;
            // 
            // ErrorLabel
            // 
            this.ErrorLabel.AutoSize = true;
            this.ErrorLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.ErrorLabel.Location = new System.Drawing.Point(58, 334);
            this.ErrorLabel.Name = "ErrorLabel";
            this.ErrorLabel.Size = new System.Drawing.Size(0, 19);
            this.ErrorLabel.TabIndex = 3;
            // 
            // DownloadButton
            // 
            this.DownloadButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DownloadButton.Location = new System.Drawing.Point(122, 177);
            this.DownloadButton.Name = "DownloadButton";
            this.DownloadButton.Size = new System.Drawing.Size(98, 35);
            this.DownloadButton.TabIndex = 4;
            this.DownloadButton.Text = "Download";
            this.DownloadButton.UseVisualStyleBackColor = true;
            this.DownloadButton.Click += new System.EventHandler(this.DownloadButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(57, 276);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 19);
            this.label1.TabIndex = 5;
            this.label1.Text = "Download speed:";
            // 
            // DownloadSpeedLabel
            // 
            this.DownloadSpeedLabel.AutoSize = true;
            this.DownloadSpeedLabel.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DownloadSpeedLabel.ForeColor = System.Drawing.Color.Green;
            this.DownloadSpeedLabel.Location = new System.Drawing.Point(204, 281);
            this.DownloadSpeedLabel.Name = "DownloadSpeedLabel";
            this.DownloadSpeedLabel.Size = new System.Drawing.Size(0, 26);
            this.DownloadSpeedLabel.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(58, 295);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "* green = fast / blue = slow";
            // 
            // ProgressLabel
            // 
            this.ProgressLabel.AutoSize = true;
            this.ProgressLabel.BackColor = System.Drawing.Color.Transparent;
            this.ProgressLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProgressLabel.Location = new System.Drawing.Point(225, 252);
            this.ProgressLabel.Name = "ProgressLabel";
            this.ProgressLabel.Size = new System.Drawing.Size(29, 19);
            this.ProgressLabel.TabIndex = 8;
            this.ProgressLabel.Text = "0%";

            // 
            // DestinationTextBox
            // 
            this.DestinationTextBox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DestinationTextBox.Location = new System.Drawing.Point(122, 132);
            this.DestinationTextBox.Multiline = true;
            this.DestinationTextBox.Name = "DestinationTextBox";
            this.DestinationTextBox.Size = new System.Drawing.Size(291, 34);
            this.DestinationTextBox.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(25, 139);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 19);
            this.label3.TabIndex = 9;
            this.label3.Text = "Destination:";
            // 
            // SaveAsButton
            // 
            this.SaveAsButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SaveAsButton.Image = global::UniversalDownloaderAgoda.Properties.Resources.save_as;
            this.SaveAsButton.Location = new System.Drawing.Point(419, 134);
            this.SaveAsButton.Name = "SaveAsButton";
            this.SaveAsButton.Size = new System.Drawing.Size(37, 31);
            this.SaveAsButton.TabIndex = 11;
            this.SaveAsButton.UseVisualStyleBackColor = true;
            this.SaveAsButton.Click += new System.EventHandler(this.SaveAsButton_Click);
            // 
            // DownloadingFileLabel
            // 
            this.DownloadingFileLabel.AutoSize = true;
            this.DownloadingFileLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DownloadingFileLabel.Location = new System.Drawing.Point(55, 228);
            this.DownloadingFileLabel.Name = "DownloadingFileLabel";
            this.DownloadingFileLabel.Size = new System.Drawing.Size(129, 19);
            this.DownloadingFileLabel.TabIndex = 12;
            this.DownloadingFileLabel.Text = "Downloading file:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 450);
            this.Controls.Add(this.DownloadingFileLabel);
            this.Controls.Add(this.SaveAsButton);
            this.Controls.Add(this.DestinationTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ProgressLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.DownloadSpeedLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DownloadButton);
            this.Controls.Add(this.ErrorLabel);
            this.Controls.Add(this.urlTextBox);
            this.Controls.Add(this.EnterUrlLabel);
            this.Controls.Add(this.DownloadProgressBar);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar DownloadProgressBar;
        private System.Windows.Forms.Label EnterUrlLabel;
        private System.Windows.Forms.TextBox urlTextBox;
        private System.Windows.Forms.Label ErrorLabel;
        private System.Windows.Forms.Button DownloadButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label DownloadSpeedLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label ProgressLabel;
        private System.Windows.Forms.TextBox DestinationTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button SaveAsButton;
        private System.Windows.Forms.Label DownloadingFileLabel;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}

