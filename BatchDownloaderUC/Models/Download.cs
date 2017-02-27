using BatchDownloaderUC.Exceptions; 
using System;
using System.Net;
using Utilities.BatchDownloaderUC;
using static Utilities.BatchDownloaderUC.Enums;

namespace BatchDownloaderUC.Models
{
    public class Download
    {
        #region Fields and properties
        public readonly RemoteFileInfo RemoteFileInfo;
        public readonly Destination Destination;


         
        /// <summary>
        /// Can be set by this object's ChangeDownloadState method, since there are built in operations handling per state choice
        /// </summary>
        public DownloadState DownloadState { get; private set; }
        
        /// <summary>
        /// Used to calculate indicators such as time remaining, speed and percent done
        /// </summary>
        public long BytesReceived { get; internal set; }
        
        public string StateMessage { get; private set; }

        public double ElapsedTimeInSeconds { get; set; }

        #endregion
        #region Constructors


        internal Download(Destination destination, RemoteFileInfo remoteFileInfo)
        {
            try
            {
                this.Destination = destination;
                this.RemoteFileInfo = remoteFileInfo;
                DownloadState = DownloadState.Pending;
            }
            catch (Exception ex)
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
        public virtual int PercentCompleted()
        {
            try
            {
                if (BytesReceived == 0) return 0;
                var percent = (int)Math.Round((1 - ((double)(RemoteFileInfo.SizeBytes - BytesReceived) / RemoteFileInfo.SizeBytes)) * 100);
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
        internal virtual void ChangeState(DownloadState state, bool delete = true, string stateMessage = "")
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
                        if (delete)
                            System.IO.File.Delete(Destination.FullPath + "/" +  RemoteFileInfo.FileFullName);
                        break;
                    case DownloadState.Canceled:
                        //deletes partial data on cancel
                        if (DownloadState == DownloadState.Started || DownloadState == DownloadState.Deleted)
                            System.IO.File.Delete(Destination.FullPath + "/" + RemoteFileInfo.FileFullName);
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
        public virtual string GetRemainingTimeString(double speedPerSecond)
        {
            long totalBytesToReceive = RemoteFileInfo.SizeBytes - BytesReceived;
            if (totalBytesToReceive == 0 || speedPerSecond == 0) return "";
            return Functions.FormatTimeToString((double)totalBytesToReceive / speedPerSecond);
        }


        #endregion

    }
}
