using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchDownloaderUC.Protocols
{
    public class SFTPDownloader : IDownloader
    {
        #region StartNextDownloadSftp

        /// <summary>
        /// TODO sFTP
        /// </summary>
        public void StartNextDownloadSftp()
        {
            String Host = "ftp.csidata.com";
            int Port = 22;
            String RemoteFileName = "TheDataFile.txt";
            String LocalDestinationFilename = "TheDataFile.txt";
            String Username = "yourusername";
            String Password = "yourpassword";

            using (var sftp = new SftpClient(Host, Port, Username, Password))
            {
                sftp.Connect();

                using (var file = File.OpenWrite(LocalDestinationFilename))
                {
                    sftp.DownloadFile(RemoteFileName, file);
                }

                sftp.Disconnect();
            }
        }

        public override void AbortCurrentDownload()
        {
            throw new NotImplementedException();
        }

        protected override void AddDownloadToList(string url, string destination, string username, string password)
        {
            throw new NotImplementedException();
        }

        protected override void StartNextDownload()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
