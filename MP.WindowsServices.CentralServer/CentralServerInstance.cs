using Autofac;
using MP.WindowsServices.CentralServerListener;
using MP.WindowsServices.MQManager;
using MP.WindowsServices.MQManager.Messages;
using System.Timers;

namespace MP.WindowsServices.CentralServer
{
    internal class CentralServerInstance
    {
        private readonly ILifetimeScope _scope;
        private Timer _settingsUpdatingTimer;

        public CentralServerInstance()
        {
            _scope = DependencyResolver.DependencyResolver.Container.BeginLifetimeScope();

            _settingsUpdatingTimer = new Timer();
            _settingsUpdatingTimer.Interval = 10000;
            _settingsUpdatingTimer.Elapsed += SendNewSettings;
        }

        public void Start()
        {
            RunService();
        }

        public void Stop()
        {
            StopService();
            _scope.Dispose();
        }

        #region Private methods

        private void RunService()
        {
            _settingsUpdatingTimer.Start();
            _scope.Resolve<IImagesBatchSubscriber>().StartListening();
            _scope.Resolve<ISettingsManager>().StartListening();
        }

        private void StopService()
        {
            _scope.Resolve<RabbitMQChannel>().Dispose();
            _settingsUpdatingTimer.Stop();
        }

        private void SendNewSettings(object source, ElapsedEventArgs e)
        {
            _scope.Resolve<ISettingsManager>().SendNewSettings(new UpdateStateInfoMessage
            {
                ServiceSendTimeoutLimit = 7000
            });
        }

        #endregion
    }
}
