using System;

namespace FileMonitor.ClassLibrary.TransferObjects
{
    public class HashedFile
    {
        public string Hash { get; set; }
        public string FilePath { get; set; }
        public DateTime LastScannedOn { get; set; }
        public DateTime FileModifiedOn { get; set; }
        public DateTime FileCreatedOn { get; set; }
        public int FileSize { get; set; }
    }
}