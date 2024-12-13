using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ITenantContextService _tenantContextService;
        public string ActiveTenantId { get; set; }
        public string ActiveTenantConnectionString { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantContextService tenantContextService) : base(options)
        {
            _tenantContextService = tenantContextService;
            ActiveTenantId = _tenantContextService.TenantId;
            ActiveTenantConnectionString = _tenantContextService.ConnectionString;
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<IdentityRole> Roles { get; set; }
        public DbSet<IdentityUserRole<string>> UserRoles { get; set; }
        public DbSet<IdentityUserClaim<string>> UserClaims { get; set; }
        public DbSet<IdentityUserLogin<string>> UserLogins { get; set; }
        public DbSet<IdentityRoleClaim<string>> RoleClaims { get; set; }
        public DbSet<IdentityUserToken<string>> UserTokens { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<InventoryMovement> InventoryMovements { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>(b =>
            {
                b.ToTable("AspNetUsers");
                b.HasKey(u => u.Id);
                b.HasIndex(u => u.NormalizedUserName).IsUnique();
                b.HasIndex(u => u.NormalizedEmail).IsUnique();
                b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();
            });

            modelBuilder.Entity<IdentityRole>(b =>
            {
                b.ToTable("AspNetRoles");
                b.HasKey(r => r.Id);
                b.HasIndex(r => r.NormalizedName).IsUnique();
            });

            modelBuilder.Entity<IdentityUserRole<string>>(b =>
            {
                b.ToTable("AspNetUserRoles");
                b.HasKey(r => new { r.UserId, r.RoleId });
            });

            modelBuilder.Entity<IdentityUserClaim<string>>(b =>
            {
                b.ToTable("AspNetUserClaims");
                b.HasKey(uc => uc.Id);
            });

            modelBuilder.Entity<IdentityUserLogin<string>>(b =>
            {
                b.ToTable("AspNetUserLogins");
                b.HasKey(l => new { l.LoginProvider, l.ProviderKey });
            });

            modelBuilder.Entity<IdentityRoleClaim<string>>(b =>
            {
                b.ToTable("AspNetRoleClaims");
                b.HasKey(rc => rc.Id);
            });

            modelBuilder.Entity<IdentityUserToken<string>>(b =>
            {
                b.ToTable("AspNetUserTokens");
                b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
            });

            // Multitenant query filter
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
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
            return base.SaveChanges();
        }
    }
}
