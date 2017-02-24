using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalDownloader.Events
{
    public class ProgressChangedEventArgs
    {
        public readonly string bytesReceived;
        public readonly int progressPercentage;
        public readonly string fileSize;
        public readonly string speed;

        public ProgressChangedEventArgs(int progressPercentage, string bytesReceived, string size, string speed)
        {
            this.progressPercentage = progressPercentage;
            this.bytesReceived = bytesReceived;
            this.fileSize = size;
            this.speed = speed;
        }
    }
}
