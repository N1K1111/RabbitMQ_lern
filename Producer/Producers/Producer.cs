using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Producer.Producers
{
    public class Producer
    {
        private string _exchangeType;
        private string _exchangeName;
        private string _routingKey;
        private IModel _channel;

        public Producer(string exchangeType, string exchangeName,string routingKey,IModel channel)
        {
            _exchangeType = exchangeType;
            _exchangeName = exchangeName;
            _routingKey = routingKey;
            _channel = channel;
            _channel.ExchangeDeclare(_exchangeName, _exchangeType);
        }

        public void Produce(string messageContent)
        {
            var message = new MessageDto()
            {
                Content = $"{messageContent} exchange: {_exchangeType}"
            };

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            _channel.BasicPublish(_exchangeName,
                _routingKey,
                basicProperties: null,
                body: body);

            Console.WriteLine($"Сообщение отправленно в обменник: {_exchangeName}");

        }
    }
}
