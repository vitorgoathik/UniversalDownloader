using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchDownloaderUC.Events
{
    public class DownloadStartedEventArgs
    {
        public readonly string fileName;
        public readonly string fileSize;

        public DownloadStartedEventArgs(string fileName, string fileSize)
        {
            this.fileName = fileName;
            this.fileSize = fileSize;
        }
    }
}
