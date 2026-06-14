using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOperationalTaskStatusAndAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CompletedBy",
                table: "OperationalTasks",
                newName: "CompletedByName");

            migrationBuilder.AddColumn<DateTime>(
                name: "AssignedAt",
                table: "OperationalTasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssignedToUserId",
                table: "OperationalTasks",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompletedByUserId",
                table: "OperationalTasks",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "OperationalTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Data migration: tareas completadas (Completed=1) quedan con Status=2 (Completada).
            // Las no completadas quedan con Status=0 (NoAsignada) por el DEFAULT anterior.
            migrationBuilder.Sql("UPDATE OperationalTasks SET Status = 2 WHERE Completed = 1");

            migrationBuilder.CreateIndex(
                name: "IX_OperationalTasks_AssignedToUserId",
                table: "OperationalTasks",
                column: "AssignedToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OperationalTasks_AppUsers_AssignedToUserId",
                table: "OperationalTasks",
                column: "AssignedToUserId",
                principalSchema: "Auth",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OperationalTasks_AppUsers_AssignedToUserId",
                table: "OperationalTasks");

            migrationBuilder.DropIndex(
                name: "IX_OperationalTasks_AssignedToUserId",
                table: "OperationalTasks");

            migrationBuilder.DropColumn(
                name: "AssignedAt",
                table: "OperationalTasks");

            migrationBuilder.DropColumn(
                name: "AssignedToUserId",
                table: "OperationalTasks");

            migrationBuilder.DropColumn(
                name: "CompletedByUserId",
                table: "OperationalTasks");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "OperationalTasks");

            migrationBuilder.RenameColumn(
                name: "CompletedByName",
                table: "OperationalTasks",
                newName: "CompletedBy");
        }
    }
}
