using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.IdentityServer.Model.Context
{
    public class SQLServerContext : IdentityDbContext<ApplicationUser>
    {
        public SQLServerContext(DbContextOptions<SQLServerContext> options) : base(options) { }
    }
}
