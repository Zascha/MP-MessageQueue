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
        private readonly RabbitMQChannel _channel;

        public RabbitMQPublisher(RabbitMQChannel channel, string exchangeName)
        {
            _channel = channel ?? throw new ArgumentNullException(nameof(channel));
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
                _channel.Channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Fanout.ToString().ToLower());

                var body = Encoding.UTF8.GetBytes(SerializeFilePatchMessage(message));

                _channel.Channel.BasicPublish(exchange: _exchangeName, routingKey: "", basicProperties: null, body: body);

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
