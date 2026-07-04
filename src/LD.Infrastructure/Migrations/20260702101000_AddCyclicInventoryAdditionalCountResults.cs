using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(LdProyectDbContext))]
    [Migration("20260702101000_AddCyclicInventoryAdditionalCountResults")]
    public partial class AddCyclicInventoryAdditionalCountResults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FourthCountResult",
                table: "CyclicInventoryDetails",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThirdCountResult",
                table: "CyclicInventoryDetails",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FourthCountResult",
                table: "CyclicInventoryDetails");

            migrationBuilder.DropColumn(
                name: "ThirdCountResult",
                table: "CyclicInventoryDetails");
        }
    }
}
