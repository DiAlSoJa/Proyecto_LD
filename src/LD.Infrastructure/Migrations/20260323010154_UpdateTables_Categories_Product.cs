using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTables_Categories_Product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "items");

            migrationBuilder.AlterColumn<string>(
                name: "PartNumber",
                table: "items",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MinUnitId",
                table: "items",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MediumUnitId",
                table: "items",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaxUnitId",
                table: "items",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AlternateEmail",
                table: "items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryIdS",
                table: "items",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Costs",
                table: "items",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryTime",
                table: "items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DistributionList",
                table: "items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBOM",
                table: "items",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTemperatureControlled",
                table: "items",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVMI",
                table: "items",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxUnitValue",
                table: "items",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Maximums",
                table: "items",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MinUnitValue",
                table: "items",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Minimus",
                table: "items",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NotificationRoute",
                table: "items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ProductionFactor",
                table: "items",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductionStatusId",
                table: "items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductionUnitId",
                table: "items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Reorder",
                table: "items",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequestDeclarationNumber",
                table: "items",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequestExchangeRate",
                table: "items",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequestNotificationEmail",
                table: "items",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequestNotificationFiles",
                table: "items",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequestNotificationMax",
                table: "items",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequestNotificationMin",
                table: "items",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequestPurchaseOrder",
                table: "items",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequestReference",
                table: "items",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StandardPackage",
                table: "items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "StandardPackageValue",
                table: "items",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StorageTypeId",
                table: "items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnitIdS",
                table: "items",
                type: "nvarchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "WarehouseFactor",
                table: "items",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Frecuency",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_items_CategoryIdS",
                table: "items",
                column: "CategoryIdS");

            migrationBuilder.CreateIndex(
                name: "IX_items_ClientId",
                table: "items",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_items_StorageTypeId",
                table: "items",
                column: "StorageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_items_UnitIdS",
                table: "items",
                column: "UnitIdS");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ClientId",
                table: "Categories",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ProjectId",
                table: "Categories",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Clients_ClientId",
                table: "Categories",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Projects_ProjectId",
                table: "Categories",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_items_Categories_CategoryIdS",
                table: "items",
                column: "CategoryIdS",
                principalTable: "Categories",
                principalColumn: "CategoryIdS");

            migrationBuilder.AddForeignKey(
                name: "FK_items_Clients_ClientId",
                table: "items",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_items_StorageTypes_StorageTypeId",
                table: "items",
                column: "StorageTypeId",
                principalTable: "StorageTypes",
                principalColumn: "StorageTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_items_Units_UnitIdS",
                table: "items",
                column: "UnitIdS",
                principalTable: "Units",
                principalColumn: "UnitIdS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Clients_ClientId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Projects_ProjectId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_items_Categories_CategoryIdS",
                table: "items");

            migrationBuilder.DropForeignKey(
                name: "FK_items_Clients_ClientId",
                table: "items");

            migrationBuilder.DropForeignKey(
                name: "FK_items_StorageTypes_StorageTypeId",
                table: "items");

            migrationBuilder.DropForeignKey(
                name: "FK_items_Units_UnitIdS",
                table: "items");

            migrationBuilder.DropIndex(
                name: "IX_items_CategoryIdS",
                table: "items");

            migrationBuilder.DropIndex(
                name: "IX_items_ClientId",
                table: "items");

            migrationBuilder.DropIndex(
                name: "IX_items_StorageTypeId",
                table: "items");

            migrationBuilder.DropIndex(
                name: "IX_items_UnitIdS",
                table: "items");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ClientId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ProjectId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "AlternateEmail",
                table: "items");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "items");

            migrationBuilder.DropColumn(
                name: "CategoryIdS",
                table: "items");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "items");

            migrationBuilder.DropColumn(
                name: "Costs",
                table: "items");

            migrationBuilder.DropColumn(
                name: "DeliveryTime",
                table: "items");

            migrationBuilder.DropColumn(
                name: "DistributionList",
                table: "items");

            migrationBuilder.DropColumn(
                name: "IsBOM",
                table: "items");

            migrationBuilder.DropColumn(
                name: "IsTemperatureControlled",
                table: "items");

            migrationBuilder.DropColumn(
                name: "IsVMI",
                table: "items");

            migrationBuilder.DropColumn(
                name: "MaxUnitValue",
                table: "items");

            migrationBuilder.DropColumn(
                name: "Maximums",
                table: "items");

            migrationBuilder.DropColumn(
                name: "MinUnitValue",
                table: "items");

            migrationBuilder.DropColumn(
                name: "Minimus",
                table: "items");

            migrationBuilder.DropColumn(
                name: "NotificationRoute",
                table: "items");

            migrationBuilder.DropColumn(
                name: "ProductionFactor",
                table: "items");

            migrationBuilder.DropColumn(
                name: "ProductionStatusId",
                table: "items");

            migrationBuilder.DropColumn(
                name: "ProductionUnitId",
                table: "items");

            migrationBuilder.DropColumn(
                name: "Reorder",
                table: "items");

            migrationBuilder.DropColumn(
                name: "RequestDeclarationNumber",
                table: "items");

            migrationBuilder.DropColumn(
                name: "RequestExchangeRate",
                table: "items");

            migrationBuilder.DropColumn(
                name: "RequestNotificationEmail",
                table: "items");

            migrationBuilder.DropColumn(
                name: "RequestNotificationFiles",
                table: "items");

            migrationBuilder.DropColumn(
                name: "RequestNotificationMax",
                table: "items");

            migrationBuilder.DropColumn(
                name: "RequestNotificationMin",
                table: "items");

            migrationBuilder.DropColumn(
                name: "RequestPurchaseOrder",
                table: "items");

            migrationBuilder.DropColumn(
                name: "RequestReference",
                table: "items");

            migrationBuilder.DropColumn(
                name: "StandardPackage",
                table: "items");

            migrationBuilder.DropColumn(
                name: "StandardPackageValue",
                table: "items");

            migrationBuilder.DropColumn(
                name: "StorageTypeId",
                table: "items");

            migrationBuilder.DropColumn(
                name: "UnitIdS",
                table: "items");

            migrationBuilder.DropColumn(
                name: "WarehouseFactor",
                table: "items");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Frecuency",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Categories");

            migrationBuilder.AlterColumn<string>(
                name: "PartNumber",
                table: "items",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "MinUnitId",
                table: "items",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MediumUnitId",
                table: "items",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MaxUnitId",
                table: "items",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "items",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
