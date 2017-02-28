using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchDownloaderUC.Downloader.Protocols
{
    internal class CustomProtocolDownloader : Downloader
    {
        public override void AbortCurrentDownload()
        {
            throw new NotImplementedException();
        }

        protected override void AddDownloadToList(string url, string destination, string username, string password)
        {
            throw new NotImplementedException();
        }

        protected override void StartNextDownload()
        {
            throw new NotImplementedException();
        }
    }
}
