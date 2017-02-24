using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BatchDownloaderUC.Models
{
    internal class FtpInfo : Uri
    {

        public readonly bool IsDirectory;
        public readonly string Directory;
        public readonly FileInfo File;
        public FtpInfo(string url) : base(url)
        {
            FtpInfo ftp = GetFtpInfo(url);
            Directory = ftp.Directory;
            IsDirectory = ftp.IsDirectory;
            File = ftp.File;
        }
        private FtpInfo(bool isDirectory, string directory, FileInfo file, string url) : base(url)
        {
            IsDirectory = isDirectory;
            Directory = directory;
            File = file;
        }

        private FtpInfo GetFtpInfo(string url)
        {
            string regex =
                    @"^" +                          //# Start of line
                    @"(?<dir>[\-ld])" +             //# File size          
                    @"(?<permission>[\-rwx]{9})" +  //# Whitespace          \n
                    @"\s+" +                        //# Whitespace          \n
                    @"(?<filecode>\d+)" +
                    @"\s+" +                        //# Whitespace          \n
                    @"(?<owner>\w+)" +
                    @"\s+" +                        //# Whitespace          \n
                    @"(?<group>\w+)" +
                    @"\s+" +                        //# Whitespace          \n
                    @"(?<size>\d+)" +
                    @"\s+" +                        //# Whitespace          \n
                    @"(?<month>\w{3})" +            //# Month (3 letters)   \n
                    @"\s+" +                        //# Whitespace          \n
                    @"(?<day>\d{1,2})" +            //# Day (1 or 2 digits) \n
                    @"\s+" +                        //# Whitespace          \n
                    @"(?<timeyear>[\d:]{4,5})" +    //# Time or year        \n
                    @"\s+" +                        //# Whitespace          \n
                    @"(?<filename>(.*))" +          //# Filename            \n
                    @"$";                           //# End of line


            var split = new Regex(regex).Match(url);
            string dir = split.Groups["dir"].ToString();
            string filename = split.Groups["filename"].ToString();
            long size;
            if (!long.TryParse(split.Groups["size"].ToString(), out size))
                throw new Exception("Could not parse " + split.Groups["size"] + " to long");
            bool isDirectory = !string.IsNullOrWhiteSpace(dir) && dir.Equals("d", StringComparison.OrdinalIgnoreCase);
            return new FtpInfo(IsDirectory,dir,IsDirectory ? new FileInfo(filename,size) : null, url)
        }
    }
}
