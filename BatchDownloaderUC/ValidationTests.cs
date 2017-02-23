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
    public static class ValidationTests
    {
        public const string UrlRegexPattern = @"^(https?|s?ftp)://[^\s/$.?#].[^\s]*$";



        /// <summary>
        /// Tests whether all the urls in the property are valid
        /// </summary>
        /// <param name="urls">list of concateneted urls</param>
        /// <param name="splittingChar">Character used to split the string</param>
        /// <param name="invalidUrls">invalid urls separated by ; or a format error message</param>
        /// <returns>if they are valid</returns>
        public static bool AreAllUrlsValid(string urls, UrlSplittingChar splittingChar, out string invalidUrls)
        {
            invalidUrls = "";
            try
            {
                foreach (string url in urls.Replace(" ", "").Split(Functions.GetUrlSplittingChar(splittingChar)))
                {
                    if (!IsUrlValid(url))
                        invalidUrls += url + ";";
                }
            }
            catch (Exception e)
            {
                invalidUrls = String.Format("Error when splitting the url text by {0}: {1}", splittingChar, e.InnerException);
                return false;
            }
            return invalidUrls == "";
        }

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

        public static bool IsDriverSpaceSufficient(string destination, List<string> listUrls)
        {
            return Functions.GetTotalFreeSpace(destination) - Functions.CheckListFileTotalSize(listUrls) > 0;
        }
    }
}
