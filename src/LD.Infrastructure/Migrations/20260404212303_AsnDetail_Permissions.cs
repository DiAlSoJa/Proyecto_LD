using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AsnDetail_Permissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_AppUsers_UserId",
                schema: "Auth",
                table: "UserRoles");

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

            migrationBuilder.AlterColumn<int>(
                name: "AsnNumber",
                table: "Projects",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
                name: "SystemField",
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
                    table.PrimaryKey("PK_SystemField", x => x.SystemFieldId);
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
                    Split = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
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
                    SaveTypeScanSaveTypeId = table.Column<int>(type: "int", nullable: false),
                    SaveValue = table.Column<int>(type: "int", nullable: true),
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
                        name: "FK_ScanConfigurations_ScanSaveTypes_SaveTypeScanSaveTypeId",
                        column: x => x.SaveTypeScanSaveTypeId,
                        principalTable: "ScanSaveTypes",
                        principalColumn: "ScanSaveTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScanConfigurations_ScanTypes_ScanTypeId",
                        column: x => x.ScanTypeId,
                        principalTable: "ScanTypes",
                        principalColumn: "ScanTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScanConfigurations_SystemField_SystemFieldId",
                        column: x => x.SystemFieldId,
                        principalTable: "SystemField",
                        principalColumn: "SystemFieldId",
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

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 11,
                columns: new[] { "ModuleName", "ParentModuleId" },
                values: new object[] { "Categorias", 10 });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 12,
                columns: new[] { "ModuleName", "ParentModuleId" },
                values: new object[] { "Dimensionador", 10 });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 13,
                columns: new[] { "ModuleName", "ParentModuleId" },
                values: new object[] { "Familias", 10 });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 14,
                columns: new[] { "ModuleName", "ParentModuleId" },
                values: new object[] { "Monedas", 10 });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 15,
                columns: new[] { "ModuleName", "ParentModuleId" },
                values: new object[] { "Status", 10 });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 16,
                columns: new[] { "ModuleName", "ParentModuleId" },
                values: new object[] { "Unidades", 10 });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 17,
                column: "ModuleName",
                value: "Surtido");

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "Modules",
                columns: new[] { "ModuleId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "IsActive", "LastModifiedAt", "LastModifiedByUserId", "ModuleName", "ParentModuleId" },
                values: new object[,]
                {
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
                    { 28, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null, "Operaciones", null }
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
                keyValue: 26,
                column: "ModuleId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 27,
                columns: new[] { "Key", "ModuleId" },
                values: new object[] { "auditing.read", 18 });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 28,
                column: "ModuleId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 29,
                column: "ModuleId",
                value: 20);

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 30,
                column: "ModuleId",
                value: 21);

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 31,
                column: "ModuleId",
                value: 22);

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 32,
                columns: new[] { "ModuleId", "PermissionName" },
                values: new object[] { 23, "Ver usuarios" });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 33,
                columns: new[] { "ModuleId", "PermissionName" },
                values: new object[] { 23, "Crear usuarios" });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 34,
                columns: new[] { "ModuleId", "PermissionName" },
                values: new object[] { 23, "Editar usuarios" });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 35,
                columns: new[] { "ModuleId", "PermissionName" },
                values: new object[] { 23, "Eliminar usuarios" });

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "Permissions",
                columns: new[] { "PermissionId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "IsActive", "Key", "LastModifiedAt", "LastModifiedByUserId", "ModuleId", "PermissionName" },
                values: new object[,]
                {
                    { 59, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "forklift-checklist.create", null, null, 8, "Crear checklist de montacargas" },
                    { 60, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "forklift-checklist.update", null, null, 8, "Actualizar checklist de montacargas" },
                    { 61, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "forklift-checklist.delete", null, null, 8, "Eliminar checklist de montacargas" },
                    { 62, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "forklift-checklist.execute", null, null, 8, "Ejecutar checklist de montacargas" },
                    { 63, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "forklift-checklist.approve", null, null, 8, "Aprobar checklist de montacargas" },
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
                    { 35, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null }
                });

            migrationBuilder.InsertData(
                table: "ScanSaveTypes",
                columns: new[] { "ScanSaveTypeId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "Description", "IsActive", "Key", "LastModifiedAt", "LastModifiedByUserId", "ScanSaveTypeName" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, true, "remove_first", null, null, "Quitar primeros dígitos" },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, true, "remove_last", null, null, "Quitar últimos dígitos" }
                });

            migrationBuilder.InsertData(
                table: "ScanTypes",
                columns: new[] { "ScanTypeId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "Description", "IsActive", "Key", "LastModifiedAt", "LastModifiedByUserId", "ScanTypeName" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, true, "starts_with", null, null, "Empieza con" },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, true, "length", null, null, "Cantidad de dígitos" },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, true, "less_than", null, null, "Es número menor a" }
                });

            migrationBuilder.InsertData(
                table: "SystemField",
                columns: new[] { "SystemFieldId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "DisplayName", "IsActive", "LastModifiedAt", "LastModifiedByUserId", "Order", "SystemFieldName" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Número de lote", true, null, null, 1, "lot_number" },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Referencia del cliente", true, null, null, 2, "customer_reference" },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Orden de compra", true, null, null, 3, "purchase_order" },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Orden de pedimento", true, null, null, 4, "customs_declaration" },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Cantidad", true, null, null, 5, "qty" }
                });

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "Permissions",
                columns: new[] { "PermissionId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "IsActive", "Key", "LastModifiedAt", "LastModifiedByUserId", "ModuleId", "PermissionName" },
                values: new object[,]
                {
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
                    { 58, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "inventory.list.export", null, null, 20, "Exportar listado de inventario" }
                });

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "IsActive", "LastModifiedAt", "LastModifiedByUserId" },
                values: new object[,]
                {
                    { 59, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 60, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 61, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 62, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 63, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
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
                    { 81, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
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
                    { 58, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null }
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ScanConfigurations_ProjectId",
                table: "ScanConfigurations",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ScanConfigurations_SaveTypeScanSaveTypeId",
                table: "ScanConfigurations",
                column: "SaveTypeScanSaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ScanConfigurations_ScanTypeId",
                table: "ScanConfigurations",
                column: "ScanTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ScanConfigurations_SystemFieldId",
                table: "ScanConfigurations",
                column: "SystemFieldId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_AppUsers_UserId",
                schema: "Auth",
                table: "UserRoles",
                column: "UserId",
                principalSchema: "Auth",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_AppUsers_UserId",
                schema: "Auth",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                schema: "Auth",
                table: "UserRoles");

            migrationBuilder.DropTable(
                name: "AsnReceiptDetails");

            migrationBuilder.DropTable(
                name: "ScanConfigurations");

            migrationBuilder.DropTable(
                name: "AsnDetails");

            migrationBuilder.DropTable(
                name: "ScanSaveTypes");

            migrationBuilder.DropTable(
                name: "ScanTypes");

            migrationBuilder.DropTable(
                name: "SystemField");

            migrationBuilder.DropTable(
                name: "Asns");

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
                keyValue: 21);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 23);

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
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 64, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 65, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 66, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 67, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 68, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 69, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 70, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 71, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 72, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 73, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 74, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 75, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 76, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 77, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 78, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 79, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 80, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 81, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

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
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 64);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 65);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 66);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 67);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 68);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 69);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 70);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 71);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 72);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 73);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 74);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 75);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 76);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 77);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 78);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 79);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 80);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 81);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 28);

            migrationBuilder.AlterColumn<string>(
                name: "AsnNumber",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 8,
                column: "ModuleName",
                value: "CheckList Montacargas");

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 11,
                columns: new[] { "ModuleName", "ParentModuleId" },
                values: new object[] { "Surtido", null });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 12,
                columns: new[] { "ModuleName", "ParentModuleId" },
                values: new object[] { "Auditar", null });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 13,
                columns: new[] { "ModuleName", "ParentModuleId" },
                values: new object[] { "Embarques", null });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 14,
                columns: new[] { "ModuleName", "ParentModuleId" },
                values: new object[] { "Inventario", null });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 15,
                columns: new[] { "ModuleName", "ParentModuleId" },
                values: new object[] { "Inventario Aleatorio", null });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 16,
                columns: new[] { "ModuleName", "ParentModuleId" },
                values: new object[] { "Reportes", null });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 17,
                column: "ModuleName",
                value: "Usuarios");

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
                keyValue: 26,
                column: "ModuleId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 27,
                columns: new[] { "Key", "ModuleId" },
                values: new object[] { "audit.read", 12 });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 28,
                column: "ModuleId",
                value: 13);

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 29,
                column: "ModuleId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 30,
                column: "ModuleId",
                value: 15);

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 31,
                column: "ModuleId",
                value: 16);

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 32,
                columns: new[] { "ModuleId", "PermissionName" },
                values: new object[] { 17, "Ver Usuarios" });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 33,
                columns: new[] { "ModuleId", "PermissionName" },
                values: new object[] { 17, "Crear Usuarios" });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 34,
                columns: new[] { "ModuleId", "PermissionName" },
                values: new object[] { 17, "Editar Usuarios" });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 35,
                columns: new[] { "ModuleId", "PermissionName" },
                values: new object[] { 17, "Eliminar Usuarios" });

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
                name: "FK_UserRoles_AppUsers_UserId",
                schema: "Auth",
                table: "UserRoles",
                column: "UserId",
                principalSchema: "Auth",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
