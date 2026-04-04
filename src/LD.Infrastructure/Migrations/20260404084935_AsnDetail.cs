using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AsnDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                schema: "Auth",
                table: "UserRoles");

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "006be5c9-bd8c-4d39-bc11-88c04640df25");

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "3d8628b6-676a-4a82-858e-898f0fd623fe");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationRoleId",
                schema: "Auth",
                table: "UserRoles",
                type: "nvarchar(450)",
                nullable: true);

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
                    IsSplit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
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
                        name: "FK_AsnReceiptDetails_items_ProductId",
                        column: x => x.ProductId,
                        principalTable: "items",
                        principalColumn: "ProductId");
                });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 8,
                column: "ModuleName",
                value: "Checklist de Montacargas");

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "Modules",
                columns: new[] { "ModuleId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "IsActive", "LastModifiedAt", "LastModifiedByUserId", "ModuleName", "ParentModuleId" },
                values: new object[,]
                {
                    { 18, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Almacenista", null },
                    { 19, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Seguridad", null },
                    { 20, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Consultas", null },
                    { 21, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Reporte de Daños", null },
                    { 22, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Operaciones", null }
                });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 23,
                columns: new[] { "Key", "PermissionName" },
                values: new object[] { "forklift-checklist.read", "Ver checklist de montacargas" });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 25,
                column: "PermissionName",
                value: "Ver catálogos");

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 27,
                column: "Key",
                value: "auditing.read");

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 32,
                column: "PermissionName",
                value: "Ver usuarios");

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 33,
                column: "PermissionName",
                value: "Crear usuarios");

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 34,
                column: "PermissionName",
                value: "Editar usuarios");

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 35,
                column: "PermissionName",
                value: "Eliminar usuarios");


            DeleteData(migrationBuilder);

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "Permissions",
                columns: new[] { "PermissionId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "IsActive", "Key", "LastModifiedAt", "LastModifiedByUserId", "ModuleId", "PermissionName" },
                values: new object[,]
                {
                    { 55, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "inventory.audit.read", null, null, 14, "Ver auditoría de inventario" },
                    { 56, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "inventory.audit.execute", null, null, 14, "Ejecutar auditoría de inventario" },
                    { 57, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "inventory.list.read", null, null, 14, "Ver listado de inventario" },
                    { 58, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "inventory.list.export", null, null, 14, "Exportar listado de inventario" },
                    { 59, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "forklift-checklist.create", null, null, 8, "Crear checklist de montacargas" },
                    { 60, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "forklift-checklist.update", null, null, 8, "Actualizar checklist de montacargas" },
                    { 61, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "forklift-checklist.delete", null, null, 8, "Eliminar checklist de montacargas" },
                    { 62, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "forklift-checklist.execute", null, null, 8, "Ejecutar checklist de montacargas" },
                    { 63, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "forklift-checklist.approve", null, null, 8, "Aprobar checklist de montacargas" }
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
                    { 35, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null }
                });

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "Permissions",
                columns: new[] { "PermissionId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "IsActive", "Key", "LastModifiedAt", "LastModifiedByUserId", "ModuleId", "PermissionName" },
                values: new object[,]
                {
                    { 36, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "warehouse-staff.read", null, null, 18, "Ver almacenista" },
                    { 37, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "warehouse-staff.location-change.execute", null, null, 18, "Ejecutar cambio de ubicación" },
                    { 38, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "warehouse-staff.supply.execute", null, null, 18, "Ejecutar surtido de mercancía" },
                    { 39, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "warehouse-staff.asn.read", null, null, 18, "Ver ASN por ubicar" },
                    { 40, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "warehouse-staff.asn.execute", null, null, 18, "Ejecutar ASN por ubicar" },
                    { 41, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "warehouse-staff.tasks.read", null, null, 18, "Ver task manager de almacenista" },
                    { 42, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "warehouse-staff.tasks.manage", null, null, 18, "Gestionar task manager de almacenista" },
                    { 43, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "security.read", null, null, 19, "Ver seguridad" },
                    { 44, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "security.vehicles.create", null, null, 19, "Registrar vehículo" },
                    { 45, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "security.vehicles.read", null, null, 19, "Ver vehículos" },
                    { 46, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "security.vehicles.update", null, null, 19, "Actualizar vehículos" },
                    { 47, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "security.vehicles.delete", null, null, 19, "Eliminar vehículos" },
                    { 48, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "security.tasks.read", null, null, 19, "Ver task manager de seguridad" },
                    { 49, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "security.tasks.manage", null, null, 19, "Gestionar task manager de seguridad" },
                    { 50, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "queries.read", null, null, 20, "Ver consultas" },
                    { 51, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "damage-report.read", null, null, 21, "Ver reporte de daños" },
                    { 52, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "damage-report.create", null, null, 21, "Crear reporte de daños" },
                    { 53, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "operations.read", null, null, 22, "Ver operaciones" },
                    { 54, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "operations.execute", null, null, 22, "Ejecutar operaciones" }
                });

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "IsActive", "LastModifiedAt", "LastModifiedByUserId" },
                values: new object[,]
                {
                    { 55, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 56, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 57, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 58, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 59, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 60, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 61, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 62, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 63, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
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
                    { 54, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_ApplicationRoleId",
                schema: "Auth",
                table: "UserRoles",
                column: "ApplicationRoleId");

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
                name: "IX_Asns_ClientId",
                table: "Asns",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Asns_ProjectId",
                table: "Asns",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_ApplicationRoleId",
                schema: "Auth",
                table: "UserRoles",
                column: "ApplicationRoleId",
                principalSchema: "Auth",
                principalTable: "Roles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                schema: "Auth",
                table: "UserRoles",
                column: "RoleId",
                principalSchema: "Auth",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }


        private void DeleteData(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                          schema: "Auth",
                          table: "RolePermissions",
                          keyColumns: new[] { "PermissionId", "RoleId" },
                          keyValues: new object[] { 1, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 2, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 3, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 4, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 5, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 6, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 7, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 8, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 9, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 10, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 11, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 12, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 13, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 14, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 15, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 16, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 17, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 18, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 19, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 20, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 21, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 22, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 23, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 24, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 25, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 26, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 27, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 28, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 29, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 30, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 31, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 32, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 33, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 34, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 35, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 36, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 37, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 38, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 39, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 40, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 41, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 42, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 43, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 44, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 45, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 46, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 47, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 48, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 49, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 50, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 51, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 52, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 53, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 54, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 55, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 56, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 57, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 58, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 59, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 60, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 61, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 62, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 63, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 44);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 45);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 46);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 47);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 48);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 49);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 50);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 51);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 52);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 53);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 54);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 55);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 56);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 57);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 58);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 59);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 60);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 61);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 62);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 63);
        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_ApplicationRoleId",
                schema: "Auth",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                schema: "Auth",
                table: "UserRoles");

            migrationBuilder.DropTable(
                name: "AsnReceiptDetails");

            migrationBuilder.DropTable(
                name: "AsnDetails");

            migrationBuilder.DropTable(
                name: "Asns");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_ApplicationRoleId",
                schema: "Auth",
                table: "UserRoles");


            DeleteData(migrationBuilder);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 44);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 45);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 46);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 47);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 48);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 49);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 50);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 51);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 52);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 53);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 54);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 55);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 56);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 57);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 58);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 59);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 60);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 61);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 62);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 63);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 22);

            migrationBuilder.DropColumn(
                name: "ApplicationRoleId",
                schema: "Auth",
                table: "UserRoles");

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 8,
                column: "ModuleName",
                value: "CheckList Montacargas");

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 23,
                columns: new[] { "Key", "PermissionName" },
                values: new object[] { "checklist-lift.read", "Ver checklist montacargas" });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 25,
                column: "PermissionName",
                value: "Ver catalogos");

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 27,
                column: "Key",
                value: "audit.read");

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 32,
                column: "PermissionName",
                value: "Ver Usuarios");

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 33,
                column: "PermissionName",
                value: "Crear Usuarios");

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 34,
                column: "PermissionName",
                value: "Editar Usuarios");

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 35,
                column: "PermissionName",
                value: "Eliminar Usuarios");

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "006be5c9-bd8c-4d39-bc11-88c04640df25", "2", "Supervisor", "SUPERVISOR" },
                    { "3d8628b6-676a-4a82-858e-898f0fd623fe", "3", "Operador", "OPERADOR" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                schema: "Auth",
                table: "UserRoles",
                column: "RoleId",
                principalSchema: "Auth",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
