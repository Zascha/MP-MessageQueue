using MP.WindowsServices.Common.FileSystemHelpers.Interfaces;
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
        private const string ReceicedFilesDirectory = @"D:\ReceivedFiles";

        private readonly ISubscriber<FileBatchMessage> _subscriber;
        private readonly IFileSystemHelper _fileSystemHelper;

        private Dictionary<int, List<FileBatchMessage>> _filesPatches;

        public ImagesBatchSubscriber(ISubscriber<FileBatchMessage> subscriber, IFileSystemHelper fileSystemHelper)
        {
            _fileSystemHelper = fileSystemHelper ?? throw new ArgumentNullException(nameof(fileSystemHelper));
            _subscriber = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
            _filesPatches = new Dictionary<int, List<FileBatchMessage>>();
        }

        public void StartListening()
        {
            _subscriber.Receive(ProcessReceivedFileBatchMessage);
        }

        #region Private methods

        private void ProcessReceivedFileBatchMessage(FileBatchMessage message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            if (!_filesPatches.ContainsKey(message.FileDataHash))
            {
                _filesPatches.Add(message.FileDataHash, new List<FileBatchMessage>());
            }

            _filesPatches[message.FileDataHash].Add(message);

            if (_filesPatches[message.FileDataHash].Count == message.TotalFilePatchNumber)
            {
                var bytesEnumerable = _filesPatches[message.FileDataHash].OrderBy(patch => patch.CurrentFilePatchNumber).Select(patch => patch.FilePatchData).ToList();

                var bytes = new List<byte>();
                bytesEnumerable.ForEach(item => bytes.AddRange(item));

                _fileSystemHelper.FileHelper.WriteBytesToFile(Path.Combine(ReceicedFilesDirectory, message.FileName), bytes.ToArray());

                _filesPatches.Remove(message.FileDataHash);
            }
        }

        #endregion

    }
}
