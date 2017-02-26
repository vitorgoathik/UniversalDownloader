using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BatchDownloaderUC.Models
{
    internal class FtpInfo : FileInfo
    {

        internal readonly List<FtpInfo> ChildrenPaths = new List<FtpInfo>();
        internal readonly NetworkCredential Credential;
        internal readonly string FtpFolder;
        internal readonly bool IsDirectory;

        internal FtpInfo(string url, NetworkCredential credential) : base(url)
        {
            Credential = credential;
            ChildrenPaths = GetFtpInfo(ChildrenPaths, url, credential, url);
            FileFullName = "Ftp_Root_" + url;
            FtpFolder = "";
            SizeBytes = 0;
            IsDirectory = true;
        }
        private FtpInfo(FileInfo fileInfo, bool isDirectory, string ftpFolder, NetworkCredential credential, string rootUrl) : base(fileInfo)
        {
            Credential = credential;
            IsDirectory = isDirectory;
            FtpFolder = ftpFolder;
            ChildrenPaths = GetFtpInfo(ChildrenPaths, fileInfo.Url, credential, rootUrl);
        }


        private List<FtpInfo> GetFtpInfo(List<FtpInfo> children, string url, NetworkCredential credentials, string rootUrl)
        {
            List<string> lines = ReadFtpDirectoryLines(url, credentials);

            if (!url.EndsWith("/"))
                url += "/";
            if (!rootUrl.EndsWith("/"))
                rootUrl += "/";


            foreach (string line in lines)
            {
                string[] tokens = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string dirOrSize = tokens[2];

                string name = line.Replace(tokens[0], "").Replace(tokens[1], "").Replace(tokens[2], "").Trim().Replace(" ", "%20");

                string fileUrl = url + name;
                string ftpFolder = url.Replace(rootUrl, "").Replace("%20", " "); ;

                if (dirOrSize == "<DIR>")
                    children.Add(new FtpInfo(new FileInfo(fileUrl, fileUrl.Replace(rootUrl,""),0),true, "", credentials, rootUrl));
                else
                    if (!url.EndsWith(name+"/"))
                        children.Add(new FtpInfo(new FileInfo(fileUrl, name, long.Parse(dirOrSize)), false, ftpFolder, credentials, rootUrl));
                
            }
            return children;
        }

        private List<string> ReadFtpDirectoryLines(string url, NetworkCredential credentials)
        {
            FtpWebRequest listRequest = (FtpWebRequest)WebRequest.Create(url);
            listRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            listRequest.Credentials = credentials;

            List<string> lines = new List<string>();

            using (FtpWebResponse listResponse = (FtpWebResponse)listRequest.GetResponse())
            using (Stream listStream = listResponse.GetResponseStream())
            using (StreamReader listReader = new StreamReader(listStream))
            {
                while (!listReader.EndOfStream)
                {
                    lines.Add(listReader.ReadLine());
                }
            }
            return lines;
        }
        
    }
}
