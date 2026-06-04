using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTablesCyclicinventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CyclicInventories",
                columns: table => new
                {
                    CyclicInventoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuditorUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    AuditorName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_CyclicInventories", x => x.CyclicInventoryId);
                    table.ForeignKey(
                        name: "FK_CyclicInventories_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseId");
                });

            migrationBuilder.CreateTable(
                name: "CyclicInventoryDetails",
                columns: table => new
                {
                    CyclicInventoryDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CyclicInventoryId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    Counted = table.Column<bool>(type: "bit", nullable: false),
                    TheoreticalQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PhysicalQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FirstCountResult = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SecondCountResult = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FinalResult = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PartNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Scanned = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_CyclicInventoryDetails", x => x.CyclicInventoryDetailId);
                    table.ForeignKey(
                        name: "FK_CyclicInventoryDetails_CyclicInventories_CyclicInventoryId",
                        column: x => x.CyclicInventoryId,
                        principalTable: "CyclicInventories",
                        principalColumn: "CyclicInventoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CyclicInventoryDetails_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CyclicInventories_WarehouseId",
                table: "CyclicInventories",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_CyclicInventoryDetails_CyclicInventoryId",
                table: "CyclicInventoryDetails",
                column: "CyclicInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CyclicInventoryDetails_LocationId",
                table: "CyclicInventoryDetails",
                column: "LocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CyclicInventoryDetails");

            migrationBuilder.DropTable(
                name: "CyclicInventories");
        }
    }
}
