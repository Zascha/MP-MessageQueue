using MP.WindowsServices.MQManager.Messages;

namespace MP.WindowsServices.CentralServerListener
{
    public interface ISettingsManager
    {
        void StartListening();

        void SendNewSettings(UpdateStateInfoMessage settings);
    }
}
