using BatchDownloaderUC.Utilities;
using System;
using static Utilities.BatchDownloaderUC.Enums;
using Utilities.BatchDownloaderUC;
using BatchDownloaderUC.Exceptions;

namespace BatchDownloaderUC.Events
{
    public class DownloadErrorEventArgs
    {
        public readonly string ErrorMessage;
        public readonly ErrorType ErrorType;
        public readonly Exception Exception;

        public DownloadErrorEventArgs(Exception exception)
        {
            if(exception.GetType() == typeof(DownloaderUCException))
            {
                ErrorMessage = ((DownloaderUCException)exception).ErrorMessage;
                ErrorType = ((DownloaderUCException)exception).Error;
                Exception = ((DownloaderUCException)exception).Exception;
            }
            else
            {
                Exception = exception;
            }
        }

        public DownloadErrorEventArgs(ErrorType errorType, string suffix1, Exception e) : this(new DownloaderUCException(errorType, suffix1, e))
        { 
        }
    }
}
