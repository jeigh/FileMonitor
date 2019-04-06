using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FileMonitor.ScanningAgent
{
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
}