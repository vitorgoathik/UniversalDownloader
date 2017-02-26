using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static BatchDownloaderUC.Enums;

namespace BatchDownloaderUC
{
    internal static class ValidationTests
    {
        public const string UrlRegexPattern = @"^(https?|s?ftp)://[^\s/$.?#].[^\s]*$";
        
        /// <summary>
        /// Tests wether a single Url is valid
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsUrlValid(string url)
        {
            return Regex.Match(url, UrlRegexPattern).Success;
        }

        /// <summary>
        /// checks if the destination directory exists in the disk
        /// </summary>
        /// <param name="destination">destination folder</param>
        /// <returns>if it exists in the disk</returns>
        public static bool IsDestinationValid(string destination)
        {
            destination = destination.Replace("/", @"\");
            if (!destination.Contains(@"\"))
                return false;
            return Directory.Exists(destination.Substring(0, destination.LastIndexOf(@"\")));
        }
        
    }
}
