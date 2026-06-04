using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AsnDetails_addnewfields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StatusLine",
                table: "AsnReceiptDetails",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MaximumQuantity",
                table: "AsnDetails",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "StandardQuantity",
                table: "AsnDetails",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusLine",
                table: "AsnDetails",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusLine",
                table: "AsnReceiptDetails");

            migrationBuilder.DropColumn(
                name: "MaximumQuantity",
                table: "AsnDetails");

            migrationBuilder.DropColumn(
                name: "StandardQuantity",
                table: "AsnDetails");

            migrationBuilder.DropColumn(
                name: "StatusLine",
                table: "AsnDetails");
        }
    }
}
