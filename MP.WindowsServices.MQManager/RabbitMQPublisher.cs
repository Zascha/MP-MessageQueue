using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace MP.WindowsServices.MQManager
{
    public class RabbitMQPublisher<T> : IPublisher<T>
    {
        private readonly string _exchangeName;

        public RabbitMQPublisher(string exchangeName)
        {
            _exchangeName = !string.IsNullOrEmpty(exchangeName) ? exchangeName : throw new ArgumentNullException(nameof(exchangeName));
        }

        public void Publish(T message)
        {
            PublishAsync(message).Wait();
        }

        public Task PublishAsync(T message)
        {
            return Task.Run(() =>
            {
                var factory = new ConnectionFactory() { HostName = RabbitMQConsts.Host};
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Fanout.ToString().ToLower());

                    var body = Encoding.UTF8.GetBytes(SerializeFilePatchMessage(message));

                    channel.BasicPublish(exchange: _exchangeName, routingKey: "", basicProperties: null, body: body);
                }
            });
        }

        #region Private methods

        private string SerializeFilePatchMessage(T message)
        {
            return JsonConvert.SerializeObject(message);
        }

        #endregion
    }
}
