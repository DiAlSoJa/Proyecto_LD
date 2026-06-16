using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWarehouseTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WarehouseTasks",
                columns: table => new
                {
                    WarehouseTaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WarehouseId = table.Column<int>(type: "int", nullable: true),
                    Priority = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Activity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Photo1Path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Photo2Path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Photo3Path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Photo4Path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ResolutionObservations = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ResolvedPhoto1Path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ResolvedPhoto2Path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ResolvedPhoto3Path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ResolvedPhoto4Path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AssignedToUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CompletedByName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
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
                    table.PrimaryKey("PK_WarehouseTasks", x => x.WarehouseTaskId);
                    table.ForeignKey(
                        name: "FK_WarehouseTasks_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTasks_WarehouseId",
                table: "WarehouseTasks",
                column: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WarehouseTasks");
        }
    }
}
