using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTruckTypeCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TruckTypes",
                columns: table => new
                {
                    TruckTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
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
                    table.PrimaryKey("PK_TruckTypes", x => x.TruckTypeId);
                });

            migrationBuilder.InsertData(
                table: "TruckTypes",
                columns: new[] { "TruckTypeId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "IsActive", "LastModifiedAt", "LastModifiedByUserId", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 7, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", null, null, true, null, null, "Caja" },
                    { 2, new DateTime(2026, 7, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", null, null, true, null, null, "Tractor" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TruckTypes_Name",
                table: "TruckTypes",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TruckTypes");
        }
    }
}
