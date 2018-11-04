using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Targets;

namespace MP.WindowsServices.ServiceInstance
{
    internal static class LoggerFactory
    {
        private static LogFactory _instance;

        public static LogFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    var config = new NLog.Config.LoggingConfiguration();

                    var fileTarget = new FileTarget()
                    {
                        Name = "ServiceLogs",
                        FileName = $"log_{ DateTime.UtcNow.ToString("dd-MM-yyyy-HH-mm-ss") }.txt",
                        Layout = "${date} ${message} ${onexception:inner=${exception:format=toString}}"
                    };

                    config.AddTarget(fileTarget);
                    config.AddRuleForAllLevels(fileTarget);

                    _instance = new LogFactory(config);
                }
                return _instance;
            }
        }
    }
}
