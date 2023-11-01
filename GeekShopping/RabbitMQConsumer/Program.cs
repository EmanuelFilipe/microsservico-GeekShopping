using GeekShopping.RabbitMQConsumer.ClientConsumer;

namespace GeekShopping.RabbitMQConsumer
{
    internal class Program
    {
        private static string queueName = "productcreatedqueue";

        static void Main(string[] args)
        {
            IReceivedMessage consumer = new ReceivedMessage();
            consumer.ConsumerMessage(queueName);
        }

    }
}