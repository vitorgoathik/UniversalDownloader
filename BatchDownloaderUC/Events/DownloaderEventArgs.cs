using BatchDownloaderUC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchDownloaderUC.Events
{
    public class DownloaderEventArgs
    {
        public readonly string EstimatedTimeCurrentDownload;
        public readonly string EstimatedTimeTotal;
        public readonly string SpeedInUnit;
        public DownloaderEventArgs(string estimatedTimeCurrentDownload, string estimatedTimeTotal, string speedInUnit)
        {
            EstimatedTimeTotal = estimatedTimeTotal;
            EstimatedTimeCurrentDownload  = estimatedTimeCurrentDownload;
            SpeedInUnit = speedInUnit;
        }
    }
}
