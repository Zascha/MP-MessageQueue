using MP.WindowsServices.Common;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using MP.WindowsServices.Common.FileSystemHelpers.Interfaces;
using MP.WindowsServices.ImagesManager.Interfaces;

namespace MP.WindowsServices.ImagesManager.ImagesBatchHandlers
{
    public class PdfImagesBatchHandler : IImagesBatchHandler
    {
        private const string PdfDocumentsDirectoryName = "TempPdfDocuments";
        private const string PdfExtention = ".pdf";

        private readonly IPdfGenerator _pdfGenerator;
        private readonly Regex _onlyLettersRegex;
        private readonly IFileSystemHelper _fileSystemHelper;

        public PdfImagesBatchHandler(IFileSystemHelper fileSystemHelper, IPdfGenerator pdfGenerator)
        {
            _fileSystemHelper = fileSystemHelper ?? throw new ArgumentNullException(nameof(fileSystemHelper));
            _pdfGenerator = pdfGenerator ?? throw new ArgumentNullException(nameof(pdfGenerator));

            _onlyLettersRegex = new Regex("[a-zA-Zа-яА-ЯёЁ]+", RegexOptions.Compiled);
        }

        public event EventHandler<FileStoragePipelineEventArgs> StepExecuted;

        public void HandlePreviousStepResult(object sender, FileStoragePipelineEventArgs args)
        {
            if (args.BatchFilePaths == null)
                throw new ArgumentNullException(nameof(args.BatchFilePaths));

            if (!args.BatchFilePaths.Any())
                throw new ArgumentException(nameof(args.BatchFilePaths), "The pathed batch contains no files.");

            ServiceStateInfo.GetInstance().ServiceState = ServiceState.IsHandlingBatch;

            foreach (var file in args.BatchFilePaths)
            {
                _pdfGenerator.AddImageToDocument(file);
            }

            var outputFilePath = GetOutputPath(args.BatchFilePaths.First());
            _pdfGenerator.SavePdf(outputFilePath);

            args.FilePath = outputFilePath;

            OnStepExecuted(this, args);
        }

        #region Private methods

        private string GetOutputPath(string filePath)
        {
            var fileDirectory = _fileSystemHelper.FileHelper.GetFileDirectory(filePath);
            var fileName = _onlyLettersRegex.Match(_fileSystemHelper.FileHelper.GetFileName(filePath)).Value.Trim();

            var pdfDirectoryName = Path.Combine(fileDirectory, PdfDocumentsDirectoryName);
            var pdfDocName = $"{fileName}_{DateTime.UtcNow.ToBinary()}{PdfExtention}";

            _fileSystemHelper.DirectoryHelper.CreateDirectoryIfNotExists(pdfDirectoryName);

            return Path.Combine(pdfDirectoryName, pdfDocName);
        }

        private void OnStepExecuted(object sender, FileStoragePipelineEventArgs e)
        {
            StepExecuted?.Invoke(this, e);
        }

        #endregion
    }
}
