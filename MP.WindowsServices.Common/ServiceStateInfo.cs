namespace MP.WindowsServices.Common
{
    public enum ServiceState
    {
        UnknownProcess,
        IsWaitingForNewFiles,
        IsProvidingBatch,
        IsHandlingBatch,
        IsPublishungBatch,
        IsCleaningBatchFiles
    }

    public class ServiceStateInfo
    {
        private static ServiceStateInfo _instance = new ServiceStateInfo();

        public static ServiceStateInfo GetInstance()
        {
            return _instance;
        }

        private ServiceStateInfo()
        {
            ServiceStateSendTimeoutLimit = 5000;
        }

        public ServiceState ServiceState { get;  set; }

        public int ServiceStateSendTimeoutLimit { get; set; }
    }
}