using BatchDownloaderUC.Events;
using System.Collections.Generic;

namespace BatchDownloaderUC
{
    public interface IDownloader
    {
        void AddDownload(string url, string destination, string username, string password);
        void AbortCurrentDownload();
        void CancelDownloads(List<int> fileIndexes);
    }
}