using LD.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Infrastructure.Persistence
{
    public class LdProyectDbContext : IdentityDbContext<ApplicationUser>
    {


        public DbSet<Category> Categories { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientFiscalData> ClientFiscalData { get; set; }
        public DbSet<ClientContact> ClientContacts { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<DireccionEntrega> DireccionEntregas { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<InventaryStatus> inventaryStatuses { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<StorageType> StorageTypes { get; set; }

        public DbSet<PickingZone> PickingZones { get; set; }
        public DbSet<Printer> Printers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Item> items { get; set; }

        public DbSet<Units> Units { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Warehouse>Warehouses{ get; set; }


        public LdProyectDbContext(DbContextOptions<LdProyectDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            const string schema = "Auth";

            builder.Entity<ApplicationUser>()
                .ToTable("AppUsers", schema);

            builder.Entity<IdentityRole>()
                .ToTable("Roles", schema);

            builder.Entity<IdentityUserRole<string>>()
                .ToTable("UserRoles", schema);

            builder.Entity<IdentityUserClaim<string>>()
                .ToTable("UserClaims", schema);

            builder.Entity<IdentityUserLogin<string>>()
                .ToTable("UserLogins", schema);

            builder.Entity<IdentityRoleClaim<string>>()
                .ToTable("RoleClaims", schema);

            builder.Entity<IdentityUserToken<string>>()
                .ToTable("UserTokens", schema);


            builder.Entity<StorageType>().HasData(
                new StorageType
                {
                    StorageTypeId = 1,
                    Code = "FIFO",
                    Name = "First In - First Out",
                    CreatedAt = new DateTime(2026, 3, 4),
                    CreatedByUserId = "system",
                    IsActive = true
                },
                new StorageType
                {
                    StorageTypeId = 2,
                    Code = "LIFO",
                    Name = "Last In - First Out",
                    CreatedAt = new DateTime(2026, 3, 4),
                    CreatedByUserId = "system",
                    IsActive = true
                },
                new StorageType
                {
                    StorageTypeId = 3,
                    Code = "LOT",
                    Name = "Número de Lote",
                    CreatedAt = new DateTime(2026, 3, 4),
                    CreatedByUserId = "system",
                    IsActive = true
                },
                new StorageType
                {
                    StorageTypeId = 4,
                    Code = "EXP",
                    Name = "Fecha de Caducidad",
                    CreatedAt = new DateTime(2026, 3, 4),
                    CreatedByUserId ="system",
                    IsActive = true
                }
            );

        }
    } 

}
