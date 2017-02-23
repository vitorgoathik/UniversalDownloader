using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalDownloader.Events
{
    public class ProgressChangedEventArgs
    {
        public readonly long bytesReceived;
        public readonly int progressPercentage;
        public readonly long totalBytesToReceive;

        public ProgressChangedEventArgs(int progressPercentage, long bytesReceived, long totalBytesToReceive)
        {
            this.progressPercentage = progressPercentage;
            this.bytesReceived = bytesReceived;
            this.totalBytesToReceive = totalBytesToReceive;
        } 
    }
}
