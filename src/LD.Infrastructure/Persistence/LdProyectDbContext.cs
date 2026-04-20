using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using LD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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

        public DbSet<UserWarehouse> UserWarehouses { get; set; }


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
        public DbSet<Dimensioner> Dimensioner{ get; set; }

        public DbSet<SecurityRegistration> SecurityRegistrations { get; set; }

        // ASN related tables
        public DbSet<Asn> Asns { get; set; }
        public DbSet<AsnDetail> AsnDetails { get; set; }
        public DbSet<AsnReceiptDetail> AsnReceiptDetails { get; set; }
        public DbSet<ScanConfiguration> ScanConfigurations { get; set; }
        public DbSet<ScanSaveType> ScanSaveTypes { get; set; }
        public DbSet<ScanType> ScanTypes { get; set; }
        public DbSet<SystemField> SystemFields { get; set; }
        public DbSet<InventoryMovement> InventoryMovements { get; set; }
        public DbSet<StandardLabel> StandardLabels { get; set; }
        public DbSet<StandarIdSequence> StandarIdSequences{ get; set; }
        public DbSet<AvailableInventory> AvailableInventories { get; set; }



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

            builder.Entity<ApplicationUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasOne(e => e.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(e => e.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


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

            builder.Entity<Project>()
                .HasOne(p => p.EntradaUnit)
                .WithMany()
                .HasForeignKey(p => p.Entrada)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Project>()
                .HasOne(p => p.StorageAreaUnit)
                .WithMany()
                .HasForeignKey(p => p.StorageArea)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Project>()
                .HasOne(p => p.ReworkAreaUnit)
                .WithMany()
                .HasForeignKey(p => p.ReworkArea)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Project>()
                .HasOne(p => p.SalidaUnit)
                .WithMany()
                .HasForeignKey(p => p.Salida)
                .OnDelete(DeleteBehavior.NoAction);


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


            builder.Entity<UserWarehouse>()
                .HasOne<ApplicationUser>() 
                .WithMany(u => u.UserWarehouses)
                .HasForeignKey(uw => uw.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserWarehouse>()
                .HasOne(uw => uw.Warehouse)
                .WithMany(w => w.UserWarehouses)
                .HasForeignKey(uw => uw.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Asn>()
                .HasOne(a => a.Client)
                .WithMany()
                .HasForeignKey(a => a.ClientId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Asn>()
                .HasOne(a => a.Project)
                .WithMany()
                .HasForeignKey(a => a.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<AsnDetail>()
            .HasOne(d => d.Asn)
            .WithMany(a => a.AsnDetails)
            .HasForeignKey(d => d.AsnId)
            .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<AsnDetail>()
                    .HasOne(d => d.Product)
                    .WithMany()
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<AsnReceiptDetail>()
                .HasOne(r => r.Product)
                .WithMany()
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

              

            builder.Entity<InventoryMovement>()
                .HasOne(x => x.Project)
                .WithMany()
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<InventoryMovement>()
                .HasOne(x => x.StandardLabel)
                .WithMany()
                .HasForeignKey(x => x.StandardId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<AsnReceiptDetail>()
                .HasOne(r => r.Location)
                .WithMany()
                .HasForeignKey(r => r.LocationId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<AsnReceiptDetail>()
                .HasOne(r => r.StandardLabel)
                .WithMany()
                .HasForeignKey(r => r.StandardId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ScanConfiguration>()
                .HasOne(sc => sc.ScanType)
                .WithMany()
                .HasForeignKey(sc => sc.ScanTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ScanConfiguration>()
                .HasOne(sc => sc.SaveType)
                .WithMany()
                .HasForeignKey(sc => sc.SaveTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<StandardLabel>(entity =>
            {
                entity.ToTable("StandardLabels");
                entity.HasKey(e => e.StandarId);
                entity.Property(e => e.StandarIdStr)
                .HasMaxLength(30)
                .IsRequired();
                entity.HasIndex(e => e.StandarIdStr).IsUnique();
                entity.HasOne(e => e.Project)
                .WithMany()
                .HasForeignKey(e => e.projectId)
                .OnDelete(DeleteBehavior.NoAction);

            });
            builder.Entity<StandarIdSequence>(entity =>
            {
                entity.ToTable("StandarIdSequences");
                entity.HasKey(e => e.StandarIdSequenceId);
                entity.HasIndex(e=>e.Year).IsUnique();
                entity.Property(e=>e.Year).IsRequired();
                entity.Property(e=> e.LastNumber).IsRequired();    
                
            });

            builder.Entity<AvailableInventory>()
               .HasOne(x => x.Project) 
               .WithMany()
               .HasForeignKey(x => x.ProjectId)
               .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<AvailableInventory>()
               .HasOne(x => x.StandardLabel)
               .WithMany()
               .HasForeignKey(x => x.StandardId)
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
                new Module { ModuleId = 1, ModuleName = "Clientes" },
                new Module { ModuleId = 2, ModuleName = "Proyectos" },
                new Module { ModuleId = 3, ModuleName = "Almacenes" },
                new Module { ModuleId = 4, ModuleName = "Ubicaciones" },
                new Module { ModuleId = 5, ModuleName = "Articulos" },
                new Module { ModuleId = 6, ModuleName = "Movimientos" },
                new Module { ModuleId = 7, ModuleName = "ASN" },

                // 👇 Ajuste de nombre (igual al UI)
                new Module { ModuleId = 8, ModuleName = "Checklist de Montacargas" },

                new Module { ModuleId = 9, ModuleName = "Control de patio" },
                new Module { ModuleId = 10, ModuleName = "Catalogos" },

                new Module { ModuleId = 11, ModuleName = "Categorias", ParentModuleId = 10 },
                new Module { ModuleId = 12, ModuleName = "Dimensionador" ,ParentModuleId = 10},
                new Module { ModuleId = 13, ModuleName = "Familias", ParentModuleId = 10 },
                new Module { ModuleId = 14, ModuleName = "Monedas", ParentModuleId = 10 },
                new Module { ModuleId = 15, ModuleName = "Status", ParentModuleId = 10 },
                new Module { ModuleId = 16, ModuleName = "Unidades", ParentModuleId = 10 },  
                new Module { ModuleId = 17, ModuleName = "Surtido" },
                new Module { ModuleId = 18, ModuleName = "Auditar" },
                new Module { ModuleId = 19, ModuleName = "Embarques" },
                new Module { ModuleId = 20, ModuleName = "Inventario" },
                new Module { ModuleId = 21, ModuleName = "Inventario Aleatorio" },
                new Module { ModuleId = 22, ModuleName = "Reportes" },
                new Module { ModuleId = 23, ModuleName = "Usuarios" },

                // 🔥 NUEVOS SEGÚN TU UI
                new Module { ModuleId = 24, ModuleName = "Almacenista" },
                new Module { ModuleId = 25, ModuleName = "Seguridad" },
                new Module { ModuleId = 26, ModuleName = "Consultas" },
                new Module { ModuleId = 27, ModuleName = "Reporte de Daños" },
                new Module { ModuleId = 28, ModuleName = "Operaciones" }
            );

            builder.Entity<Permission>().HasData(

                // CLIENTS (ModuleId = 1)
                new Permission { PermissionId = 1,  PermissionName = "Ver clientes",      Key = "clients.read",   ModuleId = 1 },
                new Permission { PermissionId = 2,  PermissionName = "Crear clientes",    Key = "clients.create", ModuleId = 1 },
                new Permission { PermissionId = 3,  PermissionName = "Editar clientes",   Key = "clients.update", ModuleId = 1 },
                new Permission { PermissionId = 4,  PermissionName = "Eliminar clientes", Key = "clients.delete", ModuleId = 1 },

                // PROJECTS (ModuleId = 2)
                new Permission { PermissionId = 5,  PermissionName = "Ver proyectos",      Key = "projects.read",   ModuleId = 2 },
                new Permission { PermissionId = 6,  PermissionName = "Crear proyectos",    Key = "projects.create", ModuleId = 2 },
                new Permission { PermissionId = 7,  PermissionName = "Editar proyectos",   Key = "projects.update", ModuleId = 2 },
                new Permission { PermissionId = 8,  PermissionName = "Eliminar proyectos", Key = "projects.delete", ModuleId = 2 },

                // WAREHOUSES (ModuleId = 3)
                new Permission { PermissionId = 9,  PermissionName = "Ver almacenes",      Key = "warehouses.read",   ModuleId = 3 },
                new Permission { PermissionId = 10, PermissionName = "Crear almacenes",    Key = "warehouses.create", ModuleId = 3 },
                new Permission { PermissionId = 11, PermissionName = "Editar almacenes",   Key = "warehouses.update", ModuleId = 3 },
                new Permission { PermissionId = 12, PermissionName = "Eliminar almacenes", Key = "warehouses.delete", ModuleId = 3 },

                // LOCATIONS (ModuleId = 4)
                new Permission { PermissionId = 13, PermissionName = "Ver ubicaciones",      Key = "locations.read",   ModuleId = 4 },
                new Permission { PermissionId = 14, PermissionName = "Crear ubicaciones",    Key = "locations.create", ModuleId = 4 },
                new Permission { PermissionId = 15, PermissionName = "Editar ubicaciones",   Key = "locations.update", ModuleId = 4 },
                new Permission { PermissionId = 16, PermissionName = "Eliminar ubicaciones", Key = "locations.delete", ModuleId = 4 },

                // PRODUCTS (ModuleId = 5)
                new Permission { PermissionId = 17, PermissionName = "Ver artículos",      Key = "products.read",   ModuleId = 5 },
                new Permission { PermissionId = 18, PermissionName = "Crear artículos",    Key = "products.create", ModuleId = 5 },
                new Permission { PermissionId = 19, PermissionName = "Editar artículos",   Key = "products.update", ModuleId = 5 },
                new Permission { PermissionId = 20, PermissionName = "Eliminar artículos", Key = "products.delete", ModuleId = 5 },

                // MOVEMENTS (ModuleId = 6)
                new Permission { PermissionId = 21, PermissionName = "Ver movimientos", Key = "movements.read", ModuleId = 6 },

                // ASN (ModuleId = 7)
                new Permission { PermissionId = 22, PermissionName = "Ver ASN", Key = "asn.read", ModuleId = 7 },
                new Permission { PermissionId = 82, PermissionName = "Crear ASN", Key = "asn.create", ModuleId = 7 },
                new Permission { PermissionId = 83, PermissionName = "Editar ASN", Key = "asn.update", ModuleId = 7 },
                new Permission { PermissionId = 84, PermissionName = "Eliminar ASN", Key = "asn.delete", ModuleId = 7 },

                // FORKLIFT CHECKLIST (ModuleId = 8)
                new Permission { PermissionId = 23, PermissionName = "Ver checklist de montacargas",      Key = "forklift-checklist.read",    ModuleId = 8 },
                new Permission { PermissionId = 59, PermissionName = "Crear checklist de montacargas",    Key = "forklift-checklist.create",  ModuleId = 8 },
                new Permission { PermissionId = 60, PermissionName = "Actualizar checklist de montacargas", Key = "forklift-checklist.update", ModuleId = 8 },
                new Permission { PermissionId = 61, PermissionName = "Eliminar checklist de montacargas", Key = "forklift-checklist.delete",  ModuleId = 8 },
                new Permission { PermissionId = 62, PermissionName = "Ejecutar checklist de montacargas", Key = "forklift-checklist.execute", ModuleId = 8 },
                new Permission { PermissionId = 63, PermissionName = "Aprobar checklist de montacargas",  Key = "forklift-checklist.approve", ModuleId = 8 },

                // YARD CONTROL (ModuleId = 9)
                new Permission { PermissionId = 24, PermissionName = "Ver control de patio", Key = "yard-control.read", ModuleId = 9 },

                // CATALOGS (ModuleId = 10)
                new Permission { PermissionId = 25, PermissionName = "Ver catálogos", Key = "catalogs.read", ModuleId = 10 },

                // PICKING (ModuleId = 17)
                new Permission { PermissionId = 26, PermissionName = "Ver surtido", Key = "picking.read", ModuleId = 17 },

                // AUDITING (ModuleId = 18)
                new Permission { PermissionId = 27, PermissionName = "Ver auditoría", Key = "auditing.read", ModuleId = 18 },

                // SHIPMENTS (ModuleId = 19)
                new Permission { PermissionId = 28, PermissionName = "Ver embarques", Key = "shipments.read", ModuleId = 19 },

                // INVENTORY (ModuleId = 20)
                new Permission { PermissionId = 29, PermissionName = "Ver inventario",                   Key = "inventory.read",          ModuleId = 20 },
                new Permission { PermissionId = 55, PermissionName = "Ver auditoría de inventario",       Key = "inventory.audit.read",    ModuleId = 20 },
                new Permission { PermissionId = 56, PermissionName = "Ejecutar auditoría de inventario",  Key = "inventory.audit.execute", ModuleId = 20 },
                new Permission { PermissionId = 57, PermissionName = "Ver listado de inventario",         Key = "inventory.list.read",     ModuleId = 20 },
                new Permission { PermissionId = 58, PermissionName = "Exportar listado de inventario",    Key = "inventory.list.export",   ModuleId = 20 },

                // CYCLE COUNT (ModuleId = 21)
                new Permission { PermissionId = 30, PermissionName = "Ver inventario aleatorio", Key = "cycle-count.read", ModuleId = 21 },

                // REPORTS (ModuleId = 22)
                new Permission { PermissionId = 31, PermissionName = "Ver reportes", Key = "reports.read", ModuleId = 22 },

                // USERS (ModuleId = 23)
                new Permission { PermissionId = 32, PermissionName = "Ver usuarios",      Key = "users.read",   ModuleId = 23 },
                new Permission { PermissionId = 33, PermissionName = "Crear usuarios",    Key = "users.create", ModuleId = 23 },
                new Permission { PermissionId = 34, PermissionName = "Editar usuarios",   Key = "users.update", ModuleId = 23 },
                new Permission { PermissionId = 35, PermissionName = "Eliminar usuarios", Key = "users.delete", ModuleId = 23 },

                // WAREHOUSE STAFF / ALMACENISTA
                new Permission { PermissionId = 36, PermissionName = "Ver almacenista",                  Key = "warehouse-staff.read",                    ModuleId = 24 },
                new Permission { PermissionId = 37, PermissionName = "Ejecutar cambio de ubicación",     Key = "warehouse-staff.location-change.execute", ModuleId = 24 },
                new Permission { PermissionId = 38, PermissionName = "Ejecutar surtido de mercancía",    Key = "warehouse-staff.supply.execute",          ModuleId = 24 },
                new Permission { PermissionId = 39, PermissionName = "Ver ASN por ubicar",              Key = "warehouse-staff.asn.read",                ModuleId = 24 },
                new Permission { PermissionId = 40, PermissionName = "Ejecutar ASN por ubicar",         Key = "warehouse-staff.asn.execute",             ModuleId = 24 },
                new Permission { PermissionId = 41, PermissionName = "Ver task manager de almacenista",  Key = "warehouse-staff.tasks.read",              ModuleId = 24 },
                new Permission { PermissionId = 42, PermissionName = "Gestionar task manager de almacenista", Key = "warehouse-staff.tasks.manage",       ModuleId = 24 },

                // SECURITY / SEGURIDAD
                new Permission { PermissionId = 43, PermissionName = "Ver seguridad",          Key = "security.read",            ModuleId = 25 },
                new Permission { PermissionId = 44, PermissionName = "Registrar vehículo",     Key = "security.vehicles.create", ModuleId = 25 },
                new Permission { PermissionId = 45, PermissionName = "Ver vehículos",          Key = "security.vehicles.read",   ModuleId = 25 },
                new Permission { PermissionId = 46, PermissionName = "Actualizar vehículos",   Key = "security.vehicles.update", ModuleId = 25 },
                new Permission { PermissionId = 47, PermissionName = "Eliminar vehículos",     Key = "security.vehicles.delete", ModuleId = 25 },
                new Permission { PermissionId = 48, PermissionName = "Ver task manager de seguridad",      Key = "security.tasks.read",   ModuleId = 25 },
                new Permission { PermissionId = 49, PermissionName = "Gestionar task manager de seguridad", Key = "security.tasks.manage", ModuleId = 25 },

                // QUERIES / CONSULTAS
                new Permission { PermissionId = 50, PermissionName = "Ver consultas", Key = "queries.read", ModuleId = 26 },

                // DAMAGE REPORT / REPORTE DE DAÑOS
                new Permission { PermissionId = 51, PermissionName = "Ver reporte de daños",    Key = "damage-report.read",   ModuleId = 27 },
                new Permission { PermissionId = 52, PermissionName = "Crear reporte de daños",  Key = "damage-report.create", ModuleId = 27 },

                // OPERATIONS / OPERACIONES
                new Permission { PermissionId = 53, PermissionName = "Ver operaciones",      Key = "operations.read",    ModuleId = 28 },
                new Permission { PermissionId = 54, PermissionName = "Ejecutar operaciones", Key = "operations.execute", ModuleId = 28 },

                // CATEGORIES / CATEGORÍAS
                new Permission { PermissionId = 64, PermissionName = "Ver categorías",    Key = "categories.read",   ModuleId = 11 },
                new Permission { PermissionId = 65, PermissionName = "Crear categorías",  Key = "categories.create", ModuleId = 11 },
                new Permission { PermissionId = 66, PermissionName = "Editar categorías", Key = "categories.update", ModuleId = 11 },

                // DIMENSIONER / DIMENSIONADOR
                new Permission { PermissionId = 67, PermissionName = "Ver dimensionador",    Key = "dimensioner.read",   ModuleId = 12 },
                new Permission { PermissionId = 68, PermissionName = "Crear dimensionador",  Key = "dimensioner.create", ModuleId = 12 },
                new Permission { PermissionId = 69, PermissionName = "Editar dimensionador", Key = "dimensioner.update", ModuleId = 12 },

                // FAMILIES / FAMILIAS
                new Permission { PermissionId = 70, PermissionName = "Ver familias",    Key = "families.read",   ModuleId = 13 },
                new Permission { PermissionId = 71, PermissionName = "Crear familias",  Key = "families.create", ModuleId = 13 },
                new Permission { PermissionId = 72, PermissionName = "Editar familias", Key = "families.update", ModuleId = 13 },

                // CURRENCIES / MONEDAS
                new Permission { PermissionId = 73, PermissionName = "Ver monedas",    Key = "currencies.read",   ModuleId = 14 },
                new Permission { PermissionId = 74, PermissionName = "Crear monedas",  Key = "currencies.create", ModuleId = 14 },
                new Permission { PermissionId = 75, PermissionName = "Editar monedas", Key = "currencies.update", ModuleId = 14 },

                // STATUS / ESTATUS
                new Permission { PermissionId = 76, PermissionName = "Ver estatus",    Key = "status.read",   ModuleId = 15 },
                new Permission { PermissionId = 77, PermissionName = "Crear estatus",  Key = "status.create", ModuleId = 15 },
                new Permission { PermissionId = 78, PermissionName = "Editar estatus", Key = "status.update", ModuleId = 15 },

                // UNITS / UNIDADES
                new Permission { PermissionId = 79, PermissionName = "Ver unidades",    Key = "units.read",   ModuleId = 16 },
                new Permission { PermissionId = 80, PermissionName = "Crear unidades",  Key = "units.create", ModuleId = 16 },
                new Permission { PermissionId = 81, PermissionName = "Editar unidades", Key = "units.update", ModuleId = 16 }
            );

            const string superAdminRoleId = "87b92599-3be7-4ab5-b19e-9e069e015d4e";

            builder.Entity<ApplicationRole>().HasData(
                new ApplicationRole
                {
                    Id = superAdminRoleId,
                    Name = "SuperAdmin",
                    NormalizedName = "SUPERADMIN",
                    ConcurrencyStamp = "1"
                }
            );

            builder.Entity<RolePermission>().HasData(
                Enumerable.Range(1, 84)
                    .Select(id => new RolePermission
                    {
                        RoleId        = superAdminRoleId,
                        PermissionId  = id
                    })
                    .ToArray()
            );
            const string devUserId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890"; 
            var devUser = new ApplicationUser { 
                Id = devUserId, 
                UserName = "admin", 
                NormalizedUserName = "ADMIN",
                Email = "admin@ld.com", 
                NormalizedEmail = "ADMIN@LD.COM", 
                EmailConfirmed = true, 
                FullName = "Administrador Dev", 
                IsActive = true, 
                SecurityStamp = "STATIC-SECURITY-STAMP-DEV", 
                ConcurrencyStamp = "00000000-0000-0000-0000-000000000001", 
                LockoutEnabled = false, 
                PasswordHash= "AQAAAAIAAYagAAAAELwhYiHkLhnB8GG70zbiuUeHdrzvTuYGbLTFm4kwRZo9h6aUhKdbe49Ka2+WdRbkoA=="
            };
            builder.Entity<ApplicationUser>().HasData(devUser);
            builder.Entity<ApplicationUserRole>().HasData(new ApplicationUserRole { UserId = devUserId, RoleId = superAdminRoleId });
             
            builder.Entity<SystemField>().HasData(
                new SystemField { SystemFieldId = 1, SystemFieldName = "lot_number", DisplayName = "Número de lote", Order = 1 },
                new SystemField { SystemFieldId = 2, SystemFieldName = "customer_reference", DisplayName = "Referencia del cliente", Order = 2 },
                new SystemField { SystemFieldId = 3, SystemFieldName = "purchase_order", DisplayName = "Orden de compra", Order = 3 },
                new SystemField { SystemFieldId = 4, SystemFieldName = "customs_declaration", DisplayName = "Orden de pedimento", Order = 4 },
                new SystemField { SystemFieldId = 5, SystemFieldName = "qty", DisplayName = "Cantidad", Order = 5 }
            );

            builder.Entity<ScanType>().HasData(
                new ScanType { ScanTypeId = 1, ScanTypeName = "Empieza con", Key = "starts_with" },
                new ScanType { ScanTypeId = 2, ScanTypeName = "Cantidad de dígitos", Key = "length" },
                new ScanType { ScanTypeId = 3, ScanTypeName = "Es número menor a", Key = "less_than" }
            );

            builder.Entity<ScanSaveType>().HasData(
                new ScanSaveType { ScanSaveTypeId = 1, ScanSaveTypeName = "Quitar primeros dígitos", Key = "remove_first" },
                new ScanSaveType { ScanSaveTypeId = 2, ScanSaveTypeName = "Quitar últimos dígitos", Key = "remove_last" }
            );
        }
    } 

}
