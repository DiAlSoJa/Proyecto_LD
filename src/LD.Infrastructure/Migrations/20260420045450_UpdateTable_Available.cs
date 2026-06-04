using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTable_Available : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AvailableInventories_StandardId",
                table: "AvailableInventories",
                column: "StandardId");

            migrationBuilder.AddForeignKey(
                name: "FK_AvailableInventories_StandardLabels_StandardId",
                table: "AvailableInventories",
                column: "StandardId",
                principalTable: "StandardLabels",
                principalColumn: "StandarId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AvailableInventories_StandardLabels_StandardId",
                table: "AvailableInventories");

            migrationBuilder.DropIndex(
                name: "IX_AvailableInventories_StandardId",
                table: "AvailableInventories");
        }
    }
}
