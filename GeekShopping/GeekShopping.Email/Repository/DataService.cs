using GeekShopping.Email.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Email.Repository
{
	public class DataService : IDataService
	{
		private readonly ApplicationDbContext _context;

		public DataService(ApplicationDbContext context)
		{
			_context = context;
		}

		public void InicializaDB()
		{
			_context.Database.Migrate();
		}
	}
}
