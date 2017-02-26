using BatchDownloaderUC.Events;
using System.Collections.Generic;

namespace BatchDownloaderUC
{
    public interface IDownloader
    {
        void AddDownload(string url, string destination);
        void CancelCurrentDownload();
        void CancelDownloads(List<int> fileIndexes);
    }
}