using System;

namespace MP.WindowsServices.AOP
{
    internal class LoggingMethodInputData
    {
        public DateTime MethodCallingTime { get; set; }

        public string MethodName { get; set; }

        public string MethodPassedParameters { get; set; }
    }

    internal class LoggingMethodData
    {
        public LoggingMethodInputData MethodInputData { get; set; }

        public object MethodReturnValue { get; set; }
    }
}
