namespace FileMonitor.ScanningAgent
{
    public interface IHashGenerator
    {
        string GenerateHash(string filePath);
    }
}