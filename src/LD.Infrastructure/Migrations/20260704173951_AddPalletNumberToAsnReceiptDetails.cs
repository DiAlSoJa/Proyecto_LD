using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPalletNumberToAsnReceiptDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AsnReceiptDetails_AsnDetailId",
                table: "AsnReceiptDetails");

            migrationBuilder.AddColumn<int>(
                name: "PalletNumber",
                table: "AsnReceiptDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(@"
WITH RankedReceipts AS (
    SELECT
        AsnReceiptDetailId,
        ROW_NUMBER() OVER (PARTITION BY AsnDetailId ORDER BY AsnReceiptDetailId) AS NewPalletNumber
    FROM AsnReceiptDetails
)
UPDATE ar
SET PalletNumber = rr.NewPalletNumber
FROM AsnReceiptDetails ar
INNER JOIN RankedReceipts rr ON ar.AsnReceiptDetailId = rr.AsnReceiptDetailId;
");

            migrationBuilder.CreateIndex(
                name: "IX_AsnReceiptDetails_AsnDetailId_PalletNumber",
                table: "AsnReceiptDetails",
                columns: new[] { "AsnDetailId", "PalletNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AsnReceiptDetails_AsnDetailId_PalletNumber",
                table: "AsnReceiptDetails");

            migrationBuilder.DropColumn(
                name: "PalletNumber",
                table: "AsnReceiptDetails");

            migrationBuilder.CreateIndex(
                name: "IX_AsnReceiptDetails_AsnDetailId",
                table: "AsnReceiptDetails",
                column: "AsnDetailId");
        }
    }
}
