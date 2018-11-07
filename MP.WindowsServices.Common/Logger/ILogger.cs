using System;

namespace MP.WindowsServices.Common.Logger
{
    public interface ILogger
    {
        void LogFatal(string message, Exception exception);

        void LogError(string message, Exception exception);

        void LogWarn(string message);

        void LogInfo(string message);
    }
}
