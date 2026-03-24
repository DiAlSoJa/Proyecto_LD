using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTables_Categories_Keys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_items_CategoryIdS",
                table: "items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ClientId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ProjectId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CategoryIdS",
                table: "items");

            migrationBuilder.RenameColumn(
                name: "CategoryIdS",
                table: "Categories",
                newName: "CategoryName");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_items_CategoryId",
                table: "items",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_items_Categories_CategoryId",
                table: "items",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_items_Categories_CategoryId",
                table: "items");

            migrationBuilder.DropIndex(
                name: "IX_items_CategoryId",
                table: "items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "Categories",
                newName: "CategoryIdS");

            migrationBuilder.AddColumn<string>(
                name: "CategoryIdS",
                table: "items",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Categories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "Categories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "CategoryIdS");

            migrationBuilder.CreateIndex(
                name: "IX_items_CategoryIdS",
                table: "items",
                column: "CategoryIdS");

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
        }
    }
}
