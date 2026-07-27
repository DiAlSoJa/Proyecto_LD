using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTruckTypeAndSecurityBoxFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TieneCaja",
                table: "TruckTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NumeroCaja",
                table: "SecurityRegistrations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PlacaCaja",
                table: "SecurityRegistrations",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sello",
                table: "SecurityRegistrations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "TieneCaja",
                table: "SecurityRegistrations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "TruckTypes",
                keyColumn: "TruckTypeId",
                keyValue: 1,
                column: "TieneCaja",
                value: true);

            migrationBuilder.UpdateData(
                table: "TruckTypes",
                keyColumn: "TruckTypeId",
                keyValue: 2,
                column: "TieneCaja",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TieneCaja",
                table: "TruckTypes");

            migrationBuilder.DropColumn(
                name: "NumeroCaja",
                table: "SecurityRegistrations");

            migrationBuilder.DropColumn(
                name: "PlacaCaja",
                table: "SecurityRegistrations");

            migrationBuilder.DropColumn(
                name: "Sello",
                table: "SecurityRegistrations");

            migrationBuilder.DropColumn(
                name: "TieneCaja",
                table: "SecurityRegistrations");
        }
    }
}
