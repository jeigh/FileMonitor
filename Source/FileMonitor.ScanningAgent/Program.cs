using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FileMonitor.ScanningAgent
{
    public interface IHashGenerator
    {
        string GenerateHash(string filePath);
    }

    public class Sha1HashGenerator : IHashGenerator
    {
        public string GenerateHash(string filePath)
        {
            StringBuilder formatted = new StringBuilder(40);  //size of sha1 hash

            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            using (BufferedStream bs = new BufferedStream(fs))
            {
                using (SHA1Managed sha1 = new SHA1Managed())
                {
                    byte[] hash = sha1.ComputeHash(bs);
                    foreach (byte b in hash)
                    {
                        formatted.AppendFormat("{0:X2}", b);
                    }
                }
            }

            return formatted.ToString();
        }
    }


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

            Dictionary<string, string> fileHashes = new Dictionary<string, string>();
            Dictionary<string, string> failures = new Dictionary<string, string>();
            List<string> hashedFiles = new List<string>();

            IHashGenerator gen = new Sha1HashGenerator();

            foreach (string filePath in files)
            {
                try
                {
                    bool alreadyHashed = hashedFiles.Contains(filePath);
                    if(!alreadyHashed)
                    {
                        string hash = gen.GenerateHash(filePath);

                        fileHashes.Add(filePath, hash);
                        hashedFiles.Add(filePath);

                        Console.Write(".");
                    }
                }

                catch (Exception e)
                {
                    failures.Add(filePath, e.ToString());
                }
            }

            Console.WriteLine(failures.LongCount() + "read failures experienced.");
        }
    }
}
