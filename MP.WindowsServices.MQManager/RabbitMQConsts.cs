namespace MP.WindowsServices.MQManager
{
    public enum ExchangeType { Direct, Topic, Headers, Fanout }

    public class RabbitMQConsts
    {
        public const string Host = "localhost";
        public const int FilePatchSize = 10000;
    }

    public class RabbitMQExchangeConsts
    {
        public const string ExchangeBatchesToProceedName = "BatchesToProceed";
        public const string ExchangeServiceStateInfoName = "ServiceStateInfo";
    }
}
