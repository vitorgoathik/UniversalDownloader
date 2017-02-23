using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchDownloaderUC.Events
{
    public class ProcessStartedEventArgs
    {
        public readonly string fileName;

        public ProcessStartedEventArgs(string fileName)
        {
            this.fileName = fileName;
        }
    }
}
