using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Utilities.BatchDownloaderUC.Enums;

namespace Utilities.BatchDownloaderUC
{
    internal static class Functions
    {
        /// <summary>
        /// Gets the protocol from a given Url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        internal static Protocol GetProtocol(string url)
        {
            if (!ValidationTests.IsUrlValid(url))
                throw new Exception("Invalid Url");
            switch (new System.Uri(url).Scheme.ToLower())
            {
                case "http":
                case "https": return Protocol.Http;
                case "ftp": return Protocol.Ftp;
                case "sftp": return Protocol.Sftp;
                default: throw new Exception("Unknown protocol for url " + url);
            }
        }
        /// <summary>
        /// User friendly representation of a file size
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string ConvertSizeToUnit(long size)
        {
            if (size <= 0) throw new Exception("ConvertSizeToUnit threw an exception: invalid size number");
            string newNumber;
            string unit = "";
            if (size >= 1073741824)
            {
                newNumber = ((decimal)size / 1073741824).ToString("0.##");
                unit = "GB";
            }
            else if (size >= 1048576)
            {
                newNumber = ((decimal)size / 1048576).ToString("0.##");
                unit = "MB";
            }
            else
            {
                newNumber = ((decimal)size / 1024).ToString("0.##");
                unit = "KB";
            }
            return newNumber+unit;
        }

        /// <summary>
        /// User friendly representation of elapsed time
        /// </summary>
        /// <param name="totalSeconds"></param>
        /// <returns></returns>
        public static string FormatTimeToString(double totalSeconds)
        {
            if (totalSeconds < 0) throw new Exception("ConvertTimeToString threw an exception: invalid amounbt of seconds");
            TimeSpan time = TimeSpan.FromSeconds(totalSeconds);
            string timeStr = "";
            if (time.Days > 0)
            {
                timeStr += time.Days + (time.Days > 1 ? " days " : " day ");
                if (time.Hours > 0)
                    timeStr += time.Hours + (time.Hours > 1 ? " hours " : " hour ");
            }
            else
            {
                if (time.Hours > 0)
                {
                    timeStr += time.Hours + (time.Hours > 1 ? " hours " : " hour ");
                    if (time.Minutes > 0)
                        timeStr += time.Minutes + (time.Minutes > 1 ? " minutes " : " minute ");
                }
                else
                {
                    if (time.Minutes > 0)
                    {
                        timeStr += time.Minutes + (time.Minutes > 1 ? " minutes " : " minute ");
                        if (time.Seconds > 0)
                            timeStr += time.Seconds + (time.Seconds > 1 ? " seconds " : " second ");
                    }
                    else
                        if (time.Seconds > 0)
                            timeStr += time.Seconds + (time.Seconds > 1 ? " seconds " : " second ");
                }
            }
            return timeStr;
        }


        /// <summary>
        /// This method will return an unique name by adding (increment) to their suffix
        /// </summary>
        /// <returns>ex: File(2).ext, in case there are two more files named "File" in the folder</returns> 
        internal static string GetDistinguishedFileNameForSaving(string oldFileName, string destination)
        {
            string ext = Path.GetExtension(oldFileName);
            string fileNameWithoutExt = ext != "" ? oldFileName.Replace(ext, "") : oldFileName;

            //then the suffix is built
            string filenameFormat = fileNameWithoutExt + "{0}" + ext;
            string filename = string.Format(filenameFormat, "");
            //and increment baseed on the other files having the same name inside the folder
            int i = 1;
            while (File.Exists(destination + "/" + filename))
                filename = string.Format(filenameFormat, "(" + (i++) + ")");
            return filename;
        }
    }
}
