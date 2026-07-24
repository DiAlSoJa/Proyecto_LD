using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAsnFolioCaptures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AsnFolioCaptures",
                columns: table => new
                {
                    AsnFolioCaptureId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AsnId = table.Column<int>(type: "int", nullable: false),
                    AsnDetailId = table.Column<int>(type: "int", nullable: true),
                    SourceFileName = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: true),
                    SourceLineNumber = table.Column<int>(type: "int", nullable: false),
                    GuideNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PartNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LotNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SourceStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
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
                    table.PrimaryKey("PK_AsnFolioCaptures", x => x.AsnFolioCaptureId);
                    table.ForeignKey(
                        name: "FK_AsnFolioCaptures_AsnDetails_AsnDetailId",
                        column: x => x.AsnDetailId,
                        principalTable: "AsnDetails",
                        principalColumn: "AsnDetailId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AsnFolioCaptures_Asns_AsnId",
                        column: x => x.AsnId,
                        principalTable: "Asns",
                        principalColumn: "AsnId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AsnFolioCaptures_AsnDetailId",
                table: "AsnFolioCaptures",
                column: "AsnDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_AsnFolioCaptures_AsnId_CreatedAt",
                table: "AsnFolioCaptures",
                columns: new[] { "AsnId", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AsnFolioCaptures");
        }
    }
}
