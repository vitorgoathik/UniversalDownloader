using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalDownloader.Events
{
    public class OverallProgressChangedEventArgs
    {
        public readonly List<string> RemainingFiles;
        
        public OverallProgressChangedEventArgs(List<string> remainingFiles)
        {
            RemainingFiles = remainingFiles;
        }
    }
}
