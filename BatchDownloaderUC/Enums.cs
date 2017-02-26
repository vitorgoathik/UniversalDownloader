using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchDownloaderUC
{
    public class Enums
    {
        public enum Protocol
        {
            Http,
            Ftp,
            Sftp
        }
        public enum ErrorType
        {
            NotLoggedIn
        }
        public enum DownloadState
        {
            Started,
            Completed,
            Pending,
            Error,
            Closed,
            Canceled,
            Deleted,
        }
    }
}
