using BatchDownloaderUC.Events;
using BatchDownloaderUC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchDownloaderUC
{
    public abstract class UIInterface
    {

        public Download CurrentDownload { get; set; }
        private DownloadingProcess downloadingProcess;
        public DownloadingProcess DownloadingProcess
        {
            get
            {
                if (downloadingProcess == null)
                    downloadingProcess = new DownloadingProcess();
                return downloadingProcess;
            }
        }
        

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
