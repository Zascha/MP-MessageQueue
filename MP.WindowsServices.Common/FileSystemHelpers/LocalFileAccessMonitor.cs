using MP.WindowsServices.Common.FileSystemHelpers.Interfaces;
using System;
using System.IO;

namespace MP.WindowsServices.Common.FileSystemHelpers
{
    internal class LocalFileAccessMonitor : IFileAccessMonitor
    {
        public bool IsFileIsReadyForAccess(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            var attemptNumber = 3;
            while (attemptNumber > 0)
            {
                try
                {
                    var fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
                    fileStream.Close();

                    return true;
                }
                catch (IOException)
                {
                    attemptNumber--;
                }
            }

            return false;
        }
    }
}
