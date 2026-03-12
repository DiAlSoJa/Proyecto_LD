using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class rolesOtraVez : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "87b92599-3be7-4ab5-b19e-9e069e015d4e");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "006be5c9-bd8c-4d39-bc11-88c04640df25", "2", "Supervisor", "SUPERVISOR" },
                    { "3d8628b6-676a-4a82-858e-898f0fd623fe", "3", "Operador", "OPERADOR" },
                    { "87b92599-3be7-4ab5-b19e-9e069e015d4e", "1", "SuperAdmin", "SUPERADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "006be5c9-bd8c-4d39-bc11-88c04640df25");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3d8628b6-676a-4a82-858e-898f0fd623fe");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "87b92599-3be7-4ab5-b19e-9e069e015d4e");

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "006be5c9-bd8c-4d39-bc11-88c04640df25", "2", "Supervisor", "SUPERVISOR" },
                    { "3d8628b6-676a-4a82-858e-898f0fd623fe", "3", "Operador", "OPERADOR" },
                    { "87b92599-3be7-4ab5-b19e-9e069e015d4e", "1", "SuperAdmin", "SUPERADMIN" }
                });
        }
    }
}
