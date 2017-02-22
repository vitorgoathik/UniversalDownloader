using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UniversalDownloaderAgoda
{
    public class ValidationTests
    {
        //source: @diegoperini https://mathiasbynens.be/demo/url-regex
        public const string UrlRegexPattern = @"^(https?|s?ftp)://[^\s/$.?#].[^\s]*$";

        /// <summary>
        /// Tests whether all the urls in the property are valid
        /// </summary>
        /// <returns>invalid urls separated by ; or a format error message</returns>
        public string AreAllUrlsValid(string urls)
        {
            string invalidUrls = "";
            try
            {
                foreach (string url in urls.Replace(" ","").Split(';'))
                {
                    if (!IsUrlValid(url))
                        invalidUrls += url + ";";
                }
            }
            catch (Exception e)
            {
                return "Error when splitting the url text by semicolon";
            }
            return invalidUrls;
        }

        /// <summary>
        /// Tests wether a single Url is valid
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool IsUrlValid(string url)
        {
            return Regex.Match(url, UrlRegexPattern).Success;
        }

        /// <summary>
        /// checks if the destination directory exists in the disk
        /// </summary>
        /// <param name="destination">destination folder</param>
        /// <returns>if it exists in the disk</returns>
        public bool IsDestinationValid(string destination)
        {
            destination = destination.Replace("/", @"\");
            if (!destination.Contains(@"\"))
                return false;
            return Directory.Exists(destination.Substring(0, destination.LastIndexOf(@"\")));
        }
    }
}
