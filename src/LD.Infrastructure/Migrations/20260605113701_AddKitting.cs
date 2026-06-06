using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddKitting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kittings",
                columns: table => new
                {
                    KittingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PreKittingCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    KittingCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GuideNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Eta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PackagesQty = table.Column<int>(type: "int", nullable: true),
                    IsReturn = table.Column<bool>(type: "bit", nullable: false),
                    IsCustomerMovementRequired = table.Column<bool>(type: "bit", nullable: false),
                    TransportLine = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    VehicleType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DriverName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    VehiclePlate = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    SealNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Contacto = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Colonia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Ciudad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CodigoPostal = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    TipoEntrega = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    FechaProgramada = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
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
                    table.PrimaryKey("PK_Kittings", x => x.KittingId);
                    table.ForeignKey(
                        name: "FK_Kittings_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId");
                    table.ForeignKey(
                        name: "FK_Kittings_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId");
                });

            migrationBuilder.CreateTable(
                name: "KittingDetails",
                columns: table => new
                {
                    KittingDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KittingId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    PartNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    SD = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LotNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomerReference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PurchaseOrder = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustomsDeclarationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StandardQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MaximumQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
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
                    table.PrimaryKey("PK_KittingDetails", x => x.KittingDetailId);
                    table.ForeignKey(
                        name: "FK_KittingDetails_Kittings_KittingId",
                        column: x => x.KittingId,
                        principalTable: "Kittings",
                        principalColumn: "KittingId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KittingDetails_items_ProductId",
                        column: x => x.ProductId,
                        principalTable: "items",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "KittingReceiptDetails",
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
                name: "IX_KittingDetails_KittingId",
                table: "KittingDetails",
                column: "KittingId");

            migrationBuilder.CreateIndex(
                name: "IX_KittingDetails_ProductId",
                table: "KittingDetails",
                column: "ProductId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Kittings_ClientId",
                table: "Kittings",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Kittings_ProjectId",
                table: "Kittings",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KittingReceiptDetails");

            migrationBuilder.DropTable(
                name: "KittingDetails");

            migrationBuilder.DropTable(
                name: "Kittings");
        }
    }
}
