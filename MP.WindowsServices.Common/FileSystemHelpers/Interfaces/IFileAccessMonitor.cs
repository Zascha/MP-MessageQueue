using System.Threading.Tasks;

namespace MP.WindowsServices.Common.FileSystemHelpers.Interfaces
{
    public interface IFileAccessMonitor
    {
        bool IsFileIsReadyForAccess(string fileName);
    }
}
