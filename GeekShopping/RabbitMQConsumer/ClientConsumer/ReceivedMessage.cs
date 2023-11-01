using GeekShopping.RabbitMQConsumer.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GeekShopping.RabbitMQConsumer.ClientConsumer
{
    public class ReceivedMessage : IReceivedMessage
    {
        private readonly string _hostName;
        private readonly string _password;
        private readonly string _userName;
        private IConnection _connection;

        public ReceivedMessage()
        {
            _hostName = "localhost";
            _password = "guest";
            _userName = "guest";
        }

        public void ConsumerMessage(string queueName)
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostName,
                Password = _password,
                UserName = _userName,
            };

            _connection = factory.CreateConnection();

            using var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: queueName, false, false, false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                var product = JsonSerializer.Deserialize<Product>(message);

                Console.WriteLine($"\n Id: {product.Id} \n " +
                                  $"Name: {product.Name} \n " +
                                  $"Price: {product.Price:N2} \n " +
                                  $"Description: {product.Description} \n " +
                                  $"Category: {product.CategoryName} \n " +
                                  $"Image: {product.ImageUrl} \n", message);

                Console.WriteLine("======================================================");
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            Console.WriteLine($"\n Products Received by RabbitMQ \n");
            Console.ReadLine();
        }
    }
}