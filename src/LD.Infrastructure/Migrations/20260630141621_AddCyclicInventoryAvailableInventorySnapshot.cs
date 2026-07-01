using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCyclicInventoryAvailableInventorySnapshot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentLocation",
                table: "CyclicInventoryScans",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "InventoryNotAvailable",
                table: "CyclicInventoryScans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCorrectScan",
                table: "CyclicInventoryScans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsInAnotherLocation",
                table: "CyclicInventoryScans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TakeNumber",
                table: "CyclicInventoryDetails",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "CyclicInventoryAvailableInventories",
                columns: table => new
                {
                    CyclicInventoryAvailableInventoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CyclicInventoryId = table.Column<int>(type: "int", nullable: false),
                    CyclicInventoryDetailId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    TakeNumber = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    AvailableInventoryId = table.Column<int>(type: "int", nullable: true),
                    StandardId = table.Column<int>(type: "int", nullable: true),
                    StandardIdCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    PartNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    LotNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AvailableReference = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    PurchaseOrder = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustomsDeclarationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DocumentId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StatusId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    SD = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AvailableStatus = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Qty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Supply = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FinalAvailable = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    table.PrimaryKey("PK_CyclicInventoryAvailableInventories", x => x.CyclicInventoryAvailableInventoryId);
                    table.ForeignKey(
                        name: "FK_CyclicInventoryAvailableInventories_AvailableInventories_AvailableInventoryId",
                        column: x => x.AvailableInventoryId,
                        principalTable: "AvailableInventories",
                        principalColumn: "AvailableInventoryId");
                    table.ForeignKey(
                        name: "FK_CyclicInventoryAvailableInventories_CyclicInventories_CyclicInventoryId",
                        column: x => x.CyclicInventoryId,
                        principalTable: "CyclicInventories",
                        principalColumn: "CyclicInventoryId");
                    table.ForeignKey(
                        name: "FK_CyclicInventoryAvailableInventories_CyclicInventoryDetails_CyclicInventoryDetailId",
                        column: x => x.CyclicInventoryDetailId,
                        principalTable: "CyclicInventoryDetails",
                        principalColumn: "CyclicInventoryDetailId");
                    table.ForeignKey(
                        name: "FK_CyclicInventoryAvailableInventories_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId");
                    table.ForeignKey(
                        name: "FK_CyclicInventoryAvailableInventories_StandardLabels_StandardId",
                        column: x => x.StandardId,
                        principalTable: "StandardLabels",
                        principalColumn: "StandarId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CyclicInventoryAvailableInventories_AvailableInventoryId",
                table: "CyclicInventoryAvailableInventories",
                column: "AvailableInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CyclicInventoryAvailableInventories_CyclicInventoryDetailId_TakeNumber_StandardIdCode",
                table: "CyclicInventoryAvailableInventories",
                columns: new[] { "CyclicInventoryDetailId", "TakeNumber", "StandardIdCode" });

            migrationBuilder.CreateIndex(
                name: "IX_CyclicInventoryAvailableInventories_CyclicInventoryId",
                table: "CyclicInventoryAvailableInventories",
                column: "CyclicInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CyclicInventoryAvailableInventories_LocationId",
                table: "CyclicInventoryAvailableInventories",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_CyclicInventoryAvailableInventories_StandardId",
                table: "CyclicInventoryAvailableInventories",
                column: "StandardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CyclicInventoryAvailableInventories");

            migrationBuilder.DropColumn(
                name: "CurrentLocation",
                table: "CyclicInventoryScans");

            migrationBuilder.DropColumn(
                name: "InventoryNotAvailable",
                table: "CyclicInventoryScans");

            migrationBuilder.DropColumn(
                name: "IsCorrectScan",
                table: "CyclicInventoryScans");

            migrationBuilder.DropColumn(
                name: "IsInAnotherLocation",
                table: "CyclicInventoryScans");

            migrationBuilder.DropColumn(
                name: "TakeNumber",
                table: "CyclicInventoryDetails");
        }
    }
}
