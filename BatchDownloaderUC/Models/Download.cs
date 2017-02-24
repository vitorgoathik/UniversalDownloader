using BatchDownloaderUC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BatchDownloaderUC.Enums;

namespace BatchDownloaderUC
{
    public class Download
    {
        public readonly string Url;
        public readonly FileInfo FileInfo;
        
        public int BytesReceived { get; set; }
        public DownloadState Started { get; set; }

        public Download(string Url, DownloadState Started)
        {
            this.Url = Url;
            this.Started = Started;
            this.SizeBytes = Functions.CheckFileSize(Url);

        }
        public int PercentCompleted()
        {
            try
            {
                if (BytesReceived == 0) return 0;
                return (int)Math.Round((1 - ((double)(SizeBytes - BytesReceived) / SizeBytes)) * 100);
            }
            catch (Exception e)
            {
                throw new Exception("PercentCompleted: Error processing SizeBytes and BytesReceived");
            }
        }
    }
}
