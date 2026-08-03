using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWarehouseToSecurityRegistrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WarehouseId",
                table: "SecurityRegistrations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SecurityRegistrations_WarehouseId",
                table: "SecurityRegistrations",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityRegistrations_Warehouses_WarehouseId",
                table: "SecurityRegistrations",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SecurityRegistrations_Warehouses_WarehouseId",
                table: "SecurityRegistrations");

            migrationBuilder.DropIndex(
                name: "IX_SecurityRegistrations_WarehouseId",
                table: "SecurityRegistrations");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "SecurityRegistrations");
        }
    }
}
