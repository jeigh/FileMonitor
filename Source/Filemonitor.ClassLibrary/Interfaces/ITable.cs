using System.Data;

namespace FileMonitor.ClassLibrary
{
    public interface ITable
    {
        void PersistDatabase();
        void Configure();
        DataRow NewRow();
        void AddRow(DataRow dr);
    }
}