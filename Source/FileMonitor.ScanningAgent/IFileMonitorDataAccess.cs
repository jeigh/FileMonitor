using System.Collections.Generic;

namespace FileMonitor.ScanningAgent
{
    interface IFileMonitorDataAccess
    {
        bool FileAlreadyHashed(string filePath);
        void FileHashed(string filePath);
        void AddFileHash(string filePath, string hash);
        void AddFailure(string filePath, string errorMessage);
        long GetFailureCount();

        Dictionary<string, string> GetAllHashedFiles();
        List<string> GetAllFailures();

    }
}