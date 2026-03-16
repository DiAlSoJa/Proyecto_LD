using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Domingo15Marzo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdClient",
                table: "inventaryStatuses");

            migrationBuilder.CreateIndex(
                name: "IX_Units_Clave",
                table: "Units",
                column: "Clave",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Units_Clave",
                table: "Units");

            migrationBuilder.AddColumn<int>(
                name: "IdClient",
                table: "inventaryStatuses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
