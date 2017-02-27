using BatchDownloaderUC.Events;
using BatchDownloaderUC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using static Utilities.BatchDownloaderUC.Enums;

namespace BatchDownloaderUC
{
    public abstract class IDownloader
    {

        #region common fields 

        public NetworkCredential Credentials;
        public Download CurrentDownload { get; set; } 
        private DownloadingProcess downloadingProcess;
        public DownloadingProcess DownloadingProcess
        {
            get
            {
                if(downloadingProcess == null)
                    downloadingProcess = new DownloadingProcess();
                return downloadingProcess;
            }
        }


        #endregion

        #region Abstract Methods

        protected abstract void AddDownloadToList(string url, string destination, string username, string password);
        public abstract void AbortCurrentDownload(); 
        protected abstract void StartNextDownload();

        #endregion

        #region Common Virtual Methods
        /// <summary>
        /// Prevent no-internet crashing 
        /// (crashes anyways, but at least deleting the partial data and displaying a message to the uses)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            if (!e.IsAvailable)
            {
                MessageBox.Show("Internet access has dropped. Closing application...");
                Process.GetCurrentProcess().CloseMainWindow();
            }
            else
                StartDownloading();
        }
        /// <summary>
        /// Prepares the client and controller for a new download to be added
        /// </summary>
        /// <param name="url"></param>
        /// <param name="destination"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public virtual void AddDownload(string url, string destination, string username, string password)
        {
            try
            {
                Credentials = new NetworkCredential(username, password);
                AddDownloadToList(url, destination, username, password);
                //the first event to be called
                OnDownloadAdded();
                StartDownloading();
            }
            catch (Exception ex)
            {
                OnProcessError(new DownloadErrorEventArgs(ex));
            }
        }
        /// <summary>
        /// Starts downloading on the right way
        /// will return if a download has started
        /// or calls the complete if there is no download next
        /// or sets the next download to start and call StartNextDownload()
        /// each protocol has a different implementation
        /// </summary>
        protected virtual void StartDownloading()
        {
            //process not started || download pending 
            if (DownloadingProcess == null || DownloadingProcess.StartedDownload != null)
                return;

            //the waiting list has been completely served
            if (DownloadingProcess.NextDownload == null)
            {
                DownloadingProcess.ClearDownloads();
                OnDownloadingProcessCompleted();
                return;
            }

            //else, we have our next download to start
            CurrentDownload = DownloadingProcess.NextDownload;

            DownloadingProcess.NextDownload.ChangeState(DownloadState.Started);

            //the second event
            OnProcessStarted();

            StartNextDownload();
        }

        /// <summary>
        /// Cancels all pending and started downloads. no partial data will be kept
        /// </summary>
        /// <param name="fileIndexes"></param>
        public virtual void CancelDownloads(List<int> fileIndexes)
        {
            if (DownloadingProcess.CancelDownloads(fileIndexes))
                AbortCurrentDownload();
            else
                OnDownloadCanceled(new DownloadCanceledEventArgs());
        }
        /// <summary>
        /// This method will return an unique name by adding (increment) to their suffix
        /// </summary>
        /// <returns>ex: File(2).ext, in case there are two more files named "File" in the folder</returns> 
        internal virtual string GetDistinguishedFileNameForSaving()
        {
            //first the file must be separated from its extension
            string ext = Path.GetExtension(CurrentDownload.RemoteFileInfo.FileFullName);
            string fileNameWithoutExt = ext != "" ? CurrentDownload.RemoteFileInfo.FileFullName.Replace(ext, "") : CurrentDownload.RemoteFileInfo.FileFullName;

            //then the suffix is built
            string filenameFormat = fileNameWithoutExt + "{0}" + ext;
            string filename = string.Format(filenameFormat, "");
            //and increment baseed on the other files having the same name inside the folder
            int i = 1;
            while (File.Exists(CurrentDownload.Destination.FullPath + "/" + filename))
                filename = string.Format(filenameFormat, "(" + (i++) + ")");
            CurrentDownload.RemoteFileInfo.FileFullName = filename;
            return CurrentDownload.Destination.FullPath + "/" + filename;
        }
        #endregion

        #region EventHandlers


        public delegate void ProcessStartedEventHandler(object sender);
        public event ProcessStartedEventHandler ProcessStarted;
        public delegate void DownloadStartedEventHandler(object sender);
        public event DownloadAddedEventHandler DownloadAdded;
        public delegate void DownloadAddedEventHandler(object sender);
        public event DownloadStartedEventHandler DownloadStarted;
        public delegate void ProgressChangedEventHandler(object sender, DownloaderEventArgs e);
        public event ProgressChangedEventHandler ProgressChanged;
        public delegate void DownloadingProcessCompletedEventHandler(object sender);
        public event DownloadingProcessCompletedEventHandler DownloadingProcessCompleted;
        public delegate void OverallProgressChangedEventHandler(object sender);
        public event OverallProgressChangedEventHandler OverallProgressChanged;
        public delegate void ProcessErrorEventHandler(object sender, DownloadErrorEventArgs e);
        public event ProcessErrorEventHandler ProcessError;
        public delegate void DownloadCanceledEventHandler(object sender, DownloadCanceledEventArgs e);
        public event DownloadCanceledEventHandler DownloadCanceled;
        public delegate void DownloadsUpdatedEventHandler(object sender);
        public event DownloadsUpdatedEventHandler DownloadsUpdated;

        protected void OnDownloadAdded()
        {
            DownloadAdded?.Invoke(this);
            DownloadsUpdated?.Invoke(this);
        }
        protected void OnProcessStarted()
        {
            ProcessStarted?.Invoke(this);
            DownloadsUpdated?.Invoke(this);
        }
        protected void OnDownloadStarted()
        {
            DownloadStarted?.Invoke(this);
            DownloadsUpdated?.Invoke(this);
        }
        protected void OnProgressChanged(DownloaderEventArgs e)
        {
            ProgressChanged?.Invoke(this, e);
        }
        protected void OnDownloadingProcessCompleted()
        {
            DownloadingProcessCompleted?.Invoke(this);
            DownloadsUpdated?.Invoke(this);
        }
        protected void OnOverallProgressChanged()
        {
            OverallProgressChanged?.Invoke(this);
            DownloadsUpdated?.Invoke(this);
        }
        protected void OnProcessError(DownloadErrorEventArgs e)
        {
            ProcessError?.Invoke(this, e);
            DownloadsUpdated?.Invoke(this);
        }
        protected void OnDownloadCanceled(DownloadCanceledEventArgs e)
        {
            DownloadCanceled?.Invoke(this, e);
            DownloadsUpdated?.Invoke(this);
        }
        protected void OnDownloadsUpdated()
        {
            DownloadsUpdated?.Invoke(this);
        }

        #endregion

        

    }
}