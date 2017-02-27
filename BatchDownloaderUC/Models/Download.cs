using BatchDownloaderUC.Exceptions;
using BatchDownloaderUC.Models;
using System;
using System.Net;
using Utilities.BatchDownloaderUC;
using static Utilities.BatchDownloaderUC.Enums;

namespace BatchDownloaderUC
{
    public class Download
    {
        #region Fields and properties

        /// <summary>
        /// Used to calculate indicators such as time remaining, speed and percent done
        /// </summary>
        public long BytesReceived { get; internal set; }

        /// <summary>
        /// Can be set by this object's ChangeDownloadState method, since there are built in operations handling per state choice
        /// </summary>
        public DownloadState DownloadState { get; private set; }

        public Destination Destination { get; }
        
        internal readonly Models.Uri Uri;
        internal readonly object DownloadFile;
        public string StateMessage { get; private set; }

        public FileInfo FileInfo { get { return (FileInfo)DownloadFile; } }
        internal HttpFileInfo HttpFileInfo { get { return Uri.Protocol == Protocol.Http ? (HttpFileInfo)DownloadFile : null; } }
        internal FtpInfo FtpFileInfo { get { return Uri.Protocol == Protocol.Ftp ? (FtpInfo)DownloadFile : null; } }

        public double ElapsedTimeInSeconds { get; set; }

        #endregion
        #region Constructors

        
        /// <summary>
        /// This override should be when there is a need to fix readonly properties such as destination after having prepared the whole FileInfo
        /// </summary>
        /// <param name="url"></param>
        /// <param name="destination"></param>
        /// <param name="alreadyPreparedFile"></param>
        internal Download(string url, string destination, FtpInfo alreadyPreparedFile) : this(url,destination,alreadyPreparedFile,"",""){}
        /// <summary>
        /// This is the FTP and sFTP override choice, for it provides username and password
        /// </summary>
        /// <param name="url"></param>
        /// <param name="destination"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        internal Download(string url, string destination, string username, string password) : this(url, destination, null, username, password) { }
        
        private Download(string url, string destination, FtpInfo alreadyPreparedFile, string username, string password) 
        {
            try
            {

            this.Uri = new Models.Uri(url);
            switch (Uri.Protocol) 
            {
                case Protocol.Http:
                    DownloadFile = new HttpFileInfo(url, username, password);
                    break;
                case Protocol.Ftp:
                    DownloadFile = alreadyPreparedFile ?? new FtpInfo(url, new System.Net.NetworkCredential(username, password));
                    break;
            }
            this.DownloadState = DownloadState.Pending;
            this.Destination = new Destination(destination, FileInfo.FileFullName);

            }catch(Exception ex)
            {
                DownloaderUCException.Throw(ex);
            }
        }
        #endregion
        #region Methods and functions

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
                var percent = (int)Math.Round((1 - ((double)(file.SizeBytes - BytesReceived) / file.SizeBytes)) * 100);
                return percent;
            }
            catch (Exception e)
            {
                throw new Exception("PercentCompleted: Error processing SizeBytes and BytesReceived");
            }
        }

        /// <summary>
        /// Handles the download before setting the new state
        /// </summary>
        /// <param name="state"></param>
        internal void ChangeState(DownloadState state, bool delete=true, string stateMessage="")
        {
            //we don't need those states to change to anything
            if (DownloadState == DownloadState.Error || DownloadState == DownloadState.Canceled)
                return;
            try
            {
                switch (state)
                {
                    case DownloadState.Error:
                        //deletes partial data on error
                        StateMessage = stateMessage;
                        if(delete)
                            System.IO.File.Delete(this.Destination.FullPathWithFile);
                    break;
                    case DownloadState.Canceled:
                        //deletes partial data on cancel
                        if (DownloadState == DownloadState.Started || DownloadState == DownloadState.Deleted)
                            System.IO.File.Delete(this.Destination.FullPathWithFile);
                    break;
                    case DownloadState.Deleted:
                        if (DownloadState == DownloadState.Completed) return;
                    break;
                }
            }
            catch (System.IO.IOException ex)
            {
                //the file was not there or it is being used change the state anyway
                DownloadState = state;
                throw ex;
            }
            DownloadState = state;



        }
        /// <summary>
        /// Returns the amount of time remaining to finish the current download.
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
        #endregion

    }
}
