using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileMonitor.ScanningAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            Config config = new Config();
            IFileMonitorDataAccess dataAccess = new SimulatedDatabase(config);


            string scanroot = System.Configuration.ConfigurationManager.AppSettings["ScanRoot"];
            var files = new List<string>();
            
            Console.WriteLine("Generating List of files ...");
            Console.WriteLine();
            foreach (string f in Directory.GetFiles(scanroot, "*", SearchOption.AllDirectories))
            {
                files.Add(f);    
                Console.Write(".");
            }
            Console.WriteLine();
            Console.WriteLine(files.LongCount() + " files discovered.");
            Console.WriteLine("Generating Hashes ...");
            Console.WriteLine();

            IHashGenerator gen = new Sha1HashGenerator();

            foreach (string filePath in files)
            {
                try
                {
                    if (!dataAccess.FileAlreadyHashed(filePath))
                    {
                        FileInfo fileInfo = new FileInfo(filePath);

                        string hash = gen.GenerateHash(filePath);

                        dataAccess.UpdateFileHash(filePath, hash, fileInfo.LastWriteTime, fileInfo.CreationTime,
                            fileInfo.Length);

                        Console.Write(".");
                    } else Console.Write("^");
                }

                catch (Exception e)
                {
                    Console.Write("!");
                    dataAccess.AddFailure(filePath, e.ToString(), e.StackTrace);
                }
            }
            Console.WriteLine();
            Console.WriteLine("Hit a key to close.");
            Console.ReadKey();
        }
    }
}
