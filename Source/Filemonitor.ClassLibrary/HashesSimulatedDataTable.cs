using System;
using System.Collections.Generic;
using System.Data;
using FileMonitor.ClassLibrary.TransferObjects;

namespace FileMonitor.ClassLibrary
{
    public class HashesSimulatedDataTable : ITable
    {
        private Config _config;
        private DataTable fileHashesDataTable = new DataTable("FileHashes");

        public HashesSimulatedDataTable(Config config)
        {
            _config = config;
        }

        public void Configure()
        {
            if (!System.IO.File.Exists(_config.DataFile))
                ConfigureDataTable();
            else
            {
                fileHashesDataTable.ReadXmlSchema(_config.DataFile);
                fileHashesDataTable.ReadXml(_config.DataFile);
            }
        }

        private void ConfigureDataTable()
        {
            fileHashesDataTable.Columns.Add("FilePath");
            fileHashesDataTable.Columns.Add("Hash");
            fileHashesDataTable.Columns.Add("LastScannedOn");

            fileHashesDataTable.Columns.Add("FileModifiedOn");
            fileHashesDataTable.Columns.Add("FileCreatedOn");
            fileHashesDataTable.Columns.Add("FileSize");
        }

        public DataRow NewRow() => fileHashesDataTable.NewRow();
        public void AddRow(DataRow dr) => fileHashesDataTable.Rows.Add(dr);
        public void PersistDatabase() => fileHashesDataTable.WriteXml(_config.DataFile, true);

        public DataRow GetHashedFile(string filePath)
        {
            foreach (DataRow dr in fileHashesDataTable.Rows)
            {
                if (dr["FilePath"].ToString() == filePath) return dr;
            }
            return null;
        }

        public Dictionary<string, string> GetAllHashedFiles()
        {
            var returnable = new Dictionary<string, string>();
            foreach (DataRow dr in fileHashesDataTable.Rows)
            {
                returnable.Add(dr["FilePath"].ToString(), dr["Hash"].ToString());
            }

            return returnable;
        }

        public List<HashedFile> GetAllHashedFileInstances()
        {
            var returnable = new List<HashedFile>();
            foreach (DataRow dr in fileHashesDataTable.Rows)
            {
                var newHash = new HashedFile();

                newHash.FilePath = dr["FilePath"].ToString();
                newHash.Hash = dr["Hash"].ToString();
                newHash.LastScannedOn = DateTime.Parse(dr["LastScannedOn"].ToString());
                newHash.FileModifiedOn = DateTime.Parse(dr["FileModifiedOn"].ToString());
                newHash.FileCreatedOn = DateTime.Parse(dr["FileCreatedOn"].ToString());
                newHash.FileSize = Int32.Parse(dr["FileSize"].ToString());
                
                returnable.Add(newHash);
                
            }

            return returnable;
        }


        public bool FileAlreadyHashed(string filePath)
        {
            foreach (DataRow dr in fileHashesDataTable.Rows)
            {
                if (dr["FilePath"].ToString() == filePath) return true;
            }
            return false;
        }
    }
}