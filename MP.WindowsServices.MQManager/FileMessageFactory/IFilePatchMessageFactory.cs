using MP.WindowsServices.MQManager.Messages;
using System.Collections.Generic;

namespace MP.WindowsServices.MQManager.FileMessageFactory
{
    public interface IFilePatchMessageFactory
    {
        IEnumerable<FileBatchMessage> GetFilePatchMessages(string filePath);
    }
}
