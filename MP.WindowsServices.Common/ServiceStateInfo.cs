using System;
using System.Reflection;

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
        private static readonly Lazy<ServiceStateInfo> lazy = new Lazy<ServiceStateInfo>(() => new ServiceStateInfo());

        public static ServiceStateInfo Instance { get { return lazy.Value; } }

        private ServiceStateInfo()
        {
            ServiceGuid = Assembly.GetExecutingAssembly().GetType().GUID.ToString();
            ServiceSendTimeoutLimit = 5000;
        }

        public string ServiceGuid { get; }

        public int ServiceSendTimeoutLimit { get; private set; }

        public ServiceState ServiceState { get; private set; }

        public void UpdateState(ServiceState state)
        {
            ServiceState = state;
        }

        public void UpdateSendTimeoutLimit(int timeoutLimit)
        {
            if (timeoutLimit < 0)
                throw new ArgumentException("Negative timeout limit.");

            if(timeoutLimit != 0)
            {
                ServiceSendTimeoutLimit = timeoutLimit;
            }
        }
    }
}