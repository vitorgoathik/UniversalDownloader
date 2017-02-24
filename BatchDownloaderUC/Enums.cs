using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchDownloaderUC
{
    public class Enums
    {
        public enum UrlSplittingChar
        {
            Semicolon,
            Comma
        }
        public enum Protocol
        {
            Http,
            Ftp,
            Sftp
        }
        public enum DownloadState
        {
            Started,
            Completed,
            Pending
        }
    }
}
