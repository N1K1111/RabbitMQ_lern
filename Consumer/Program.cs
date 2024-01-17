using Consumer.Settings;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Consumer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var connection = GetRabbitConnection(configuration);
            var channel = connection.CreateModel();

            Consumers.Consumer.Register(channel, "exchange.direct", "queue.direct", "Key_1");
            //Consumers.Consumer.Register(channel, "exchange.fanout", "queue.fanout",  "Key_1");
            //Consumers.Consumer.Register(channel, "exchange.topic", "queue.topic",  "Key_1");

        }

        private static IConnection GetRabbitConnection(IConfiguration configuration)
        {
            var rmqSettings = configuration.Get<AppSettings>().RmqSettings;
            ConnectionFactory factory = new ConnectionFactory
            {
                HostName = rmqSettings.Host,
                VirtualHost = rmqSettings.VHost,
                UserName = rmqSettings.Login,
                Password = rmqSettings.Password,
            };
            IConnection conn = factory.CreateConnection();
            return conn;
        }
    }
}
