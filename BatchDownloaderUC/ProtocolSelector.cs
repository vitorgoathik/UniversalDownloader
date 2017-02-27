using BatchDownloaderUC.Exceptions;
using BatchDownloaderUC.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.BatchDownloaderUC;
using static Utilities.BatchDownloaderUC.Enums;

namespace BatchDownloaderUC
{
    public static class ProtocolSelector
    { 
        private static IDownloader downloader;

        public static object GetInstance(string url)
        {
            if (downloader != null)
                return downloader;
            switch (Functions.GetProtocol(url))
            {
                case Protocol.Ftp:
                    downloader = new FTPDownloader();
                    break;
                case Protocol.Http:
                    downloader = new HTTPDownloader();
                    break;
                case Protocol.Sftp:
                    downloader = new SFTPDownloader();
                    break;
                default:
                    throw new DownloaderUCException(ErrorType.ProtocolNotImplemented);
            }
            return downloader;
        }
    }
}
