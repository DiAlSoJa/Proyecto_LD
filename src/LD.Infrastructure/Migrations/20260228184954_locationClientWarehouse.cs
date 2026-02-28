using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class locationClientWarehouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WarehouseNumber",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "Aisle",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Dimension",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "LocationCode",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Rack",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "WarehouseCode",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "BusinessName",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Fax",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Rfc",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "IsShipping",
                table: "Locations",
                newName: "IsSencillo");

            migrationBuilder.RenameColumn(
                name: "IsReceipt",
                table: "Locations",
                newName: "IsReciboYEmbarque");

            migrationBuilder.RenameColumn(
                name: "IsQuarantine",
                table: "Locations",
                newName: "IsRack");

            migrationBuilder.AddColumn<decimal>(
                name: "Depth",
                table: "Locations",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasControlledTemperature",
                table: "Locations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasCortina",
                table: "Locations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasPaso",
                table: "Locations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Height",
                table: "Locations",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompartido",
                table: "Locations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCuarentena",
                table: "Locations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDoble",
                table: "Locations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEmbarque",
                table: "Locations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFiscal",
                table: "Locations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Width",
                table: "Locations",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClientFiscalData",
                columns: table => new
                {
                    ClientFiscalDataId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    BusinessName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rfc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FiscalAddres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Neightbourhoud = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientFiscalData", x => x.ClientFiscalDataId);
                    table.ForeignKey(
                        name: "FK_ClientFiscalData_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientFiscalData_ClientId",
                table: "ClientFiscalData",
                column: "ClientId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientFiscalData");

            migrationBuilder.DropColumn(
                name: "Depth",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "HasControlledTemperature",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "HasCortina",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "HasPaso",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "IsCompartido",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "IsCuarentena",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "IsDoble",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "IsEmbarque",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "IsFiscal",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Locations");

            migrationBuilder.RenameColumn(
                name: "IsSencillo",
                table: "Locations",
                newName: "IsShipping");

            migrationBuilder.RenameColumn(
                name: "IsReciboYEmbarque",
                table: "Locations",
                newName: "IsReceipt");

            migrationBuilder.RenameColumn(
                name: "IsRack",
                table: "Locations",
                newName: "IsQuarantine");

            migrationBuilder.AddColumn<string>(
                name: "WarehouseNumber",
                table: "Warehouses",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Aisle",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Dimension",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Level",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationCode",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Rack",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WarehouseCode",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BusinessName",
                table: "Clients",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fax",
                table: "Clients",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Rfc",
                table: "Clients",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: true);
        }
    }
}
