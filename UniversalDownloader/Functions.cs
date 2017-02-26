using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalDownloader
{
    public class Functions
    {
        public static string GetDownloadsFolder()
        {
            try
            {
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = Path.Combine(pathUser, "Downloads");
                return pathDownload;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
