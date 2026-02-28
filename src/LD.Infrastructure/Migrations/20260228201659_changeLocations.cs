using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeLocations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientNumber",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "FiscalAddres",
                table: "ClientFiscalData",
                newName: "FiscalAddress");

            migrationBuilder.AlterColumn<decimal>(
                name: "Capacity",
                table: "Warehouses",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompartidoType",
                table: "Locations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompartidoType",
                table: "Locations");

            migrationBuilder.RenameColumn(
                name: "FiscalAddress",
                table: "ClientFiscalData",
                newName: "FiscalAddres");

            migrationBuilder.AlterColumn<int>(
                name: "Capacity",
                table: "Warehouses",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "ClientNumber",
                table: "Clients",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
