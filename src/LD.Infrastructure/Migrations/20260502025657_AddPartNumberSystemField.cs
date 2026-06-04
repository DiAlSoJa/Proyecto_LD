using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPartNumberSystemField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SystemFields",
                columns: new[] { "SystemFieldId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "DisplayName", "IsActive", "LastModifiedAt", "LastModifiedByUserId", "Order", "SystemFieldName" },
                values: new object[] { 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Número de Parte", true, null, null, 7, "part_number" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SystemFields",
                keyColumn: "SystemFieldId",
                keyValue: 7);
        }
    }
}
