using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BatchDownloaderUC
{
    internal static class Functions
    {       

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
                        timeStr += time.Minutes + (time.Minutes > 1 ? " minute " : " minute ");
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



    }
}
