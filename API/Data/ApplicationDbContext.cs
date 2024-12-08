
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class ApplicationDbContext : DbContext
    {
        // ApplicationDbContext is responsible for managing database operations in a multitenant context.
        // The ActiveTenantId and ActiveTenantConnectionString properties are read-only and fetch values 
        // from the ITenantContextService, ensuring the context always uses the current tenant's information.

        // The constructor ensures that the TenantContextService is provided and not null, guaranteeing proper initialization.
        private readonly ITenantContextService _tenantContextService;
        public string ActiveTenantId { get; set; }
        public string ActiveTenantConnectionString { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantContextService tenantContextService) : base(options)
        {
            _tenantContextService = tenantContextService;
            ActiveTenantId = _tenantContextService.TenantId;
            ActiveTenantConnectionString = _tenantContextService.ConnectionString;
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<InventoryMovement> InventoryMovements { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // Multitenant query filter
            // ApplyTenantQueryFilters(modelBuilder);
            modelBuilder.Entity<Product>().HasQueryFilter(x => x.TenantId == ActiveTenantId);
            modelBuilder.Entity<Brand>().HasQueryFilter(x => x.TenantId == ActiveTenantId);
            modelBuilder.Entity<Category>().HasQueryFilter(x => x.TenantId == ActiveTenantId);
            modelBuilder.Entity<InventoryMovement>().HasQueryFilter(x => x.TenantId == ActiveTenantId);
            modelBuilder.Entity<Supplier>().HasQueryFilter(x => x.TenantId == ActiveTenantId);
            modelBuilder.Entity<Warehouse>().HasQueryFilter(x => x.TenantId == ActiveTenantId);
            modelBuilder.Entity<AppUser>().HasQueryFilter(x => x.TenantId == ActiveTenantId);

            modelBuilder.Entity<Product>()
                .Property(p => p.UnitPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<InventoryMovement>()
            .HasOne(im => im.AppUser)
            .WithMany(au => au.InventoryMovements)
            .HasForeignKey(im => im.AppUserId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<IdentityRole>().HasData(
                    new IdentityRole
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "SuperAdmin",
                        NormalizedName = "SUPERADMIN"
                    },
                    new IdentityRole
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    },
                    new IdentityRole
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Member",
                        NormalizedName = "MEMBER"
                    }
                );
        }

        // private void ApplyTenantQueryFilters(ModelBuilder modelBuilder)
        // {


        // }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Use the tenant connection string enabled
            string tenantConnectionString = ActiveTenantConnectionString;
            if (!string.IsNullOrEmpty(tenantConnectionString))
            {
                _ = optionsBuilder.UseSqlServer(tenantConnectionString);
            }
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<ITenantEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                    case EntityState.Modified:
                        entry.Entity.TenantId = ActiveTenantId;
                        break;
                }
            }
            var result = base.SaveChanges();
            return result;
        }
    }
}