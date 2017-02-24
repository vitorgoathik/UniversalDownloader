using BatchDownloaderUC.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniversalDownloader.Events;
using static BatchDownloaderUC.Enums;

namespace BatchDownloaderUC
{
    public class DownloaderUC : IDownloader
    {
        #region private fields 

        private readonly List<string> _listUrls;
        private readonly Queue<string> _urls;
        private readonly string _destination; 
        private readonly int _timeoutInMilliSecPerDownload;
        private bool _result = false;

        #endregion

        #region EventHandlers

        public delegate void ProcessStartedEventHandler(object sender, ProcessStartedEventArgs e);
        public event ProcessStartedEventHandler ProcessStarted;
        public delegate void DownloadStartedEventHandler(object sender, DownloadStartedEventArgs e);
        public event DownloadStartedEventHandler DownloadStarted;
        public delegate void ProgressChangedEventHandler(object sender, UniversalDownloader.Events.ProgressChangedEventArgs e);
        public event ProgressChangedEventHandler ProgressChanged;
        public delegate void ProcessCompletedEventHandler(object sender, ProcessCompletedEventArgs e);
        public event ProcessCompletedEventHandler ProcessCompleted;
        public delegate void OverallProgressChangedEventHandler(object sender, OverallProgressChangedEventArgs e);
        public event OverallProgressChangedEventHandler OverallProgressChanged;
        public delegate void ProcessErrorEventHandler(object sender, ProcessErrorEventArgs e);
        public event ProcessErrorEventHandler ProcessError;


        protected virtual void OnProcessStarted(ProcessStartedEventArgs e)
        {
            if (ProcessStarted != null)
                ProcessStarted(this, e);
        }
        protected virtual void OnDownloadStarted(DownloadStartedEventArgs e)
        {
            if (DownloadStarted != null)
                DownloadStarted(this, e);
        }
        protected virtual void OnProgressChanged(UniversalDownloader.Events.ProgressChangedEventArgs e)
        {
            if (ProgressChanged != null)
                ProgressChanged(this, e);
        }
        protected virtual void OnProcessCompleted(ProcessCompletedEventArgs e)
        {
            if (ProcessCompleted != null)
                ProcessCompleted(this, e);
        }
        protected virtual void OnOverallProgressChanged(OverallProgressChangedEventArgs e)
        {
            if (OverallProgressChanged != null)
                OverallProgressChanged(this, e);
        }
        protected virtual void OnProcessError(ProcessErrorEventArgs e)
        {
            if (ProcessError != null)
                ProcessError(this, e);
        }

        #endregion

        public DownloaderUC(string urls, UrlSplittingChar splittingChar, string destination, int timeoutInMilliSecPerDownload)
        {
            if (urls.Count() == 0) throw new Exception("Urls are empty");
            if (string.IsNullOrEmpty(destination)) throw new ArgumentNullException("destination");
            this._urls = Functions.ConvertStringToQueueUrls(urls, splittingChar);
            if (!ValidationTests.IsDestinationValid(destination)) throw new Exception("Invalid destination path");

            this._listUrls = _urls.ToList();

            if (!ValidationTests.IsDriverSpaceSufficient(destination, _listUrls)) throw new Exception("Insufficient memory disk space");

            this._destination = destination;
            this._timeoutInMilliSecPerDownload = timeoutInMilliSecPerDownload;
        }

        public string CheckListFileTotalSizeInUnits()
        {
            return Functions.CheckListFileTotalSizeInUnits(_listUrls);
        }

        public void StartDownloading()
        {
            OnProcessStarted(new ProcessStartedEventArgs(_urls.Count));
            StartNextDownload();
            
        }
        DownloadSpeed speed;
        private void StartNextDownload()
        {
            string nextUri = _urls.Dequeue();
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_destination));
                
                using (WebClient client = new WebClient())
                {
                    speed = new DownloadSpeed(client);
                    speed.Start();
                    var ur = new Uri(nextUri);
                    // client.Credentials = new NetworkCredential("username", "password");
                    client.DownloadProgressChanged += WebClientDownloadProgressChanged;
                    client.DownloadFileCompleted += WebClientDownloadCompleted;
                    string file = Functions.GetFileName(nextUri);
                    string fullPath = _destination + "/" + file; 
                    OnDownloadStarted(new DownloadStartedEventArgs(file, Functions.ConvertSizeToUnit(Functions.CheckFileSize(nextUri))));
                    client.DownloadFileAsync(ur, fullPath);
                }
            }
            catch (Exception e)
            {
                OnProcessError(new ProcessErrorEventArgs("Was not able to download file!", e, nextUri));
            }
            finally
            {
                OnOverallProgressChanged (new OverallProgressChangedEventArgs(_urls.ToList()));
            }
        }
        
        private void WebClientDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            
            OnProgressChanged(new UniversalDownloader.Events.ProgressChangedEventArgs(
                e.ProgressPercentage, 
                Functions.ConvertSizeToUnit(e.BytesReceived), 
                Functions.ConvertSizeToUnit(e.TotalBytesToReceive + e.BytesReceived),
                speed.Speed > 0 ? Functions.ConvertSizeToUnit((long)Math.Round(speed.Speed)) : ""));
        }

        private void WebClientDownloadCompleted(object sender, AsyncCompletedEventArgs args)
        {
            _result = !args.Cancelled;
            if (!_result)
            {
                OnProcessError(new ProcessErrorEventArgs("error"));
            }
            if (_urls.Count > 0)
                StartNextDownload();
            else
            {
                speed.Stop();
                OnProcessCompleted(new ProcessCompletedEventArgs());
            }
        }

    }
}