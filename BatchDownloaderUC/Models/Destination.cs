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
        #region Fields and properties

        /// <summary>
        /// Ex: C:/Program Files/Installed Software
        /// </summary>
        public readonly string FullPath;
        /// <summary>
        /// Ex: C:/Program Files/Installed Software/New Software.exe
        /// </summary>
        public string FullPathWithFile { get { return FullPath + "/" + FileFullName; } }
        /// <summary>
        /// Ex: New Software.exe
        /// </summary>
        public string FileFullName
        {
            get
            {
                return fileFullName;
            }
            internal set { fileFullName = value; }
        }

        private string fileFullName;

        #endregion
        internal Destination(string fullPath, string fileFullName)
        {
            this.FullPath = fullPath;
            this.fileFullName = fileFullName;
        }
        #region Functions
        /// <summary>
        /// This method will assure the files names will be unique, by adding (increment) to their suffix
        /// </summary>
        /// <returns>ex: File(2).ext, in case there are two more files named "File" in the folder</returns>
        public string SetFullUniquePathWithFile()
        {
            //first the file must be separated from its extension
            string ext = Path.GetExtension(FileFullName);
            string fileNameWithoutExt = ext != "" ? FileFullName.Replace(ext,"") : FileFullName;
            
            //then the suffix is built
            string filenameFormat = fileNameWithoutExt + "{0}" + ext;
            string filename = string.Format(filenameFormat, "");
            //and increment baseed on the other files having the same name inside the folder
            int i = 1;
            while (File.Exists(FullPath + "/" + filename))
                filename = string.Format(filenameFormat, "(" + (i++) + ")");
            this.FileFullName = filename;
            return FullPath + "/" + filename;
        }

        /// <summary>
        /// Gives the total free space for that folder's driver. 
        /// </summary>
        /// <returns></returns>
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

        #endregion
    }
}
