using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BatchDownloaderUC.Enums;

namespace BatchDownloaderUC.Models
{
    public class DownloadingProcess
    {
        private readonly List<Download> AllDownloads = new List<Download>();

        internal Download NextDownload => AllDownloads.FirstOrDefault(o => o.DownloadState == DownloadState.Pending);
        internal Download StartedDownload => AllDownloads.FirstOrDefault(o => o.DownloadState == DownloadState.Started);

        public ReadOnlyCollection<Download> DownloadsCollection
        {
            get
            {
                return new ReadOnlyCollection<Download>(AllDownloads);
            }
        }

        internal void AddDownloadToList(string url, string destination, string username, string password)
        {
            if (string.IsNullOrEmpty(url)) throw new Exception("Url is empty");
            if (!ValidationTests.IsUrlValid(url)) throw new Exception("Invalid URL");
            if (string.IsNullOrEmpty(destination)) throw new Exception("Destination is empty");
            if (!ValidationTests.IsDestinationValid(destination)) throw new Exception("Invalid destination path");

            
            Download download = new Download(url, destination, username, password);
            switch(download.Uri.Protocol)
            {
                case Protocol.Http: CheckSpaceToAddDownload(download);
                    break;
                case Protocol.Ftp: CheckSpaceToAddDownloadTree(download.FtpFileInfo, download.Destination.FullPath);
                    break;
            }
        }

        private void CheckSpaceToAddDownloadTree(FtpInfo ftpFileInfo, string chosenDestination)
        {
            foreach(FtpInfo child in ftpFileInfo.ChildrenPaths)
            {
                if (!child.IsDirectory)
                {
                    string newDestination = chosenDestination + "/" + child.FtpFolder;
                    CheckSpaceToAddDownload(new Download(child.Url, newDestination, child));
                }
                else
                    CheckSpaceToAddDownloadTree(child, chosenDestination);
            }
        }

        private void CheckSpaceToAddDownload(Download download)
        {
            if (download.Destination.GetTotalFreeSpace()-TotalSizeBytesRemainingToDownload() <= 0)
                throw new Exception("Not enough space on disk");
            AllDownloads.Add(download);
        }

        public long TotalSizeBytesRemainingToDownload()
        {
            long totalSize = 0;
            AllDownloads.Where(o => o.DownloadState == Enums.DownloadState.Pending).ToList()
                .ForEach(o => totalSize += ((FileInfo)o.DownloadFile).SizeBytes);
            Download download = AllDownloads.FirstOrDefault(o => o.DownloadState == Enums.DownloadState.Started);
            if(download != null)
                totalSize += download.FileInfo.SizeBytes - download.BytesReceived;
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
