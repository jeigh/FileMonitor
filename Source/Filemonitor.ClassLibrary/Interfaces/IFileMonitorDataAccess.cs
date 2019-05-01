using System;
using System.Collections.Generic;

namespace FileMonitor.ClassLibrary
{
    public interface IFileMonitorDataAccess
    {
        bool FileAlreadyHashed(string filePath);
        void UpdateFileHash(string filePath, string hash, DateTime fileMofifiedOn, DateTime fileCreatedOn, long fileSize);
        Dictionary<string, string> GetAllHashedFiles();
        void AddFailure(string filePath, string errorMessage, string stackTrace);

    }
}