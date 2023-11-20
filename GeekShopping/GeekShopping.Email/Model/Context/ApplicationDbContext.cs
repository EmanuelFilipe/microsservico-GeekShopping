using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Email.Model.Context
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		public DbSet<EmailLog> Emails { get; set; }

	}
}
