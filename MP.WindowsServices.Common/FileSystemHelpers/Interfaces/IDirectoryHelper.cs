namespace MP.WindowsServices.Common.FileSystemHelpers.Interfaces
{
    public interface IDirectoryHelper
    {
        bool IsValidDirectoryPath(string directoryPath);

        bool DoesDirectoryExist(string directoryPath);

        void CreateDirectoryIfNotExists(string directoryPath);
    }
}
