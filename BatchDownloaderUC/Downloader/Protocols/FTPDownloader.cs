using BatchDownloaderUC.Events;
using BatchDownloaderUC.Exceptions;
using BatchDownloaderUC.Models;
using BatchDownloaderUC.Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;
using Utilities.BatchDownloaderUC;
using static Utilities.BatchDownloaderUC.Enums;

namespace BatchDownloaderUC.Downloader.Protocols
{
    public class FTPDownloader : Downloader
    {

        internal FTPDownloader()
        {
            NetworkChange.NetworkAvailabilityChanged += NetworkAvailabilityChanged;
        }

        #region Contract methods

        protected override void AddDownloadToList(string url, string destination, string username, string password)
        {
            Credentials = new NetworkCredential(username, password);
            try
            {
                foreach (Download download in GetFTPDownloadTree(new List<Download>(), Credentials, destination, url, url))
                {
                    try
                    {//this inner try catch will prevent the rest of the downloads to not be added in case this one fails
                        string newDestination = download.Destination.FullPath;
                        download.Destination.FullPath = destination;
                        DownloadsController.AddDownloadToList(download);
                        download.Destination.FullPath = newDestination;
                    }
                    catch (Exception ex)
                    {
                        OnProcessError(new DownloadErrorEventArgs(ex));
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("logged in"))
                    throw new DownloaderUCException(ErrorType.NotLoggedIn);
                else
                    DownloaderUCException.Throw(ex);
            }
        }

        
        public override void AbortCurrentDownload()
        {
            backgroundWorker.CancelAsync();
        }

        BackgroundWorker backgroundWorker; //our FTP starts with a background worker
        protected override void StartNextDownload()
        {
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.RunWorkerAsync();
        }

        #endregion
        #region StartNextDownloadFtp


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
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(DownloadsController.CurrentDownload.RemoteFileInfo.Url);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.UsePassive = false;
                request.UseBinary = true;// use true for .zip file or false for a text file
                request.KeepAlive = true;
                request.Credentials = Credentials;

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
                            Directory.CreateDirectory(DownloadsController.CurrentDownload.Destination.FullPath);
                            //repeated names will be renamed
                            

                            using (FileStream outputStream = new FileStream(UpdateFileNameWithDistinguishedName(), FileMode.Create))
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
                                        DownloadsController.CurrentDownload.ChangeState(DownloadState.Canceled);
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
                                    if (sleep == 15)
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
                DownloadsController.CurrentDownload.ChangeState(DownloadState.Error, true, Enums.GetEnumDescription(ErrorType.InsufficientDiskSpace));
                //this is a hack to access the view under the same thread of the background worker.
                //the UI thread is only accessible from the events Report and Completed fired by the BW
                backgroundWorker.ReportProgress(0, new DownloadErrorEventArgs(ErrorType.InsufficientDiskSpaceFor, DownloadsController.CurrentDownload.RemoteFileInfo.FileFullName, ex));
                //Disk is full = cancel
                e.Cancel = true;
                backgroundWorker.CancelAsync();
            }
            catch (Exception ex)
            {
                //on regular unpredicted exceptions, the client will also be updated
                string name = DownloadsController.CurrentDownload.RemoteFileInfo.FileFullName;
                DownloadsController.CurrentDownload.ChangeState(DownloadState.Error, true, Enums.GetEnumDescription(ErrorType.GeneralErrorOnDownload));
                if (DownloadsController.CurrentDownload.DownloadState == DownloadState.Canceled)
                    backgroundWorker.ReportProgress(0, name);
                else
                    backgroundWorker.ReportProgress(0, new DownloadErrorEventArgs(ErrorType.GeneralErrorOnDownload, DownloadsController.CurrentDownload.RemoteFileInfo.FileFullName, ex));
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
            DownloadsController.CurrentDownload.BytesReceived = progress;
            backgroundWorker.ReportProgress(0, new DownloaderEventArgs(
            DownloadsController.CurrentDownload.GetRemainingTimeString(speed),
            DownloadsController.GetRemainingTimeString(speed),
            speed > 0 ? Functions.ConvertSizeToUnit(speed) : "",
            DownloadsController.CurrentDownload.PercentCompleted(), DownloadsController.PercentCompleted()));
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //when it finishes the current ftp download
            DownloadsController.CurrentDownload.ChangeState(DownloadState.Completed);
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






        private List<Download> GetFTPDownloadTree(List<Download> downloads, NetworkCredential credentials, string destination, string url, string rootUrl)
        {
            //those are the lines provided by the WebRequestMethods.Ftp.ListDirectoryDetails Method.
            //they are all the information we can get from the headers of the files
            List<string> lines = ReadFtpDirectoryLines(url, credentials);

            if (!url.EndsWith("/"))
                url += "/";
            if (!rootUrl.EndsWith("/"))
                rootUrl += "/";

            //going through this information, we find a specific formating logic
            foreach (string line in lines)
            {
                string[] tokens = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                //this token will tell dir when its a directory, or it will be a number that represents the size
                string dirOrSize = tokens[2];

                //the name should be escaped to compose a url. 
                //ex: "this folder has spaces", should be "this%20folder%20has%20spaces" in the url
                string name = line.Replace(tokens[0], "").Replace(tokens[1], "").Replace(tokens[2], "").Trim();
                //the same way we have to unescape to represent a directory. it's just the other way around
                string ftpFolder = System.Uri.UnescapeDataString(url.Replace(rootUrl, ""));
                //full url
                string fileUrl = url + System.Uri.EscapeDataString(name);
                string fullPath = Path.Combine(destination,ftpFolder);

                //if it is a DIR, enter the recursion by adding a new child
                if (dirOrSize == "<DIR>")
                    return GetFTPDownloadTree(downloads, credentials, destination, fileUrl, url);
                else
                    downloads.Add(new Download(
                        new Destination(fullPath), 
                        new RemoteFileInfo(fileUrl, name, long.Parse(dirOrSize))));

            }
            return downloads;
        }

        /// <summary>
        /// reads the headers of the ftp directory and file lines
        /// </summary>
        /// <param name="url"></param>
        /// <param name="credentials"></param>
        /// <returns></returns>
        private List<string> ReadFtpDirectoryLines(string url, NetworkCredential credentials)
        {
            FtpWebRequest listRequest = (FtpWebRequest)WebRequest.Create(url);
            listRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails; //this is the method required to do so
            listRequest.Credentials = credentials;

            List<string> lines = new List<string>();
            //those are the same streams used to open a FTP download, as seen in the Downloader.cs class
            using (FtpWebResponse listResponse = (FtpWebResponse)listRequest.GetResponse())
            using (Stream listStream = listResponse.GetResponseStream())
            using (StreamReader listReader = new StreamReader(listStream))
            {
                while (!listReader.EndOfStream)
                {
                    lines.Add(listReader.ReadLine());
                }
            }
            return lines;
        }

    }
}
