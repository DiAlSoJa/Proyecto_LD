using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ItemName",
                table: "items",
                newName: "ProductName");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "items",
                newName: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "items",
                newName: "ItemName");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "items",
                newName: "ItemId");
        }
    }
}
