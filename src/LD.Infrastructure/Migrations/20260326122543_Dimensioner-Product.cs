using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DimensionerProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DimensionerId",
                table: "items",
                type: "nvarchar(20)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Dimensioner",
                columns: table => new
                {
                    DimensionerId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Width = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Length = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dimensioner", x => x.DimensionerId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_items_DimensionerId",
                table: "items",
                column: "DimensionerId");

            migrationBuilder.AddForeignKey(
                name: "FK_items_Dimensioner_DimensionerId",
                table: "items",
                column: "DimensionerId",
                principalTable: "Dimensioner",
                principalColumn: "DimensionerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_items_Dimensioner_DimensionerId",
                table: "items");

            migrationBuilder.DropTable(
                name: "Dimensioner");

            migrationBuilder.DropIndex(
                name: "IX_items_DimensionerId",
                table: "items");

            migrationBuilder.DropColumn(
                name: "DimensionerId",
                table: "items");
        }
    }
}
