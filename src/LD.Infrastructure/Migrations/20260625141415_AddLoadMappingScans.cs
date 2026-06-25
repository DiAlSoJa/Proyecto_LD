using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLoadMappingScans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoadMappingScans",
                columns: table => new
                {
                    LoadMappingScanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoadMappingId = table.Column<int>(type: "int", nullable: false),
                    KittingReceiptDetailId = table.Column<int>(type: "int", nullable: true),
                    Side = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StandardId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Result = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    Kitting = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PartNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LotNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ScannedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    table.PrimaryKey("PK_LoadMappingScans", x => x.LoadMappingScanId);
                    table.ForeignKey(
                        name: "FK_LoadMappingScans_KittingIssueDetails_KittingReceiptDetailId",
                        column: x => x.KittingReceiptDetailId,
                        principalTable: "KittingIssueDetails",
                        principalColumn: "KittingReceiptDetailId");
                    table.ForeignKey(
                        name: "FK_LoadMappingScans_LoadMappings_LoadMappingId",
                        column: x => x.LoadMappingId,
                        principalTable: "LoadMappings",
                        principalColumn: "LoadMappingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoadMappingScans_KittingReceiptDetailId",
                table: "LoadMappingScans",
                column: "KittingReceiptDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadMappingScans_LoadMappingId_ScannedAt",
                table: "LoadMappingScans",
                columns: new[] { "LoadMappingId", "ScannedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoadMappingScans");
        }
    }
}
