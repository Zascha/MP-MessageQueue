using System;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace MP.WindowsServices.Common.Logger
{
    public class Logger : ILogger
    {
        private readonly NLog.Logger _logger; 

        public Logger()
        {
            var config = new LoggingConfiguration();
            var fileTarget = new FileTarget(){FileName = "logFile.log", Layout = @"${date:format=HH\:mm\:ss} ${level} ${message} ${exception}" };

            var rule = new LoggingRule("*", LogLevel.Trace, fileTarget);
            config.LoggingRules.Add(rule);

            LogManager.Configuration = config;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public void LogError(string message, Exception exception)
        {
            _logger.Error(exception, message);
        }

        public void LogFatal(string message, Exception exception)
        {
            _logger.Fatal(exception, message);
        }

        public void LogInfo(string message)
        {
            _logger.Info(message);
        }

        public void LogWarn(string message)
        {
            _logger.Warn(message);
        }
    }
}
