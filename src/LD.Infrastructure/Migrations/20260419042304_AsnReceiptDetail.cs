using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AsnReceiptDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StandardLabelStandarId",
                table: "AsnReceiptDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AsnReceiptDetails_StandardLabelStandarId",
                table: "AsnReceiptDetails",
                column: "StandardLabelStandarId");

            migrationBuilder.AddForeignKey(
                name: "FK_AsnReceiptDetails_StandardLabels_StandardLabelStandarId",
                table: "AsnReceiptDetails",
                column: "StandardLabelStandarId",
                principalTable: "StandardLabels",
                principalColumn: "StandarId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AsnReceiptDetails_StandardLabels_StandardLabelStandarId",
                table: "AsnReceiptDetails");

            migrationBuilder.DropIndex(
                name: "IX_AsnReceiptDetails_StandardLabelStandarId",
                table: "AsnReceiptDetails");

            migrationBuilder.DropColumn(
                name: "StandardLabelStandarId",
                table: "AsnReceiptDetails");
        }
    }
}
