using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;


namespace FileMonitor.ScanningAgent
{
    public class Config
    {
        public string DataFile => ConfigurationManager.AppSettings["DataFile"];
        public string ErrorsDataFile => ConfigurationManager.AppSettings["ErrorsDataFile"];
    }

    public interface ITable
    {
        void PersistDatabase();
        void Configure();
        DataRow NewRow();
        void AddRow(DataRow dr);
    }

    public class SimulatedDatabase : IFileMonitorDataAccess
    {

        private Config _config;
        
        public SimulatedErrorsDataTable _errors = null;
        public HashesSimulatedDataTable _hashes = null;

        public SimulatedDatabase(Config config)
        {
            _config = config;

            _errors = new SimulatedErrorsDataTable(_config);
            _hashes = new HashesSimulatedDataTable(_config);

            ConfigureDatabase();
        }

        ~SimulatedDatabase()
        {
            PersistDatabase();
        }

        private void PersistDatabase()
        {
            _hashes.PersistDatabase();
            _errors.PersistDatabase();
        }

        private void ConfigureDatabase()
        {
            _errors.Configure();
            _hashes.Configure();
            Console.WriteLine("Database Recovered.");
        }

        public bool FileAlreadyHashed(string filePath) => _hashes.FileAlreadyHashed(filePath);
        
        public void UpdateFileHash(string filePath, string hash, DateTime fileMofifiedOn, DateTime fileCreatedOn, long fileSize)
        {
            DataRow dr = _hashes.GetHashedFile(filePath);
            if (dr == null) dr = _hashes.NewRow();

            dr["FilePath"] = filePath;
            dr["Hash"] = hash;
            dr["LastScannedOn"] = DateTime.Now;

            dr["FileModifiedOn"] = fileMofifiedOn;
            dr["FileCreatedOn"] = fileCreatedOn;
            dr["FileSize"] = fileSize; 

            _hashes.AddRow(dr);
            PersistDatabase();
        }

        public Dictionary<string, string> GetAllHashedFiles() => _hashes.GetAllHashedFiles();

        public void AddFailure(string filePath, string errorMessage, string stackTrace)
        {
            DataRow dr = _errors.NewRow();
            
            dr["ReceivedOn"] = DateTime.Now;
            dr["ErrorMessage"] = errorMessage;
            dr["StackTrace"] = stackTrace;

            _errors.AddRow(dr);
            _errors.PersistDatabase();
        }
    }
}