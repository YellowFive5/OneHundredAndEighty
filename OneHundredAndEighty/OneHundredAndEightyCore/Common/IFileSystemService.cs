namespace OneHundredAndEightyCore.Common
{
    public interface IFileSystemService
    {
        bool CheckFileExists(string filePath);
        void CreateFileCopy(string sourceFileName, string destinationFileName, bool withOverride);
        void DeleteFile(string filePath);
    }
}