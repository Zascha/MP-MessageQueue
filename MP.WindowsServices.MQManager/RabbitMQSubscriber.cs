using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace MP.WindowsServices.MQManager
{
    public class RabbitMQSubscriber<T> : ISubscriber<T>
    {
        private readonly string _exchangeName;
        private readonly RabbitMQChannel _channel;

        public RabbitMQSubscriber(RabbitMQChannel channel, string exchangeName)
        {
            _channel = channel ?? throw new ArgumentNullException(nameof(channel));
            _exchangeName = !string.IsNullOrEmpty(exchangeName) ? exchangeName : throw new ArgumentNullException(nameof(exchangeName));
        }

        public void Receive(Action<T> procceddMessage)
        {
            var queueName = _channel.Channel.QueueDeclare().QueueName;
            _channel.Channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Fanout.ToString().ToLower());
            _channel.Channel.QueueBind(queue: queueName, exchange: _exchangeName, routingKey: "");

            var consumer = new EventingBasicConsumer(_channel.Channel);
            consumer.Received += (model, ea) =>
            {
                var message = GetFilePatchMessage(ea.Body);
                procceddMessage(message);
            };

            _channel.Channel.BasicConsume(queue: queueName,
                                          autoAck: true,
                                          consumer: consumer);

            while (!_channel.IsDisposed);
        }

        #region Private methods

        private T GetFilePatchMessage(byte[] messageBytes)
        {
            var messageString = Encoding.UTF8.GetString(messageBytes);

            return JsonConvert.DeserializeObject<T>(messageString);
        }

        #endregion
    }
}
