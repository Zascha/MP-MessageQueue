using System;
using MP.WindowsServices.Common.Logger;
using PostSharp.Aspects;

namespace MP.WindowsServices.AOP
{
    [Serializable]
    public class LogMethodExceptionsAspect : OnExceptionAspect
    {
        private ILogger _logger;

        public Type HandlingExceptionType { private get; set; }
        public string HandlingExceptionMessage { private get; set; }
        public LoggingLevel LoggingLevel { private get; set; }

        public override void OnException(MethodExecutionArgs args)
        {
            LogMethodData(args.Exception);

            args.FlowBehavior = FlowBehavior.Continue;

            throw (Exception)Activator.CreateInstance(HandlingExceptionType, 
                                                      new object[] { HandlingExceptionMessage, HandlingExceptionType });
        }

        #region Private methods

        private  const string ExceptionMessage = "Finished with exception: ";

        private void LogMethodData(Exception exception)
        {
            if (_logger == null)
            {
                _logger = new Logger();
            }

            switch (LoggingLevel)
            {
                case LoggingLevel.Info:
                    _logger.LogInfo($"{ExceptionMessage}{exception.Message}"); break;
                case LoggingLevel.Warn:
                    _logger.LogWarn($"{ExceptionMessage}{exception.Message}"); break;
                case LoggingLevel.Error:
                    _logger.LogError(ExceptionMessage, exception); break;
                case LoggingLevel.Fatal:
                    _logger.LogFatal(ExceptionMessage, exception); break;
            }
        }

        #endregion
    }
}
