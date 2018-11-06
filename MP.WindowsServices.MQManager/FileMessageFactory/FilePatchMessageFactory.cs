using MP.WindowsServices.Common.FileSystemHelpers.Interfaces;
using MP.WindowsServices.MQManager.Messages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MP.WindowsServices.MQManager.FileMessageFactory
{
    public class FilePatchMessageFactory : IFilePatchMessageFactory
    {
        private readonly IFileSystemHelper _fileSystemHelper;

        public FilePatchMessageFactory(IFileSystemHelper fileSystemHelper)
        {
            _fileSystemHelper = fileSystemHelper ?? throw new ArgumentNullException(nameof(fileSystemHelper));
        }

        public IEnumerable<FileBatchMessage> GetFilePatchMessages(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));

            var filePatchMessages = new List<FileBatchMessage>();

            if(_fileSystemHelper.FileAccessMonitor.IsFileIsReadyForAccess(filePath))
            {
                var fileBytes = _fileSystemHelper.FileHelper.GetFileBytes(filePath);
                var fileName = _fileSystemHelper.FileHelper.GetFileName(filePath);
                var fileDataHash = fileBytes.GetHashCode();
                var totalPatchNumber = (int)Math.Ceiling((double)fileBytes.Length / RabbitMQConsts.FilePatchSize);

                for(int i=0; i< totalPatchNumber; i++)
                {
                    filePatchMessages.Add(
                    new FileBatchMessage
                    {
                        FileName = fileName,
                        FileDataHash = fileDataHash,
                        TotalFilePatchNumber = totalPatchNumber,
                        CurrentFilePatchNumber = i,
                        FilePatchData = fileBytes.Skip(i * RabbitMQConsts.FilePatchSize).Take(RabbitMQConsts.FilePatchSize).ToArray()
                    });
                }
            }

            return filePatchMessages;
        }
    }
}
