using GeekShopping.MessageBus;

namespace GeekShopping.ProductAPI.RabbitMQSender
{
    public interface IRabbitMQMessageSender
    {
        void SendMessage(BaseMessage baseMessage, string queueName);
    }
}
