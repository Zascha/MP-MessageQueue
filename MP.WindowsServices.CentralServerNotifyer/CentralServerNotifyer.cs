using MP.WindowsServices.Common;
using MP.WindowsServices.MQManager;
using System;
using System.Timers;

namespace MP.WindowsServices.CentralServerNotify
{
    public class CentralServerNotifyer : ICentralServerNotifyer
    {
        private readonly IPublisher<ServiceStateInfo> _publisher;
        private Timer _observingTimer;

        public CentralServerNotifyer(IPublisher<ServiceStateInfo> publisher)
        {
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            _observingTimer = new Timer();
            _observingTimer.Interval = ServiceStateInfo.GetInstance().ServiceStateSendTimeoutLimit;
            _observingTimer.Start();
        }

        public void StartNotify()
        {
            _observingTimer.Elapsed += OnTimerElapsed;
        }

        public void StopNotify()
        {
            _observingTimer.Elapsed -= OnTimerElapsed;
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
            _publisher.Publish(ServiceStateInfo.GetInstance());
        }

        #endregion

    }
}
