using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BatchDownloaderUC.Enums;

namespace BatchDownloaderUC.Events
{
    public class DownloadErrorEventArgs
    {
        public readonly string ErrorMessage;
        public readonly ErrorType ErrorType;
        public readonly Exception Exception;

        public DownloadErrorEventArgs(string errorMessage, Exception exception)
        {
            ErrorMessage = errorMessage;
            Exception = exception;
        }

        public DownloadErrorEventArgs(string errorMessage, ErrorType errorType, Exception exception)
        {
            this.ErrorMessage = errorMessage;
            this.ErrorType = errorType;
            this.Exception = exception;
        }
    }
}
