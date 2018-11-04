namespace MP.WindowsServices.CentralServerNotify
{
    public interface ICentralServerNotifyer
    {
        void StartNotify();

        void StopNotify();

        void SetCentralServerNotifyerTimerLimit(int timerLimitInSeconds);
    }
}
