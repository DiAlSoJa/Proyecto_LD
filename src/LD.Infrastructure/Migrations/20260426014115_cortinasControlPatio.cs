using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class cortinasControlPatio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CortinaId",
                table: "SecurityRegistrations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Estado",
                table: "SecurityRegistrations",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "Permissions",
                columns: new[] { "PermissionId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "IsActive", "Key", "LastModifiedAt", "LastModifiedByUserId", "ModuleId", "PermissionName" },
                values: new object[] { 85, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "security.cortina.assign", null, null, 25, "Asignar cortina" });

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
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "IsActive", "LastModifiedAt", "LastModifiedByUserId" },
                values: new object[] { 85, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_SecurityRegistrations_CortinaId",
                table: "SecurityRegistrations",
                column: "CortinaId");

            migrationBuilder.CreateIndex(
                name: "IX_Cortinas_WarehouseId",
                table: "Cortinas",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityTasks_SecurityRegistrationId",
                table: "SecurityTasks",
                column: "SecurityRegistrationId");

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityRegistrations_Cortinas_CortinaId",
                table: "SecurityRegistrations",
                column: "CortinaId",
                principalTable: "Cortinas",
                principalColumn: "CortinaId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SecurityRegistrations_Cortinas_CortinaId",
                table: "SecurityRegistrations");

            migrationBuilder.DropTable(
                name: "Cortinas");

            migrationBuilder.DropTable(
                name: "SecurityTasks");

            migrationBuilder.DropIndex(
                name: "IX_SecurityRegistrations_CortinaId",
                table: "SecurityRegistrations");

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 85, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                table: "Warehouses",
                keyColumn: "WarehouseId",
                keyValue: 100);

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 85);

            migrationBuilder.DropColumn(
                name: "CortinaId",
                table: "SecurityRegistrations");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "SecurityRegistrations");
        }
    }
}
