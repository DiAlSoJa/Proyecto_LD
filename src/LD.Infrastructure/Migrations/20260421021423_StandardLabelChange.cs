using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StandardLabelChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StandarIdSequences_Year",
                table: "StandarIdSequences");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "StandarIdSequences");

            migrationBuilder.AddColumn<DateTime>(
                name: "SequenceDate",
                table: "StandarIdSequences",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_StandarIdSequences_SequenceDate",
                table: "StandarIdSequences",
                column: "SequenceDate",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StandarIdSequences_SequenceDate",
                table: "StandarIdSequences");

            migrationBuilder.DropColumn(
                name: "SequenceDate",
                table: "StandarIdSequences");

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "StandarIdSequences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StandarIdSequences_Year",
                table: "StandarIdSequences",
                column: "Year",
                unique: true);
        }
    }
}
