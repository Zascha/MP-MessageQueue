using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using System;

namespace MP.WindowsServices.ImagesManager.ImagesBatchHandlers
{
    public class MigraDocPdfGenerator : IPdfGenerator
    {
        private const string MigraDocFilenamePrefix = "base64:";

        private readonly Document _pdfDocument;
        private readonly PdfDocumentRenderer _pdfRenderer;

        public MigraDocPdfGenerator()
        {
            _pdfDocument = new Document();
            _pdfRenderer = new PdfDocumentRenderer(true, PdfFontEmbedding.Always);
        }

        public string PdfFilenamePrefix { get { return MigraDocFilenamePrefix; } }

        public void AddImageToDocument(string image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            _pdfDocument.AddSection().AddImage(image);
        }

        public void SavePdf(string outputPath)
        {
            _pdfRenderer.Document = _pdfDocument;
            _pdfRenderer.RenderDocument();
            _pdfRenderer.PdfDocument.Save(outputPath);
        }

        private string GetMigraDocImageFileName(byte[] image)
        {
            return $"{MigraDocFilenamePrefix}{Convert.ToBase64String(image)}";
        }
    }
}
