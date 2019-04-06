using System.Collections.Generic;
using System.Linq;

namespace FileMonitor.ScanningAgent
{
    public class SimulatedDatabase : IFileMonitorDataAccess
    {
        //todo: find some way of pulling these from storage

        private List<string> _hashedFiles = new List<string>();
        private Dictionary<string, string> fileHashes = new Dictionary<string, string>();
        private Dictionary<string, string> failures = new Dictionary<string, string>();
        
        public bool FileAlreadyHashed(string filePath)
        {
            return _hashedFiles.Contains(filePath);
        }

        public void FileHashed(string filePath)
        {
            _hashedFiles.Add(filePath);
        }

        public void AddFileHash(string filePath, string hash)
        {
            fileHashes.Add(filePath, hash);
            this.FileHashed(filePath);
        }

        public void AddFailure(string filePath, string errorMessage)
        {
            failures.Add(filePath, errorMessage);
        }

        public long GetFailureCount()
        {
            return failures.LongCount();
        }

        public Dictionary<string, string> GetAllHashedFiles()
        {
            return fileHashes;
        }

        public List<string> GetAllFailures()
        {
            return failures.Values.ToList();
        }
    }
}