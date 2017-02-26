using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchDownloaderUC.Models
{
    public class Destination
    {
        public readonly string FullPath;
        public string FullPathWithFile { get { return FullPath + "/" + FileFullName; } }

        public string FileFullName
        {
            get
            {
                return fileFullName;
            }
            internal set { fileFullName = value; }
        }

        private string fileFullName;

        internal Destination(string fullPath, string fileFullName)
        {
            this.FullPath = fullPath;
            this.fileFullName = fileFullName;
        }

        public string SetFullUniquePathWithFile()
        {
            string ext = Path.GetExtension(FullPathWithFile);
            string fileNameWithoutExt = FullPathWithFile.Replace(ext,"");
            
            string filenameFormat = fileNameWithoutExt + "{0}" + ext;
            string filename = string.Format(filenameFormat, "");
            int i = 1;
            while (File.Exists(FullPath + "/" + filename))
                filename = string.Format(filenameFormat, "(" + (i++) + ")");
            this.FileFullName = filename;
            return FullPath + "/" + filename;
        }
        public long GetTotalFreeSpace()
        {
            string driveName = Path.GetPathRoot(FullPath);
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady && drive.Name == driveName)
                {
                    return drive.TotalFreeSpace;
                }
            }
            return -1;
        }
    }
}
