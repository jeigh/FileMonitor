using System.Collections.Generic;
using System.Data;

namespace FileMonitor.ScanningAgent
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