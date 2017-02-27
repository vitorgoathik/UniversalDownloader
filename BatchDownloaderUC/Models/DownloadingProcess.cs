﻿using BatchDownloaderUC.Exceptions;
using BatchDownloaderUC.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.BatchDownloaderUC;
using static Utilities.BatchDownloaderUC.Enums;

namespace BatchDownloaderUC.Models
{
    public class DownloadingProcess
    {
        /// <summary>
        /// Private downloads list
        /// </summary>
        private readonly List<Download> AllDownloads = new List<Download>();

        internal Download NextDownload => AllDownloads.FirstOrDefault(o => o.DownloadState == DownloadState.Pending);
        internal Download StartedDownload => AllDownloads.FirstOrDefault(o => o.DownloadState == DownloadState.Started);

        /// <summary>
        /// Downloads read-only collection
        /// </summary>
        public ReadOnlyCollection<Download> DownloadsCollection
        {
            get
            {
                return new ReadOnlyCollection<Download>(AllDownloads);
            }
        }

        internal void AddDownloadToList(string url, string destination, string username, string password)
        {
            //basic validations
            if (string.IsNullOrEmpty(url)) throw new DownloaderUCException(ErrorType.EmptyField, "Url");
            if (!ValidationTests.IsUrlValid(url)) throw new DownloaderUCException(ErrorType.InvalidField, "Url");
            if (string.IsNullOrEmpty(destination)) throw new DownloaderUCException(ErrorType.EmptyField, "Destination");
            if (!ValidationTests.IsDestinationValid(destination)) throw new DownloaderUCException(ErrorType.InvalidField, "Destination");

            try
            {

                //the download is born
                Download download = new Download(url, destination, username, password);

                //Check the space in disk before adding it. 
                //in FTP case it also fixes the destination paths to prepare the directory tree recursively
                switch (download.Uri.Protocol)
                {
                    case Protocol.Http:
                        CheckSpaceToAddDownload(download);
                        break;
                    case Protocol.Ftp:
                        CheckSpaceToAddDownloadTree(download.FtpFileInfo, download.Destination.FullPath);
                        break;
                    case Protocol.Sftp:
                        //TODO
                        break;
                }

            }
            catch (Exception ex)
            {
                DownloaderUCException.ThrowNewGeneral(new DownloaderUCException(ErrorType.GeneralErrorAddingDownload, ex));
            }


        }

        /// <summary>
        /// Runs through each child (file or directory) and set the Path property in the Destination object of the download
        /// thus why there is this need for that specific constructor in Download
        /// </summary>
        /// <param name="ftpFileInfo"></param>
        /// <param name="chosenDestination"></param>
        private void CheckSpaceToAddDownloadTree(FtpInfo ftpFileInfo, string chosenDestination)
        {
            foreach(FtpInfo child in ftpFileInfo.ChildrenPaths)
            {
                try
                {

                    if (!child.IsDirectory)
                    {
                        string newDestination = chosenDestination + "/" + child.FtpFolder;
                        CheckSpaceToAddDownload(new Download(child.Url, newDestination, child));
                    }
                    else
                        CheckSpaceToAddDownloadTree(child, chosenDestination);

                }
                catch(Exception ex)
                {
                    DownloaderUCException.Throw(ex);
                }
            }
        }

        /// <summary>
        /// Checks if there is space in disk for this next download. In case there isn't, an exception will 
        /// </summary>
        /// <param name="download"></param>
        private void CheckSpaceToAddDownload(Download download)
        {
            if (download.Destination.GetTotalFreeSpace() - TotalSizeBytesRemainingToDownload(download.FileInfo.SizeBytes) <= 0)
            {
                download.ChangeState(DownloadState.Error, false, Enums.GetEnumDescription(ErrorType.InsufficientDiskSpace));
                AllDownloads.Add(download);
                throw new DownloaderUCException(ErrorType.InsufficientDiskSpaceFor,download.FileInfo.FileFullName,download.FileInfo.SizeInUnit);
            }
            AllDownloads.Add(download);
        }

        public long TotalSizeBytesRemainingToDownload()
        {
            return TotalSizeBytesRemainingToDownload(0);
        }
        public long TotalSizeBytesRemainingToDownload(long newDownloadSize)
        {
            long totalSize = 0;
            AllDownloads.Where(o => o.DownloadState == Enums.DownloadState.Pending).ToList()
                .ForEach(o => totalSize += ((FileInfo)o.DownloadFile).SizeBytes);
            Download download = AllDownloads.FirstOrDefault(o => o.DownloadState == Enums.DownloadState.Started);
            if(download != null)
                totalSize += download.FileInfo.SizeBytes - download.BytesReceived;
            totalSize += newDownloadSize;
            return totalSize;
        }
        public long TotalSizeBytes()
        {
            long totalSize = 0;
            AllDownloads.Where(o => o.DownloadState != DownloadState.Error
                                && o.DownloadState != DownloadState.Closed
                                && o.DownloadState != DownloadState.Canceled
                                && o.DownloadState != DownloadState.Deleted).ToList()
                        .ForEach(o => totalSize += ((FileInfo)o.DownloadFile).SizeBytes);
            return totalSize;
        }
        public int PercentCompleted()
        {
            try
            {
                long TotalSizeBytesRemainingToDownload = this.TotalSizeBytesRemainingToDownload();
                long TotalSizeBytes = this.TotalSizeBytes();
                int percent = (int)Math.Round((1 - ((double)(TotalSizeBytesRemainingToDownload) / TotalSizeBytes)) * 100);
                return (percent >= 0 && percent <= 100) ? percent : 0;
            }
            catch (Exception e)
            {
                throw new Exception("PercentCompleted: Error processing SizeBytes and BytesReceived");
            }
        }
        /// <summary>
        /// Returns the amount of time remaning.
        /// Requires BytesReceived property setted
        /// </summary>
        /// <param name="speedPerSecond"></param>
        /// <returns></returns>
        internal string GetRemainingTimeString(double speedPerSecond)
        {
            long totalSizeBytesRemainingToDownload = TotalSizeBytesRemainingToDownload();
            if (totalSizeBytesRemainingToDownload == 0 || speedPerSecond == 0) return "";
            return Functions.FormatTimeToString((double)totalSizeBytesRemainingToDownload / speedPerSecond);
        }

        internal void ClearDownloads()
        {
            AllDownloads.Where(o => o.DownloadState != DownloadState.Error 
                                    && o.DownloadState != DownloadState.Canceled
                                    && o.DownloadState != DownloadState.Deleted).ToList()
                .ForEach(o => o.ChangeState(DownloadState.Closed));
        }

        internal bool CancelDownloads(List<int> fileIndexes)
        {
            bool canceledCurrent = (fileIndexes.Contains(AllDownloads.FindIndex(o => o == StartedDownload)));
            fileIndexes.ForEach(o => AllDownloads[o].ChangeState(DownloadState.Deleted));
            return canceledCurrent;
        }
    }
}
