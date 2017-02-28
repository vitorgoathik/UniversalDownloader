using BatchDownloaderUC.Exceptions;
using BatchDownloaderUC.Models;
using BatchDownloaderUC.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.BatchDownloaderUC;
using static Utilities.BatchDownloaderUC.Enums;

namespace BatchDownloaderUC.Controller
{
    public class DownloadsController
    {
        static DownloadsController instance;
        internal static DownloadsController Instance
        {
            get
            {
                if (instance == null)
                    instance = new DownloadsController();
                return instance;
            }
        }
        /// <summary>
        /// Private downloads list
        /// </summary>
        private readonly List<Download> AllDownloads = new List<Download>();
        public Download CurrentDownload { get; set; }
        internal Download NextDownload => AllDownloads.FirstOrDefault(o => o.DownloadState == DownloadState.Pending);

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

        internal virtual void AddDownloadToList(Download download)
        {
            
            if (!ValidationTests.IsUrlValid(download.RemoteFileInfo.Url)) throw new DownloaderUCException(ErrorType.InvalidField, "Url");
            if (string.IsNullOrEmpty(download.Destination.FullPath)) throw new DownloaderUCException(ErrorType.EmptyField, "Destination");
            if (!ValidationTests.IsDestinationValid(download.Destination.FullPath)) throw new DownloaderUCException(ErrorType.InvalidField, "Destination");
            if (download.RemoteFileInfo.SizeBytes == -1) throw new DownloaderUCException(ErrorType.InvalidField, "Url");

            CheckSpaceToAddDownload(download);
        }


        /// <summary>
        /// Checks if there is space in disk for this next download. In case there isn't, an exception will 
        /// </summary>
        /// <param name="download"></param>
        internal virtual void CheckSpaceToAddDownload(Download download)
        {
            if (download.Destination.GetTotalFreeSpace() - TotalSizeBytesRemainingToDownload(download.RemoteFileInfo.SizeBytes) <= 0)
            {
                download.ChangeState(DownloadState.Error, false, Enums.GetEnumDescription(ErrorType.InsufficientDiskSpace));
                AllDownloads.Add(download);
                throw new DownloaderUCException(ErrorType.InsufficientDiskSpaceFor,download.RemoteFileInfo.FileFullName,download.RemoteFileInfo.SizeInUnit);
            }
            AllDownloads.Add(download);
        }

        public virtual long TotalSizeBytesRemainingToDownload()
        {
            return TotalSizeBytesRemainingToDownload(0);
        }
        public virtual long TotalSizeBytesRemainingToDownload(long newDownloadSize)
        {
            long totalSize = 0;
            AllDownloads.Where(o => o.DownloadState == Enums.DownloadState.Pending).ToList()
                .ForEach(o => totalSize += o.RemoteFileInfo.SizeBytes);
            Download download = AllDownloads.FirstOrDefault(o => o.DownloadState == Enums.DownloadState.Started);
            if(download != null)
                totalSize += download.RemoteFileInfo.SizeBytes - download.BytesReceived;
            totalSize += newDownloadSize;
            return totalSize;
        }
        public virtual long TotalSizeBytes()
        {
            long totalSize = 0;
            AllDownloads.Where(o => o.DownloadState != DownloadState.Error
                                && o.DownloadState != DownloadState.Closed
                                && o.DownloadState != DownloadState.Canceled
                                && o.DownloadState != DownloadState.Deleted).ToList()
                        .ForEach(o => totalSize += o.RemoteFileInfo.SizeBytes);
            return totalSize;
        }
        public virtual int PercentCompleted()
        {
            long totalSizeBytesRemainingToDownload = TotalSizeBytesRemainingToDownload();
            long totalSizeBytes = TotalSizeBytes();
            int percent = (int)Math.Round((1 - ((double)(totalSizeBytesRemainingToDownload) / totalSizeBytes)) * 100);
            return (percent >= 0 && percent <= 100) ? percent : 0;
        }
        /// <summary>
        /// Returns the amount of time remaning.
        /// Requires BytesReceived property setted
        /// </summary>
        /// <param name="speedPerSecond"></param>
        /// <returns></returns>
        internal virtual string GetRemainingTimeString(double speedPerSecond)
        {
            long totalSizeBytesRemainingToDownload = TotalSizeBytesRemainingToDownload();
            if (totalSizeBytesRemainingToDownload == 0 || speedPerSecond == 0) return "";
            return Functions.FormatTimeToString((double)totalSizeBytesRemainingToDownload / speedPerSecond);
        }

        internal virtual void ClearDownloads()
        {
            AllDownloads.Where(o => o.DownloadState == DownloadState.Completed).ToList()
                .ForEach(o => o.ChangeState(DownloadState.Closed));
        }

        internal virtual bool CancelDownloads(List<int> fileIndexes)
        {
            bool canceledCurrent = (fileIndexes.Contains(AllDownloads.FindIndex(o => o == CurrentDownload)));
            fileIndexes.ForEach(o => AllDownloads[o].ChangeState(DownloadState.Deleted));
            return canceledCurrent;
        }
    }
}
