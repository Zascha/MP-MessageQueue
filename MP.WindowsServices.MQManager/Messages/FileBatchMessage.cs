namespace MP.WindowsServices.MQManager.Messages
{
    public class FileBatchMessage
    {
        public string FileName { get; set; }

        public int FileDataHash { get; set; }

        public int CurrentFilePatchNumber { get; set; }

        public int TotalFilePatchNumber { get; set; }

        public byte[] FilePatchData { get; set; }
    }
}
