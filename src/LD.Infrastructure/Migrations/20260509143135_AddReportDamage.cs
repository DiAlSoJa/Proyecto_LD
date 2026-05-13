using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReportDamage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DamageReports",
                columns: table => new
                {
                    DamageReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AvailableInventoryId = table.Column<int>(type: "int", nullable: true),
                    StandardId = table.Column<int>(type: "int", nullable: true),
                    StandardIdCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    WarehouseId = table.Column<int>(type: "int", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    PartNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CurrentStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ReceivedQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AvailableQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Warehouse = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Project = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Client = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Asn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReceptionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InventoryState = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DamageType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NewStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Photo1Path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Photo2Path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Photo3Path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Photo4Path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReportedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    ReportedByName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
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
                    table.PrimaryKey("PK_DamageReports", x => x.DamageReportId);
                    table.ForeignKey(
                        name: "FK_DamageReports_AvailableInventories_AvailableInventoryId",
                        column: x => x.AvailableInventoryId,
                        principalTable: "AvailableInventories",
                        principalColumn: "AvailableInventoryId");
                    table.ForeignKey(
                        name: "FK_DamageReports_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId");
                    table.ForeignKey(
                        name: "FK_DamageReports_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId");
                    table.ForeignKey(
                        name: "FK_DamageReports_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId");
                    table.ForeignKey(
                        name: "FK_DamageReports_StandardLabels_StandardId",
                        column: x => x.StandardId,
                        principalTable: "StandardLabels",
                        principalColumn: "StandarId");
                    table.ForeignKey(
                        name: "FK_DamageReports_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseId");
                    table.ForeignKey(
                        name: "FK_DamageReports_items_ProductId",
                        column: x => x.ProductId,
                        principalTable: "items",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DamageReports_AvailableInventoryId",
                table: "DamageReports",
                column: "AvailableInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DamageReports_ClientId",
                table: "DamageReports",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_DamageReports_LocationId",
                table: "DamageReports",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_DamageReports_ProductId",
                table: "DamageReports",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_DamageReports_ProjectId",
                table: "DamageReports",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_DamageReports_StandardId",
                table: "DamageReports",
                column: "StandardId");

            migrationBuilder.CreateIndex(
                name: "IX_DamageReports_WarehouseId",
                table: "DamageReports",
                column: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DamageReports");
        }
    }
}
