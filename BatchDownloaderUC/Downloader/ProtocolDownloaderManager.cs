using BatchDownloaderUC.Controller;
using BatchDownloaderUC.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.BatchDownloaderUC;
using static Utilities.BatchDownloaderUC.Enums;

namespace BatchDownloaderUC.Downloader
{
    public static class ProtocolDownloaderManager
    {
        private static Downloader downloader;
        internal static bool Shutdown = false;
        /// <summary>
        /// Gets the right instance of transfer protocol to perform the tasks
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static object GetInstance(string url)
        {
            switch (Functions.GetProtocol(url))
            {//the singletons cannot be in Downloader level, 
             //because if so, only one protocol will be available
             //i decided to make a factory to return the instances
                case Protocol.Ftp:
                    downloader = ProtocolDownloaderFactory.FTPDownloaderInstance;
                    break;
                case Protocol.Http:
                    downloader = ProtocolDownloaderFactory.HTTPDownloaderInstance;
                    break;
                case Protocol.Sftp:
                    downloader = ProtocolDownloaderFactory.SFTPDownloaderInstance;
                    break;
                default:
                    throw new DownloaderUCException(ErrorType.ProtocolNotImplemented);
            }
            downloader.DownloadsController = DownloadsController.Instance;
            return downloader;
        }
    }
}
