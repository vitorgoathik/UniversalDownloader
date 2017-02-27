using BatchDownloaderUC.Events;
using BatchDownloaderUC.Models;
using BatchDownloaderUC.Utilities;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities.BatchDownloaderUC;
using static Utilities.BatchDownloaderUC.Enums;

namespace BatchDownloaderUC
{
    public class DownloaderUC : IDownloader
    {
        #region private fields 

        public DownloadingProcess DownloadingProcess { get; private set; }
        public Download CurrentDownload { get; private set; }

        #endregion

        
        private static DownloaderUC downloaderUC;
        /// <summary>
        /// Single instance
        /// </summary>
        /// <returns></returns>
        public static DownloaderUC GetInstance()
        {
            if (downloaderUC == null)
                downloaderUC = new DownloaderUC();
            return downloaderUC;
        }

        public DownloaderUC()
        {
            NetworkChange.NetworkAvailabilityChanged += this.NetworkAvailabilityChanged;
        }

        /// <summary>
        /// Prevent no-internet crashing 
        /// (crashes anyways, but at least deleting the partial data and displaying a message to the uses)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            if (!e.IsAvailable)
            {
                MessageBox.Show("Internet access has dropped. Closing application...");
                Process.GetCurrentProcess().CloseMainWindow();
            }
            else
                StartDownloading();
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

        protected virtual void OnDownloadAdded()
        {
            DownloadAdded?.Invoke(this);
            DownloadsUpdated?.Invoke(this);
        }
        protected virtual void OnProcessStarted()
        {
            ProcessStarted?.Invoke(this);
            DownloadsUpdated?.Invoke(this);
        }
        protected virtual void OnDownloadStarted()
        {
            DownloadStarted?.Invoke(this);
            DownloadsUpdated?.Invoke(this);
        }
        protected virtual void OnProgressChanged(DownloaderEventArgs e)
        {
            ProgressChanged?.Invoke(this, e);
        }
        protected virtual void OnDownloadingProcessCompleted()
        {
            DownloadingProcessCompleted?.Invoke(this);
            DownloadsUpdated?.Invoke(this);
        }
        protected virtual void OnOverallProgressChanged()
        {
            OverallProgressChanged?.Invoke(this);
            DownloadsUpdated?.Invoke(this);
        }
        protected virtual void OnProcessError(DownloadErrorEventArgs e)
        {
            ProcessError?.Invoke(this, e);
            DownloadsUpdated?.Invoke(this);
        }
        protected virtual void OnDownloadCanceled(DownloadCanceledEventArgs e)
        {
            DownloadCanceled?.Invoke(this, e);
            DownloadsUpdated?.Invoke(this);
        }
        protected virtual void OnDownloadsUpdated()
        {
            DownloadsUpdated?.Invoke(this);
        }

        #endregion
        
