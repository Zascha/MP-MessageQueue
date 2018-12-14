using System;
using System.Linq;
using MP.WindowsServices.Common.Logger;
using MP.WindowsServices.Common.Serializer;
using PostSharp.Aspects;

namespace MP.WindowsServices.AOP
{
    [Serializable]
    public class LogMethodInfoAspect : OnMethodBoundaryAspect
    {
        private ILogger _logger;
        private ISerializer _serializer;

        public override void OnEntry(MethodExecutionArgs args)
        {
            var loggingData = new LoggingMethodInputData
            {
                MethodCallingTime = DateTime.Now,
                MethodName = args.Method.Name,
                MethodPassedParameters = string.Join(";", args.Arguments.AsEnumerable())
            };

            LogMethodData(loggingData);
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            LogMethodData(args.ReturnValue);
        }

        #region Private methods

        private void LogMethodData<T>(T data)
        {
            if (_logger == null)
            {
                _logger = new Logger();
            }

            if (_serializer == null)
            {
                _serializer = new JsonSerializer();
            }

            var message = _serializer.Serialize(data);

            _logger.LogInfo(message);
        }

        #endregion
    }
}
