using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class userCategoryFamilies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.AddColumn<int>(
                name: "FamilyId",
                table: "items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Height",
                table: "items",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Length",
                table: "items",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Weight",
                table: "items",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Width",
                table: "items",
                type: "decimal(18,2)",
                nullable: true);*/

          /*  migrationBuilder.CreateTable(
                name: "Families",
                columns: table => new
                {
                    FamilyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FamilyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_Families", x => x.FamilyId);
                    table.ForeignKey(
                        name: "FK_Families_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId");
                    table.ForeignKey(
                        name: "FK_Families_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId");
                });*/

            migrationBuilder.CreateTable(
                name: "UserWarehouses",
                columns: table => new
                {
                    UserWarehouseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    WarehouseId = table.Column<int>(type: "int", nullable: true),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    WarehouseId1 = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_UserWarehouses", x => x.UserWarehouseId);
                    table.ForeignKey(
                        name: "FK_UserWarehouses_AppUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalSchema: "Auth",
                        principalTable: "AppUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserWarehouses_AppUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "Auth",
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserWarehouses_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserWarehouses_Warehouses_WarehouseId1",
                        column: x => x.WarehouseId1,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseId");
                });

         /*   migrationBuilder.CreateIndex(
                name: "IX_items_FamilyId",
                table: "items",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_Families_ClientId",
                table: "Families",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Families_ProjectId",
                table: "Families",
                column: "ProjectId");*/

            migrationBuilder.CreateIndex(
                name: "IX_UserWarehouses_ApplicationUserId",
                table: "UserWarehouses",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWarehouses_UserId",
                table: "UserWarehouses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWarehouses_WarehouseId",
                table: "UserWarehouses",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWarehouses_WarehouseId1",
                table: "UserWarehouses",
                column: "WarehouseId1");
/*
            migrationBuilder.AddForeignKey(
                name: "FK_items_Families_FamilyId",
                table: "items",
                column: "FamilyId",
                principalTable: "Families",
                principalColumn: "FamilyId");*/
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           /* migrationBuilder.DropForeignKey(
                name: "FK_items_Families_FamilyId",
                table: "items");

            migrationBuilder.DropTable(
                name: "Families");**/

            migrationBuilder.DropTable(
                name: "UserWarehouses");

           /* migrationBuilder.DropIndex(
                name: "IX_items_FamilyId",
                table: "items");

            migrationBuilder.DropColumn(
                name: "FamilyId",
                table: "items");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "items");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "items");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "items");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "items");*/
        }
    }
}
