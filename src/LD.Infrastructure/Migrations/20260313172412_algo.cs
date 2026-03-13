using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class algo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 8,
                column: "ModuleName",
                value: "CheckList Montacargas");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 10,
                column: "ModuleName",
                value: "Catalogos");

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
                columns: new[] { "Key", "PermissionName" },
                values: new object[] { "catalogs.read", "Ver catalogos" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 8,
                column: "ModuleName",
                value: "Recepcion de Material");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 10,
                column: "ModuleName",
                value: "Validacion de recepcion");

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 23,
                columns: new[] { "Key", "PermissionName" },
                values: new object[] { "material-receiving.read", "Ver recepción de material" });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 25,
                columns: new[] { "Key", "PermissionName" },
                values: new object[] { "receiving-validation.read", "Ver validación de recepción" });
        }
    }
}
