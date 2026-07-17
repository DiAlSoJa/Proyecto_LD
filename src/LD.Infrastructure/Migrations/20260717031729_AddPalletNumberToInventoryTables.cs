using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPalletNumberToInventoryTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PalletNumber",
                table: "InventoryMovements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PalletNumber",
                table: "AvailableInventories",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PalletNumber",
                table: "InventoryMovements");

            migrationBuilder.DropColumn(
                name: "PalletNumber",
                table: "AvailableInventories");
        }
    }
}
