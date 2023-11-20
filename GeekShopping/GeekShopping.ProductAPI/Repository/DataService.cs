using GeekShopping.ProductAPI.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.ProductAPI.Repository
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
