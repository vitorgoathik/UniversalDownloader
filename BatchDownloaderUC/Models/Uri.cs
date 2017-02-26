using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BatchDownloaderUC.Enums;

namespace BatchDownloaderUC.Models
{
    public class Uri
    { 
        public readonly string Url;
        internal readonly Protocol Protocol;
        internal Uri(string url)
        {
            this.Url = url;
            this.Protocol = GetProtocol(url);
        }
        internal Protocol GetProtocol(string url)
        {
            if (!ValidationTests.IsUrlValid(url))
                throw new Exception("Uri GetProtocol: Invalid Url");
            switch(new System.Uri(url).Scheme.ToLower())
            {
                case "http":
                case "https": return Protocol.Http;
                case "ftp": return Protocol.Ftp;
                case "sftp": return Protocol.Sftp;
                default: throw new Exception("Unknown protocol for url " + url);
            }
        }
    }
}
