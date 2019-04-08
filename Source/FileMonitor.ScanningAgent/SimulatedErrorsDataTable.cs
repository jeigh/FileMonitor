using System.Data;

namespace FileMonitor.ScanningAgent
{
    public class SimulatedErrorsDataTable : ITable
    {
        private Config _config;

        private DataTable errorsDataTable = new DataTable("Errors");
        

        public SimulatedErrorsDataTable(Config config)
        {
            _config = config;
        }

        private void ConfigureErrorsDataTable()
        {
            errorsDataTable.Columns.Add("ReceivedOn");
            errorsDataTable.Columns.Add("ErrorMessage");
            errorsDataTable.Columns.Add("StackTrace");
        }

        public void Configure()
        {
            if (!System.IO.File.Exists(_config.ErrorsDataFile))
                ConfigureErrorsDataTable();
            else
            {
                errorsDataTable.ReadXmlSchema(_config.ErrorsDataFile);
                errorsDataTable.ReadXml(_config.ErrorsDataFile);
            }
        }

        public DataRow NewRow() =>  errorsDataTable.NewRow();
        public void AddRow(DataRow dr) =>  errorsDataTable.Rows.Add(dr);
        public void PersistDatabase() => errorsDataTable.WriteXml(_config.ErrorsDataFile, true);

    }
}