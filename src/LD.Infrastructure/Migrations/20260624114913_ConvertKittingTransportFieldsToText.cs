using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConvertKittingTransportFieldsToText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasCaja",
                table: "Kittings");

            migrationBuilder.DropColumn(
                name: "HasCortina",
                table: "Kittings");

            migrationBuilder.AddColumn<string>(
                name: "Caja",
                table: "Kittings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cortina",
                table: "Kittings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Caja",
                table: "Kittings");

            migrationBuilder.DropColumn(
                name: "Cortina",
                table: "Kittings");

            migrationBuilder.AddColumn<bool>(
                name: "HasCaja",
                table: "Kittings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasCortina",
                table: "Kittings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
