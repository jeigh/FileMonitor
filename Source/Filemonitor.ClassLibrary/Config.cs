using System.Configuration;

namespace FileMonitor.ClassLibrary
{
    public class Config
    {
        public string DataFile => ConfigurationManager.AppSettings["DataFile"];
        public string ErrorsDataFile => ConfigurationManager.AppSettings["ErrorsDataFile"];
    }
}