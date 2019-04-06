using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileMonitor.ScanningAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            string scanroot = System.Configuration.ConfigurationManager.AppSettings["ScanRoot"];
            var files = new List<string>();
            
            Console.WriteLine("Generating List of files ...");
            Console.WriteLine();
            foreach (string f in System.IO.Directory.GetFiles(scanroot, "*", System.IO.SearchOption.AllDirectories))
            {
                files.Add(f);    
                Console.Write(".");
            }
            Console.WriteLine();
            Console.WriteLine(files.LongCount() + " files discovered.");
            Console.WriteLine("Generating Hashes ...");
            Console.WriteLine();

            IHashGenerator gen = new Sha1HashGenerator();
            IFileMonitorDataAccess dataAccess = new SimulatedDatabase();

            foreach (string filePath in files)
            {
                try
                {
                    bool alreadyHashed = dataAccess.FileAlreadyHashed(filePath);
                    if(!alreadyHashed)
                    {
                        string hash = gen.GenerateHash(filePath);

                        dataAccess.AddFileHash(filePath, hash);
                        
                        Console.Write(".");
                    }
                }

                catch (Exception e)
                {
                    dataAccess.AddFailure(filePath, e.ToString());
                    
                }
            }

            Console.WriteLine(dataAccess.GetFailureCount() + "read failures experienced.");
        }
    }
}
