using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace MP.WindowsServices.MQManager
{
    public class RabbitMQSubscriber<T> : ISubscriber<T>
    {
        private readonly string _exchangeName;

        public RabbitMQSubscriber(string exchangeName)
        {
            _exchangeName = !string.IsNullOrEmpty(exchangeName) ? exchangeName : throw new ArgumentNullException(nameof(exchangeName));
        }

        public void Receive(Action<T> procceddMessage)
        {
            ReceiveAsync(procceddMessage).Wait();
        }

        public Task ReceiveAsync(Action<T> procceddMessage)
        {
            return Task.Run(() =>
            {
                var factory = new ConnectionFactory() { HostName = RabbitMQConsts.Host };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    var queueName = channel.QueueDeclare().QueueName;
                    channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Fanout.ToString().ToLower());
                    channel.QueueBind(queue: queueName, exchange: _exchangeName, routingKey: "");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var message = GetFilePatchMessage(ea.Body);
                        procceddMessage(message);
                    };

                    channel.BasicConsume(queue: queueName,
                                         autoAck: true,
                                         consumer: consumer);
                }
            });
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
