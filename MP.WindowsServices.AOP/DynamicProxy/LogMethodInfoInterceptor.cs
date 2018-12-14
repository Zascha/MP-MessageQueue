using System;
using Castle.DynamicProxy;
using MP.WindowsServices.Common.Logger;
using MP.WindowsServices.Common.Serializer;

namespace MP.WindowsServices.AOP
{
    public class LogMethodInfoInterceptor : IInterceptor
    {
        private readonly ILogger _logger;
        private readonly ISerializer _serializer;

        public LogMethodInfoInterceptor(ILogger logger, ISerializer serializer)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        public void Intercept(IInvocation invocation)
        {
            var loggingInputData = new LoggingMethodInputData
            {
                MethodCallingTime = DateTime.Now,
                MethodName = invocation.Method.Name,
                MethodPassedParameters = string.Join(";", invocation.Arguments)
            };

            var loggingData = new LoggingMethodData
            {
                MethodInputData = loggingInputData,
                MethodReturnValue = invocation.ReturnValue
            };

            LogMethodData(loggingData);
        }

        #region Private methods

        private void LogMethodData<T>(T data)
        {
            var message = _serializer.Serialize(data);

            _logger.LogInfo(message);
        }

        #endregion
    }
}
