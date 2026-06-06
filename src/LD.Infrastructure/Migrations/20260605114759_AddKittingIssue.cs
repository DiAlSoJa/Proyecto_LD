using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddKittingIssue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KittingReceiptDetails");

            migrationBuilder.CreateTable(
                name: "KittingIssueDetails",
                columns: table => new
                {
                    KittingReceiptDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KittingDetailId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    DeleteRow = table.Column<bool>(type: "bit", nullable: false),
                    StandardId = table.Column<int>(type: "int", nullable: true),
                    PartNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    StandardQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MaximumQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SD = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReceivedQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    LocationCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LotNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PurchaseOrder = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustomsDeclarationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StatusLine = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
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
                    table.PrimaryKey("PK_KittingIssueDetails", x => x.KittingReceiptDetailId);
                    table.ForeignKey(
                        name: "FK_KittingIssueDetails_KittingDetails_KittingDetailId",
                        column: x => x.KittingDetailId,
                        principalTable: "KittingDetails",
                        principalColumn: "KittingDetailId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KittingIssueDetails_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId");
                    table.ForeignKey(
                        name: "FK_KittingIssueDetails_StandardLabels_StandardId",
                        column: x => x.StandardId,
                        principalTable: "StandardLabels",
                        principalColumn: "StandarId");
                    table.ForeignKey(
                        name: "FK_KittingIssueDetails_items_ProductId",
                        column: x => x.ProductId,
                        principalTable: "items",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_KittingIssueDetails_KittingDetailId",
                table: "KittingIssueDetails",
                column: "KittingDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_KittingIssueDetails_LocationId",
                table: "KittingIssueDetails",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_KittingIssueDetails_ProductId",
                table: "KittingIssueDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_KittingIssueDetails_StandardId",
                table: "KittingIssueDetails",
                column: "StandardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KittingIssueDetails");

            migrationBuilder.CreateTable(
                name: "KittingReceiptDetails",
                columns: table => new
                {
                    KittingReceiptDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KittingDetailId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    StandardId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomsDeclarationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeleteRow = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LotNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaximumQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PartNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PurchaseOrder = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReceivedQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SD = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StandardQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    StatusLine = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KittingReceiptDetails", x => x.KittingReceiptDetailId);
                    table.ForeignKey(
                        name: "FK_KittingReceiptDetails_KittingDetails_KittingDetailId",
                        column: x => x.KittingDetailId,
                        principalTable: "KittingDetails",
                        principalColumn: "KittingDetailId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KittingReceiptDetails_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId");
                    table.ForeignKey(
                        name: "FK_KittingReceiptDetails_StandardLabels_StandardId",
                        column: x => x.StandardId,
                        principalTable: "StandardLabels",
                        principalColumn: "StandarId");
                    table.ForeignKey(
                        name: "FK_KittingReceiptDetails_items_ProductId",
                        column: x => x.ProductId,
                        principalTable: "items",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_KittingReceiptDetails_KittingDetailId",
                table: "KittingReceiptDetails",
                column: "KittingDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_KittingReceiptDetails_LocationId",
                table: "KittingReceiptDetails",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_KittingReceiptDetails_ProductId",
                table: "KittingReceiptDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_KittingReceiptDetails_StandardId",
                table: "KittingReceiptDetails",
                column: "StandardId");
        }
    }
}
