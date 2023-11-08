using GeekShopping.OrderAPI.Messages;
using GeekShopping.OrderAPI.Model;
using GeekShopping.OrderAPI.Model.Base;
using GeekShopping.OrderAPI.RabbitMQSender;
using GeekShopping.OrderAPI.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GeekShopping.OrderAPI.MessageConsumer
{
    public class RabbitMQCheckoutConsumer : BackgroundService
    {
        private readonly OrderRepository _repository;
        private IConnection _connection;
        private IModel _channel;
        private IRabbitMQMessageSender _rabbitMQMessageSender;

        public RabbitMQCheckoutConsumer(OrderRepository repository,
            IRabbitMQMessageSender rabbitMQMessageSender)
        {
            _repository = repository;
            _rabbitMQMessageSender = rabbitMQMessageSender;
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "checkoutqueue", false, false, false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (chanel, evt) =>
            {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());
                CheckoutHeaderDTO dto = JsonSerializer.Deserialize<CheckoutHeaderDTO>(content);
                ProcessOrder(dto).GetAwaiter().GetResult();
                _channel.BasicAck(evt.DeliveryTag, false); // apaga da fila
            };
            _channel.BasicConsume("checkoutqueue", false, consumer);
            return Task.CompletedTask;
        }

        private async Task ProcessOrder(CheckoutHeaderDTO dto)
        {
            OrderHeader order = new()
            {
                UserId = dto.UserId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                OrderDetails = new List<OrderDetail>(),
                CardNumber = dto.CardNumber,
                CouponCode = dto.CouponCode,
                CVV = dto.CVV,
                DiscountAmount = dto.DiscountAmount,
                Email = dto.Email,
                ExpireMonthYear = dto.ExpireMothYear,
                OrderTime = DateTime.Now,
                PurchaseAmount = dto.PurchaseAmount,
                PaymentStatus = false,
                Phone = dto.Phone,
                DateTime = dto.DateTime
            };

            foreach (var details in dto.CartDetails)
            {
                OrderDetail detail = new()
                {
                    ProductId = details.ProductId,
                    ProductName = details.Product.Name,
                    Price = details.Product.Price,
                    Count = details.Count,
                };
                order.CartTotalItens += details.Count;
                order.OrderDetails.Add(detail);
            }

            await _repository.AddOrder(order);

            PaymentDTO payment = new()
            {
                Name = order.FirstName + " " + order.LastName,
                CardNumber = order.CardNumber,
                CVV = order.CVV,
                ExpiryMonthYear = order.ExpireMonthYear,
                OrderId = order.Id,
                PurchaseAmount = order.PurchaseAmount,
                Email = order.Email
            };
            try
            {
                _rabbitMQMessageSender.SendMessage(payment, "orderpaymentprocessqueue");
            }
            catch (Exception)
            {
                //Log
                throw;
            }
        }
    }
}
