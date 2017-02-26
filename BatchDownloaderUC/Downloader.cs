using BatchDownloaderUC.Events;
using BatchDownloaderUC.Models;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static BatchDownloaderUC.Enums;

namespace BatchDownloaderUC
{
    public class DownloaderUC : IDownloader
    {
        #region private fields 

        public DownloadingProcess DownloadingProcess { get; private set; }
        public Download CurrentDownload { get; private set; }

        #endregion

        #region EventHandlers

        public delegate void ProcessStartedEventHandler(object sender, DownloaderEventArgs e);
        public event ProcessStartedEventHandler ProcessStarted;
        public delegate void DownloadStartedEventHandler(object sender, DownloaderEventArgs e);

        private static DownloaderUC downloaderUC;
        public static DownloaderUC GetInstance()
        {
            if (downloaderUC == null)
                downloaderUC = new DownloaderUC();
            return downloaderUC;
        }

        public event DownloadStartedEventHandler DownloadStarted;
        public delegate void ProgressChangedEventHandler(object sender, DownloaderEventArgs e);
        public event ProgressChangedEventHandler ProgressChanged;
        public delegate void DownloadingProcessCompletedEventHandler(object sender, DownloaderEventArgs e);
        public event DownloadingProcessCompletedEventHandler DownloadingProcessCompleted;
        public delegate void OverallProgressChangedEventHandler(object sender, DownloaderEventArgs e);
        public event OverallProgressChangedEventHandler OverallProgressChanged;
        public delegate void ProcessErrorEventHandler(object sender, DownloadErrorEventArgs e);
        public event ProcessErrorEventHandler ProcessError;
        public delegate void DownloadCanceledEventHandler(object sender, DownloaderEventArgs e);
        public event DownloadCanceledEventHandler DownloadCanceled;

        protected virtual void OnProcessStarted(DownloaderEventArgs e)
        {
            ProcessStarted?.Invoke(this, e);
        }
        protected virtual void OnDownloadStarted(DownloaderEventArgs e)
        {
            DownloadStarted?.Invoke(this, e);
        }
        protected virtual void OnProgressChanged(DownloaderEventArgs e)
        {
            ProgressChanged?.Invoke(this, e);
        }
        protected virtual void OnDownloadingProcessCompleted(DownloaderEventArgs e)
        {
            DownloadingProcessCompleted?.Invoke(this, e);
        }
        protected virtual void OnOverallProgressChanged(DownloaderEventArgs e)
        {
            OverallProgressChanged?.Invoke(this, e);
        }
        protected virtual void OnProcessError(DownloadErrorEventArgs e)
        {
            ProcessError?.Invoke(this, e);
        }
        protected virtual void OnDownloadCanceled(DownloaderEventArgs e)
        {
            DownloadCanceled?.Invoke(this, e);
        }

        #endregion

        public void AddDownload(string url, string destination)
        {
            AddDownload(url, destination, "", "");
        }
        public void AddDownload(string url, string destination, string username, string password)
        {
            if (DownloadingProcess == null)
                DownloadingProcess = new DownloadingProcess();
            try
            {
                DownloadingProcess.AddDownloadToList(url, destination, username, password);
                StartDownloading();
            }
            catch(Exception ex)
            {
                if (ex.Message.Contains("Not logged in."))
                    OnProcessError(new DownloadErrorEventArgs("Not logged in.", ErrorType.NotLoggedIn, ex));
                else
                    OnProcessError(new DownloadErrorEventArgs("Error on AddDownload", ex));
            }
        }
        private void StartDownloading()
        {
            if (DownloadingProcess.StartedDownload != null)
                return;
            else
            {
                if (DownloadingProcess.NextDownload == null)
                {
                    speed?.Stop();
                    DownloadingProcess.ClearDownloads();
                    OnDownloadingProcessCompleted(new DownloaderEventArgs());
                    return;
                }
            }

            CurrentDownload = DownloadingProcess.NextDownload;

            DownloadingProcess.NextDownload.ChangeState(DownloadState.Started);

            OnProcessStarted(new DownloaderEventArgs());

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


        public void CancelCurrentDownload()
        {
            client?.CancelAsync();
        }

        public void CancelDownloads(List<int> fileIndexes)
        {
            if (DownloadingProcess.CancelDownloads(fileIndexes))
                client.CancelAsync();
            else
                OnDownloadCanceled(new DownloaderEventArgs());
        }
        DownloadSpeed speed; 

        #region StartNextDownloadHTTP
        WebClient client;

        private void StartNextDownloadHTTP()
        {
            try
            {
                Directory.CreateDirectory(CurrentDownload.Destination.FullPath) ;
                
                using (client = new WebClient())
                {
                    speed = new DownloadSpeed(client);
                    speed.Start();
                    // client.Credentials = new NetworkCredential("username", "password");
                    client.DownloadProgressChanged += WebClientDownloadProgressChanged;
                    client.DownloadFileCompleted += WebClientDownloadCompleted;

                    OnDownloadStarted(new DownloaderEventArgs());
                    CurrentDownload.Destination.SetFullUniquePathWithFile();
                    client.DownloadFileAsync(new System.Uri(CurrentDownload.Uri.Url), CurrentDownload.Destination.FullPathWithFile);
                }
            }
            catch (Exception e)
            { 
                OnProcessError(new DownloadErrorEventArgs("Was not able to download file (http)!", e));
            }
            finally
            {
                OnOverallProgressChanged (new DownloaderEventArgs());
            }
        }


        private void WebClientDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            CurrentDownload.BytesReceived = e.BytesReceived;
            OnProgressChanged(new DownloaderEventArgs(
                CurrentDownload.GetRemainingTimeString(speed.Speed),
                DownloadingProcess.GetRemainingTimeString(speed.Speed),
                speed.SpeedInUnit));
        }

        private void WebClientDownloadCompleted(object sender, AsyncCompletedEventArgs args)
        {
            if (args.Cancelled)
            {
                CurrentDownload.ChangeState(DownloadState.Canceled);
                OnDownloadCanceled(new DownloaderEventArgs());
            }
            else
                CurrentDownload.ChangeState(DownloadState.Completed);

            CurrentDownload.ElapsedTimeInSeconds = speed.ElapsedTimeInSeconds;
            StartDownloading();
        }
        #endregion

        #region StartNextDownloadFtp
        

        private void StartNextDownloadFtp()
        {
            try
            { 
                bool UseBinary = true; // use true for .zip file or false for a text file
                bool UsePassive = false;

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(CurrentDownload.Uri.Url);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.KeepAlive = true;
                request.UsePassive = UsePassive;
                request.UseBinary = UseBinary;

                request.Credentials = CurrentDownload.FtpFileInfo.Credential;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                Directory.CreateDirectory(CurrentDownload.Destination.FullPath);
                using (FileStream writer = new FileStream(CurrentDownload.Destination.FullPathWithFile, FileMode.Create))
                {

                    long length = response.ContentLength;
                    int bufferSize = 2048;
                    int readCount;
                    byte[] buffer = new byte[2048];

                    readCount = responseStream.Read(buffer, 0, bufferSize);
                    while (readCount > 0)
                    {
                        writer.Write(buffer, 0, readCount);
                        readCount = responseStream.Read(buffer, 0, bufferSize);
                    }
                }

                reader.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                CurrentDownload.ChangeState(DownloadState.Error);
                OnProcessError(new DownloadErrorEventArgs("Was not able to download file (ftp)!", ex));
            }
            CurrentDownload.ChangeState(DownloadState.Completed);
            StartDownloading();
        }
        #endregion


        #region StartNextDownloadSftp
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