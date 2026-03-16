using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateTableVehiculeStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Placas",
                table: "Vehicles",
                newName: "Plates");

            migrationBuilder.RenameColumn(
                name: "Largo",
                table: "Vehicles",
                newName: "Long");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Plates",
                table: "Vehicles",
                newName: "Placas");

            migrationBuilder.RenameColumn(
                name: "Long",
                table: "Vehicles",
                newName: "Largo");
        }
    }
}
