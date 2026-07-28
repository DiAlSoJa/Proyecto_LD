using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExchangeRateToAsnReceiptDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ExchangeRate",
                table: "AsnReceiptDetails",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                table: "AsnReceiptDetails");
        }
    }
}
