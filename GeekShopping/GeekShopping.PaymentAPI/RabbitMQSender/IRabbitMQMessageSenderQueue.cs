using GeekShopping.MessageBus;

namespace GeekShopping.PaymentAPI.RabbitMQSender
{
    public interface IRabbitMQMessageSenderQueue
    {
        void SendMessage(BaseMessage baseMessage, string queueName);
    }
}
