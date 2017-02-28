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
        public readonly int PercentCompletedCurrentDownload;
        public readonly int PercentCompletedTotal;
        public DownloaderEventArgs(string estimatedTimeCurrentDownload, string estimatedTimeTotal, 
            string speedInUnit, int percentCompletedCurrentDownload, int percentCompletedTotal)
        {
            EstimatedTimeTotal = estimatedTimeTotal;
            EstimatedTimeCurrentDownload  = estimatedTimeCurrentDownload;
            SpeedInUnit = speedInUnit;
            PercentCompletedCurrentDownload = percentCompletedCurrentDownload;
            PercentCompletedTotal = percentCompletedTotal;
        }
    }
}
