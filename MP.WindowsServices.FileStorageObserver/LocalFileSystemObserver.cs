using System;
using System.Collections.Generic;
using System.IO;
using MP.WindowsServices.AOP;
using MP.WindowsServices.Common;
using MP.WindowsServices.Common.FileSystemHelpers.Interfaces;
using MP.WindowsServices.Common.SafeExecuteManagers;
using MP.WindowsServices.FileStorageObserver.Helpers;
using MP.WindowsServices.FileStorageObserver.Interfaces;

namespace MP.WindowsServices.FileStorageObserver
{
    public class LocalFileSystemObserver : IFileStorageObserver
    {
        private readonly ISafeExecuteManager _safeExecuteManager;
        private readonly IFileSystemHelper _fileSystemHelper;
        private readonly AppConfigHelper _appConfigHelper;

        private List<FileSystemWatcher> _fileSystemWatchers;

        public event EventHandler<FileStoragePipelineEventArgs> FileAdded;

        public LocalFileSystemObserver(ISafeExecuteManager safeExecuteManager, IFileSystemHelper fileSystemHelper)
        {
            _safeExecuteManager = safeExecuteManager ?? throw new ArgumentNullException(nameof(safeExecuteManager));
            _fileSystemHelper = fileSystemHelper ?? throw new ArgumentNullException(nameof(fileSystemHelper));

            _appConfigHelper = new AppConfigHelper();

            InitFileSystemWatchersDictionary();
        }

        public void ObserverAndProceedExistingFiles()
        {
            ServiceStateInfo.Instance.UpdateState(ServiceState.IsWaitingForNewFiles);

            foreach (var path in _appConfigHelper.ObservableFolders)
            {
                foreach (var file in Directory.EnumerateFileSystemEntries(path))
                {
                    OnFileAdded(this, new FileSystemEventArgs(WatcherChangeTypes.Created, path, _fileSystemHelper.FileHelper.GetFileName(file)));
                }
            }
        }

        public void StopObserving()
        {
            foreach (var watcher in _fileSystemWatchers)
            {
                watcher.EnableRaisingEvents = false;
            }
        }

        #region Private methods

        /// <summary>
        /// Initializes FileSystemWatchers with paths enumerated in App.config file in observableFolders section
        /// </summary>
        private void InitFileSystemWatchersDictionary()
        {
            ServiceStateInfo.Instance.UpdateState(ServiceState.IsWaitingForNewFiles);

            _fileSystemWatchers = new List<FileSystemWatcher>();

            _safeExecuteManager.ExecuteWithExceptionLogging(() =>
            {
                foreach (var path in _appConfigHelper.ObservableFolders)
                {
                    if (!_fileSystemHelper.DirectoryHelper.IsValidDirectoryPath(path))
                        throw new ArgumentException($"Invalid directory path: {path}");

                    _fileSystemHelper.DirectoryHelper.CreateDirectoryIfNotExists(path);

                    var watcher = new FileSystemWatcher(path);
                    watcher.EnableRaisingEvents = true;
                    watcher.Created += OnFileAdded;

                    _fileSystemWatchers.Add(watcher);
                }
            });
        }

        private void OnFileAdded(object sender, FileSystemEventArgs e)
        {
            if (_appConfigHelper.FileNameRegex.IsMatch(e.Name))
            {
                FileAdded?.Invoke(this, new FileStoragePipelineEventArgs() { FilePath = e.FullPath });
            }
        }

        #endregion
    }
}
