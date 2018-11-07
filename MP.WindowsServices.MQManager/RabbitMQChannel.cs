using RabbitMQ.Client;
using System;

namespace MP.WindowsServices.MQManager
{
    public class RabbitMQChannel : IDisposable
    {
        public IModel Channel { get; private set; }

        public bool IsDisposed { get; private set; }

        private readonly IConnection _connection;

        public RabbitMQChannel()
        {
            var factory = new ConnectionFactory() {HostName = RabbitMQConsts.Host, ContinuationTimeout = TimeSpan.FromMinutes(10)};
            _connection = factory.CreateConnection();
            Channel = _connection.CreateModel();
        }

        public void Dispose()
        {
            Channel.Dispose();
            _connection.Dispose();

            IsDisposed = true;
        }
    }
}
