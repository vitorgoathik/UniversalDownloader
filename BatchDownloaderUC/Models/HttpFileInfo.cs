using BatchDownloaderUC.Exceptions;
using BatchDownloaderUC.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Net;
using Utilities.BatchDownloaderUC;

namespace BatchDownloaderUC.Models
{
    public class HttpFileInfo : FileInfo
    {
        internal readonly NetworkCredential Credentials;

        //constructor 1
        /// System.IO.Path.GetFileName is the non header way to get the file name within the url. this will be the default name
        public HttpFileInfo(string url, string username, string password) : this(url, System.IO.Path.GetFileName(new System.Uri(url).AbsolutePath))
        {
            Credentials = new NetworkCredential(username, password);
        }
        
        //constructor 3
        private HttpFileInfo(string url, string defaultFileName) : base(GetFileInfo(url, defaultFileName)) { }
        //constructor 2
        private static FileInfo GetFileInfo(string url, string defaultFileName)
        {
            long sizeBytes;
            string fileFullName;
            GetHttpHeaderInfo(url, defaultFileName, out fileFullName, out sizeBytes);
            return new FileInfo(url, fileFullName, sizeBytes);
        }
        
        /// <summary>
        /// Gets the built in name inside the url request headers, or use the dafault one
        /// </summary>
        /// <param name="url"></param>
        /// <param name="defaultFileName"></param>
        /// <param name="fileFullName"></param>
        /// <param name="fileSize"></param>
        /// <returns></returns>
        private static bool GetHttpHeaderInfo(string url, string defaultFileName, out string fileFullName, out long fileSize)
        {
            if (!ValidationTests.IsUrlValid(url))
                throw new Exception("GetFileName: Url is not valid");
            fileFullName = "";
            fileSize = 0;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                HttpWebResponse res = (HttpWebResponse)request.GetResponse();
                fileSize = res.ContentLength; //the size comes from here
                using (Stream rstream = res.GetResponseStream())
                {
                    //the header + extension should come from here.
                    fileFullName = res.Headers["Content-Disposition"] != null ?
                        res.Headers["Content-Disposition"].Replace("attachment; filename=", "").Replace("\"", "") :
                        res.Headers["Location"] != null ? Path.GetFileName(res.Headers["Location"]) :
                        Path.GetFileName(url).Contains('?') || Path.GetFileName(url).Contains('=') ?
                        Path.GetFileName(res.ResponseUri.ToString()) : defaultFileName;
                }
                res.Close();
            }
            catch (Exception e)
            {
                //if the internet is not there, now is the time to warn. 
                //because if it drops during the download, i have made the system to crash with a message
                if(e.Message.Contains("The remote name could not be resolved"))
                    throw new DownloaderUCException(Enums.ErrorType.NoInternet, e);
            }
            return fileFullName != defaultFileName;
        }

    }
}
