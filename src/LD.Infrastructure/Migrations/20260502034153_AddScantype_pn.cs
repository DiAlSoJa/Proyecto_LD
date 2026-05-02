using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddScantype_pn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ScanTypes",
                columns: new[] { "ScanTypeId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "Description", "IsActive", "Key", "LastModifiedAt", "LastModifiedByUserId", "ScanTypeName" },
                values: new object[] { 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, true, "is_part_number", null, null, "Es número de parte" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ScanTypes",
                keyColumn: "ScanTypeId",
                keyValue: 6);
        }
    }
}
