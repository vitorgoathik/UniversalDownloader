using BatchDownloaderUC;
using BatchDownloaderUC.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FTPDownloadImplementation
{
    internal partial class FTPDownloadImplementation : DownloaderUC
    {

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
                if (CurrentDownload.DownloadState == DownloadState.Canceled)
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
    }
}
