
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace API.Data
{
    public class DesignTimeTenantDbcontextFactory : IDesignTimeDbContextFactory<TenantDbcontext>
    {
        public TenantDbcontext CreateDbContext(string[] args)
        {

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");

            DbContextOptionsBuilder<TenantDbcontext> optionsBuilder = new();
            _ =optionsBuilder.UseSqlServer(connectionString);

            return new TenantDbcontext(optionsBuilder.Options);
        }
    }
}