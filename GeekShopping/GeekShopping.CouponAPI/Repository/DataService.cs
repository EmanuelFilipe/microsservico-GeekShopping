using GeekShopping.CouponAPI.Model.Context;
using GeekShopping.CouponAPI.Repository;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CouponAPI.Repository
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
