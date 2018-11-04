using MP.WindowsServices.Common;
using System;

namespace MP.WindowsServices.FileStorageObserver.Interfaces
{
    public interface IFileStorageObserver
    {
        event EventHandler<FileStoragePipelineEventArgs> FileAdded;

        void ObserverAndProceedExistingFiles();

        void StopObserving();
    }
}
