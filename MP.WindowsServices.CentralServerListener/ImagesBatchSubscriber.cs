using MP.WindowsServices.Common.FileSystemHelpers.Interfaces;
using MP.WindowsServices.Common.SafeExecuteManagers;
using MP.WindowsServices.MQManager;
using MP.WindowsServices.MQManager.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MP.WindowsServices.CentralServerListener
{
    public class ImagesBatchSubscriber : IImagesBatchSubscriber
    {
        private const string ReceivedFilesDirectory = @"D:\ReceivedFiles";

        private readonly ISafeExecuteManager _safeExecuteManager;
        private readonly ISubscriber<FileBatchMessage> _subscriber;
        private readonly IFileSystemHelper _fileSystemHelper;

        private Dictionary<int, List<FileBatchMessage>> _filesPatches;

        public ImagesBatchSubscriber(ISafeExecuteManager safeExecuteManager,
                                     ISubscriber<FileBatchMessage> subscriber,
                                     IFileSystemHelper fileSystemHelper)
        {
            _safeExecuteManager = safeExecuteManager ?? throw new ArgumentNullException(nameof(safeExecuteManager));
            _fileSystemHelper = fileSystemHelper ?? throw new ArgumentNullException(nameof(fileSystemHelper));
            _subscriber = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
            _filesPatches = new Dictionary<int, List<FileBatchMessage>>();
        }

        public void StartListening()
        {
            _safeExecuteManager.ExecuteWithExceptionLogging(() =>
            {
                _subscriber.Receive(ProcessReceivedFileBatchMessage);
            });
            
        }

        #region Private methods

        private void ProcessReceivedFileBatchMessage(FileBatchMessage message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            _safeExecuteManager.ExecuteWithExceptionLogging(() =>
            {
                AddMessageToDictionary(message);

                if (HasAllAllPatchesBeenReceived(message))
                {
                    var fileBytes = GetReceivedBytes(message);
                    ProceedReceivedBytes(message, fileBytes);
                }
            });
        }

        private void AddMessageToDictionary(FileBatchMessage message)
        {
            if (!_filesPatches.ContainsKey(message.FileDataHash))
            {
                _filesPatches.Add(message.FileDataHash, new List<FileBatchMessage>());
            }

            _filesPatches[message.FileDataHash].Add(message);
        }

        private bool HasAllAllPatchesBeenReceived(FileBatchMessage message)
        {
            return _filesPatches[message.FileDataHash].Count == message.TotalFilePatchNumber;
        }

        private byte[] GetReceivedBytes(FileBatchMessage message)
        {
            var bytesEnumerable = _filesPatches[message.FileDataHash].OrderBy(patch => patch.CurrentFilePatchNumber)
                                                                     .Select(patch => patch.FilePatchData)
                                                                     .ToList();

            var bytes = new List<byte>();
            bytesEnumerable.ForEach(item => bytes.AddRange(item));

            return bytes.ToArray();
        }

        private void ProceedReceivedBytes(FileBatchMessage message, byte[] fileBytes)
        {
            _fileSystemHelper.FileHelper.WriteBytesToFile(Path.Combine(ReceivedFilesDirectory, message.FileName), fileBytes.ToArray());

            _filesPatches.Remove(message.FileDataHash);
        }

        #endregion

    }
}
