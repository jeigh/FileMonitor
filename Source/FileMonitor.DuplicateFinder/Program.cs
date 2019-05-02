using FileMonitor.ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using FileMonitor.ClassLibrary.TransferObjects;

namespace FileMonitor.DuplicateFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            Config config = new Config();
            IFileMonitorDataAccess dataAccess = new SimulatedDatabase(config);

            List<HashedFile> theHashedFiles = dataAccess.GetAllHashedFileInstances().ToList();

            Dictionary<string, List<HashedFile>> hashDictionary = new Dictionary<string, List<HashedFile>>();
            List<string> duplicateHashes = new List<string>();

            foreach (HashedFile thf in theHashedFiles)
            {
                if (hashDictionary.ContainsKey(thf.Hash))
                {
                    hashDictionary[thf.Hash].Add(thf);
                    if (!duplicateHashes.Contains(thf.Hash))
                        duplicateHashes.Add(thf.Hash);
                }


                else
                    hashDictionary.Add(thf.Hash, new List<HashedFile>() {thf});
            }

            foreach (string dupHash in duplicateHashes)
            {
                foreach (HashedFile xyz in hashDictionary[dupHash])
                {

                    //todo: confirm that it is still on the filesystem.   if not. delete it from filesystem, list, and database.
                }

                
            }

            if (duplicateHashes.Count > 0)
            {
                Console.WriteLine("Duplicates Found: " + duplicateHashes.LongCount());
                //foreach (string dh in duplicateHashes)
                //{
                //    Console.WriteLine(dh);
                //}
            }

            Console.ReadKey();


        }
    }
}
