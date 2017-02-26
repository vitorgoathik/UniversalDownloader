
namespace BatchDownloaderUC.Models
{
    public class FileInfo : Uri
    {
        public string FileFullName { get; protected set; }
        public long SizeBytes { get; protected set; }
        public string SizeInUnit => SizeBytes > 0 ? Functions.ConvertSizeToUnit(SizeBytes) : "";

        internal FileInfo(FileInfo fileInfo) : this(fileInfo.Url, fileInfo.FileFullName, fileInfo.SizeBytes) { }
        internal FileInfo(string url, string fileFullName, long sizeBytes) : base(url)
        {
            this.FileFullName = fileFullName;
            this.SizeBytes = sizeBytes;
        }

        protected FileInfo(string url) : base(url)
        {
        }
    }
}
