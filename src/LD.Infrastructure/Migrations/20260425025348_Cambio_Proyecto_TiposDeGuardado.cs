using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Cambio_Proyecto_TiposDeGuardado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ScanSaveTypes",
                keyColumn: "ScanSaveTypeId",
                keyValue: 1,
                columns: new[] { "Key", "ScanSaveTypeName" },
                values: new object[] { "none", "Ninguno" });

            migrationBuilder.UpdateData(
                table: "ScanSaveTypes",
                keyColumn: "ScanSaveTypeId",
                keyValue: 2,
                columns: new[] { "Key", "ScanSaveTypeName" },
                values: new object[] { "remove_first", "Quitar primeros dígitos" });

            migrationBuilder.InsertData(
                table: "ScanSaveTypes",
                columns: new[] { "ScanSaveTypeId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "Description", "IsActive", "Key", "LastModifiedAt", "LastModifiedByUserId", "ScanSaveTypeName" },
                values: new object[] { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, true, "remove_last", null, null, "Quitar últimos dígitos" });

            migrationBuilder.UpdateData(
                table: "ScanTypes",
                keyColumn: "ScanTypeId",
                keyValue: 1,
                columns: new[] { "Key", "ScanTypeName" },
                values: new object[] { "none", "Ninguno" });

            migrationBuilder.UpdateData(
                table: "ScanTypes",
                keyColumn: "ScanTypeId",
                keyValue: 2,
                columns: new[] { "Key", "ScanTypeName" },
                values: new object[] { "starts_with", "Empieza con" });

            migrationBuilder.UpdateData(
                table: "ScanTypes",
                keyColumn: "ScanTypeId",
                keyValue: 3,
                columns: new[] { "Key", "ScanTypeName" },
                values: new object[] { "length", "Cantidad de dígitos" });

            migrationBuilder.InsertData(
                table: "ScanTypes",
                columns: new[] { "ScanTypeId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "Description", "IsActive", "Key", "LastModifiedAt", "LastModifiedByUserId", "ScanTypeName" },
                values: new object[] { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, true, "less_than", null, null, "Es número menor a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ScanSaveTypes",
                keyColumn: "ScanSaveTypeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ScanTypes",
                keyColumn: "ScanTypeId",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "ScanSaveTypes",
                keyColumn: "ScanSaveTypeId",
                keyValue: 1,
                columns: new[] { "Key", "ScanSaveTypeName" },
                values: new object[] { "remove_first", "Quitar primeros dígitos" });

            migrationBuilder.UpdateData(
                table: "ScanSaveTypes",
                keyColumn: "ScanSaveTypeId",
                keyValue: 2,
                columns: new[] { "Key", "ScanSaveTypeName" },
                values: new object[] { "remove_last", "Quitar últimos dígitos" });

            migrationBuilder.UpdateData(
                table: "ScanTypes",
                keyColumn: "ScanTypeId",
                keyValue: 1,
                columns: new[] { "Key", "ScanTypeName" },
                values: new object[] { "starts_with", "Empieza con" });

            migrationBuilder.UpdateData(
                table: "ScanTypes",
                keyColumn: "ScanTypeId",
                keyValue: 2,
                columns: new[] { "Key", "ScanTypeName" },
                values: new object[] { "length", "Cantidad de dígitos" });

            migrationBuilder.UpdateData(
                table: "ScanTypes",
                keyColumn: "ScanTypeId",
                keyValue: 3,
                columns: new[] { "Key", "ScanTypeName" },
                values: new object[] { "less_than", "Es número menor a" });
        }
    }
}
