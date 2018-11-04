using MP.WindowsServices.Common.FileSystemHelpers.Interfaces;

namespace MP.WindowsServices.Common.FileSystemHelpers
{
    public class LocalFileSystemHelper : IFileSystemHelper
    {
        private IFileAccessMonitor _fileAccessMonitor;
        public IFileAccessMonitor FileAccessMonitor
        {
            get
            {
                if(_fileAccessMonitor == null)
                {
                    _fileAccessMonitor = new LocalFileAccessMonitor();
                }

                return _fileAccessMonitor;
            }
        }

        private IFileHelper _fileHelper;
        public IFileHelper FileHelper
        {
            get
            {
                if (_fileHelper == null)
                {
                    _fileHelper = new LocalFileHelper();
                }

                return _fileHelper;
            }
        }

        private IDirectoryHelper _directoryHelper;
        public IDirectoryHelper DirectoryHelper
        {
            get
            {
                if (_directoryHelper == null)
                {
                    _directoryHelper = new LocalDirectoryHelper();
                }

                return _directoryHelper;
            }
        }
    }
}
