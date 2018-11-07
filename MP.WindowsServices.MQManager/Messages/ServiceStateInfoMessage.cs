using MP.WindowsServices.Common;
using System;

namespace MP.WindowsServices.MQManager.Messages
{
    public class ServiceStateInfoMessage
    {
        public ServiceStateInfo ServiceStateInfo { get; set; }

        public DateTime StateTime { get; set; }
    }
}
