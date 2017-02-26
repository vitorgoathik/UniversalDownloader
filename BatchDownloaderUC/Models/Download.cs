using BatchDownloaderUC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BatchDownloaderUC.Enums;

namespace BatchDownloaderUC
{
    public class Download
    {
        
        public long BytesReceived { get; internal set; }

        public DownloadState DownloadState { get; private set; }

        public Destination Destination { get; }
        internal readonly Models.Uri Uri;
        internal readonly object DownloadFile;

        public FileInfo FileInfo { get { return (FileInfo)DownloadFile; } }
        internal HttpFileInfo HttpFileInfo { get { return Uri.Protocol == Protocol.Http ? (HttpFileInfo)DownloadFile : null; } }
        internal FtpInfo FtpFileInfo { get { return Uri.Protocol == Protocol.Ftp ? (FtpInfo)DownloadFile : null; } }

        public double ElapsedTimeInSeconds { get; set; }

        internal Download(string url, string destination) : this(url, destination, null) { }
        internal Download(string url, string destination, FtpInfo alreadyPreparedFile) : this(url,destination,alreadyPreparedFile,"",""){}
        internal Download(string url, string destination, string username, string password) : this(url, destination, null, username, password) { }
        private Download(string url, string destination, FtpInfo alreadyPreparedFile, string username, string password) 
        {
            this.Uri = new Models.Uri(url);
            switch (Uri.Protocol)
            {
                case Protocol.Http:
                    DownloadFile = new HttpFileInfo(url);
                    break;
                case Protocol.Ftp:
                    DownloadFile = alreadyPreparedFile ?? new FtpInfo(url, new System.Net.NetworkCredential(username, password));
                    break;
            }
            this.DownloadState = DownloadState.Pending;
            this.Destination = new Destination(destination, FileInfo.FileFullName);
        }


        /// <summary>
        /// Returns the amount of data received as percentage.
        /// Requires BytesReceived property setted
        /// </summary>
        /// <returns></returns>
        public int PercentCompleted()
        {
            FileInfo file = FileInfo;
            try
            {
                if (BytesReceived == 0) return 0;
                return (int)Math.Round((1 - ((double)(file.SizeBytes - BytesReceived) / file.SizeBytes)) * 100);
            }
            catch (Exception e)
            {
                throw new Exception("PercentCompleted: Error processing SizeBytes and BytesReceived");
            }
        }

        internal void ChangeState(DownloadState state)
        {
            try
            {
                switch (state)
                {
                    case DownloadState.Error:

                        System.IO.File.Delete(this.Destination.FullPathWithFile);
                    break;
                    case DownloadState.Canceled:
                    if (DownloadState == DownloadState.Started || DownloadState == DownloadState.Deleted)
                        System.IO.File.Delete(this.Destination.FullPathWithFile);
                    break;
                }
            }
            catch (Exception ex)
            {
                DownloadState = state;
                throw ex;
            }
            DownloadState = state;



        }
        /// <summary>
        /// Returns the amount of time remaning.
        /// Requires BytesReceived property setted
        /// </summary>
        /// <param name="speedPerSecond"></param>
        /// <returns></returns>
        internal string GetRemainingTimeString(double speedPerSecond)
        {
            long totalBytesToReceive = FileInfo.SizeBytes - BytesReceived;
            if (totalBytesToReceive == 0 || speedPerSecond == 0) return "";
            return Functions.FormatTimeToString((double)totalBytesToReceive / speedPerSecond);
        }
    }
}
