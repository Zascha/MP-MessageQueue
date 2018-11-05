namespace MP.WindowsServices.Common.FileSystemHelpers.Interfaces
{
    public interface IFileHelper
    {
        bool IsValidFilePath(string filePath);

        string GetFileName(string filePath);

        string GetFileExtention(string filePath);

        string GetFileDirectory(string filePath);

        byte[] GetFileBytes(string filePath);

        void WriteBytesToFile(string filePath, byte[] fileData);

        void DeleteFile(string filePath);
    }
}
