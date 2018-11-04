using System;
using System.Collections.Generic;
using MP.WindowsServices.FileStorageObserver.Interfaces;
using MP.WindowsServices.ImagesManager.Interfaces;
using MP.WindowsServices.ProcessingBuilder.Extentions;

namespace MP.WindowsServices.ProcessingBuilder
{
    public class FileStorageWorkflowBuilder : IFileStorageWorkflowBuilder
    {
        private readonly IFileStorageObserver _fileStorageObserver;
        private readonly IImagesBatchProvider _imagesBatchProvider;
        private readonly IImagesBatchCleaner _imagesBatchFilesCleaner;

        public FileStorageWorkflowBuilder(IFileStorageObserver fileStorageObserver,
                                          IImagesBatchProvider imagesBatchProvider,
                                          IImagesBatchCleaner imagesBatchFilesCleaner)
        {
            _fileStorageObserver = fileStorageObserver ?? throw new ArgumentNullException(nameof(fileStorageObserver));
            _imagesBatchProvider = imagesBatchProvider ?? throw new ArgumentNullException(nameof(imagesBatchProvider));
            _imagesBatchFilesCleaner = imagesBatchFilesCleaner ?? throw new ArgumentNullException(nameof(imagesBatchFilesCleaner));
        }

        public void StartProcessing(IEnumerable<WorkflowStepChain> procceedChains)
        {
            _fileStorageObserver.FileAdded += _imagesBatchProvider.HandlePreviousStepResult;

            foreach (var chain in procceedChains)
            {
                _imagesBatchProvider.StepExecuted += chain.ExecutorsChain.First.Value.HandlePreviousStepResult;
                chain.ExecutorsChain.Last.Value.StepExecuted += _imagesBatchFilesCleaner.HandlePreviousStepResult;
            }

            _fileStorageObserver.ObserverAndProceedExistingFiles();
        }
    }
}
