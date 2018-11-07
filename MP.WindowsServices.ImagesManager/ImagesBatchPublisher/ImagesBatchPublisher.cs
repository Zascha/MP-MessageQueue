using MP.WindowsServices.Common;
using MP.WindowsServices.Common.FileSystemHelpers.Interfaces;
using MP.WindowsServices.Common.SafeExecuteManagers;
using MP.WindowsServices.ImagesManager.Interfaces;
using MP.WindowsServices.MQManager;
using MP.WindowsServices.MQManager.FileMessageFactory;
using MP.WindowsServices.MQManager.Messages;
using System;

namespace MP.WindowsServices.ImagesManager.ImagesBatchPublisher
{
    public class ImagesBatchPublisher : IImagesBatchPublisher
    {
        private readonly ISafeExecuteManager _safeExecuteManager;
        private readonly IPublisher<FileBatchMessage> _publisher;
        private readonly IFilePatchMessageFactory _filePatchMessageFactory;
        private readonly IFileSystemHelper _fileSystemHelper;

        public ImagesBatchPublisher(ISafeExecuteManager safeExecuteManager,
                                    IPublisher<FileBatchMessage> publisher,
                                    IFilePatchMessageFactory filePatchMessageFactory,
                                    IFileSystemHelper fileSystemHelper)
        {
            _safeExecuteManager = safeExecuteManager ?? throw new ArgumentNullException(nameof(safeExecuteManager));
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            _filePatchMessageFactory = filePatchMessageFactory ?? throw new ArgumentNullException(nameof(filePatchMessageFactory));
            _fileSystemHelper = fileSystemHelper ?? throw new ArgumentNullException(nameof(fileSystemHelper));
        }

        public event EventHandler<FileStoragePipelineEventArgs> StepExecuted;

        public void HandlePreviousStepResult(object sender, FileStoragePipelineEventArgs args)
        {
            ServiceStateInfo.Instance.UpdateState(ServiceState.IsPublishungBatch);

            _safeExecuteManager.ExecuteWithExceptionLogging(() =>
            {
                if (_fileSystemHelper.FileAccessMonitor.IsFileIsReadyForAccess(args.FilePath))
                {
                    foreach (var fileMessage in _filePatchMessageFactory.GetFilePatchMessages(args.FilePath))
                    {
                        _publisher.Publish(fileMessage);
                    }

                    _fileSystemHelper.FileHelper.DeleteFile(args.FilePath);
                }

                OnStepExecuted(this, args);
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
