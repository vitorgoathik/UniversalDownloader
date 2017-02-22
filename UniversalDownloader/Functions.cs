using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UniversalDownloaderAgoda
{
    public class Functions
    {
        ValidationTests validation = new ValidationTests();
        public string urls = "";
        public string destination = "";

        /// <summary>
        /// Will return the last piece of the url, which is the file name
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetFileName(string url)
        {
            if (!validation.IsDestinationValid(destination))
                return "";

            url = url.Replace("/", @"\");
            int lastSlash = url.LastIndexOf(@"\");
            return url.Substring(lastSlash, url.Length - lastSlash);
        }

        public int CheckFileSize(string uri)
        {
            System.Net.WebRequest req = System.Net.HttpWebRequest.Create(uri);
            req.Method = "HEAD";
            using (System.Net.WebResponse resp = req.GetResponse())
            {
                int ContentLength;
                if (int.TryParse(resp.Headers.Get("Content-Length"), out ContentLength))
                {
                    return ContentLength;
                }
            }
            return 0;
        }
    }
}
