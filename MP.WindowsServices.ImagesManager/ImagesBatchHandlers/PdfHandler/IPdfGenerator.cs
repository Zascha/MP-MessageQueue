namespace MP.WindowsServices.ImagesManager.ImagesBatchHandlers
{
    public interface IPdfGenerator
    {
        void AddImageToDocument(string image);

        void SavePdf(string outputPath);
    }
}
