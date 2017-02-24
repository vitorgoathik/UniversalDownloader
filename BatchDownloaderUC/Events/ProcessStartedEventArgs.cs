using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchDownloaderUC.Events
{
    public class ProcessStartedEventArgs
    {
        public readonly int numberOfFiles;

        public ProcessStartedEventArgs(int numberOfFiles)
        {
            this.numberOfFiles = numberOfFiles;
        }
    }
}
