using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableFamilies_Client : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Families_Warehouses_WarehouseId",
                table: "Families");

            migrationBuilder.RenameColumn(
                name: "WarehouseId",
                table: "Families",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Families_WarehouseId",
                table: "Families",
                newName: "IX_Families_ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Families_Clients_ClientId",
                table: "Families",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Families_Clients_ClientId",
                table: "Families");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Families",
                newName: "WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_Families_ClientId",
                table: "Families",
                newName: "IX_Families_WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Families_Warehouses_WarehouseId",
                table: "Families",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId");
        }
    }
}
