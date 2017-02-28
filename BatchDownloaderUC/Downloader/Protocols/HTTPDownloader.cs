using BatchDownloaderUC.Events;
using BatchDownloaderUC.Exceptions;
using BatchDownloaderUC.Models;
using BatchDownloaderUC.Controller;
using BatchDownloaderUC.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utilities.BatchDownloaderUC;
using static Utilities.BatchDownloaderUC.Enums;

namespace BatchDownloaderUC.Downloader.Protocols
{
    public class HTTPDownloader : Downloader
    {        
        protected override void AddDownloadToList(string url, string destination, string username, string password)
        {
            string fileName;
            long size;
            GetHttpHeaderInfo(url, out fileName, out size);
            DownloadsController.AddDownloadToList(new Download(new Destination(destination), new RemoteFileInfo(url, fileName, size)));
        }

        public override void AbortCurrentDownload()
        {
            client.CancelAsync();
        }
        

        #region StartNextDownloadHTTP
        CustomWebClient client;
        DownloadSpeed speed;

        protected override void StartNextDownload()
        {
            try
            {
                Directory.CreateDirectory(DownloadsController.CurrentDownload.Destination.FullPath);
                //Http is way more simple than FTP, since it has a built in background worker, no authentication and no folders
                using (client = new CustomWebClient())
                {
                    speed = new DownloadSpeed(client);
                    speed.Start();
                    client.Credentials = Credentials;
                    client.DownloadProgressChanged += CustomWebClientDownloadProgressChanged;
                    client.DownloadFileCompleted += CustomWebClientDownloadCompleted;
                    OnDownloadStarted();
                    //downloads the file and save in the selected destination
                    client.DownloadFileAsync(new Uri(DownloadsController.CurrentDownload.RemoteFileInfo.Url), UpdateFileNameWithDistinguishedName());
                }
            }
            catch (Exception e)
            {
                DownloadsController.CurrentDownload.ChangeState(DownloadState.Error, true);
                OnProcessError(new DownloadErrorEventArgs(ErrorType.GeneralErrorOnDownload, DownloadsController.CurrentDownload.RemoteFileInfo.FileFullName, e));
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
            DownloadsController.CurrentDownload.BytesReceived = e.BytesReceived;
            OnProgressChanged(new DownloaderEventArgs(
                DownloadsController.CurrentDownload.GetRemainingTimeString(speed.Speed),
                DownloadsController.GetRemainingTimeString(speed.Speed),
                speed.SpeedInUnit,
                DownloadsController.CurrentDownload.PercentCompleted(), DownloadsController.PercentCompleted()));
        }

        private void CustomWebClientDownloadCompleted(object sender, AsyncCompletedEventArgs args)
        {
            //the two ways to reach here are: completion and cancelation
            if (args.Cancelled)
            {
                string name = DownloadsController.CurrentDownload.RemoteFileInfo.FileFullName;
                DownloadsController.CurrentDownload.ChangeState(DownloadState.Canceled);
                //calls the view providing the canceled name
                OnDownloadCanceled(new DownloadCanceledEventArgs(name));
            }
            else
                DownloadsController.CurrentDownload.ChangeState(DownloadState.Completed);
            //the "speed" is a watch that marks time data such as ElapsedTimeInSeconds
            DownloadsController.CurrentDownload.ElapsedTimeInSeconds = speed.ElapsedTimeInSeconds;
            //proceed to the next of the line
            StartDownloading();
        }
        #endregion


        /// <summary>
        /// Gets the built in name inside the url request headers, or use the dafault one
        /// </summary>
        /// <param name="url"></param>
        /// <param name="defaultFileName"></param>
        /// <param name="fileFullName"></param>
        /// <param name="fileSize"></param>
        /// <returns></returns>
        private static bool GetHttpHeaderInfo(string url, out string fileFullName, out long fileSize)
        {
            string defaultFileName = Path.GetFileName(new System.Uri(url).AbsolutePath);
            fileFullName = "";
            fileSize = 0;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                HttpWebResponse res = (HttpWebResponse)request.GetResponse();
                fileSize = res.ContentLength; //the size comes from here
                using (Stream rstream = res.GetResponseStream())
                {
                    //the header + extension should come from here.
                    fileFullName = res.Headers["Content-Disposition"] != null ?
                        res.Headers["Content-Disposition"].Replace("attachment; filename=", "").Replace("\"", "") :
                        res.Headers["Location"] != null ? Path.GetFileName(res.Headers["Location"]) :
                        Path.GetFileName(url).Contains('?') || Path.GetFileName(url).Contains('=') ?
                        Path.GetFileName(res.ResponseUri.ToString()) : defaultFileName;
                }
                res.Close();
            }
            catch (Exception e)
            {
                if (e.Message.Contains("An unexpected error occurred on a send"))
                    throw new DownloaderUCException(e.InnerException.Message);
                //if the internet is not there, now is the time to warn. 
                //because if it drops during the download, i have made the system to crash with a message
                if (e.Message.Contains("The remote name could not be resolved"))
                    throw new DownloaderUCException(Enums.ErrorType.NoInternet, e);
            }
            return fileFullName != defaultFileName;
        }

    }
}
