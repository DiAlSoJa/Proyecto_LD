using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStandardIdSystemFieldv : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SystemFields",
                columns: new[] { "SystemFieldId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "DisplayName", "IsActive", "LastModifiedAt", "LastModifiedByUserId", "Order", "SystemFieldName" },
                values: new object[] { 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "StandardId", true, null, null, 6, "standard_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SystemFields",
                keyColumn: "SystemFieldId",
                keyValue: 6);
        }
    }
}
