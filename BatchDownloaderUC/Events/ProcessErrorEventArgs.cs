using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchDownloaderUC.Events
{
    public class ProcessErrorEventArgs
    {
        public readonly string ErrorMessage;
        public readonly Exception e;
        public readonly string fileName;

        public ProcessErrorEventArgs(string error)
        {
            ErrorMessage = error;
        }

        public ProcessErrorEventArgs(string error, Exception e, string fileName) : this(error)
        {
            this.e = e;
            this.fileName = fileName;
        }
    }
}
