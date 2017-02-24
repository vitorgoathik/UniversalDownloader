using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchDownloaderUC.Models
{
    public class DownloadingProcess
    {
        public List<Download> AllDownloads { get; set; }
        
        public long TotalSizeBytesRemaining()
        {
            long totalSize = 0;
            AllDownloads.ToList().Where(o => !o.Started).ToList().ForEach(o => totalSize += o.SizeBytes);
            totalSize += AllDownloads.ToList();
            return totalSize;
        }

        public int PercentCompleted()
        {
            try
            {
                return (int)Math.Round((1 - ((double)(SizeBytes - BytesReceived) / SizeBytes)) * 100);
            }
            catch (Exception e)
            {
                throw new Exception("PercentCompleted: Error processing SizeBytes and BytesReceived");
            }
        }

    }
}
