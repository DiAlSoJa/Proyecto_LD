using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCyclicInventoryScans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CyclicInventoryScans",
                columns: table => new
                {
                    CyclicInventoryScanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CyclicInventoryId = table.Column<int>(type: "int", nullable: false),
                    CyclicInventoryDetailId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    StandardId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
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
                    table.PrimaryKey("PK_CyclicInventoryScans", x => x.CyclicInventoryScanId);
                    table.ForeignKey(
                        name: "FK_CyclicInventoryScans_CyclicInventories_CyclicInventoryId",
                        column: x => x.CyclicInventoryId,
                        principalTable: "CyclicInventories",
                        principalColumn: "CyclicInventoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CyclicInventoryScans_CyclicInventoryDetails_CyclicInventoryDetailId",
                        column: x => x.CyclicInventoryDetailId,
                        principalTable: "CyclicInventoryDetails",
                        principalColumn: "CyclicInventoryDetailId");
                    table.ForeignKey(
                        name: "FK_CyclicInventoryScans_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CyclicInventoryScans_CyclicInventoryDetailId",
                table: "CyclicInventoryScans",
                column: "CyclicInventoryDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_CyclicInventoryScans_CyclicInventoryId_CyclicInventoryDetailId_ScannedAt",
                table: "CyclicInventoryScans",
                columns: new[] { "CyclicInventoryId", "CyclicInventoryDetailId", "ScannedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_CyclicInventoryScans_LocationId",
                table: "CyclicInventoryScans",
                column: "LocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CyclicInventoryScans");
        }
    }
}
