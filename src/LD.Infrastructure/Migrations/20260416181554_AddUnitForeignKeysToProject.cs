using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUnitForeignKeysToProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SaveValue",
                table: "ScanConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StorageArea",
                table: "Projects",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Salida",
                table: "Projects",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ReworkArea",
                table: "Projects",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Entrada",
                table: "Projects",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Entrada",
                table: "Projects",
                column: "Entrada");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ReworkArea",
                table: "Projects",
                column: "ReworkArea");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Salida",
                table: "Projects",
                column: "Salida");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_StorageArea",
                table: "Projects",
                column: "StorageArea");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Units_Entrada",
                table: "Projects",
                column: "Entrada",
                principalTable: "Units",
                principalColumn: "UnitIdS");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Units_ReworkArea",
                table: "Projects",
                column: "ReworkArea",
                principalTable: "Units",
                principalColumn: "UnitIdS");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Units_Salida",
                table: "Projects",
                column: "Salida",
                principalTable: "Units",
                principalColumn: "UnitIdS");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Units_StorageArea",
                table: "Projects",
                column: "StorageArea",
                principalTable: "Units",
                principalColumn: "UnitIdS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Units_Entrada",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Units_ReworkArea",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Units_Salida",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Units_StorageArea",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_Entrada",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ReworkArea",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_Salida",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_StorageArea",
                table: "Projects");

            migrationBuilder.AlterColumn<int>(
                name: "SaveValue",
                table: "ScanConfigurations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "StorageArea",
                table: "Projects",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Salida",
                table: "Projects",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReworkArea",
                table: "Projects",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Entrada",
                table: "Projects",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);
        }
    }
}
