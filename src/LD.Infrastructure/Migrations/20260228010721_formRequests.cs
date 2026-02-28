using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class formRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlmacenFiscal",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "AlmacenId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Backorder",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Distribution",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Etiquetas",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "HeightCm",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "ProfundidadCm",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "WeightCm",
                table: "Locations");

            migrationBuilder.RenameColumn(
                name: "Produccion",
                table: "Warehouses",
                newName: "IsProduction");

            migrationBuilder.RenameColumn(
                name: "Colonia",
                table: "Warehouses",
                newName: "Neighborhood");

            migrationBuilder.RenameColumn(
                name: "UsaLote",
                table: "Projects",
                newName: "ScanQuantity");

            migrationBuilder.RenameColumn(
                name: "UsaFIFO",
                table: "Projects",
                newName: "ScanPartNumber");

            migrationBuilder.RenameColumn(
                name: "Subdimension",
                table: "Projects",
                newName: "ScanDub");

            migrationBuilder.RenameColumn(
                name: "NumeroDeLote",
                table: "Projects",
                newName: "RequireLot");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Projects",
                newName: "ProjectName");

            migrationBuilder.RenameColumn(
                name: "FechaCaducidad",
                table: "Projects",
                newName: "RequireExpirationDate");

            migrationBuilder.RenameColumn(
                name: "TemperaturaControlada",
                table: "Locations",
                newName: "IsShipping");

            migrationBuilder.RenameColumn(
                name: "Fiscal",
                table: "Locations",
                newName: "IsReceipt");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Clients",
                newName: "CommercialAddress");

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Warehouses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WarehouseId",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AddColumn<bool>(
                name: "IsGeneral",
                table: "Locations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsQuarantine",
                table: "Locations",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.AddColumn<int>(
                name: "WarehouseId",
                table: "Locations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxUnitId",
                table: "items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MediumUnitId",
                table: "items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinUnitId",
                table: "items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartNumber",
                table: "items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequestExpirationDate",
                table: "items",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequestLotNumber",
                table: "items",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_WarehouseId",
                table: "Projects",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_WarehouseId",
                table: "Locations",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_items_ProjectId",
                table: "items",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_items_Projects_ProjectId",
                table: "items",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Warehouses_WarehouseId",
                table: "Locations",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Warehouses_WarehouseId",
                table: "Projects",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_items_Projects_ProjectId",
                table: "items");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Warehouses_WarehouseId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Warehouses_WarehouseId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_WarehouseId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Locations_WarehouseId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_items_ProjectId",
                table: "items");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Aisle",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Dimension",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "IsGeneral",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "IsQuarantine",
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
                name: "WarehouseId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "items");

            migrationBuilder.DropColumn(
                name: "MaxUnitId",
                table: "items");

            migrationBuilder.DropColumn(
                name: "MediumUnitId",
                table: "items");

            migrationBuilder.DropColumn(
                name: "MinUnitId",
                table: "items");

            migrationBuilder.DropColumn(
                name: "PartNumber",
                table: "items");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "items");

            migrationBuilder.DropColumn(
                name: "RequestExpirationDate",
                table: "items");

            migrationBuilder.DropColumn(
                name: "RequestLotNumber",
                table: "items");

            migrationBuilder.RenameColumn(
                name: "Neighborhood",
                table: "Warehouses",
                newName: "Colonia");

            migrationBuilder.RenameColumn(
                name: "IsProduction",
                table: "Warehouses",
                newName: "Produccion");

            migrationBuilder.RenameColumn(
                name: "ScanQuantity",
                table: "Projects",
                newName: "UsaLote");

            migrationBuilder.RenameColumn(
                name: "ScanPartNumber",
                table: "Projects",
                newName: "UsaFIFO");

            migrationBuilder.RenameColumn(
                name: "ScanDub",
                table: "Projects",
                newName: "Subdimension");

            migrationBuilder.RenameColumn(
                name: "RequireLot",
                table: "Projects",
                newName: "NumeroDeLote");

            migrationBuilder.RenameColumn(
                name: "RequireExpirationDate",
                table: "Projects",
                newName: "FechaCaducidad");

            migrationBuilder.RenameColumn(
                name: "ProjectName",
                table: "Projects",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "IsShipping",
                table: "Locations",
                newName: "TemperaturaControlada");

            migrationBuilder.RenameColumn(
                name: "IsReceipt",
                table: "Locations",
                newName: "Fiscal");

            migrationBuilder.RenameColumn(
                name: "CommercialAddress",
                table: "Clients",
                newName: "Address");

            migrationBuilder.AddColumn<bool>(
                name: "AlmacenFiscal",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "AlmacenId",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Backorder",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Distribution",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Etiquetas",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Locations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "HeightCm",
                table: "Locations",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ProfundidadCm",
                table: "Locations",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "WeightCm",
                table: "Locations",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
