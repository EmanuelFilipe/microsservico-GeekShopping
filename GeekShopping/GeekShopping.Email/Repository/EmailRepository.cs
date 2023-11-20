using GeekShopping.Email.Messages;
using GeekShopping.Email.Model;
using GeekShopping.Email.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Email.Repository
{
    public class EmailRepository : IEmailRepository
	{
		private readonly DbContextOptions<ApplicationDbContext> _context;
		//private IMapper _mapper;

		public EmailRepository(DbContextOptions<ApplicationDbContext> context)
		{
			_context = context;
		}

        public async Task LogEmail(UpdatePaymentResultMessage message)
        {
            EmailLog email = new()
            {
                Email = message.Email,
                SentDate = DateTime.Now,
                Log = $"Order - {message.OrderId} has been created successfully!"
            };

            await using var _db = new ApplicationDbContext(_context);
            _db.Emails.Add(email);
            await _db.SaveChangesAsync();
        }
    }
}
