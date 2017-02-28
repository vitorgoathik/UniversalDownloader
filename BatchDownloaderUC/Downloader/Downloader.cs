using BatchDownloaderUC.Events;
using BatchDownloaderUC.Exceptions;
using BatchDownloaderUC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using Utilities.BatchDownloaderUC;
using BatchDownloaderUC.Controller;
using static Utilities.BatchDownloaderUC.Enums;
using System.Threading;

namespace BatchDownloaderUC.Downloader
{
    public abstract class Downloader
    {

        #region common fields 

        /// <summary>
        /// This object will be referenced by a single instance
        /// </summary>
        public DownloadsController DownloadsController { get; set; }

        internal NetworkCredential Credentials;


        #endregion

        #region Abstract Methods

        protected abstract void AddDownloadToList(string url, string destination, string username, string password);
        protected abstract void AbortCurrentDownload(); 
        protected abstract void StartNextDownload();

        #endregion

        #region Actions requiring protocol selection

        public void ProtocolAbortCurrentDownload(bool shutdown)
        {
            ProtocolDownloaderManager.Shutdown = shutdown;
            string file = Path.Combine(DownloadsController.CurrentDownload.Destination.FullPath, DownloadsController.CurrentDownload.RemoteFileInfo.FileFullName);

            //depending on the url, the correct protocol will be assigned for the task
            ((Downloader)ProtocolDownloaderManager.GetInstance(DownloadsController.CurrentDownload.RemoteFileInfo.Url)).AbortCurrentDownload();

            //async calls might need a few seconds to marshal
            Thread.Sleep(2000);
        }

        /// <summary>
        /// will exit if a download has started
        /// or calls the complete if there is no download next
        /// or sets the next download to start and call StartNextDownload()
        /// finally each protocol has a different implementation
        /// </summary>
        protected void ProtocolStartDownloading()
        {
            BeforeStartDownloading();
            //process not started || download pending 
            if (DownloadsController.CurrentDownload?.DownloadState == DownloadState.Started || ProtocolDownloaderManager.Shutdown)
                return;

            //the waiting list has been completely served
            if (DownloadsController.NextDownload == null)
            {
                DownloadsController.ClearDownloads();
                OnDownloadsControllerCompleted();
                return;
            }

            //else, we have our next download to start
            DownloadsController.CurrentDownload = DownloadsController.NextDownload;

            DownloadsController.CurrentDownload.ChangeState(DownloadState.Started);
            //the second event
            OnProcessStarted();
            //depending on the url, the correct protocol will be assigned for the task
            ((Downloader)ProtocolDownloaderManager.GetInstance(DownloadsController.CurrentDownload.RemoteFileInfo.Url)).StartNextDownload();
        }

        #endregion

        #region Actions shared by all protocols

        /// <summary>
        /// Prevent no-internet crashing 
        /// (crashes anyways, but at least deleting the partial data and displaying a message to the uses)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            if (BeforeNetworkAvailabilityChangedHandling(sender, e))
                return;
            if (!e.IsAvailable)
            {
                MessageBox.Show("Internet access has dropped. Closing application...");
                ProtocolAbortCurrentDownload(true);
                Process.GetCurrentProcess().Kill();
            }
        }

        /// <summary>
        /// Prepares the client and controller for a new download to be added
        /// </summary>
        /// <param name="url"></param>
        /// <param name="destination"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void AddDownload(string url, string destination, string username, string password)
        {
            try
            {
                Credentials = new NetworkCredential(username, password);
                BeforeAddDownload(url, destination, username, password);
                AddDownloadToList(url, destination, username, password);
                //the first event to be called
                OnDownloadAdded();
                ProtocolStartDownloading();
            }
            catch (Exception ex)
            {
                OnProcessError(new DownloadErrorEventArgs(ex));
            }
        } 
        /// <summary>
        /// Cancels all pending and started downloads. no partial data will be kept
        /// </summary>
        /// <param name="fileIndexes"></param>
        public void CancelDownloads(List<int> fileIndexes)
        {
            BeforeCancelDownloads(fileIndexes);
            //depending on the url, the correct protocol will be assigned for the task
            bool canceledCurrent = DownloadsController.CancelDownloads(fileIndexes);
            if (canceledCurrent)
                ProtocolAbortCurrentDownload(false);
            else
                OnDownloadCanceled(new DownloadCanceledEventArgs());
        }

        internal virtual string UpdateFileNameWithDistinguishedName()
        {
            DownloadsController.CurrentDownload.RemoteFileInfo.FileFullName =
                Functions.GetDistinguishedFileNameForSaving(
                    DownloadsController.CurrentDownload.RemoteFileInfo.FileFullName,
                    DownloadsController.CurrentDownload.Destination.FullPath);

            return Path.Combine(DownloadsController.CurrentDownload.Destination.FullPath,
                DownloadsController.CurrentDownload.RemoteFileInfo.FileFullName);
        }
        #endregion

        #region Custom Method extensions
        /// <summary>
        /// Custom handling of NetworkAvailabilityChanged. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns>true=handled</returns>
        protected virtual bool BeforeNetworkAvailabilityChangedHandling(object sender, NetworkAvailabilityEventArgs e)
        {
            return false;
        }

        protected virtual void BeforeAddDownload(string url, string destination, string username, string password)
        {
            //custom implementation
        }
        

        protected virtual void BeforeStartDownloading()
        {
            //custom implementation
        }

        protected virtual void BeforeCancelDownloads(List<int> fileIndexes)
        {
            //custom implementation on cancel
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
        public delegate void DownloadsControllerCompletedEventHandler(object sender);
        public event DownloadsControllerCompletedEventHandler DownloadsControllerCompleted;
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
        protected void OnDownloadsControllerCompleted()
        {
            DownloadsControllerCompleted?.Invoke(this);
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