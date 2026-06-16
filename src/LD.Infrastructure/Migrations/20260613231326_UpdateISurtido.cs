using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateISurtido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 19,
                column: "ModuleName",
                value: "Surtidos");

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 28,
                column: "PermissionName",
                value: "Ver surtidos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 19,
                column: "ModuleName",
                value: "Embarques");

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 28,
                column: "PermissionName",
                value: "Ver embarques");
        }
    }
}
