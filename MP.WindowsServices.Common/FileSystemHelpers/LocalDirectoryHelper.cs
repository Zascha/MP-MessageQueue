using MP.WindowsServices.Common.FileSystemHelpers.Interfaces;
using System;
using System.IO;

namespace MP.WindowsServices.Common.FileSystemHelpers
{
    internal class LocalDirectoryHelper : IDirectoryHelper
    {
        public bool IsValidDirectoryPath(string directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath))
            {
                return false;
            }

            try
            {
                Path.GetFullPath(directoryPath);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        public bool DoesDirectoryExist(string directoryPath)
        {
            return !string.IsNullOrEmpty(directoryPath) && Directory.Exists(directoryPath);
        }

        public void CreateDirectoryIfNotExists(string directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath))
                throw new ArgumentNullException(nameof(directoryPath));

            if (!DoesDirectoryExist(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
    }
}
