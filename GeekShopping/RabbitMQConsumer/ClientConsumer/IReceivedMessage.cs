
namespace GeekShopping.RabbitMQConsumer.ClientConsumer
{
    public interface IReceivedMessage
    {
        void ConsumerMessage(string queueName);
    }
}
