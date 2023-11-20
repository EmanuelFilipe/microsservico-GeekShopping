using GeekShopping.IdentityServer.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.IdentityServer.Initializer
{
	public class DataService : IDataService
	{
		private readonly SQLServerContext _context;

		public DataService(SQLServerContext context)
		{
			_context = context;
		}

		public void InicializaDB()
		{
			_context.Database.Migrate();
		}
	}
}
