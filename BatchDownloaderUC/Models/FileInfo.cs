
namespace BatchDownloaderUC.Models
{
    public class FileInfo
    {
        public readonly string FileFullName;
        public readonly long SizeBytes;
        public FileInfo(string fileFullName, long sizeBytes)
        {
            this.FileFullName = fileFullName;
            this.SizeBytes = sizeBytes;
        }

    }
}
