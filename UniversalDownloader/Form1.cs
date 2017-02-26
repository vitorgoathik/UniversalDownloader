using BatchDownloaderUC;
using BatchDownloaderUC.Events;
using BatchDownloaderUC.Models;
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
            DestinationTextBox.Text = Functions.GetDownloadsFolder();
            DownloadProgressBar.Maximum = 100;
            OverallProgressProgressBar.Maximum = 100;
            downloaderUC = DownloaderUC.GetInstance();
            downloaderUC.ProcessStarted += DownloaderUC_ProcessStarted;
            downloaderUC.DownloadStarted += DownloaderUC_DownloadStarted;
            downloaderUC.ProgressChanged += DownloaderUC_ProgressChanged;
            downloaderUC.OverallProgressChanged += DownloaderUC_OverallProgressChanged;
            downloaderUC.DownloadingProcessCompleted += DownloaderUC_DownloadingProcessCompleted;
            downloaderUC.ProcessError += DownloaderUC_ProcessError;
            downloaderUC.DownloadCanceled += DownloaderUC_DownloadCanceled;
        }
        DownloaderUC downloaderUC;
        private string estimatedTimeTotal;
        private string estimatedTimeCurrentDownload;

        private void DownloadButton_Click(object sender, EventArgs e)
        {
            try
            {
                downloaderUC.AddDownload(urlTextBox.Text, DestinationTextBox.Text, UsernameTextBox.Text,passwordTextBox.Text);
            }
            catch (Exception ex)
            {
                validationLabel.ForeColor = Color.Red;
                validationLabel.Text = ex.Message;
            }
        }

        private void LoadDataGrid()
        {
            List<DownloadDataGridItem> dgList = new List<DownloadDataGridItem>();
            downloaderUC.DownloadingProcess.DownloadsCollection.ToList().ForEach(o => dgList.Add(new DownloadDataGridItem()
            {
                FileName = o.FileInfo.FileFullName,
                FileSize = o.FileInfo.SizeInUnit,
                DownloadState = o.DownloadState == Enums.DownloadState.Closed ? "Completed" : o.DownloadState.ToString(),
                ElapsedTime = o.ElapsedTimeInSeconds > 0 ? TimeSpan.FromSeconds(o.ElapsedTimeInSeconds).ToString(@"d\.hh\:mm\:ss") : "",
                Destination = o.Destination.FullPath,
                Url = o.FileInfo.Url
            }));

            downloadsGridView.DataSource = dgList;
        }

        private void DownloaderUC_ProcessStarted(object sender, DownloaderEventArgs e)
        {
            downloadPanel.Visible = true;
            LoadDataGrid();
        }

        private void DownloaderUC_DownloadStarted(object sender, DownloaderEventArgs e)
        {
            validationLabel.ForeColor = Color.DarkGreen;
            validationLabel.Text = String.Format("Total file size: {0}", downloaderUC.CurrentDownload.FileInfo.SizeBytes);
            fileNameLabel.Text = downloaderUC.CurrentDownload.FileInfo.FileFullName;
            LoadDataGrid();
        }
        private void DownloaderUC_ProgressChanged(object sender, DownloaderEventArgs e)
        {
            DownloadSpeedLabel.Text = e.SpeedInUnit.ToString() + "/s";

            DownloadProgressBar.Value = downloaderUC.CurrentDownload.PercentCompleted();
            ProgressLabel.Text = DownloadProgressBar.Value + "%";

            OverallProgressProgressBar.Value = downloaderUC.DownloadingProcess.PercentCompleted();
            OverallProgressPercentLabel.Text = OverallProgressProgressBar.Value + "%";

            estimatedTimeCurrentDownload = e.EstimatedTimeCurrentDownload;
            estimatedTimeTotal = e.EstimatedTimeTotal;
        }

        private void DownloaderUC_OverallProgressChanged(object sender, DownloaderEventArgs e)
        {
            LoadDataGrid();
        }


        private void DownloaderUC_DownloadingProcessCompleted(object sender, DownloaderEventArgs e)
        {
            DownloadProgressBar.Value = 100;
            ProgressLabel.Text = "100%";
            OverallProgressProgressBar.Value = 100;
            OverallProgressPercentLabel.Text = "100%";
            LoadDataGrid();
        }
        private void DownloaderUC_ProcessError(object sender, DownloadErrorEventArgs e)
        {
            switch(e.ErrorType)
            {
                case Enums.ErrorType.NotLoggedIn:
                    
                default:
                    validationLabel.ForeColor = Color.Red;
                    validationLabel.Text = e.ErrorMessage;
                    LoadDataGrid();
                break;
            }

        }

        private void DownloaderUC_DownloadCanceled(object sender, DownloaderEventArgs e)
        {
            LoadDataGrid();
        }

        private void SaveAsButton_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                DestinationTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void secondTicker_Tick(object sender, EventArgs e)
        {
            estimatedTimeLabel.Text = estimatedTimeCurrentDownload;
            overallEstimatedTime.Text = estimatedTimeTotal;
        }
        

        private void DeleteSelectedButton_Click(object sender, EventArgs e)
        {
            List<int> fileIndexes = new List<int>();
            if(downloadsGridView.SelectedRows.Count > 0)
            {
                foreach(DataGridViewRow row in downloadsGridView.SelectedRows)
                {
                    fileIndexes.Add(row.Index);
                }
                downloaderUC.CancelDownloads(fileIndexes);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            downloaderUC?.CancelCurrentDownload();
        }
    }
    public class DownloadDataGridItem
    {
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string ElapsedTime { get; set; }
        public string DownloadState { get; set; }
        public string Destination { get; set; }
        public string Url { get; set; }
    }
}
