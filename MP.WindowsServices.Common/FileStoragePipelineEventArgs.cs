using System;
using System.Collections.Generic;

namespace MP.WindowsServices.Common
{
    public class FileStoragePipelineEventArgs : EventArgs
    {
        public string FilePath { get; set; }

        public List<string> BatchFilePaths { get; set; }
    }
}
