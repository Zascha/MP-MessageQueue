using MP.WindowsServices.Common.Logger;
using MP.WindowsServices.Common.SafeExecuteManagers;
using MP.WindowsServices.MQManager;
using MP.WindowsServices.MQManager.Messages;
using System;

namespace MP.WindowsServices.CentralServerListener
{
    public class SettingsManager : ISettingsManager
    {
        private readonly ISafeExecuteManager _safeExecuteManager;
        private readonly ILogger _logger;
        private readonly IPublisher<UpdateStateInfoMessage> _publisher;
        private readonly ISubscriber<ServiceStateInfoMessage> _subscriber;

        public SettingsManager(ISubscriber<ServiceStateInfoMessage> subscriber,
                               IPublisher<UpdateStateInfoMessage> publisher,
                               ILogger logger,
                               ISafeExecuteManager safeExecuteManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _subscriber = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            _safeExecuteManager = safeExecuteManager ?? throw new ArgumentNullException(nameof(safeExecuteManager));
        }

        public void StartListening()
        {
            _safeExecuteManager.ExecuteWithExceptionLogging(() =>
            {
                _subscriber.Receive(LogServiceState);
            });
        }

        public void SendNewSettings(UpdateStateInfoMessage settings)
        {
            _safeExecuteManager.ExecuteWithExceptionLogging(() =>
            {
                _publisher.Publish(settings);
            });
        }

        #region Private methods

        private void LogServiceState(ServiceStateInfoMessage message)
        {
            var info = $"Service {message.ServiceStateInfo.ServiceGuid} ({message.StateTime}): {message.ServiceStateInfo.ServiceState} ";

            _logger.LogInfo(info);
        }

        #endregion
    }
}
