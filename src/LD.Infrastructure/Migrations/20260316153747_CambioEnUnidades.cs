using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CambioEnUnidades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Units",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_Clave",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "Clave",
                table: "Units");

            migrationBuilder.AddColumn<string>(
                name: "UnitIdS",
                table: "Units",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Units",
                table: "Units",
                column: "UnitIdS");

            migrationBuilder.CreateIndex(
                name: "IX_Units_UnitIdS",
                table: "Units",
                column: "UnitIdS",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Units",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_UnitIdS",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "UnitIdS",
                table: "Units");

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "Units",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Clave",
                table: "Units",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Units",
                table: "Units",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_Clave",
                table: "Units",
                column: "Clave",
                unique: true);
        }
    }
}
