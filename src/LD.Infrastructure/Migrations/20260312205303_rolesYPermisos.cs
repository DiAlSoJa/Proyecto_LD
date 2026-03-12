using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class rolesYPermisos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "006be5c9-bd8c-4d39-bc11-88c04640df25",
                column: "ConcurrencyStamp",
                value: "2");

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "3d8628b6-676a-4a82-858e-898f0fd623fe",
                column: "ConcurrencyStamp",
                value: "3");

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "87b92599-3be7-4ab5-b19e-9e069e015d4e",
                column: "ConcurrencyStamp",
                value: "1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "006be5c9-bd8c-4d39-bc11-88c04640df25",
                column: "ConcurrencyStamp",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "3d8628b6-676a-4a82-858e-898f0fd623fe",
                column: "ConcurrencyStamp",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "87b92599-3be7-4ab5-b19e-9e069e015d4e",
                column: "ConcurrencyStamp",
                value: null);
        }
    }
}
