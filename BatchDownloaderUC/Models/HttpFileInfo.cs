using System;
using System.IO;
using System.Linq;
using System.Net; 

namespace BatchDownloaderUC.Models
{
    public class HttpFileInfo : FileInfo
    {
        internal HttpFileInfo(string url) : this(url, System.IO.Path.GetFileName(new System.Uri(url).AbsolutePath)) { }
        private HttpFileInfo(string url, string defaultFileName) : base(GetFileInfo(url, defaultFileName)) { }
        private static FileInfo GetFileInfo(string url, string defaultFileName)
        {
            long sizeBytes;
            string fileFullName;
            GetHttpHeaderInfo(url, defaultFileName, out fileFullName, out sizeBytes);
            return new FileInfo(url, fileFullName, sizeBytes);
        }
        
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
                fileSize = res.ContentLength;
                using (Stream rstream = res.GetResponseStream())
                {
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
                throw new Exception("GetFileName Error: " + e.Message);
            }
            return fileFullName != defaultFileName;
        }

    }
}
