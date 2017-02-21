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
        public string uri = "";
        public string destination = "";
        //source: @diegoperini https://mathiasbynens.be/demo/url-regex
        public const string UrlRegexPattern = @"_^(?:(?:https?|ftp)://)(?:\S+(?::\S*)?@)?(?:(?!10(?:\.\d{1,3}){3})(?!127(?:\.\d{1,3}){3})(?!169\.254(?:\.\d{1,3}){2})(?!192\.168(?:\.\d{1,3}){2})(?!172\.(?:1[6-9]|2\d|3[0-1])(?:\.\d{1,3}){2})(?:[1-9]\d?|1\d\d|2[01]\d|22[0-3])(?:\.(?:1?\d{1,2}|2[0-4]\d|25[0-5])){2}(?:\.(?:[1-9]\d?|1\d\d|2[0-4]\d|25[0-4]))|(?:(?:[a-z\x{00a1}-\x{ffff}0-9]+-?)*[a-z\x{00a1}-\x{ffff}0-9]+)(?:\.(?:[a-z\x{00a1}-\x{ffff}0-9]+-?)*[a-z\x{00a1}-\x{ffff}0-9]+)*(?:\.(?:[a-z\x{00a1}-\x{ffff}]{2,})))(?::\d{2,5})?(?:/[^\s]*)?$_iuS";


        public bool IsUriValid()
        {
            return Regex.Match(uri, UrlRegexPattern).Success;
        }
        public bool IsDestinationValid()
        {
            destination = destination.Replace("/", @"\");
            return Directory.Exists(destination.Substring(0, destination.LastIndexOf(@"\")));
        }

        public string GetFileName()
        {
            destination = destination.Replace("/", @"\");
            int lastSlash = destination.LastIndexOf(@"\");
            return destination.Substring(lastSlash, destination.Length - lastSlash);
        }

        public int CheckFileSize()
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
