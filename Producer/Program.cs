using Microsoft.Extensions.Configuration;
using Producer.Settings;
using RabbitMQ.Client;

namespace Producer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json",true,true)
                .Build();

            var connection = GetRabbitConnection(configuration);
            var channel = connection.CreateModel();

            //var producer = new Producers.Producer("direct", "exchange.direct", "key_1", channel);
            var producer = new Producers.Producer("fanout", "exchange.fanout", "key_1", channel);
            //var producer = new Producers.Producer("topic", "exchange.topic", "key_1", channel);
        
            
            while (true)
            {
                producer.Produce("Message");
                //Thread.Sleep(1000);
            }
            

            //channel.Close();
            //connection.Close();
        }

        private static IConnection GetRabbitConnection(IConfiguration configuration)
        {
            var rmqSettings = configuration.Get<AppSettings>().RmqSettings;

            ConnectionFactory connectionFactory = new ConnectionFactory 
            {
                HostName = rmqSettings.Host,
                VirtualHost = rmqSettings.VHost,
                UserName = rmqSettings.Login,
                Password = rmqSettings.Password,
            };

            return connectionFactory.CreateConnection();
        }
    }
}
