using MP.WindowsServices.Common.Logger;
using System;
using System.Threading.Tasks;

namespace MP.WindowsServices.Common.SafeExecuteManagers
{
    public class SafeExecuteManager : ISafeExecuteManager
    {
        private readonly ILogger _logger;

        public SafeExecuteManager(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void ExecuteWithExceptionLogging(Action action)
        {
            try
            {
                action();
            }
            catch (Exception exception)
            {
                _logger.LogError("Finished with exception.", exception);
            }
        }

        public void ExecuteWithExceptionLogging(Func<Task> func)
        {
            try
            {
                func().Wait();
            }
            catch (Exception exception)
            {
                _logger.LogError("Finished with exception.", exception);
            }
        }
    }
}
