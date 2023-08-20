using System.Text;
using System.Text.Json;
using PlatformService.DTOs;
using RabbitMQ.Client;

namespace PlatformServices.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration ;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory() {HostName = _configuration["RabbitMQHost"], 
            Port = int.Parse(_configuration["RabbitMQPort"]!)};

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown!;
                System.Console.WriteLine($"--> Connected to MessageBus...");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"--> Couldn't connect to RabbitMQ bus: {ex.Message}");
            }
        }
        public void PublishNewPlatform(PlatformPublishedDto platformPublishedDtoPlatform)
        {
            var message = JsonSerializer.Serialize(platformPublishedDtoPlatform);
            if(_connection.IsOpen)
            {
                System.Console.WriteLine($"RabbitMq connection is opened, Sending message ...");
                SendMessage(message);
            }
            else
            {
                System.Console.WriteLine("---> RabbitMQ connection is not opened");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "trigger", 
                                    routingKey: "", 
                                    body: body);
            System.Console.WriteLine($"--> We have send {message}");
        }

        public void Dispose()
        {
            System.Console.WriteLine("MessageBus Disposed");
            if(_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            System.Console.WriteLine($"--> RabbitMQ Connection Shutdown");
        }
    }
}