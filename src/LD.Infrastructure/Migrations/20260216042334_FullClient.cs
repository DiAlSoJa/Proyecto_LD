using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FullClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ComercialName",
                table: "Clients",
                newName: "CommercialName");

            migrationBuilder.AddColumn<string>(
                name: "BusinessName",
                table: "Clients",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Rfc",
                table: "Clients",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessName",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Rfc",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "CommercialName",
                table: "Clients",
                newName: "ComercialName");
        }
    }
}
