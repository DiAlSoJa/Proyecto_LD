using LD.Domain.Entities;
using MediatR;
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
    public class LdProyectDbContext : IdentityDbContext<
                                            ApplicationUser,
                                            ApplicationRole,
                                            string,
                                            IdentityUserClaim<string>,
                                            ApplicationUserRole,
                                            IdentityUserLogin<string>,
                                            IdentityRoleClaim<string>,
                                            IdentityUserToken<string>>
    {


        public DbSet<Module> Modules { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }



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
        public DbSet<Product> items { get; set; }

        public DbSet<Units> Units { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Warehouse>Warehouses{ get; set; }
        public DbSet<Family> Families{ get; set; }


        public LdProyectDbContext(DbContextOptions<LdProyectDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            const string schema = "Auth";

            builder.Entity<ApplicationUser>()
                .ToTable("AppUsers", schema);

            builder.Entity<ApplicationRole>()
                .ToTable("Roles", schema);

            builder.Entity<ApplicationUserRole>()
                .ToTable("UserRoles", schema);

            builder.Entity<IdentityUserClaim<string>>()
                .ToTable("UserClaims", schema);

            builder.Entity<IdentityUserLogin<string>>()
                .ToTable("UserLogins", schema);

            builder.Entity<IdentityRoleClaim<string>>()
                .ToTable("RoleClaims", schema);

            builder.Entity<IdentityUserToken<string>>()
                .ToTable("UserTokens", schema);

            builder.Entity<ApplicationUser>()
                     .HasMany(u => u.UserRoles)
                     .WithOne(ur => ur.User)
                     .HasForeignKey(ur => ur.UserId)
                     .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationRole>()
                .HasMany(r => r.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Module>()
                .HasOne(m => m.ParentModule)
                .WithMany(m => m.Children)
                .HasForeignKey(m => m.ParentModuleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RolePermission>()
                 .HasKey(x => new { x.RoleId, x.PermissionId });

            builder.Entity<RolePermission>()
                .HasOne<ApplicationRole>()
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(x => x.RoleId);

            builder.Entity<RolePermission>()
                .HasOne(x => x.Permission)
                .WithMany()
                .HasForeignKey(x => x.PermissionId);

            builder.Entity<Units>()
                .HasIndex(u => u.UnitIdS)
                .IsUnique();


            builder.Entity<Category>()
                    .HasOne(c => c.Client)
                    .WithMany()
                    .HasForeignKey(c => c.ClientId)
                    .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Category>()
                .HasOne(c => c.Project)
                .WithMany()
                .HasForeignKey(c => c.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Family>()
                    .HasOne(c => c.Client)
                    .WithMany()
                    .HasForeignKey(c => c.ClientId)
                    .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Family>()
                .HasOne(c => c.Project)
                .WithMany()
                .HasForeignKey(c => c.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);


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
                    CreatedByUserId = "system",
                    IsActive = true
                }
            );

            builder.Entity<Module>().HasData(
                new Module { ModuleId = 1, ModuleName = "Clientes" },//si
                new Module { ModuleId = 2, ModuleName = "Proyectos" },//si
                new Module { ModuleId = 3, ModuleName = "Almacenes" },//si
                new Module { ModuleId = 4, ModuleName = "Ubicaciones" },//si
                new Module { ModuleId = 5, ModuleName = "Articulos" },//si
                new Module { ModuleId = 6, ModuleName = "Movimientos" },//si
                new Module { ModuleId = 7, ModuleName = "ASN" },//si
                new Module { ModuleId = 8, ModuleName = "CheckList Montacargas" }, //no
                new Module { ModuleId = 9, ModuleName = "Control de patio" },//si
                new Module { ModuleId = 10, ModuleName = "Catalogos" },//no
                new Module { ModuleId = 11, ModuleName = "Surtido" },//si
                new Module { ModuleId = 12, ModuleName = "Auditar" },//si
                new Module { ModuleId = 13, ModuleName = "Embarques" },//si
                new Module { ModuleId = 14, ModuleName = "Inventario" },//si
                new Module { ModuleId = 15, ModuleName = "Inventario Aleatorio" },//si
                new Module { ModuleId = 16, ModuleName = "Reportes" },//si
                new Module { ModuleId = 17, ModuleName = "Usuarios" }//si

            );

            builder.Entity<Permission>().HasData(

                // CLIENTS
                new Permission { PermissionId = 1, PermissionName = "Ver clientes", Key = "clients.read", ModuleId = 1 },
                new Permission { PermissionId = 2, PermissionName = "Crear clientes", Key = "clients.create", ModuleId = 1 },
                new Permission { PermissionId = 3, PermissionName = "Editar clientes", Key = "clients.update", ModuleId = 1 },
                new Permission { PermissionId = 4, PermissionName = "Eliminar clientes", Key = "clients.delete", ModuleId = 1 },

                // PROJECTS
                new Permission { PermissionId = 5, PermissionName = "Ver proyectos", Key = "projects.read", ModuleId = 2 },
                new Permission { PermissionId = 6, PermissionName = "Crear proyectos", Key = "projects.create", ModuleId = 2 },
                new Permission { PermissionId = 7, PermissionName = "Editar proyectos", Key = "projects.update", ModuleId = 2 },
                new Permission { PermissionId = 8, PermissionName = "Eliminar proyectos", Key = "projects.delete", ModuleId = 2 },

                // WAREHOUSES
                new Permission { PermissionId = 9, PermissionName = "Ver almacenes", Key = "warehouses.read", ModuleId = 3 },
                new Permission { PermissionId = 10, PermissionName = "Crear almacenes", Key = "warehouses.create", ModuleId = 3 },
                new Permission { PermissionId = 11, PermissionName = "Editar almacenes", Key = "warehouses.update", ModuleId = 3 },
                new Permission { PermissionId = 12, PermissionName = "Eliminar almacenes", Key = "warehouses.delete", ModuleId = 3 },

                // LOCATIONS
                new Permission { PermissionId = 13, PermissionName = "Ver ubicaciones", Key = "locations.read", ModuleId = 4 },
                new Permission { PermissionId = 14, PermissionName = "Crear ubicaciones", Key = "locations.create", ModuleId = 4 },
                new Permission { PermissionId = 15, PermissionName = "Editar ubicaciones", Key = "locations.update", ModuleId = 4 },
                new Permission { PermissionId = 16, PermissionName = "Eliminar ubicaciones", Key = "locations.delete", ModuleId = 4 },

                // Productos
                new Permission { PermissionId = 17, PermissionName = "Ver artículos", Key = "products.read", ModuleId = 5 },
                new Permission { PermissionId = 18, PermissionName = "Crear artículos", Key = "products.create", ModuleId = 5 },
                new Permission { PermissionId = 19, PermissionName = "Editar artículos", Key = "products.update", ModuleId = 5 },
                new Permission { PermissionId = 20, PermissionName = "Eliminar artículos", Key = "products.delete", ModuleId = 5 },


                new Permission { PermissionId = 21, PermissionName = "Ver movimientos", Key = "movements.read", ModuleId = 6 },
                new Permission { PermissionId = 22, PermissionName = "Ver ASN", Key = "asn.read", ModuleId = 7 },
                new Permission { PermissionId = 23, PermissionName = "Ver checklist montacargas", Key = "checklist-lift.read", ModuleId = 8 },//nno
                new Permission { PermissionId = 24, PermissionName = "Ver control de patio", Key = "yard-control.read", ModuleId = 9 },
                new Permission { PermissionId = 25, PermissionName = "Ver catalogos", Key = "catalogs.read", ModuleId = 10 }, //nno
                new Permission { PermissionId = 26, PermissionName = "Ver surtido", Key = "picking.read", ModuleId = 11 },
                new Permission { PermissionId = 27, PermissionName = "Ver auditoría", Key = "audit.read", ModuleId = 12 },
                new Permission { PermissionId = 28, PermissionName = "Ver embarques", Key = "shipments.read", ModuleId = 13 },
                new Permission { PermissionId = 29, PermissionName = "Ver inventario", Key = "inventory.read", ModuleId = 14 },
                new Permission { PermissionId = 30, PermissionName = "Ver inventario aleatorio", Key = "cycle-count.read", ModuleId = 15 },
                new Permission { PermissionId = 31, PermissionName = "Ver reportes", Key = "reports.read", ModuleId = 16 },

                // User
                new Permission { PermissionId = 32, PermissionName = "Ver Usuarios", Key = "users.read", ModuleId = 17 },
                new Permission { PermissionId = 33, PermissionName = "Crear Usuarios", Key = "users.create", ModuleId = 17 },
                new Permission { PermissionId = 34, PermissionName = "Editar Usuarios", Key = "users.update", ModuleId = 17 },
                new Permission { PermissionId = 35, PermissionName = "Eliminar Usuarios", Key = "users.delete", ModuleId = 17 }
            );

            builder.Entity<ApplicationRole>().HasData(
                new ApplicationRole
                {
                    Id = "87b92599-3be7-4ab5-b19e-9e069e015d4e",
                    Name = "SuperAdmin",
                    NormalizedName = "SUPERADMIN",
                    ConcurrencyStamp = "1"
                },
                new ApplicationRole
                {
                    Id = "006be5c9-bd8c-4d39-bc11-88c04640df25",
                    Name = "Supervisor",
                    NormalizedName = "SUPERVISOR",
                    ConcurrencyStamp = "2"
                },
                new ApplicationRole
                {
                    Id = "3d8628b6-676a-4a82-858e-898f0fd623fe",
                    Name = "Operador",
                    NormalizedName = "OPERADOR",
                    ConcurrencyStamp = "3"
                }
            );
           
        }
    } 

}
