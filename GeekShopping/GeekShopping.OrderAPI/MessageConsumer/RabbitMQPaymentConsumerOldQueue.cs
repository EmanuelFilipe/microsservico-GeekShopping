using GeekShopping.OrderAPI.Messages;
using GeekShopping.OrderAPI.Model;
using GeekShopping.OrderAPI.Model.Base;
using GeekShopping.OrderAPI.RabbitMQSender;
using GeekShopping.OrderAPI.Repository;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GeekShopping.OrderAPI.MessageConsumer
{
    public class RabbitMQPaymentConsumerOldQueue : BackgroundService
    {
        private readonly OrderRepository _repository;
        private IConnection _connection;
        private IModel _channel;
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public RabbitMQPaymentConsumerOldQueue(IServiceScopeFactory serviceScopeFactory, OrderRepository repository)
        {
			_serviceScopeFactory = serviceScopeFactory;
			_repository = repository;
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "orderpaymentresultqueue", false, false, false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            //var consumer = new EventingBasicConsumer(_channel);
            //consumer.Received += (chanel, evt) =>
            //{
            //    var content = Encoding.UTF8.GetString(evt.Body.ToArray());
            //    UpdatePaymentResultDTO dto = JsonSerializer.Deserialize<UpdatePaymentResultDTO>(content);
            //    UpdatePaymentStatus(dto).GetAwaiter().GetResult();
            //    _channel.BasicAck(evt.DeliveryTag, false); // apaga da fila
            //};
            //_channel.BasicConsume("orderpaymentresultqueue", false, consumer);
            return Task.CompletedTask;
        }

        private async Task UpdatePaymentStatus(UpdatePaymentResultDTO dto)
        {
            try
            {
				//using (var scope = _serviceScopeFactory.CreateScope())
				//{
				//	var dbContextOptions = scope.ServiceProvider.GetRequiredService<DbContextOptions<ApplicationContext>>();
				//	using var _db = new ApplicationContext(dbContextOptions);

				//	// Resolva o ITesteRepository manualmente
				//	var testeRepository = scope.ServiceProvider.GetRequiredService<ITesteRepository>();

                    // Use testeRepository aqui...

                    //if (testeRepository != null)
                    //{
                    //  var lista = testeRepository.GetAll();
				    //}
				//}
			}
            catch (Exception)
            {
                //Log
                throw;
            }
        }
    }
}
