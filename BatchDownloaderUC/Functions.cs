using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UniversalDownloader;
using static BatchDownloaderUC.Enums;

namespace BatchDownloaderUC
{
    public static class Functions
    {
        /// <summary>
        /// Will return the last piece of the url, which is the file name
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetFileName(string url)
        {
            if (!ValidationTests.IsUrlValid(url))
                throw new Exception("Url is not valid");
            string name = System.IO.Path.GetFileName(url);
            if (name.Contains("?"))
                name = name.Substring(0, name.IndexOf("?"));
            if (name == "")
                return url;
            return name;
        }

        /// <summary>
        /// Convert a concateneted String of URLs to a Queue
        /// </summary>
        /// <param name="urls"></param>
        /// <param name="splittingChar"></param>
        /// <returns></returns>
        public static Queue<string> ConvertStringToQueueUrls(string urls, UrlSplittingChar splittingChar)
        {
            urls = urls.Replace(" ", "");
            if (string.IsNullOrEmpty(urls)) throw new Exception("Urls are empty");
            string invalidUrls = "";
            if (!ValidationTests.AreAllUrlsValid(urls, splittingChar, out invalidUrls))
                throw new Exception("Invalid URLs " + invalidUrls);

            return new Queue<string>(urls.Split(GetUrlSplittingChar(splittingChar)));
        }

        public static char GetUrlSplittingChar(UrlSplittingChar splittingChar)
        {
            return splittingChar == UrlSplittingChar.Semicolon ? ';' : ',';
        }


        public static long CheckFileSize(string uri)
        {
            if(!ValidationTests.IsUrlValid(uri))
                throw new Exception("CheckFileSize threw an invalid url exception");

            HttpWebRequest wrq = (HttpWebRequest)WebRequest.Create(uri);
            wrq.Method = "HEAD";

            using (var resp = (HttpWebResponse)wrq.GetResponse())
            {
                if(resp.ContentLength <= 0)
                    throw new Exception("Invalid size for url: " + uri);

                return resp.ContentLength;
            } 
        }

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

        public static long GetTotalFreeSpace(string fullPath)
        {
            string driveName = Path.GetPathRoot(fullPath);
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady && drive.Name == driveName)
                {
                    return drive.TotalFreeSpace;
                }
            }
            return -1;
        }

        public static string CheckListFileTotalSizeInUnits(List<string> listUrls)
        {
            return ConvertSizeToUnit(CheckListFileTotalSize(listUrls));
        }
        public static long CheckListFileTotalSize(List<string> listUrls)
        {
            long totalSize = 0;
            listUrls.ForEach(o => totalSize += CheckFileSize(o));
            return totalSize;
        }

        public static string GetDownloadsFolder()
        {
            try
            {
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = Path.Combine(pathUser, "Downloads");
                return pathDownload;
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
