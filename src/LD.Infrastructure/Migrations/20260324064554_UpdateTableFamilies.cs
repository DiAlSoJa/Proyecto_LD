using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableFamilies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Families_Clients_ClientId",
                table: "Families");

            migrationBuilder.DropForeignKey(
                name: "FK_Families_Warehouses_WarehouseId",
                table: "Families");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Families",
                newName: "ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Families_ClientId",
                table: "Families",
                newName: "IX_Families_ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Families_Projects_ProjectId",
                table: "Families",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Families_Warehouses_WarehouseId",
                table: "Families",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Families_Projects_ProjectId",
                table: "Families");

            migrationBuilder.DropForeignKey(
                name: "FK_Families_Warehouses_WarehouseId",
                table: "Families");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "Families",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Families_ProjectId",
                table: "Families",
                newName: "IX_Families_ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Families_Clients_ClientId",
                table: "Families",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Families_Warehouses_WarehouseId",
                table: "Families",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
