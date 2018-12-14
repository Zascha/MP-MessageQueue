using System;
using Castle.DynamicProxy;
using MP.WindowsServices.Common.Logger;

namespace MP.WindowsServices.AOP
{
    public class LogMethodExceptionsInterceptor : IInterceptor
    {
        private readonly ILogger _logger;

        public LogMethodExceptionsInterceptor(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception exception)
            {
                _logger.LogError($"Finished with exception: {exception.Message}", exception);

                throw;
            }
        }
    }
}
