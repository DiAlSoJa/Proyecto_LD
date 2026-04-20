using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMovementsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_InventoryMovements_StandardId",
                table: "InventoryMovements",
                column: "StandardId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryMovements_StandardLabels_StandardId",
                table: "InventoryMovements",
                column: "StandardId",
                principalTable: "StandardLabels",
                principalColumn: "StandarId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryMovements_StandardLabels_StandardId",
                table: "InventoryMovements");

            migrationBuilder.DropIndex(
                name: "IX_InventoryMovements_StandardId",
                table: "InventoryMovements");
        }
    }
}
