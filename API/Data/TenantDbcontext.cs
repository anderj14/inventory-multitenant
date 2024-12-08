using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class TenantDbcontext : DbContext
    {
        public TenantDbcontext(DbContextOptions<TenantDbcontext> options) : base(options)
        {
        }
        
        public DbSet<Tenant> Tenants { get; set; }
    }
}