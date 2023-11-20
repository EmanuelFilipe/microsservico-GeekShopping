using GeekShopping.OrderAPI.Model.Base;
using GeekShopping.OrderAPI.Repository;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.OrderAPI.Repository
{
	public class DataService : IDataService
	{
		private readonly ApplicationContext _context;

		public DataService(ApplicationContext context)
		{
			_context = context;
		}

		public void InicializaDB()
		{
			_context.Database.Migrate();
		}
	}
}
