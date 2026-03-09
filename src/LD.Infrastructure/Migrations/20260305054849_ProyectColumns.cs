using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProyectColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ScanQuantity",
                table: "Projects",
                newName: "ShipmentNotificationEnabled");

            migrationBuilder.RenameColumn(
                name: "ScanPartNumber",
                table: "Projects",
                newName: "RequiresLabels");

            migrationBuilder.RenameColumn(
                name: "ScanDub",
                table: "Projects",
                newName: "ReciveRequired");

            migrationBuilder.RenameColumn(
                name: "RequireLot",
                table: "Projects",
                newName: "ReceiptNotificationEnabled");

            migrationBuilder.RenameColumn(
                name: "RequireExpirationDate",
                table: "Projects",
                newName: "IsFiscalWarehouse");

            migrationBuilder.AddColumn<bool>(
                name: "AllowsBackorder",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AllowsOversizedItems",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AsnNumber",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AsnPrefix",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AutoPicking",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryOrderNumber",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryOrderPrefix",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DoNumber",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DoPrefix",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Entrada",
                table: "Projects",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "InternalNotificationEnabled",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "InternalNotificationMethod",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDistributionArea",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "KittingNumber",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KittingPrefix",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NormalHrs",
                table: "Projects",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiptNotificationMethod",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReworkArea",
                table: "Projects",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Salida",
                table: "Projects",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShipmentNotificationMethod",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StorageArea",
                table: "Projects",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StorageTypeId",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "UrgentHrs",
                table: "Projects",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Rfc",
                table: "ClientFiscalData",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FiscalAddress",
                table: "ClientFiscalData",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "ClientFiscalData",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "BusinessName",
                table: "ClientFiscalData",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "StorageTypes",
                columns: table => new
                {
                    StorageTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_StorageTypes", x => x.StorageTypeId);
                });

            migrationBuilder.InsertData(
                table: "StorageTypes",
                columns: new[] { "StorageTypeId", "Code", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "IsActive", "LastModifiedAt", "LastModifiedByUserId", "Name" },
                values: new object[,]
                {
                    { 1, "FIFO", new DateTime(2026, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", null, null, true, null, null, "First In - First Out" },
                    { 2, "LIFO", new DateTime(2026, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", null, null, true, null, null, "Last In - First Out" },
                    { 3, "LOT", new DateTime(2026, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", null, null, true, null, null, "Número de Lote" },
                    { 4, "EXP", new DateTime(2026, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", null, null, true, null, null, "Fecha de Caducidad" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_StorageTypeId",
                table: "Projects",
                column: "StorageTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_StorageTypes_StorageTypeId",
                table: "Projects",
                column: "StorageTypeId",
                principalTable: "StorageTypes",
                principalColumn: "StorageTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_StorageTypes_StorageTypeId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "StorageTypes");

            migrationBuilder.DropIndex(
                name: "IX_Projects_StorageTypeId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "AllowsBackorder",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "AllowsOversizedItems",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "AsnNumber",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "AsnPrefix",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "AutoPicking",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "DeliveryOrderNumber",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "DeliveryOrderPrefix",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "DoNumber",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "DoPrefix",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Entrada",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "InternalNotificationEnabled",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "InternalNotificationMethod",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsDistributionArea",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "KittingNumber",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "KittingPrefix",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "NormalHrs",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ReceiptNotificationMethod",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ReworkArea",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Salida",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ShipmentNotificationMethod",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "StorageArea",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "StorageTypeId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "UrgentHrs",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "ShipmentNotificationEnabled",
                table: "Projects",
                newName: "ScanQuantity");

            migrationBuilder.RenameColumn(
                name: "RequiresLabels",
                table: "Projects",
                newName: "ScanPartNumber");

            migrationBuilder.RenameColumn(
                name: "ReciveRequired",
                table: "Projects",
                newName: "ScanDub");

            migrationBuilder.RenameColumn(
                name: "ReceiptNotificationEnabled",
                table: "Projects",
                newName: "RequireLot");

            migrationBuilder.RenameColumn(
                name: "IsFiscalWarehouse",
                table: "Projects",
                newName: "RequireExpirationDate");

            migrationBuilder.AlterColumn<string>(
                name: "Rfc",
                table: "ClientFiscalData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FiscalAddress",
                table: "ClientFiscalData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "ClientFiscalData",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "BusinessName",
                table: "ClientFiscalData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);
        }
    }
}
