using System;
using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    [DbContext(typeof(LdProyectDbContext))]
    [Migration("20260623193000_AddDeliveryOrders")]
    public partial class AddDeliveryOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeliveryOrders",
                columns: table => new
                {
                    DeliveryOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliveryOrderCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    PreDeliveryOrderCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_DeliveryOrders", x => x.DeliveryOrderId);
                    table.ForeignKey(
                        name: "FK_DeliveryOrders_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_DeliveryOrders_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryOrderKittings",
                columns: table => new
                {
                    DeliveryOrderKittingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliveryOrderId = table.Column<int>(type: "int", nullable: false),
                    KittingId = table.Column<int>(type: "int", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_DeliveryOrderKittings", x => x.DeliveryOrderKittingId);
                    table.ForeignKey(
                        name: "FK_DeliveryOrderKittings_DeliveryOrders_DeliveryOrderId",
                        column: x => x.DeliveryOrderId,
                        principalTable: "DeliveryOrders",
                        principalColumn: "DeliveryOrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeliveryOrderKittings_Kittings_KittingId",
                        column: x => x.KittingId,
                        principalTable: "Kittings",
                        principalColumn: "KittingId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOrders_ClientId",
                table: "DeliveryOrders",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOrders_ProjectId",
                table: "DeliveryOrders",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOrderKittings_DeliveryOrderId",
                table: "DeliveryOrderKittings",
                column: "DeliveryOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOrderKittings_KittingId",
                table: "DeliveryOrderKittings",
                column: "KittingId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryOrderKittings");

            migrationBuilder.DropTable(
                name: "DeliveryOrders");
        }
    }
}
