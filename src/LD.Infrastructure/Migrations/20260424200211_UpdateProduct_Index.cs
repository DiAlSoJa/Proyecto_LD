using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProduct_Index : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_items_ClientId",
                table: "items");

            migrationBuilder.CreateIndex(
                name: "IX_items_ClientId_ProjectId_PartNumber",
                table: "items",
                columns: new[] { "ClientId", "ProjectId", "PartNumber" },
                unique: true,
                filter: "[ClientId] IS NOT NULL AND [ProjectId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_items_ClientId_ProjectId_PartNumber",
                table: "items");

            migrationBuilder.CreateIndex(
                name: "IX_items_ClientId",
                table: "items",
                column: "ClientId");
        }
    }
}
