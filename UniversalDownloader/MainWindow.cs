using BatchDownloaderUC.Events;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static Utilities.BatchDownloaderUC.Enums;
using BatchDownloaderUC.Downloader;
using System.IO;

namespace UniversalDownloader
{
    public partial class MainWindow : Form
    {
        Downloader downloaderUC;

        string speedInUnit = "", estimatedTimeCurrentDownload = "", estimatedTimeTotal = "";
        public MainWindow()
        {
            InitializeComponent();
            DestinationTextBox.Text = GetDownloadsFolder();
            DownloadProgressBar.Maximum = 100;
            OverallProgressProgressBar.Maximum = 100;
        }


        private void LoadDataGrid()
        {
            List<DownloadDataGridItem> dgList = new List<DownloadDataGridItem>();
            downloaderUC.DownloadsController.DownloadsCollection.ToList().ForEach(o => dgList.Add(new DownloadDataGridItem()
            {
                FileName = o.RemoteFileInfo.FileFullName,
                FileSize = o.RemoteFileInfo.SizeInUnit,
                DownloadState = o.DownloadState == DownloadState.Closed ? "Completed" : o.DownloadState.ToString(),
                ElapsedTime = o.ElapsedTimeInSeconds > 0 ? TimeSpan.FromSeconds(o.ElapsedTimeInSeconds).ToString(@"d\.hh\:mm\:ss") : "",
                Destination = o.Destination.FullPath,
                Url = o.RemoteFileInfo.Url,
                Message = o.StateMessage
            }));

            downloadsGridView.DataSource = dgList;
        }

        #region Form events
        private void DownloadButton_Click(object sender, EventArgs e)
        {
            try
            {
                InitializeDownloader(urlTextBox.Text);
                //This is the initial call of the downloader UC. 
                //I have made it so there is no other way to hack into adding a new download from the client
                downloaderUC.AddDownload(urlTextBox.Text, DestinationTextBox.Text, UsernameTextBox.Text, passwordTextBox.Text);
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
            }
        }

        private void InitializeDownloader(string url)
        {
                downloaderUC = ((Downloader)ProtocolDownloaderManager.GetInstance(url));
                
                downloaderUC.DownloadAdded += DownloaderUC_DownloadAdded;
                downloaderUC.ProcessStarted += DownloaderUC_ProcessStarted;
            downloaderUC.DownloadStarted += DownloaderUC_DownloadStarted;
                downloaderUC.ProgressChanged += DownloaderUC_ProgressChanged;
                downloaderUC.OverallProgressChanged += DownloaderUC_OverallProgressChanged;
                downloaderUC.DownloadsControllerCompleted += DownloaderUC_DownloadsControllerCompleted;
                downloaderUC.ProcessError += DownloaderUC_ProcessError;
                downloaderUC.DownloadCanceled += DownloaderUC_DownloadCanceled;
                downloaderUC.DownloadsUpdated += DownloaderUC_DownloadsUpdated;
        }

        private void SaveAsButton_Click(object sender, EventArgs e)
        {
            //explorer folder browser dialog
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                DestinationTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void DeleteSelectedButton_Click(object sender, EventArgs e)
        {
            //the way to delete is by sending a list of indexes that the client selected to delete.
            List<int> fileIndexes = new List<int>();
            if (downloadsGridView.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in downloadsGridView.SelectedRows)
                {
                    fileIndexes.Add(row.Index);
                }
                downloaderUC.CancelDownloads(fileIndexes);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //cancel (and delete parts) on close
            downloaderUC?.AbortCurrentDownload();
        }

        private void secondTicker_Tick(object sender, EventArgs e)
        {
            //educate the UC to hold on before updating the texts
            if (speedInUnit != "")
            {
                DownloadSpeedLabel.Text = speedInUnit + "/s";
            }
            if (estimatedTimeCurrentDownload != "")
            {
                estimatedTimeLabel.Text = estimatedTimeCurrentDownload;
            }
            if (estimatedTimeTotal != "")
            {
                overallEstimatedTime.Text = estimatedTimeTotal;
            }
        }

        #endregion

        #region Downloader events
        private void DownloaderUC_DownloadAdded(object sender)
        {
            //The first thing is adding a new download
        }

        private void DownloaderUC_ProcessStarted(object sender)
        {
            //if the download is valid (no errors), the process starts
            secondTicker.Enabled = true;
            downloadPanel.Visible = true;
        }

        private void DownloaderUC_DownloadStarted(object sender)
        {
            //next, the download itself starts
            validationLabel.ForeColor = Color.DarkGreen;
            validationLabel.Text = String.Format("Total file size: {0}", downloaderUC.DownloadsController.CurrentDownload.RemoteFileInfo.SizeInUnit);
            fileNameLabel.Text = downloaderUC.DownloadsController.CurrentDownload.RemoteFileInfo.FileFullName;
        }
        private void DownloaderUC_ProgressChanged(object sender, DownloaderEventArgs e)
        {
            //each data iteration will be sent here
            if (e.SpeedInUnit != "")
                speedInUnit = e.SpeedInUnit?.ToString() + "/s";

            if (e.EstimatedTimeCurrentDownload != "")
                estimatedTimeCurrentDownload = e.EstimatedTimeCurrentDownload;

            if (e.EstimatedTimeTotal != "")
                estimatedTimeTotal = e.EstimatedTimeTotal;

            DownloadProgressBar.Value = e.PercentCompletedCurrentDownload;
            ProgressLabel.Text = DownloadProgressBar.Value + "%";

            OverallProgressProgressBar.Value = e.PercentCompletedTotal;
            OverallProgressPercentLabel.Text = OverallProgressProgressBar.Value + "%";

        }

        private void DownloaderUC_OverallProgressChanged(object sender)
        {
            //when the overall process changes, it doesnt necessarily means all downloads have finished
        }

        private void DownloaderUC_DownloadsControllerCompleted(object sender)
        {
            //but when it is completed, it does
            OverallProgressPercentLabel.Text = ProgressLabel.Text = "100%";
            OverallProgressProgressBar.Value = DownloadProgressBar.Value = 100;
            OverallProgressPercentLabel.Text = "100%";
            secondTicker.Enabled = false;
            DownloadSpeedLabel.Text = estimatedTimeLabel.Text = overallEstimatedTime.Text = "";
        }

        private void DownloaderUC_ProcessError(object sender, DownloadErrorEventArgs e)
        {
            SetErrorMessage(e.ErrorMessage);
        }

        private void DownloaderUC_DownloadCanceled(object sender, DownloadCanceledEventArgs e)
        {
            validationLabel.ForeColor = Color.Red;
            if(e.FileName != "")
                validationLabel.Text = "Download canceled: " + e.FileName;
            else
                validationLabel.Text = "Downloads canceled";
        }
        private void DownloaderUC_DownloadsUpdated(object sender)
        {
            LoadDataGrid();
        }

        void SetErrorMessage(string message)
        {
            validationLabel.Visible = true;
            validationLabel.ForeColor = Color.Red;
            validationLabel.Text = message;
        }
        #endregion

        public string GetDownloadsFolder()
        {
            try
            {
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = Path.Combine(pathUser, "Downloads");
                return pathDownload;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
    public class DownloadDataGridItem
    {
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string ElapsedTime { get; set; }
        public string DownloadState { get; set; }
        public string Message { get; set; }
        public string Destination { get; set; }
        public string Url { get; set; }
    }
}
