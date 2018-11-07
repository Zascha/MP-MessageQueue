using MP.WindowsServices.Common;
using MP.WindowsServices.Common.FileSystemHelpers.Interfaces;
using MP.WindowsServices.Common.SafeExecuteManagers;
using MP.WindowsServices.ImagesManager.Interfaces;
using System;
using System.Linq;

namespace MP.WindowsServices.ImagesManager.ImagesBatchCleaner
{
    public class ImagesBatchFilesCleaner : IImagesBatchCleaner
    {
        private readonly ISafeExecuteManager _safeExecuteManager;
        private readonly IFileSystemHelper _fileSystemHelper;

        public ImagesBatchFilesCleaner(ISafeExecuteManager safeExecuteManager, IFileSystemHelper fileSystemHelper)
        {
            _safeExecuteManager = safeExecuteManager ?? throw new ArgumentNullException(nameof(safeExecuteManager));
            _fileSystemHelper = fileSystemHelper ?? throw new ArgumentNullException(nameof(fileSystemHelper));
        }

        public event EventHandler<FileStoragePipelineEventArgs> StepExecuted;

        public void HandlePreviousStepResult(object sender, FileStoragePipelineEventArgs args)
        {
            if (args.BatchFilePaths == null)
                throw new ArgumentNullException(nameof(args.BatchFilePaths));

            if (!args.BatchFilePaths.Any())
                throw new ArgumentException(nameof(args.BatchFilePaths), "The pathed batch contains no files.");

            ServiceStateInfo.Instance.UpdateState(ServiceState.IsCleaningBatchFiles);

            _safeExecuteManager.ExecuteWithExceptionLogging(() =>
            {
                foreach (var file in args.BatchFilePaths)
                {
                    _fileSystemHelper.FileHelper.DeleteFile(file);
                }
            });
        }

        #region Private methods

        private void OnStepExecuted(object sender, FileStoragePipelineEventArgs e)
        {
            StepExecuted?.Invoke(this, e);
        }

        #endregion
    }
}
