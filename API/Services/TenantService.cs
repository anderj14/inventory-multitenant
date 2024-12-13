
using API.Data;
using API.Dtos;
using API.Entities;
using API.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class TenantService(
        TenantDbcontext _context,
        IConfiguration _configuration,
        IServiceProvider _serviceProvider
        ) : ITenantService
    {
        public async Task<Tenant> CreateAsync(TenantCreateDto tenantCreateDto)
        {
            string newConnectionString = null;

            if (tenantCreateDto.Isolated == true)
            {
                string dbName = "inventorytenant-" + tenantCreateDto.Id;
                string defaultConnectionString = _configuration.GetConnectionString("DefaultConnection");
                newConnectionString = defaultConnectionString.Replace("inventorytenant", dbName);

                try
                {
                    using IServiceScope serviceScope = _serviceProvider.CreateScope();
                    ApplicationDbContext applicationDbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    applicationDbContext.Database.SetConnectionString(newConnectionString);
                    if ((await applicationDbContext.Database.GetPendingMigrationsAsync()).Any())
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($"Applying application migration for new '{tenantCreateDto.Id}'.");
                        Console.ResetColor();
                        await applicationDbContext.Database.MigrateAsync();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            Tenant tenant = new()
            {
                Id = tenantCreateDto.Id,
                Name = tenantCreateDto.Name,
                ConnectionString = newConnectionString
            };

            await _context.AddAsync(tenant);
            await _context.SaveChangesAsync();

            return tenant;
        }

        public async Task<ICollection<Tenant>> GetAllTenantsAsync()
        {
            return await _context.Tenants.ToListAsync();
        }

        public async Task<Tenant> GetTenantByIdAsync(string tenantId)
        {
            var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Id == tenantId) ?? throw new Exception($"Tenant with ID '{tenantId}' not found");

            return tenant;
        }

        public async Task<bool> DeleteTenantAsync(string tenantId)
        {
            var tenant = await _context.Tenants.FirstOrDefaultAsync(x => x.Id == tenantId) ?? throw new Exception($"Tenant with ID '{tenantId}' not found.");


            // if the tenant has an isolated database            
            if (!string.IsNullOrEmpty(tenant.ConnectionString))
            {
                try
                {
                    var dbName = new SqlConnectionStringBuilder(tenant.ConnectionString).InitialCatalog;

                    IServiceScope scopeTenant = _serviceProvider.CreateScope();
                    var dbContext = scopeTenant.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    dbContext.Database.SetConnectionString(tenant.ConnectionString);

                    var deleted = await dbContext.Database.EnsureDeletedAsync();
                    if (!deleted) throw new Exception($"Failed to delete database '{dbName}' for tenant '{tenantId}'.");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to delete database for tenant '{tenantId}'. Error: {ex.Message}");
                }
            }

            _context.Tenants.Remove(tenant);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> TenantExistsAsync(string tenantId)
        {
            return await _context.Tenants.AnyAsync(t => t.Id == tenantId);
        }
    }
}