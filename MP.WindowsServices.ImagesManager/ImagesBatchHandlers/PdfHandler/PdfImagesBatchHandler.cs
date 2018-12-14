using MP.WindowsServices.Common;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using MP.WindowsServices.Common.FileSystemHelpers.Interfaces;
using MP.WindowsServices.ImagesManager.Interfaces;
using MP.WindowsServices.Common.SafeExecuteManagers;

namespace MP.WindowsServices.ImagesManager.ImagesBatchHandlers
{
    public class PdfImagesBatchHandler : IImagesBatchHandler
    {
        private const string PdfExtention = ".pdf";

        private readonly ISafeExecuteManager _safeExecuteManager;
        private readonly IPdfGenerator _pdfGenerator;
        private readonly Regex _onlyLettersRegex;
        private readonly IFileSystemHelper _fileSystemHelper;

        public PdfImagesBatchHandler(ISafeExecuteManager safeExecuteManager, IFileSystemHelper fileSystemHelper, IPdfGenerator pdfGenerator)
        {
            _safeExecuteManager = safeExecuteManager ?? throw new ArgumentNullException(nameof(safeExecuteManager));
            _fileSystemHelper = fileSystemHelper ?? throw new ArgumentNullException(nameof(fileSystemHelper));
            _pdfGenerator = pdfGenerator ?? throw new ArgumentNullException(nameof(pdfGenerator));

            _onlyLettersRegex = new Regex("[a-zA-Zа-яА-ЯёЁ]+", RegexOptions.Compiled);
        }

        public event EventHandler<FileStoragePipelineEventArgs> StepExecuted;

        public virtual void HandlePreviousStepResult(object sender, FileStoragePipelineEventArgs args)
        {
            if (args.BatchFilePaths == null)
                throw new ArgumentNullException(nameof(args.BatchFilePaths));

            if (!args.BatchFilePaths.Any())
                throw new ArgumentException(nameof(args.BatchFilePaths), "The pathed batch contains no files.");

            ServiceStateInfo.Instance.UpdateState(ServiceState.IsHandlingBatch);

            _safeExecuteManager.ExecuteWithExceptionLogging(() =>
            {
                foreach (var file in args.BatchFilePaths)
                {
                    _pdfGenerator.AddImageToDocument(file);
                }

                var outputFilePath = GetOutputPath(args.BatchFilePaths.First());
                _pdfGenerator.SavePdf(outputFilePath);

                args.FilePath = outputFilePath;

                OnStepExecuted(this, args);
            });
        }

        #region Private methods

        private string GetOutputPath(string filePath)
        {
            var fileDirectory = _fileSystemHelper.FileHelper.GetFileDirectory(filePath);
            var fileName = _onlyLettersRegex.Match(_fileSystemHelper.FileHelper.GetFileName(filePath)).Value.Trim();
            var pdfDocName = $"{fileName}_{DateTime.UtcNow.ToBinary()}{PdfExtention}";

            return pdfDocName;
        }

        private void OnStepExecuted(object sender, FileStoragePipelineEventArgs e)
        {
            StepExecuted?.Invoke(this, e);
        }

        #endregion
    }
}
