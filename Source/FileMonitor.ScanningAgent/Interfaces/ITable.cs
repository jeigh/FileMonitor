using System.Data;

namespace FileMonitor.ScanningAgent
{
    public interface ITable
    {
        void PersistDatabase();
        void Configure();
        DataRow NewRow();
        void AddRow(DataRow dr);
    }
}