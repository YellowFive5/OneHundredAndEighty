#region Usings

using System.IO;

#endregion

namespace OneHundredAndEightyCore.Common
{
    public class FileSystemService : IFileSystemService
    {
        public bool CheckFileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public void CreateFileCopy(string sourceFileName,
                                   string destinationFileName,
                                   bool withOverride)
        {
            File.Copy(sourceFileName,
                      destinationFileName,
                      withOverride);
        }

        public void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }
    }
}