        /// <summary>
        /// Add a new download to the waiting list
        /// </summary>
        /// <param name="url"></param>
        /// <param name="destination"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void AddDownload(string url, string destination, string username, string password)
        {
            if (DownloadingProcess == null)
                DownloadingProcess = new DownloadingProcess();
            try
            {
                //This method has a series of validation checks
                DownloadingProcess.AddDownloadToList(url, destination, username, password);
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
        /// each protocol has a different implementation
        /// </summary>
        private void StartDownloading()
        {
            //process not started || download pending 
            if (DownloadingProcess == null || DownloadingProcess.StartedDownload != null)
                return;

            //the waiting list has been completely served
            if (DownloadingProcess.NextDownload == null)
            {
                speed?.Stop();
                DownloadingProcess.ClearDownloads();
                OnDownloadingProcessCompleted();
                return;
            }

            //else, we have our next download to start
            CurrentDownload = DownloadingProcess.NextDownload;

            DownloadingProcess.NextDownload.ChangeState(DownloadState.Started);

            //the second event
            OnProcessStarted();

            //each protocol has a special implementation, 
            //but both follow the same idea: a background thread with the ability to be interrupted smoothly
            switch (CurrentDownload.Uri.Protocol)
            {
                case Protocol.Http:
                    StartNextDownloadHTTP();
                    break;
                case Protocol.Ftp:
                    StartNextDownloadFtp();
                    break;
            }
        }


        public void AbortCurrentDownload()
        {
            switch(CurrentDownload.Uri.Protocol)
            {
                case Protocol.Http:
                    client?.CancelAsync();
                    break;
                case Protocol.Ftp:
                    backgroundWorker.CancelAsync();
                    break;
            }
        }

        /// <summary>
        /// Cancels all pending and started downloads. no partial data will be kept
        /// </summary>
        /// <param name="fileIndexes"></param>
        public void CancelDownloads(List<int> fileIndexes)
        {
            if (DownloadingProcess.CancelDownloads(fileIndexes))
                AbortCurrentDownload();
            else
                OnDownloadCanceled(new DownloadCanceledEventArgs());
        }

        #region StartNextDownloadHTTP
        CustomWebClient client;
        DownloadSpeed speed;
        
        private void StartNextDownloadHTTP()
        {
            try
            {
                Directory.CreateDirectory(CurrentDownload.Destination.FullPath) ;
                //Http is way more simple than FTP, since it has a built in background worker, no authentication and no folders
                using (client = new CustomWebClient())
                {
                    speed = new DownloadSpeed(client);
                    speed.Start();
                    client.Credentials = CurrentDownload.HttpFileInfo.Credentials;
                    client.DownloadProgressChanged += CustomWebClientDownloadProgressChanged;
                    client.DownloadFileCompleted += CustomWebClientDownloadCompleted;
                    OnDownloadStarted();
                    CurrentDownload.Destination.SetFullUniquePathWithFile();
                    //downloads the file and save in the selected destination
                    client.DownloadFileAsync(new System.Uri(CurrentDownload.Uri.Url), CurrentDownload.Destination.FullPathWithFile);
                }
            }
            catch (Exception e)
            {
                CurrentDownload.ChangeState(DownloadState.Error, true);
                OnProcessError(new DownloadErrorEventArgs(ErrorType.GeneralErrorOnDownload,CurrentDownload.FileInfo.FileFullName, e));
            }
            finally
            {
                //the progress of the download list has changed
                OnOverallProgressChanged();
            }
        }
        
        private void CustomWebClientDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //File level progress has changed. The download is fed with BytesReceived, 
            //so that it can calculate some status data internally and send to the view
            CurrentDownload.BytesReceived = e.BytesReceived;
            OnProgressChanged(new DownloaderEventArgs(
                CurrentDownload.GetRemainingTimeString(speed.Speed),
                DownloadingProcess.GetRemainingTimeString(speed.Speed),
                speed.SpeedInUnit));
        }

        private void CustomWebClientDownloadCompleted(object sender, AsyncCompletedEventArgs args)
        {
            //the two ways to reach here are: completion and cancelation
            if (args.Cancelled)
            {
                string name = CurrentDownload.FileInfo.FileFullName;
                CurrentDownload.ChangeState(DownloadState.Canceled);
                //calls the view providing the canceled name
                OnDownloadCanceled(new DownloadCanceledEventArgs(name));
            }
            else
                CurrentDownload.ChangeState(DownloadState.Completed);
            //the "speed" is a watch that marks time data such as ElapsedTimeInSeconds
            CurrentDownload.ElapsedTimeInSeconds = speed.ElapsedTimeInSeconds;
            //proceed to the next of the line
            StartDownloading();
        }
        #endregion

        #region StartNextDownloadFtp

        BackgroundWorker backgroundWorker; //our FTP starts with a background worker
        private void StartNextDownloadFtp()
        {
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.RunWorkerAsync();
        }

        /// <summary>
        /// DoWork cannot be canceled abruptly. A flag is sent to it and eventually it will see and break the execution
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //a few settings are needed to start a FTP download
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(CurrentDownload.Uri.Url);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.UsePassive = false;
                request.UseBinary = true;// use true for .zip file or false for a text file
                request.KeepAlive = true;
                request.Credentials = CurrentDownload.FtpFileInfo.Credential;

                //a response is opened from the requested URL + authentication
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {

                    //it opens a stream thanks to the downloadfile Method WebRequestMethods.Ftp.DownloadFile
                    using (Stream responseStream = response.GetResponseStream())
                    {

                        using (StreamReader inputStream = new StreamReader(responseStream))
                        {
                            //the directory will be created.
                            //now this mirrors the whole tree from the Url selected (the root) into the specified destination folder
                            Directory.CreateDirectory(CurrentDownload.Destination.FullPath);
                            //repeated names will be renamed
                            CurrentDownload.Destination.SetFullUniquePathWithFile();

                            using (FileStream outputStream = new FileStream(CurrentDownload.Destination.FullPathWithFile, FileMode.Create))
                            { 
                                int readCount = 0;  //bytes read from buffer
                                long totalWrittenBytesCount = 0; //total bytes read
                                byte[] buffer = new byte[8000];//buffer sized 8KB (a bit above average for higher speed)
                                DateTime DownloadStart = DateTime.Now; //this will help calculating the remaining time
                                readCount = responseStream.Read(buffer, 0, buffer.Length); //start the read
                                int sleep = 1; //this is to make the thread sleep for a fraction of second
                                
                                while (readCount > 0)
                                {
                                    if (backgroundWorker.CancellationPending)
                                    {
                                        //cancelation was called. stop everything and report a cancel event to the view
                                        CurrentDownload.ChangeState(DownloadState.Canceled);
                                        backgroundWorker.ReportProgress(0, true);
                                        e.Cancel = true;
                                        break;
                                    }

                                    //write on disk
                                    outputStream.Write(buffer, 0, readCount);
                                    readCount = responseStream.Read(buffer, 0, buffer.Length);
                                    //increment total
                                    totalWrittenBytesCount += readCount;
                                    //update the view with the change of progress in a single download
                                    CalculateAndReportProgress(buffer.Length, totalWrittenBytesCount, DownloadStart);

                                    DownloadStart = DateTime.Now;

                                    //if not for this, the view simply freezes. the lower 
                                    //this number is (sleep == 30), more responsive is the UI, but slower the download
                                    if (sleep == 20)
                                    {
                                        sleep = 1;
                                        Thread.Sleep(1); //a fraction of 1/30 milisecond)
                                    }
                                    sleep++;
                                }
                            }
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                //IOException means the disk is FULL. 
                //the state will change with a message to the view can show in some sort of datagrid
                //Changestate will delete the partial data
                CurrentDownload.ChangeState(DownloadState.Error, true, Enums.GetEnumDescription(ErrorType.InsufficientDiskSpace));
                //this is a hack to access the view under the same thread of the background worker.
                //the UI thread is only accessible from the events Report and Completed fired by the BW
                backgroundWorker.ReportProgress(0, new DownloadErrorEventArgs(ErrorType.InsufficientDiskSpaceFor, CurrentDownload.FileInfo.FileFullName, ex));
                //Disk is full = cancel
                e.Cancel = true;
                backgroundWorker.CancelAsync();
            }
            catch (Exception ex)
            {
                //on regular unpredicted exceptions, the client will also be updated
                string name = CurrentDownload.FileInfo.FileFullName;
                CurrentDownload.ChangeState(DownloadState.Error, true, Enums.GetEnumDescription(ErrorType.GeneralErrorOnDownload));
                if(CurrentDownload.DownloadState == DownloadState.Canceled)
                    backgroundWorker.ReportProgress(0, name);
                else
                    backgroundWorker.ReportProgress(0, new DownloadErrorEventArgs(ErrorType.GeneralErrorOnDownload, CurrentDownload.FileInfo.FileFullName, ex));
            }
        }
        /// <summary>
        /// Calculates the time remaining + report changes, updating the progress bar
        /// </summary>
        /// <param name="bufferLength">the amount of data per block (buffer)</param>
        /// <param name="progress">download progress</param>
        /// <param name="DownloadStart">was updated last report to .Now</param>
        private void CalculateAndReportProgress(int bufferLength, long progress, DateTime DownloadStart)
        {
            long speed;

            TimeSpan DownloadSub = DateTime.Now.Subtract(
                DownloadStart);
            
            speed = DownloadSub.TotalMilliseconds > 0 ? 
                (long)Math.Round(bufferLength / DownloadSub.TotalMilliseconds) * 10000
                : 0;
            //the line below is always needed for those equations
            CurrentDownload.BytesReceived = progress;
            backgroundWorker.ReportProgress(0, new DownloaderEventArgs(
            CurrentDownload.GetRemainingTimeString(speed),
            DownloadingProcess.GetRemainingTimeString(speed),
            speed > 0 ? Functions.ConvertSizeToUnit(speed) : ""));
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //when it finishes the current ftp download
            CurrentDownload.ChangeState(DownloadState.Completed);
            //start the next one
            StartDownloading();
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Type type = e.UserState.GetType();
            if (type == typeof(DownloaderEventArgs))
                OnProgressChanged((DownloaderEventArgs)e.UserState); //this one is the actual progress changed
            else //and as mentioned before, those two are just ways to reach the view from the BW
                if (type == typeof(DownloadErrorEventArgs)) 
                    OnProcessError((DownloadErrorEventArgs)e.UserState); //an error
                else //and a cancel
                    OnDownloadCanceled(new DownloadCanceledEventArgs(e.UserState.ToString())); //File name
        }
        #endregion


        #region StartNextDownloadSftp

        /// <summary>
        /// TODO sFTP
        /// </summary>
        public void StartNextDownloadSftp()
        {
            String Host = "ftp.csidata.com";
            int Port = 22;
            String RemoteFileName = "TheDataFile.txt";
            String LocalDestinationFilename = "TheDataFile.txt";
            String Username = "yourusername";
            String Password = "yourpassword";

            using (var sftp = new SftpClient(Host, Port, Username, Password))
            {
                sftp.Connect();

                using (var file = File.OpenWrite(LocalDestinationFilename))
                {
                    sftp.DownloadFile(RemoteFileName, file);
                }

                sftp.Disconnect();
            }
        }
        #endregion
    }
}