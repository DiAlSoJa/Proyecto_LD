using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(LdProyectDbContext))]
    [Migration("20260701101500_AddCyclicInventoryDetailLocationCounts")]
    public partial class AddCyclicInventoryDetailLocationCounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AnotherLocationQty",
                table: "CyclicInventoryDetails",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SameLocationQty",
                table: "CyclicInventoryDetails",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentLocationId",
                table: "CyclicInventoryScans",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CyclicInventoryScans_CurrentLocationId",
                table: "CyclicInventoryScans",
                column: "CurrentLocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CyclicInventoryScans_CurrentLocationId",
                table: "CyclicInventoryScans");

            migrationBuilder.DropColumn(
                name: "AnotherLocationQty",
                table: "CyclicInventoryDetails");

            migrationBuilder.DropColumn(
                name: "SameLocationQty",
                table: "CyclicInventoryDetails");

            migrationBuilder.DropColumn(
                name: "CurrentLocationId",
                table: "CyclicInventoryScans");
        }
    }
}
