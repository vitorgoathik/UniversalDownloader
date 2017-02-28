using BatchDownloaderUC.Downloader.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchDownloaderUC.Downloader
{
    static class ProtocolDownloaderFactory
    {

        static FTPDownloader fTPDownloader;

        internal static FTPDownloader FTPDownloaderInstance
        {
            get
            {
                if (fTPDownloader == null)
                    fTPDownloader = new FTPDownloader();
                return fTPDownloader;
            }
        }

        static HTTPDownloader httpDownloader;

        internal static HTTPDownloader HTTPDownloaderInstance
        {
            get
            {
                if (httpDownloader == null)
                    httpDownloader = new HTTPDownloader();
                return httpDownloader;
            }
        }

        static SFTPDownloader sFTPDownloader;

        internal static SFTPDownloader SFTPDownloaderInstance
        {
            get
            {
                if (sFTPDownloader == null)
                    sFTPDownloader = new SFTPDownloader();
                return sFTPDownloader;
            }
        }

        static CustomProtocolDownloader customProtocolDownloader;

        internal static CustomProtocolDownloader CustomProtocolDownloaderInstance
        {
            get
            {
                if (customProtocolDownloader == null)
                    customProtocolDownloader = new CustomProtocolDownloader();
                return customProtocolDownloader;
            }
        }
    }
}
