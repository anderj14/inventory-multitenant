
using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class MultiTenantMigrationHelper
    {
        // Applies pending migrations for the base tenant database and all individual tenant databases.
        public static IServiceCollection ApplyTenantMigrations(this IServiceCollection services, IConfiguration configuration)
        {
            // Apply migrations to the base tenant database (TenantDbContext).
            using IServiceScope scopeTenant = services.BuildServiceProvider().CreateScope();
            TenantDbcontext tenantDbContext = scopeTenant.ServiceProvider.GetRequiredService<TenantDbcontext>();

            if (tenantDbContext.Database.GetPendingMigrations().Any())
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Applying BaseDb Migrations.");
                Console.ResetColor();
                tenantDbContext.Database.Migrate(); // apply migrations on baseDbContext
            }

            // Retrieve all tenants from the base tenant database.
            List<Tenant> tenantList = tenantDbContext.Tenants.ToList();
            string defaultConnectionString = configuration.GetConnectionString("DefaultConnection");

            // Apply migrations to each tenant's individual database.
            foreach (Tenant tenant in tenantList)
            {
                string connectionString = string.IsNullOrEmpty(tenant.ConnectionString) ? defaultConnectionString : tenant.ConnectionString;

                using IServiceScope tenantScope = services.BuildServiceProvider().CreateScope();
                ApplicationDbContext applicationDbContext = tenantScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Dynamically set the connection string for the tenant database.
                applicationDbContext.Database.SetConnectionString(connectionString);

                // Check and apply pending migrations for the tenant database.
                if (tenantDbContext.Database.GetPendingMigrations().Any())
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Applying migrations for tenant with ID '{tenant.Id}'.");
                    Console.ResetColor();
                    tenantDbContext.Database.Migrate();
                }
            }

            return services;
        }
    }
}