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
        /// Convert a concateneted String of URLs to a List
        /// </summary>
        /// <param name="urls"></param>
        /// <param name="splittingChar"></param>
        /// <returns></returns>
        public static List<string> ConvertStringToListUrls(string urls, UrlSplittingChar splittingChar)
        {
            urls = urls.Replace(" ", "");
            if (string.IsNullOrEmpty(urls)) throw new Exception("Urls are empty");
            string invalidUrls = "";
            if (!ValidationTests.AreAllUrlsValid(urls, splittingChar, out invalidUrls))
                throw new Exception("Invalid URLs " + invalidUrls);

            return urls.Split(GetUrlSplittingChar(splittingChar)).ToList();
        }

        public static char GetUrlSplittingChar(UrlSplittingChar splittingChar)
        {
            return splittingChar == UrlSplittingChar.Semicolon ? ';' : ',';
        }


        public static bool TryGetFileFullName(string url, string defaultFileName, out string fileFullName)
        {
            if (!ValidationTests.IsUrlValid(url))
                throw new Exception("GetFileName: Url is not valid");
            fileFullName = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                HttpWebResponse res = (HttpWebResponse)request.GetResponse();
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
