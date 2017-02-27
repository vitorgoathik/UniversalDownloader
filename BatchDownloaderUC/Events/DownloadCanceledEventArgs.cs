using BatchDownloaderUC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchDownloaderUC.Events
{
    public class DownloadCanceledEventArgs
    {
        public readonly string FileName;

        public DownloadCanceledEventArgs()
        {
        }
        public DownloadCanceledEventArgs(string fileName)
        {
            FileName = fileName;
        }
    }
}
