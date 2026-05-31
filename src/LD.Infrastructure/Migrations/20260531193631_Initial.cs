using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Auth");

            migrationBuilder.CreateTable(
                name: "AppUsers",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommercialName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CommercialAddress = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Neightbourhoud = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsProvider = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ClientId);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    CurrencyIdS = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.CurrencyIdS);
                });

            migrationBuilder.CreateTable(
                name: "Dimensioner",
                columns: table => new
                {
                    DimensionerId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Width = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Length = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dimensioner", x => x.DimensionerId);
                });

            migrationBuilder.CreateTable(
                name: "DireccionEntregas",
                columns: table => new
                {
                    DireccionEntregaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    ProyectoId = table.Column<int>(type: "int", nullable: false),
                    Contacto = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Colonia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Ciudad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CodigoPostal = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DireccionEntregas", x => x.DireccionEntregaId);
                });

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    DriverId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Licence = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IMSS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.DriverId);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentSuppliers",
                columns: table => new
                {
                    EquipmentSupplierId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentSupplierName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentSuppliers", x => x.EquipmentSupplierId);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentTypes",
                columns: table => new
                {
                    EquipmentTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsBattery = table.Column<bool>(type: "bit", nullable: false),
                    ImagePathLeft = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePathRight = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentTypes", x => x.EquipmentTypeId);
                });

            migrationBuilder.CreateTable(
                name: "inventaryStatuses",
                columns: table => new
                {
                    InventoryStatusIdS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventaryStatuses", x => x.InventoryStatusIdS);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                schema: "Auth",
                columns: table => new
                {
                    ModuleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ParentModuleId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.ModuleId);
                    table.ForeignKey(
                        name: "FK_Modules_Modules_ParentModuleId",
                        column: x => x.ParentModuleId,
                        principalSchema: "Auth",
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Printers",
                columns: table => new
                {
                    PrinterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Addres = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Printers", x => x.PrinterId);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReplacedByToken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScanSaveTypes",
                columns: table => new
                {
                    ScanSaveTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScanSaveTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScanSaveTypes", x => x.ScanSaveTypeId);
                });

            migrationBuilder.CreateTable(
                name: "ScanTypes",
                columns: table => new
                {
                    ScanTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScanTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScanTypes", x => x.ScanTypeId);
                });

            migrationBuilder.CreateTable(
                name: "StandarIdSequences",
                columns: table => new
                {
                    StandarIdSequenceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SequenceDate = table.Column<DateTime>(type: "date", nullable: false),
                    LastNumber = table.Column<int>(type: "int", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandarIdSequences", x => x.StandarIdSequenceId);
                });

            migrationBuilder.CreateTable(
                name: "StorageTypes",
                columns: table => new
                {
                    StorageTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageTypes", x => x.StorageTypeId);
                });

            migrationBuilder.CreateTable(
                name: "SystemFields",
                columns: table => new
                {
                    SystemFieldId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SystemFieldName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemFields", x => x.SystemFieldId);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    UnitIdS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.UnitIdS);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Plates = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VehicleNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Capacity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Long = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Wight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Plates);
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    WarehouseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WarehouseName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Neighborhood = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Capacity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsProduction = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.WarehouseId);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_AppUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "Auth",
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                schema: "Auth",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_AppUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "Auth",
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                schema: "Auth",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_AppUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "Auth",
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientContacts",
                columns: table => new
                {
                    ContactId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Puesto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Fax = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientContacts", x => x.ContactId);
                    table.ForeignKey(
                        name: "FK_ClientContacts_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientFiscalData",
                columns: table => new
                {
                    ClientFiscalDataId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    BusinessName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Rfc = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    FiscalAddress = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Neightbourhoud = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientFiscalData", x => x.ClientFiscalDataId);
                    table.ForeignKey(
                        name: "FK_ClientFiscalData_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentQuestions",
                columns: table => new
                {
                    EquipmentQuestionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentTypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentQuestions", x => x.EquipmentQuestionId);
                    table.ForeignKey(
                        name: "FK_EquipmentQuestions_EquipmentTypes_EquipmentTypeId",
                        column: x => x.EquipmentTypeId,
                        principalTable: "EquipmentTypes",
                        principalColumn: "EquipmentTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                schema: "Auth",
                columns: table => new
                {
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.PermissionId);
                    table.ForeignKey(
                        name: "FK_Permissions_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalSchema: "Auth",
                        principalTable: "Modules",
                        principalColumn: "ModuleId");
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Auth",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "Auth",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_AppUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "Auth",
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Auth",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cortinas",
                columns: table => new
                {
                    CortinaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    EstaDisponible = table.Column<bool>(type: "bit", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cortinas", x => x.CortinaId);
                    table.ForeignKey(
                        name: "FK_Cortinas_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CyclicInventories",
                columns: table => new
                {
                    CyclicInventoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuditorUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    AuditorName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyclicInventories", x => x.CyclicInventoryId);
                    table.ForeignKey(
                        name: "FK_CyclicInventories_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseId");
                });

            migrationBuilder.CreateTable(
                name: "Equipments",
                columns: table => new
                {
                    EquipmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hourmeter = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsOperative = table.Column<bool>(type: "bit", nullable: false),
                    EquipmentTypeId = table.Column<int>(type: "int", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    EquipmentSupplierId = table.Column<int>(type: "int", nullable: false),
                    Turn1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Turn2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Turn3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePathLeft = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePathRight = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipments", x => x.EquipmentId);
                    table.ForeignKey(
                        name: "FK_Equipments_EquipmentSuppliers_EquipmentSupplierId",
                        column: x => x.EquipmentSupplierId,
                        principalTable: "EquipmentSuppliers",
                        principalColumn: "EquipmentSupplierId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Equipments_EquipmentTypes_EquipmentTypeId",
                        column: x => x.EquipmentTypeId,
                        principalTable: "EquipmentTypes",
                        principalColumn: "EquipmentTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Equipments_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    LocationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsFiscal = table.Column<bool>(type: "bit", nullable: false),
                    HasControlledTemperature = table.Column<bool>(type: "bit", nullable: false),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Width = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Depth = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsRack = table.Column<bool>(type: "bit", nullable: false),
                    IsCompartidoType = table.Column<bool>(type: "bit", nullable: false),
                    IsGeneral = table.Column<bool>(type: "bit", nullable: false),
                    IsCuarentena = table.Column<bool>(type: "bit", nullable: false),
                    IsEmbarque = table.Column<bool>(type: "bit", nullable: false),
                    IsCompartido = table.Column<bool>(type: "bit", nullable: false),
                    IsReciboYEmbarque = table.Column<bool>(type: "bit", nullable: false),
                    IsDoble = table.Column<bool>(type: "bit", nullable: false),
                    IsSencillo = table.Column<bool>(type: "bit", nullable: false),
                    HasPaso = table.Column<bool>(type: "bit", nullable: false),
                    HasCortina = table.Column<bool>(type: "bit", nullable: false),
                    Rack = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Level = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Position = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationId);
                    table.ForeignKey(
                        name: "FK_Locations_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationalTasks",
                columns: table => new
                {
                    OperationalTaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WarehouseId = table.Column<int>(type: "int", nullable: true),
                    Priority = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Activity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Photo1Path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Photo2Path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Photo3Path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Photo4Path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ResolutionObservations = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ResolvedPhoto1Path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ResolvedPhoto2Path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ResolvedPhoto3Path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ResolvedPhoto4Path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Completed = table.Column<bool>(type: "bit", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedBy = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationalTasks", x => x.OperationalTaskId);
                    table.ForeignKey(
                        name: "FK_OperationalTasks_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "UserWarehouses",
                columns: table => new
                {
                    UserWarehouseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    WarehouseId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWarehouses", x => x.UserWarehouseId);
                    table.ForeignKey(
                        name: "FK_UserWarehouses_AppUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "Auth",
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserWarehouses_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentQuestionDets",
                columns: table => new
                {
                    EquipmentQuestionDetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentQuestionId = table.Column<int>(type: "int", nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionAnswerText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsYesNo = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentQuestionDets", x => x.EquipmentQuestionDetId);
                    table.ForeignKey(
                        name: "FK_EquipmentQuestionDets_EquipmentQuestions_EquipmentQuestionId",
                        column: x => x.EquipmentQuestionId,
                        principalTable: "EquipmentQuestions",
                        principalColumn: "EquipmentQuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                schema: "Auth",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "Auth",
                        principalTable: "Permissions",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Auth",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SecurityRegistrations",
                columns: table => new
                {
                    SecurityRegistrationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    CortinaId = table.Column<int>(type: "int", nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Licencia = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Vencimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Celular = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TipoVehiculo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Linea = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Origen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Numero = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Placa = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityRegistrations", x => x.SecurityRegistrationId);
                    table.ForeignKey(
                        name: "FK_SecurityRegistrations_Cortinas_CortinaId",
                        column: x => x.CortinaId,
                        principalTable: "Cortinas",
                        principalColumn: "CortinaId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Checklists",
                columns: table => new
                {
                    ChecklistId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    EquipmentTypeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Turno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Horometro = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checklists", x => x.ChecklistId);
                    table.ForeignKey(
                        name: "FK_Checklists_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "EquipmentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CyclicInventoryDetails",
                columns: table => new
                {
                    CyclicInventoryDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CyclicInventoryId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    Counted = table.Column<bool>(type: "bit", nullable: false),
                    TheoreticalQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PhysicalQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FirstCountResult = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SecondCountResult = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FinalResult = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PartNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Scanned = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyclicInventoryDetails", x => x.CyclicInventoryDetailId);
                    table.ForeignKey(
                        name: "FK_CyclicInventoryDetails_CyclicInventories_CyclicInventoryId",
                        column: x => x.CyclicInventoryId,
                        principalTable: "CyclicInventories",
                        principalColumn: "CyclicInventoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CyclicInventoryDetails_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId");
                });

            migrationBuilder.CreateTable(
                name: "PickingZones",
                columns: table => new
                {
                    PickingZoneId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ProyectId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    PartNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Minimun = table.Column<int>(type: "int", nullable: false),
                    Particioned = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickingZones", x => x.PickingZoneId);
                    table.ForeignKey(
                        name: "FK_PickingZones_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PickingZones_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    StorageTypeId = table.Column<int>(type: "int", nullable: false),
                    ProjectName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AutoPicking = table.Column<bool>(type: "bit", nullable: false),
                    AllowsBackorder = table.Column<bool>(type: "bit", nullable: false),
                    IsDistributionArea = table.Column<bool>(type: "bit", nullable: false),
                    IsFiscalWarehouse = table.Column<bool>(type: "bit", nullable: false),
                    AllowsOversizedItems = table.Column<bool>(type: "bit", nullable: false),
                    RequiresLabels = table.Column<bool>(type: "bit", nullable: false),
                    Entrada = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    StorageArea = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ReworkArea = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Salida = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ReceiptNotificationEnabled = table.Column<bool>(type: "bit", nullable: false),
                    ReceiptNotificationMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShipmentNotificationEnabled = table.Column<bool>(type: "bit", nullable: false),
                    ShipmentNotificationMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternalNotificationEnabled = table.Column<bool>(type: "bit", nullable: false),
                    InternalNotificationMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalHrs = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UrgentHrs = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AsnNumber = table.Column<int>(type: "int", nullable: true),
                    AsnPrefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KittingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KittingPrefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryOrderNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryOrderPrefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoPrefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReciveRequired = table.Column<bool>(type: "bit", nullable: false),
                    ScanRequired = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectId);
                    table.ForeignKey(
                        name: "FK_Projects_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projects_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId");
                    table.ForeignKey(
                        name: "FK_Projects_StorageTypes_StorageTypeId",
                        column: x => x.StorageTypeId,
                        principalTable: "StorageTypes",
                        principalColumn: "StorageTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projects_Units_Entrada",
                        column: x => x.Entrada,
                        principalTable: "Units",
                        principalColumn: "UnitIdS");
                    table.ForeignKey(
                        name: "FK_Projects_Units_ReworkArea",
                        column: x => x.ReworkArea,
                        principalTable: "Units",
                        principalColumn: "UnitIdS");
                    table.ForeignKey(
                        name: "FK_Projects_Units_Salida",
                        column: x => x.Salida,
                        principalTable: "Units",
                        principalColumn: "UnitIdS");
                    table.ForeignKey(
                        name: "FK_Projects_Units_StorageArea",
                        column: x => x.StorageArea,
                        principalTable: "Units",
                        principalColumn: "UnitIdS");
                    table.ForeignKey(
                        name: "FK_Projects_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SecurityRegistrationPhotos",
                columns: table => new
                {
                    SecurityRegistrationPhotoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SecurityRegistrationId = table.Column<int>(type: "int", nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityRegistrationPhotos", x => x.SecurityRegistrationPhotoId);
                    table.ForeignKey(
                        name: "FK_SecurityRegistrationPhotos_SecurityRegistrations_SecurityRegistrationId",
                        column: x => x.SecurityRegistrationId,
                        principalTable: "SecurityRegistrations",
                        principalColumn: "SecurityRegistrationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SecurityTasks",
                columns: table => new
                {
                    SecurityTaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SecurityRegistrationId = table.Column<int>(type: "int", nullable: false),
                    TipoAccion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Completada = table.Column<bool>(type: "bit", nullable: false),
                    FechaCompletada = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RealizadaPor = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityTasks", x => x.SecurityTaskId);
                    table.ForeignKey(
                        name: "FK_SecurityTasks_SecurityRegistrations_SecurityRegistrationId",
                        column: x => x.SecurityRegistrationId,
                        principalTable: "SecurityRegistrations",
                        principalColumn: "SecurityRegistrationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChecklistAnswers",
                columns: table => new
                {
                    ChecklistAnswerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChecklistId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    QuestionTextSnapshot = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AnswerText = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsOk = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistAnswers", x => x.ChecklistAnswerId);
                    table.ForeignKey(
                        name: "FK_ChecklistAnswers_Checklists_ChecklistId",
                        column: x => x.ChecklistId,
                        principalTable: "Checklists",
                        principalColumn: "ChecklistId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChecklistDefectMarks",
                columns: table => new
                {
                    ChecklistDefectMarkId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChecklistId = table.Column<int>(type: "int", nullable: false),
                    Side = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    XPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    YPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistDefectMarks", x => x.ChecklistDefectMarkId);
                    table.ForeignKey(
                        name: "FK_ChecklistDefectMarks_Checklists_ChecklistId",
                        column: x => x.ChecklistId,
                        principalTable: "Checklists",
                        principalColumn: "ChecklistId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChecklistPhotos",
                columns: table => new
                {
                    ChecklistPhotoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChecklistId = table.Column<int>(type: "int", nullable: false),
                    RelativePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Side = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistPhotos", x => x.ChecklistPhotoId);
                    table.ForeignKey(
                        name: "FK_ChecklistPhotos_Checklists_ChecklistId",
                        column: x => x.ChecklistId,
                        principalTable: "Checklists",
                        principalColumn: "ChecklistId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Asns",
                columns: table => new
                {
                    AsnId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PreAsnCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    AsnCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GuideNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Eta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PackagesQty = table.Column<int>(type: "int", nullable: true),
                    IsReturn = table.Column<bool>(type: "bit", nullable: false),
                    IsCustomerMovementRequired = table.Column<bool>(type: "bit", nullable: false),
                    TransportLine = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    VehicleType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DriverName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    VehiclePlate = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    SealNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asns", x => x.AsnId);
                    table.ForeignKey(
                        name: "FK_Asns_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId");
                    table.ForeignKey(
                        name: "FK_Asns_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId");
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Frecuency = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                    table.ForeignKey(
                        name: "FK_Categories_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId");
                    table.ForeignKey(
                        name: "FK_Categories_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId");
                });

            migrationBuilder.CreateTable(
                name: "Families",
                columns: table => new
                {
                    FamilyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FamilyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Families", x => x.FamilyId);
                    table.ForeignKey(
                        name: "FK_Families_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId");
                    table.ForeignKey(
                        name: "FK_Families_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId");
                });

            migrationBuilder.CreateTable(
                name: "ScanConfigurations",
                columns: table => new
                {
                    ScanConfigurationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    SystemFieldId = table.Column<int>(type: "int", nullable: false),
                    ClientField = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScanTypeId = table.Column<int>(type: "int", nullable: false),
                    ScanValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SaveTypeId = table.Column<int>(type: "int", nullable: false),
                    SaveValue = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScanConfigurations", x => x.ScanConfigurationId);
                    table.ForeignKey(
                        name: "FK_ScanConfigurations_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScanConfigurations_ScanSaveTypes_SaveTypeId",
                        column: x => x.SaveTypeId,
                        principalTable: "ScanSaveTypes",
                        principalColumn: "ScanSaveTypeId");
                    table.ForeignKey(
                        name: "FK_ScanConfigurations_ScanTypes_ScanTypeId",
                        column: x => x.ScanTypeId,
                        principalTable: "ScanTypes",
                        principalColumn: "ScanTypeId");
                    table.ForeignKey(
                        name: "FK_ScanConfigurations_SystemFields_SystemFieldId",
                        column: x => x.SystemFieldId,
                        principalTable: "SystemFields",
                        principalColumn: "SystemFieldId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StandardLabels",
                columns: table => new
                {
                    StandarId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StandarIdStr = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PartNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    clientId = table.Column<int>(type: "int", nullable: true),
                    projectId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandardLabels", x => x.StandarId);
                    table.ForeignKey(
                        name: "FK_StandardLabels_Clients_clientId",
                        column: x => x.clientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId");
                    table.ForeignKey(
                        name: "FK_StandardLabels_Projects_projectId",
                        column: x => x.projectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId");
                });

            migrationBuilder.CreateTable(
                name: "items",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    DimensionerId = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    PartNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsTemperatureControlled = table.Column<bool>(type: "bit", nullable: true),
                    IsVMI = table.Column<bool>(type: "bit", nullable: true),
                    IsBOM = table.Column<bool>(type: "bit", nullable: true),
                    RequestLotNumber = table.Column<bool>(type: "bit", nullable: true),
                    RequestExpirationDate = table.Column<bool>(type: "bit", nullable: true),
                    RequestDeclarationNumber = table.Column<bool>(type: "bit", nullable: true),
                    RequestExchangeRate = table.Column<bool>(type: "bit", nullable: true),
                    RequestPurchaseOrder = table.Column<bool>(type: "bit", nullable: true),
                    RequestReference = table.Column<bool>(type: "bit", nullable: true),
                    StorageTypeId = table.Column<int>(type: "int", nullable: true),
                    MinUnitId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MediumUnitId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxUnitId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StandardPackage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MediumUnitValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MaxUnitValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StandardPackageValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Costs = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WarehouseFactor = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProductionFactor = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProductionUnitId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductionStatusId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestNotificationMax = table.Column<bool>(type: "bit", nullable: true),
                    RequestNotificationMin = table.Column<bool>(type: "bit", nullable: true),
                    Maximums = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Minimus = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DeliveryTime = table.Column<int>(type: "int", nullable: true),
                    Reorder = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RequestNotificationEmail = table.Column<bool>(type: "bit", nullable: true),
                    RequestNotificationFiles = table.Column<bool>(type: "bit", nullable: true),
                    DistributionList = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotificationRoute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AlternateEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FamilyId = table.Column<int>(type: "int", nullable: true),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Width = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Length = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UnitIdS = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_items", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_items_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId");
                    table.ForeignKey(
                        name: "FK_items_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId");
                    table.ForeignKey(
                        name: "FK_items_Dimensioner_DimensionerId",
                        column: x => x.DimensionerId,
                        principalTable: "Dimensioner",
                        principalColumn: "DimensionerId");
                    table.ForeignKey(
                        name: "FK_items_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Families",
                        principalColumn: "FamilyId");
                    table.ForeignKey(
                        name: "FK_items_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId");
                    table.ForeignKey(
                        name: "FK_items_StorageTypes_StorageTypeId",
                        column: x => x.StorageTypeId,
                        principalTable: "StorageTypes",
                        principalColumn: "StorageTypeId");
                    table.ForeignKey(
                        name: "FK_items_Units_UnitIdS",
                        column: x => x.UnitIdS,
                        principalTable: "Units",
                        principalColumn: "UnitIdS");
                });

            migrationBuilder.CreateTable(
                name: "AsnDetails",
                columns: table => new
                {
                    AsnDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AsnId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    PartNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    SD = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LotNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomerReference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PurchaseOrder = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustomsDeclarationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StandardQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MaximumQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StatusLine = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsnDetails", x => x.AsnDetailId);
                    table.ForeignKey(
                        name: "FK_AsnDetails_Asns_AsnId",
                        column: x => x.AsnId,
                        principalTable: "Asns",
                        principalColumn: "AsnId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AsnDetails_items_ProductId",
                        column: x => x.ProductId,
                        principalTable: "items",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "AvailableInventories",
                columns: table => new
                {
                    AvailableInventoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    PartNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hora = table.Column<TimeSpan>(type: "time", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LotNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PurchaseOrder = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustomsDeclarationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DocumentId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StatusId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    Qty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StandardId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailableInventories", x => x.AvailableInventoryId);
                    table.ForeignKey(
                        name: "FK_AvailableInventories_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AvailableInventories_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId");
                    table.ForeignKey(
                        name: "FK_AvailableInventories_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId");
                    table.ForeignKey(
                        name: "FK_AvailableInventories_StandardLabels_StandardId",
                        column: x => x.StandardId,
                        principalTable: "StandardLabels",
                        principalColumn: "StandarId");
                    table.ForeignKey(
                        name: "FK_AvailableInventories_items_ProductId",
                        column: x => x.ProductId,
                        principalTable: "items",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryMovements",
                columns: table => new
                {
                    MovementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    PartNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hora = table.Column<TimeSpan>(type: "time", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LotNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PurchaseOrder = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustomsDeclarationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    MovementType = table.Column<int>(type: "int", nullable: false),
                    DocumentId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StatusId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    Qty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StandardId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryMovements", x => x.MovementId);
                    table.ForeignKey(
                        name: "FK_InventoryMovements_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryMovements_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId");
                    table.ForeignKey(
                        name: "FK_InventoryMovements_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId");
                    table.ForeignKey(
                        name: "FK_InventoryMovements_StandardLabels_StandardId",
                        column: x => x.StandardId,
                        principalTable: "StandardLabels",
                        principalColumn: "StandarId");
                    table.ForeignKey(
                        name: "FK_InventoryMovements_items_ProductId",
                        column: x => x.ProductId,
                        principalTable: "items",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AsnReceiptDetails",
                columns: table => new
                {
                    AsnReceiptDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AsnDetailId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    DeleteRow = table.Column<bool>(type: "bit", nullable: false),
                    StandardId = table.Column<int>(type: "int", nullable: true),
                    PartNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    StandardQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MaximumQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SD = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReceivedQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    LocationCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LotNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PurchaseOrder = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustomsDeclarationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StatusLine = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsnReceiptDetails", x => x.AsnReceiptDetailId);
                    table.ForeignKey(
                        name: "FK_AsnReceiptDetails_AsnDetails_AsnDetailId",
                        column: x => x.AsnDetailId,
                        principalTable: "AsnDetails",
                        principalColumn: "AsnDetailId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AsnReceiptDetails_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId");
                    table.ForeignKey(
                        name: "FK_AsnReceiptDetails_StandardLabels_StandardId",
                        column: x => x.StandardId,
                        principalTable: "StandardLabels",
                        principalColumn: "StandarId");
                    table.ForeignKey(
                        name: "FK_AsnReceiptDetails_items_ProductId",
                        column: x => x.ProductId,
                        principalTable: "items",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "DamageReports",
                columns: table => new
                {
                    DamageReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AvailableInventoryId = table.Column<int>(type: "int", nullable: true),
                    StandardId = table.Column<int>(type: "int", nullable: true),
                    StandardIdCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    WarehouseId = table.Column<int>(type: "int", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    PartNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CurrentStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ReceivedQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AvailableQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Warehouse = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Project = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Client = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Asn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReceptionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InventoryState = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DamageType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NewStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Photo1Path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Photo2Path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Photo3Path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Photo4Path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReportedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    ReportedByName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DamageReports", x => x.DamageReportId);
                    table.ForeignKey(
                        name: "FK_DamageReports_AvailableInventories_AvailableInventoryId",
                        column: x => x.AvailableInventoryId,
                        principalTable: "AvailableInventories",
                        principalColumn: "AvailableInventoryId");
                    table.ForeignKey(
                        name: "FK_DamageReports_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId");
                    table.ForeignKey(
                        name: "FK_DamageReports_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId");
                    table.ForeignKey(
                        name: "FK_DamageReports_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId");
                    table.ForeignKey(
                        name: "FK_DamageReports_StandardLabels_StandardId",
                        column: x => x.StandardId,
                        principalTable: "StandardLabels",
                        principalColumn: "StandarId");
                    table.ForeignKey(
                        name: "FK_DamageReports_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseId");
                    table.ForeignKey(
                        name: "FK_DamageReports_items_ProductId",
                        column: x => x.ProductId,
                        principalTable: "items",
                        principalColumn: "ProductId");
                });

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "AppUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "IsActive", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "a1b2c3d4-e5f6-7890-abcd-ef1234567890", 0, "00000000-0000-0000-0000-000000000001", "admin@ld.com", true, "Administrador Dev", true, false, null, "ADMIN@LD.COM", "ADMIN", "AQAAAAIAAYagAAAAELwhYiHkLhnB8GG70zbiuUeHdrzvTuYGbLTFm4kwRZo9h6aUhKdbe49Ka2+WdRbkoA==", null, false, "STATIC-SECURITY-STAMP-DEV", false, "admin" },
                    { "f78a2f0d-32f4-4d66-9db8-0f69f5d3u101", 0, "00000000-0000-0000-0000-000000000101", "checklist.mobile@ld.com", true, "Checklist Mobile", true, false, null, "CHECKLIST.MOBILE@LD.COM", "CHECKLIST", "AQAAAAIAAYagAAAAELwhYiHkLhnB8GG70zbiuUeHdrzvTuYGbLTFm4kwRZo9h6aUhKdbe49Ka2+WdRbkoA==", null, false, "STATIC-SECURITY-STAMP-CHECKLIST", false, "checklist" },
                    { "f78a2f0d-32f4-4d66-9db8-0f69f5d3u102", 0, "00000000-0000-0000-0000-000000000102", "security.mobile@ld.com", true, "Security Mobile", true, false, null, "SECURITY.MOBILE@LD.COM", "SECURITY", "AQAAAAIAAYagAAAAELwhYiHkLhnB8GG70zbiuUeHdrzvTuYGbLTFm4kwRZo9h6aUhKdbe49Ka2+WdRbkoA==", null, false, "STATIC-SECURITY-STAMP-SECURITY", false, "security" },
                    { "f78a2f0d-32f4-4d66-9db8-0f69f5d3u103", 0, "00000000-0000-0000-0000-000000000103", "controlpatio.mobile@ld.com", true, "Control Patio Mobile", true, false, null, "CONTROLPATIO.MOBILE@LD.COM", "CONTROLPATIO", "AQAAAAIAAYagAAAAELwhYiHkLhnB8GG70zbiuUeHdrzvTuYGbLTFm4kwRZo9h6aUhKdbe49Ka2+WdRbkoA==", null, false, "STATIC-SECURITY-STAMP-CONTROLPATIO", false, "controlpatio" }
                });

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "Modules",
                columns: new[] { "ModuleId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "IsActive", "LastModifiedAt", "LastModifiedByUserId", "ModuleName", "ParentModuleId" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Clientes", null },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Proyectos", null },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Almacenes", null },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Ubicaciones", null },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Articulos", null },
                    { 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Movimientos", null },
                    { 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "ASN", null },
                    { 8, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Checklist de Montacargas", null },
                    { 9, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Control de patio", null },
                    { 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Catalogos", null },
                    { 17, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Surtido", null },
                    { 18, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Auditar", null },
                    { 19, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Embarques", null },
                    { 20, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Inventario", null },
                    { 21, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Inventario Aleatorio", null },
                    { 22, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Reportes", null },
                    { 23, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Usuarios", null },
                    { 24, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Almacenista", null },
                    { 25, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Seguridad", null },
                    { 26, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Consultas", null },
                    { 27, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Reporte de Daños", null },
                    { 28, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Operaciones", null },
                    { 29, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Impresion", null }
                });

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "87b92599-3be7-4ab5-b19e-9e069e015d4e", "1", "SuperAdmin", "SUPERADMIN" },
                    { "f78a2f0d-32f4-4d66-9db8-0f69f5d3f101", "1", "ChecklistMobile", "CHECKLISTMOBILE" },
                    { "f78a2f0d-32f4-4d66-9db8-0f69f5d3f102", "1", "SecurityMobile", "SECURITYMOBILE" },
                    { "f78a2f0d-32f4-4d66-9db8-0f69f5d3f103", "1", "ControlPatioMobile", "CONTROLPATIOMOBILE" }
                });

            migrationBuilder.InsertData(
                table: "ScanSaveTypes",
                columns: new[] { "ScanSaveTypeId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "Description", "IsActive", "Key", "LastModifiedAt", "LastModifiedByUserId", "ScanSaveTypeName" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, true, "none", null, null, "Ninguno" },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, true, "remove_first", null, null, "Quitar primeros dígitos" },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, true, "remove_last", null, null, "Quitar últimos dígitos" }
                });

            migrationBuilder.InsertData(
                table: "ScanTypes",
                columns: new[] { "ScanTypeId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "Description", "IsActive", "Key", "LastModifiedAt", "LastModifiedByUserId", "ScanTypeName" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, true, "none", null, null, "Ninguno" },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, true, "starts_with", null, null, "Empieza con" },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, true, "length", null, null, "Cantidad de dígitos" },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, true, "less_than", null, null, "Es número menor a" },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, true, "is_ld_label", null, null, "Es etiqueta LD" },
                    { 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, true, "is_part_number", null, null, "Es número de parte" }
                });

            migrationBuilder.InsertData(
                table: "StorageTypes",
                columns: new[] { "StorageTypeId", "Code", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "IsActive", "LastModifiedAt", "LastModifiedByUserId", "Name" },
                values: new object[,]
                {
                    { 1, "FIFO", new DateTime(2026, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", null, null, true, null, null, "First In - First Out" },
                    { 2, "LIFO", new DateTime(2026, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", null, null, true, null, null, "Last In - First Out" },
                    { 3, "LOT", new DateTime(2026, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", null, null, true, null, null, "Número de Lote" },
                    { 4, "EXP", new DateTime(2026, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", null, null, true, null, null, "Fecha de Caducidad" }
                });

            migrationBuilder.InsertData(
                table: "SystemFields",
                columns: new[] { "SystemFieldId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "DisplayName", "IsActive", "LastModifiedAt", "LastModifiedByUserId", "Order", "SystemFieldName" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Número de lote", true, null, null, 1, "lot_number" },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Referencia del cliente", true, null, null, 2, "customer_reference" },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Orden de compra", true, null, null, 3, "purchase_order" },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Orden de pedimento", true, null, null, 4, "customs_declaration" },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Cantidad", true, null, null, 5, "qty" },
                    { 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "StandardId", true, null, null, 6, "standard_id" },
                    { 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Número de Parte", true, null, null, 7, "partnumber" }
                });

            migrationBuilder.InsertData(
                table: "Warehouses",
                columns: new[] { "WarehouseId", "Address", "Capacity", "City", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "IsActive", "IsProduction", "LastModifiedAt", "LastModifiedByUserId", "Neighborhood", "WarehouseName", "ZipCode" },
                values: new object[] { 100, "Dirección por configurar", 0m, "", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, true, false, null, null, "", "Almacén Principal", "" });

            migrationBuilder.InsertData(
                table: "Cortinas",
                columns: new[] { "CortinaId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "Descripcion", "EstaDisponible", "IsActive", "LastModifiedAt", "LastModifiedByUserId", "Numero", "WarehouseId" },
                values: new object[,]
                {
                    { 100, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Cortina 1 — Muelle Norte", true, true, null, null, "C-01", 100 },
                    { 200, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Cortina 2 — Muelle Norte", true, true, null, null, "C-02", 100 },
                    { 300, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Cortina 3 — Muelle Sur", true, true, null, null, "C-03", 100 },
                    { 400, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Cortina 4 — Muelle Sur", true, true, null, null, "C-04", 100 },
                    { 500, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Cortina 5 — Muelle Este", true, true, null, null, "C-05", 100 }
                });

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "Modules",
                columns: new[] { "ModuleId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "IsActive", "LastModifiedAt", "LastModifiedByUserId", "ModuleName", "ParentModuleId" },
                values: new object[,]
                {
                    { 11, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Categorias", 10 },
                    { 12, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Dimensionador", 10 },
                    { 13, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Familias", 10 },
                    { 14, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Monedas", 10 },
                    { 15, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Status", 10 },
                    { 16, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Unidades", 10 }
                });

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "Permissions",
                columns: new[] { "PermissionId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "IsActive", "Key", "LastModifiedAt", "LastModifiedByUserId", "ModuleId", "PermissionName" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "clients.read", null, null, 1, "Ver clientes" },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "clients.create", null, null, 1, "Crear clientes" },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "clients.update", null, null, 1, "Editar clientes" },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "clients.delete", null, null, 1, "Eliminar clientes" },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "projects.read", null, null, 2, "Ver proyectos" },
                    { 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "projects.create", null, null, 2, "Crear proyectos" },
                    { 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "projects.update", null, null, 2, "Editar proyectos" },
                    { 8, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "projects.delete", null, null, 2, "Eliminar proyectos" },
                    { 9, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "warehouses.read", null, null, 3, "Ver almacenes" },
                    { 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "warehouses.create", null, null, 3, "Crear almacenes" },
                    { 11, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "warehouses.update", null, null, 3, "Editar almacenes" },
                    { 12, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "warehouses.delete", null, null, 3, "Eliminar almacenes" },
                    { 13, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "locations.read", null, null, 4, "Ver ubicaciones" },
                    { 14, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "locations.create", null, null, 4, "Crear ubicaciones" },
                    { 15, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "locations.update", null, null, 4, "Editar ubicaciones" },
                    { 16, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "locations.delete", null, null, 4, "Eliminar ubicaciones" },
                    { 17, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "products.read", null, null, 5, "Ver artículos" },
                    { 18, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "products.create", null, null, 5, "Crear artículos" },
                    { 19, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "products.update", null, null, 5, "Editar artículos" },
                    { 20, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "products.delete", null, null, 5, "Eliminar artículos" },
                    { 21, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "movements.read", null, null, 6, "Ver movimientos" },
                    { 22, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "asn.read", null, null, 7, "Ver ASN" },
                    { 23, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "forklift-checklist.read", null, null, 8, "Ver checklist de montacargas" },
                    { 24, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "yard-control.read", null, null, 9, "Ver control de patio" },
                    { 25, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "catalogs.read", null, null, 10, "Ver catálogos" },
                    { 26, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "picking.read", null, null, 17, "Ver surtido" },
                    { 27, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "auditing.read", null, null, 18, "Ver auditoría" },
                    { 28, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "shipments.read", null, null, 19, "Ver embarques" },
                    { 29, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "inventory.read", null, null, 20, "Ver inventario" },
                    { 30, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "cycle-count.read", null, null, 21, "Ver inventario aleatorio" },
                    { 31, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "reports.read", null, null, 22, "Ver reportes" },
                    { 32, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "users.read", null, null, 23, "Ver usuarios" },
                    { 33, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "users.create", null, null, 23, "Crear usuarios" },
                    { 34, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "users.update", null, null, 23, "Editar usuarios" },
                    { 35, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "users.delete", null, null, 23, "Eliminar usuarios" },
                    { 36, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "warehouse-staff.read", null, null, 24, "Ver almacenista" },
                    { 37, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "warehouse-staff.location-change.execute", null, null, 24, "Ejecutar cambio de ubicación" },
                    { 38, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "warehouse-staff.supply.execute", null, null, 24, "Ejecutar surtido de mercancía" },
                    { 39, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "warehouse-staff.asn.read", null, null, 24, "Ver ASN por ubicar" },
                    { 40, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "warehouse-staff.asn.execute", null, null, 24, "Ejecutar ASN por ubicar" },
                    { 41, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "warehouse-staff.tasks.read", null, null, 24, "Ver task manager de almacenista" },
                    { 42, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "warehouse-staff.tasks.manage", null, null, 24, "Gestionar task manager de almacenista" },
                    { 43, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "security.read", null, null, 25, "Ver seguridad" },
                    { 44, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "security.vehicles.create", null, null, 25, "Registrar vehículo" },
                    { 45, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "security.vehicles.read", null, null, 25, "Ver vehículos" },
                    { 46, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "security.vehicles.update", null, null, 25, "Actualizar vehículos" },
                    { 47, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "security.vehicles.delete", null, null, 25, "Eliminar vehículos" },
                    { 48, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "security.tasks.read", null, null, 25, "Ver task manager de seguridad" },
                    { 49, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "security.tasks.manage", null, null, 25, "Gestionar task manager de seguridad" },
                    { 50, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "queries.read", null, null, 26, "Ver consultas" },
                    { 51, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "damage-report.read", null, null, 27, "Ver reporte de daños" },
                    { 52, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "damage-report.create", null, null, 27, "Crear reporte de daños" },
                    { 53, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "operations.read", null, null, 28, "Ver operaciones" },
                    { 54, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "operations.execute", null, null, 28, "Ejecutar operaciones" },
                    { 55, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "inventory.audit.read", null, null, 20, "Ver auditoría de inventario" },
                    { 56, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "inventory.audit.execute", null, null, 20, "Ejecutar auditoría de inventario" },
                    { 57, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "inventory.list.read", null, null, 20, "Ver listado de inventario" },
                    { 58, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "inventory.list.export", null, null, 20, "Exportar listado de inventario" },
                    { 59, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "forklift-checklist.create", null, null, 8, "Crear checklist de montacargas" },
                    { 60, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "forklift-checklist.update", null, null, 8, "Actualizar checklist de montacargas" },
                    { 61, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "forklift-checklist.delete", null, null, 8, "Eliminar checklist de montacargas" },
                    { 62, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "forklift-checklist.execute", null, null, 8, "Ejecutar checklist de montacargas" },
                    { 63, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "forklift-checklist.approve", null, null, 8, "Aprobar checklist de montacargas" },
                    { 82, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "asn.create", null, null, 7, "Crear ASN" },
                    { 83, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "asn.update", null, null, 7, "Editar ASN" },
                    { 84, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "asn.delete", null, null, 7, "Eliminar ASN" },
                    { 85, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "security.cortina.assign", null, null, 25, "Asignar cortina" },
                    { 86, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "standard-label.print", null, null, 29, "Imprimir etiquetas StandardId" }
                });

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "87b92599-3be7-4ab5-b19e-9e069e015d4e", "a1b2c3d4-e5f6-7890-abcd-ef1234567890" },
                    { "f78a2f0d-32f4-4d66-9db8-0f69f5d3f101", "f78a2f0d-32f4-4d66-9db8-0f69f5d3u101" },
                    { "f78a2f0d-32f4-4d66-9db8-0f69f5d3f102", "f78a2f0d-32f4-4d66-9db8-0f69f5d3u102" },
                    { "f78a2f0d-32f4-4d66-9db8-0f69f5d3f103", "f78a2f0d-32f4-4d66-9db8-0f69f5d3u103" }
                });

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "Permissions",
                columns: new[] { "PermissionId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "IsActive", "Key", "LastModifiedAt", "LastModifiedByUserId", "ModuleId", "PermissionName" },
                values: new object[,]
                {
                    { 64, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "categories.read", null, null, 11, "Ver categorías" },
                    { 65, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "categories.create", null, null, 11, "Crear categorías" },
                    { 66, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "categories.update", null, null, 11, "Editar categorías" },
                    { 67, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "dimensioner.read", null, null, 12, "Ver dimensionador" },
                    { 68, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "dimensioner.create", null, null, 12, "Crear dimensionador" },
                    { 69, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "dimensioner.update", null, null, 12, "Editar dimensionador" },
                    { 70, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "families.read", null, null, 13, "Ver familias" },
                    { 71, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "families.create", null, null, 13, "Crear familias" },
                    { 72, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "families.update", null, null, 13, "Editar familias" },
                    { 73, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "currencies.read", null, null, 14, "Ver monedas" },
                    { 74, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "currencies.create", null, null, 14, "Crear monedas" },
                    { 75, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "currencies.update", null, null, 14, "Editar monedas" },
                    { 76, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "status.read", null, null, 15, "Ver estatus" },
                    { 77, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "status.create", null, null, 15, "Crear estatus" },
                    { 78, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "status.update", null, null, 15, "Editar estatus" },
                    { 79, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "units.read", null, null, 16, "Ver unidades" },
                    { 80, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "units.create", null, null, 16, "Crear unidades" },
                    { 81, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "units.update", null, null, 16, "Editar unidades" }
                });

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "IsActive", "LastModifiedAt", "LastModifiedByUserId" },
                values: new object[,]
                {
                    { 1, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 2, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 3, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 4, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 5, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 6, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 7, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 8, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 9, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 10, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 11, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 12, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 13, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 14, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 15, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 16, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 17, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 18, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 19, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 20, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 21, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 22, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 23, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 24, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 25, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 26, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 27, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 28, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 29, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 30, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 31, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 32, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 33, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 34, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 35, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 36, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 37, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 38, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 39, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 40, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 41, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 42, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 43, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 44, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 45, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 46, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 47, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 48, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 49, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 50, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 51, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 52, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 53, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 54, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 55, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 56, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 57, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 58, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 59, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 60, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 61, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 62, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 63, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 82, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 83, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 84, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 85, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 86, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 23, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f101", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 59, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f101", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 60, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f101", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 61, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f101", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 62, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f101", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 63, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f101", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 43, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f102", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 44, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f102", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 45, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f102", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 46, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f102", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 47, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f102", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 48, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f102", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 49, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f102", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 85, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f102", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 24, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f103", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 64, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 65, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 66, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 67, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 68, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 69, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 70, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 71, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 72, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 73, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 74, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 75, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 76, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 77, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 78, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 79, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 80, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 81, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "Auth",
                table: "AppUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "Auth",
                table: "AppUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AsnDetails_AsnId",
                table: "AsnDetails",
                column: "AsnId");

            migrationBuilder.CreateIndex(
                name: "IX_AsnDetails_ProductId",
                table: "AsnDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_AsnReceiptDetails_AsnDetailId",
                table: "AsnReceiptDetails",
                column: "AsnDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_AsnReceiptDetails_LocationId",
                table: "AsnReceiptDetails",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_AsnReceiptDetails_ProductId",
                table: "AsnReceiptDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_AsnReceiptDetails_StandardId",
                table: "AsnReceiptDetails",
                column: "StandardId");

            migrationBuilder.CreateIndex(
                name: "IX_Asns_ClientId",
                table: "Asns",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Asns_ProjectId",
                table: "Asns",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AvailableInventories_ClientId",
                table: "AvailableInventories",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AvailableInventories_LocationId",
                table: "AvailableInventories",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_AvailableInventories_ProductId",
                table: "AvailableInventories",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_AvailableInventories_ProjectId",
                table: "AvailableInventories",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AvailableInventories_StandardId",
                table: "AvailableInventories",
                column: "StandardId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ClientId",
                table: "Categories",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ProjectId",
                table: "Categories",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistAnswers_ChecklistId",
                table: "ChecklistAnswers",
                column: "ChecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistDefectMarks_ChecklistId",
                table: "ChecklistDefectMarks",
                column: "ChecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistPhotos_ChecklistId",
                table: "ChecklistPhotos",
                column: "ChecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_Checklists_EquipmentId",
                table: "Checklists",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientContacts_ClientId",
                table: "ClientContacts",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientFiscalData_ClientId",
                table: "ClientFiscalData",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cortinas_WarehouseId",
                table: "Cortinas",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_CyclicInventories_WarehouseId",
                table: "CyclicInventories",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_CyclicInventoryDetails_CyclicInventoryId",
                table: "CyclicInventoryDetails",
                column: "CyclicInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CyclicInventoryDetails_LocationId",
                table: "CyclicInventoryDetails",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_DamageReports_AvailableInventoryId",
                table: "DamageReports",
                column: "AvailableInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DamageReports_ClientId",
                table: "DamageReports",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_DamageReports_LocationId",
                table: "DamageReports",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_DamageReports_ProductId",
                table: "DamageReports",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_DamageReports_ProjectId",
                table: "DamageReports",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_DamageReports_StandardId",
                table: "DamageReports",
                column: "StandardId");

            migrationBuilder.CreateIndex(
                name: "IX_DamageReports_WarehouseId",
                table: "DamageReports",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentQuestionDets_EquipmentQuestionId",
                table: "EquipmentQuestionDets",
                column: "EquipmentQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentQuestions_EquipmentTypeId",
                table: "EquipmentQuestions",
                column: "EquipmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_EquipmentSupplierId",
                table: "Equipments",
                column: "EquipmentSupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_EquipmentTypeId",
                table: "Equipments",
                column: "EquipmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_WarehouseId",
                table: "Equipments",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Families_ClientId",
                table: "Families",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Families_ProjectId",
                table: "Families",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryMovements_ClientId",
                table: "InventoryMovements",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryMovements_LocationId",
                table: "InventoryMovements",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryMovements_ProductId",
                table: "InventoryMovements",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryMovements_ProjectId",
                table: "InventoryMovements",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryMovements_StandardId",
                table: "InventoryMovements",
                column: "StandardId");

            migrationBuilder.CreateIndex(
                name: "IX_items_CategoryId",
                table: "items",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_items_ClientId_ProjectId_PartNumber",
                table: "items",
                columns: new[] { "ClientId", "ProjectId", "PartNumber" },
                unique: true,
                filter: "[ClientId] IS NOT NULL AND [ProjectId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_items_DimensionerId",
                table: "items",
                column: "DimensionerId");

            migrationBuilder.CreateIndex(
                name: "IX_items_FamilyId",
                table: "items",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_items_ProjectId",
                table: "items",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_items_StorageTypeId",
                table: "items",
                column: "StorageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_items_UnitIdS",
                table: "items",
                column: "UnitIdS");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_WarehouseId",
                table: "Locations",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_ParentModuleId",
                schema: "Auth",
                table: "Modules",
                column: "ParentModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationalTasks_WarehouseId",
                table: "OperationalTasks",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_ModuleId",
                schema: "Auth",
                table: "Permissions",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_PickingZones_ClientId",
                table: "PickingZones",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_PickingZones_LocationId",
                table: "PickingZones",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ClientId",
                table: "Projects",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Entrada",
                table: "Projects",
                column: "Entrada");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_LocationId",
                table: "Projects",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ReworkArea",
                table: "Projects",
                column: "ReworkArea");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Salida",
                table: "Projects",
                column: "Salida");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_StorageArea",
                table: "Projects",
                column: "StorageArea");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_StorageTypeId",
                table: "Projects",
                column: "StorageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_WarehouseId",
                table: "Projects",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Token",
                schema: "Auth",
                table: "RefreshTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                schema: "Auth",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                schema: "Auth",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "Auth",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ScanConfigurations_ProjectId",
                table: "ScanConfigurations",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ScanConfigurations_SaveTypeId",
                table: "ScanConfigurations",
                column: "SaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ScanConfigurations_ScanTypeId",
                table: "ScanConfigurations",
                column: "ScanTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ScanConfigurations_SystemFieldId",
                table: "ScanConfigurations",
                column: "SystemFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityRegistrationPhotos_SecurityRegistrationId",
                table: "SecurityRegistrationPhotos",
                column: "SecurityRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityRegistrations_CortinaId",
                table: "SecurityRegistrations",
                column: "CortinaId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityTasks_SecurityRegistrationId",
                table: "SecurityTasks",
                column: "SecurityRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_StandardLabels_clientId",
                table: "StandardLabels",
                column: "clientId");

            migrationBuilder.CreateIndex(
                name: "IX_StandardLabels_projectId",
                table: "StandardLabels",
                column: "projectId");

            migrationBuilder.CreateIndex(
                name: "IX_StandardLabels_StandarIdStr",
                table: "StandardLabels",
                column: "StandarIdStr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StandarIdSequences_SequenceDate",
                table: "StandarIdSequences",
                column: "SequenceDate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Units_UnitIdS",
                table: "Units",
                column: "UnitIdS",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                schema: "Auth",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                schema: "Auth",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                schema: "Auth",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWarehouses_UserId",
                table: "UserWarehouses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWarehouses_WarehouseId",
                table: "UserWarehouses",
                column: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AsnReceiptDetails");

            migrationBuilder.DropTable(
                name: "ChecklistAnswers");

            migrationBuilder.DropTable(
                name: "ChecklistDefectMarks");

            migrationBuilder.DropTable(
                name: "ChecklistPhotos");

            migrationBuilder.DropTable(
                name: "ClientContacts");

            migrationBuilder.DropTable(
                name: "ClientFiscalData");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "CyclicInventoryDetails");

            migrationBuilder.DropTable(
                name: "DamageReports");

            migrationBuilder.DropTable(
                name: "DireccionEntregas");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "EquipmentQuestionDets");

            migrationBuilder.DropTable(
                name: "inventaryStatuses");

            migrationBuilder.DropTable(
                name: "InventoryMovements");

            migrationBuilder.DropTable(
                name: "OperationalTasks");

            migrationBuilder.DropTable(
                name: "PickingZones");

            migrationBuilder.DropTable(
                name: "Printers");

            migrationBuilder.DropTable(
                name: "RefreshTokens",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "RoleClaims",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "RolePermissions",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "ScanConfigurations");

            migrationBuilder.DropTable(
                name: "SecurityRegistrationPhotos");

            migrationBuilder.DropTable(
                name: "SecurityTasks");

            migrationBuilder.DropTable(
                name: "StandarIdSequences");

            migrationBuilder.DropTable(
                name: "UserClaims",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "UserLogins",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "UserTokens",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "UserWarehouses");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "AsnDetails");

            migrationBuilder.DropTable(
                name: "Checklists");

            migrationBuilder.DropTable(
                name: "CyclicInventories");

            migrationBuilder.DropTable(
                name: "AvailableInventories");

            migrationBuilder.DropTable(
                name: "EquipmentQuestions");

            migrationBuilder.DropTable(
                name: "Permissions",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "ScanSaveTypes");

            migrationBuilder.DropTable(
                name: "ScanTypes");

            migrationBuilder.DropTable(
                name: "SystemFields");

            migrationBuilder.DropTable(
                name: "SecurityRegistrations");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "AppUsers",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "Asns");

            migrationBuilder.DropTable(
                name: "Equipments");

            migrationBuilder.DropTable(
                name: "StandardLabels");

            migrationBuilder.DropTable(
                name: "items");

            migrationBuilder.DropTable(
                name: "Modules",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "Cortinas");

            migrationBuilder.DropTable(
                name: "EquipmentSuppliers");

            migrationBuilder.DropTable(
                name: "EquipmentTypes");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Dimensioner");

            migrationBuilder.DropTable(
                name: "Families");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "StorageTypes");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "Warehouses");
        }
    }
}
