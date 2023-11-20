using GeekShopping.CartAPI.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CartAPI.Repository
{
	public class DataService : IDataService
	{
		private readonly MySQLContext _context;

		public DataService(MySQLContext context)
		{
			_context = context;
		}

		public void InicializaDB()
		{
			_context.Database.Migrate();
		}
	}
}
