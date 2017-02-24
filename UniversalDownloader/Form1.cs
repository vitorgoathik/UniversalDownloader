using BatchDownloaderUC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniversalDownloader;

namespace UniversalDownloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            splittingCharComboBox.SelectedIndex = 0;
            DestinationTextBox.Text = Functions.GetDownloadsFolder();
            DownloadProgressBar.Maximum = 100;
            OverallProgressProgressBar.Maximum = 100;
        }
        DownloaderUC downloaderUC;
        bool validate = true;
        private void DownloadButton_Click(object sender, EventArgs e)
        {
            if(validate)
            try
            {
                 downloaderUC = new DownloaderUC(urlTextBox.Text,
                    splittingCharComboBox.SelectedText == "," ? Enums.UrlSplittingChar.Comma : Enums.UrlSplittingChar.Semicolon,
                    DestinationTextBox.Text, 5000);

                downloaderUC.ProcessStarted += DownloaderUC_ProcessStarted;
                downloaderUC.DownloadStarted += DownloaderUC_DownloadStarted;
                downloaderUC.ProgressChanged += DownloaderUC_ProgressChanged;
                downloaderUC.OverallProgressChanged += DownloaderUC_OverallProgressChanged;
                downloaderUC.ProcessCompleted += DownloaderUC_ProcessCompleted;
                downloaderUC.ProcessError += DownloaderUC_ProcessError;
                    
                validationLabel.ForeColor = Color.DarkGreen;
                validationLabel.Text = String.Format("Total file size: {0}", downloaderUC.CheckListFileTotalSizeInUnits());
                    
                fieldsPanel.Enabled = false;
                BackButton.Visible = true;
                DownloadButton.Text = "Download";
                downloadPanel.Visible = true;
                    validate = false;
                }
            catch (Exception ex)
            {
                validationLabel.Text = ex.Message;
            }
            else
            {
                downloaderUC.StartDownloading();
            }
        }

        int numberFilesToDownload = 0;
        private void DownloaderUC_ProcessStarted(object sender, BatchDownloaderUC.Events.ProcessStartedEventArgs e)
        {
            numberFilesToDownload = e.numberOfFiles;
        }

        private void DownloaderUC_DownloadStarted(object sender, BatchDownloaderUC.Events.DownloadStartedEventArgs e)
        {
            fileNameLabel.Text = e.fileName;
        }
        private void DownloaderUC_ProgressChanged(object sender, Events.ProgressChangedEventArgs e)
        {
            DownloadProgressBar.Value = e.progressPercentage;
            ProgressLabel.Text = e.progressPercentage + "%";
            DownloadSpeedLabel.Text = e.speed.ToString() + "/s";
        }

        private void DownloaderUC_OverallProgressChanged(object sender, Events.OverallProgressChangedEventArgs e)
        {
            int percentage = ((int)Math.Round((double)(100 / numberFilesToDownload)) * (numberFilesToDownload - (e.RemainingFiles.Count+1)));
            OverallProgressProgressBar.Value = percentage;
            OverallProgressPercentLabel.Text = percentage + "%";
        }

        private void DownloaderUC_ProcessCompleted(object sender, BatchDownloaderUC.Events.ProcessCompletedEventArgs e)
        {
            DownloadProgressBar.Value = 100;
            ProgressLabel.Text = "100%";
            OverallProgressProgressBar.Value = 100;
            OverallProgressPercentLabel.Text = "100%";
        }
        
        private void DownloaderUC_ProcessError(object sender, BatchDownloaderUC.Events.ProcessErrorEventArgs e)
        {
            
        }

        private void SaveAsButton_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                DestinationTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            fieldsPanel.Enabled = true;
            BackButton.Visible = false;
            DownloadButton.Text = "Validade";
            downloadPanel.Visible = false;
            validationLabel.ForeColor = Color.Red;
            validationLabel.Text = "";
            validate = true;
        }
    }
}
