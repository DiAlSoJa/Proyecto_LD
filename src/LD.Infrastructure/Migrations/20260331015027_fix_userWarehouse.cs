using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fix_userWarehouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserWarehouses_AppUsers_ApplicationUserId",
                table: "UserWarehouses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWarehouses_Warehouses_WarehouseId1",
                table: "UserWarehouses");

            migrationBuilder.DropIndex(
                name: "IX_UserWarehouses_ApplicationUserId",
                table: "UserWarehouses");

            migrationBuilder.DropIndex(
                name: "IX_UserWarehouses_WarehouseId1",
                table: "UserWarehouses");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "UserWarehouses");

            migrationBuilder.DropColumn(
                name: "WarehouseId1",
                table: "UserWarehouses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "UserWarehouses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WarehouseId1",
                table: "UserWarehouses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserWarehouses_ApplicationUserId",
                table: "UserWarehouses",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWarehouses_WarehouseId1",
                table: "UserWarehouses",
                column: "WarehouseId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserWarehouses_AppUsers_ApplicationUserId",
                table: "UserWarehouses",
                column: "ApplicationUserId",
                principalSchema: "Auth",
                principalTable: "AppUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserWarehouses_Warehouses_WarehouseId1",
                table: "UserWarehouses",
                column: "WarehouseId1",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId");
        }
    }
}
