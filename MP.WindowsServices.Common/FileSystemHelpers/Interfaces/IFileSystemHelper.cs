namespace MP.WindowsServices.Common.FileSystemHelpers.Interfaces
{
    public interface IFileSystemHelper
    {
        IFileAccessMonitor FileAccessMonitor { get; }

        IFileHelper FileHelper { get; }

        IDirectoryHelper DirectoryHelper { get; }
    }
}
