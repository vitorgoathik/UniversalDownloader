using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace BatchDownloaderUC.Models
{
    internal class FtpInfo : FileInfo
    {

        internal readonly List<FtpInfo> ChildrenPaths = new List<FtpInfo>();
        internal readonly NetworkCredential Credential;
        internal readonly string FtpFolder;
        internal readonly bool IsDirectory;
        /// <summary>
        /// It starts from here. It asks for a Url and credentials
        /// </summary>
        /// <param name="url"></param>
        /// <param name="credential"></param>
        internal FtpInfo(string url, NetworkCredential credential) : base(url)
        {
            Credential = credential;
            //then it calls this recursive method the recursion will happen through the call of constructors inside a loop
            ChildrenPaths = GetFtpInfo(ChildrenPaths, url, credential, url);
            FileFullName = "Ftp_Root_" + url;
            FtpFolder = "";
            SizeBytes = 0;
            IsDirectory = true;
        }
        /// <summary>
        /// Constructor needed by the recursive tree method
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="isDirectory"></param>
        /// <param name="ftpFolder"></param>
        /// <param name="credential"></param>
        /// <param name="rootUrl"></param>
        private FtpInfo(FileInfo fileInfo, bool isDirectory, string ftpFolder, NetworkCredential credential, string rootUrl) : base(fileInfo)
        {
            Credential = credential;
            IsDirectory = isDirectory;
            FtpFolder = ftpFolder;
            ChildrenPaths = GetFtpInfo(ChildrenPaths, fileInfo.Url, credential, rootUrl);
        }


        private List<FtpInfo> GetFtpInfo(List<FtpInfo> children, string url, NetworkCredential credentials, string rootUrl)
        {
            //those are the lines provided by the WebRequestMethods.Ftp.ListDirectoryDetails Method.
            //they are all the information we can get from the headers of the files
            List<string> lines = ReadFtpDirectoryLines(url, credentials);

            if (!url.EndsWith("/"))
                url += "/";
            if (!rootUrl.EndsWith("/"))
                rootUrl += "/";

            //going through this information, we find a specific formating logic
            foreach (string line in lines)
            {
                string[] tokens = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                //this token will tell dir when its a directory, or it will be a number that represents the size
                string dirOrSize = tokens[2];

                //the name should be escaped to compose a url. 
                //ex: "this folder has spaces", should be "this%20folder%20has%20spaces" in the url
                string name = System.Uri.EscapeDataString(line.Replace(tokens[0], "").Replace(tokens[1], "").Replace(tokens[2], "").Trim());
                //the same way we have to unescape to represent a directory. it's just the other way around
                string ftpFolder = System.Uri.UnescapeDataString(url.Replace(rootUrl, ""));
                //full url
                string fileUrl = url + name;


                //if it is a DIR, enter the recursion by adding a new child
                if (dirOrSize == "<DIR>")
                    children.Add(new FtpInfo(new FileInfo(fileUrl, fileUrl.Replace(rootUrl,""),0),true, "", credentials, rootUrl));
                else
                //if it is a file is also enters the recursion, however, this 'if' will assure things wont crash during the ReadFtpDirectoryLines
                    if (!url.EndsWith(name+"/"))
                        children.Add(new FtpInfo(new FileInfo(fileUrl, name, long.Parse(dirOrSize)), false, ftpFolder, credentials, rootUrl));
                
            }
            return children;
        }

        /// <summary>
        /// reads the headers of the ftp directory and file lines
        /// </summary>
        /// <param name="url"></param>
        /// <param name="credentials"></param>
        /// <returns></returns>
        private List<string> ReadFtpDirectoryLines(string url, NetworkCredential credentials)
        {
            FtpWebRequest listRequest = (FtpWebRequest)WebRequest.Create(url);
            listRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails; //this is the method required to do so
            listRequest.Credentials = credentials;

            List<string> lines = new List<string>();
            //those are the same streams used to open a FTP download, as seen in the Downloader.cs class
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
