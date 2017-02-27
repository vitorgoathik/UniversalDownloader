
using Utilities.BatchDownloaderUC;

namespace BatchDownloaderUC.Models
{
    public class RemoteFileInfo
    {
        public readonly string Url;
        public string FileFullName { get; internal set; }
        public readonly long SizeBytes; 
        public string SizeInUnit => SizeBytes > 0 ? Functions.ConvertSizeToUnit(SizeBytes) : "";

        internal RemoteFileInfo(string url, string fileFullName, long sizeBytes)
        {
            this.FileFullName = fileFullName;
            this.SizeBytes = sizeBytes;
            this.Url = url;
        }
    }
}
