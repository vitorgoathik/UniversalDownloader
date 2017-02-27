using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.BatchDownloaderUC
{
    public static class Enums
    {
        internal enum Protocol
        {
            Http,
            Ftp,
            Sftp
        }
        public enum ErrorType
        {
            [Description("Invalid field: {0}")]
            InvalidField,
            [Description("Field cannot be empty: {0}")]
            EmptyField,
            [Description("Invalid FTP username/password")]
            NotLoggedIn,
            [Description("Insufficient space in disk")]
            InsufficientDiskSpace,
            [Description("Insufficient space in disk for: {0}, size {1}")]
            InsufficientDiskSpaceFor,
            [Description("Failed to add download: no access to internet")]
            NoInternet,
            [Description("Download failed: no access to internet. File name: {0}")]
            NoInternetFor,
            [Description("There was an error when adding this download: {0}")]
            GeneralErrorAddingDownload,
            [Description("There was an error when downloading this file: {0}")]
            GeneralErrorOnDownload,
        }
        public enum DownloadState
        {
            Started,
            Completed,
            Pending,
            Error,
            Closed,
            Canceled,
            Deleted,
        }

        internal static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
}
