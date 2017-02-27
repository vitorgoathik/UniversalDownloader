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
        /// Ex: New Software.exe
        /// </summary>

        #endregion
        internal Destination(string fullPath)
        {
            this.FullPath = fullPath;
        }
        #region Functions
        

        /// <summary>
        /// Gives the total free space for that folder's driver. 
        /// </summary>
        /// <returns></returns>
        internal long GetTotalFreeSpace()
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
