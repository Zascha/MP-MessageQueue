using MP.WindowsServices.Common;
using MP.WindowsServices.Common.SafeExecuteManagers;
using MP.WindowsServices.MQManager;
using MP.WindowsServices.MQManager.Messages;
using System;
using System.Timers;

namespace MP.WindowsServices.CentralServerNotify
{
    public class CentralServerNotifyer : ICentralServerNotifyer
    {
        private readonly ISafeExecuteManager _safeExecuteManager;
        private readonly IPublisher<ServiceStateInfoMessage> _publisher;
        private readonly ISubscriber<UpdateStateInfoMessage> _subscriber;
        private Timer _observingTimer;

        public CentralServerNotifyer(ISafeExecuteManager safeExecuteManager,
                                     IPublisher<ServiceStateInfoMessage> publisher,
                                     ISubscriber<UpdateStateInfoMessage> subscriber)
        {
            _safeExecuteManager = safeExecuteManager ?? throw new ArgumentNullException(nameof(safeExecuteManager));
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            _subscriber = subscriber ?? throw new ArgumentNullException(nameof(subscriber));

            _observingTimer = new Timer();
            _observingTimer.Interval = ServiceStateInfo.Instance.ServiceSendTimeoutLimit;
            _observingTimer.Elapsed += OnTimerElapsed;
        }

        public void StartNotify()
        {
            _safeExecuteManager.ExecuteWithExceptionLogging(() =>
            {
                _observingTimer.Start();
                _subscriber.Receive(SetReceivedSettings);
            });
        }

        public void StopNotify()
        {
            _safeExecuteManager.ExecuteWithExceptionLogging(() =>
            {
                _observingTimer.Stop();
            });
        }

        public void SetCentralServerNotifyerTimerLimit(int timerLimitInSeconds)
        {
            if (timerLimitInSeconds < 0)
                throw new ArgumentException("Negative timerLimitInSeconds value.");

            _observingTimer.Interval = timerLimitInSeconds;
        }

        public void SetNextFileAddingLimitInSeconds(int nextFileAddingLimit)
        {
            _observingTimer = new Timer();
            _observingTimer.Interval = nextFileAddingLimit;
            _observingTimer.Enabled = true;
        }

        #region Private methods

        private void OnTimerElapsed(object source, ElapsedEventArgs e)
        {
            _publisher.Publish(new ServiceStateInfoMessage
            {
                ServiceStateInfo = ServiceStateInfo.Instance,
                StateTime = DateTime.UtcNow
            });
        }

        private void SetReceivedSettings(UpdateStateInfoMessage message)
        {
            ServiceStateInfo.Instance.UpdateSendTimeoutLimit(message.ServiceSendTimeoutLimit);
        }

        #endregion
    }
}
