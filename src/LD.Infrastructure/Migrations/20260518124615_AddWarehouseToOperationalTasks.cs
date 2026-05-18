using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWarehouseToOperationalTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WarehouseId",
                table: "OperationalTasks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OperationalTasks_WarehouseId",
                table: "OperationalTasks",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_OperationalTasks_Warehouses_WarehouseId",
                table: "OperationalTasks",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OperationalTasks_Warehouses_WarehouseId",
                table: "OperationalTasks");

            migrationBuilder.DropIndex(
                name: "IX_OperationalTasks_WarehouseId",
                table: "OperationalTasks");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "OperationalTasks");
        }
    }
}
