using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameSecurityTasksToYardTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SecurityRegistrationPhotos_SecurityTasks_SecurityTaskId",
                table: "SecurityRegistrationPhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_SecurityTasks_SecurityRegistrations_SecurityRegistrationId",
                table: "SecurityTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SecurityTasks",
                table: "SecurityTasks");

            migrationBuilder.RenameTable(
                name: "SecurityTasks",
                newName: "YardTasks");

            migrationBuilder.RenameIndex(
                name: "IX_SecurityTasks_SecurityRegistrationId",
                table: "YardTasks",
                newName: "IX_YardTasks_SecurityRegistrationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_YardTasks",
                table: "YardTasks",
                column: "SecurityTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityRegistrationPhotos_YardTasks_SecurityTaskId",
                table: "SecurityRegistrationPhotos",
                column: "SecurityTaskId",
                principalTable: "YardTasks",
                principalColumn: "SecurityTaskId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_YardTasks_SecurityRegistrations_SecurityRegistrationId",
                table: "YardTasks",
                column: "SecurityRegistrationId",
                principalTable: "SecurityRegistrations",
                principalColumn: "SecurityRegistrationId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SecurityRegistrationPhotos_YardTasks_SecurityTaskId",
                table: "SecurityRegistrationPhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_YardTasks_SecurityRegistrations_SecurityRegistrationId",
                table: "YardTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_YardTasks",
                table: "YardTasks");

            migrationBuilder.RenameTable(
                name: "YardTasks",
                newName: "SecurityTasks");

            migrationBuilder.RenameIndex(
                name: "IX_YardTasks_SecurityRegistrationId",
                table: "SecurityTasks",
                newName: "IX_SecurityTasks_SecurityRegistrationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SecurityTasks",
                table: "SecurityTasks",
                column: "SecurityTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityRegistrationPhotos_SecurityTasks_SecurityTaskId",
                table: "SecurityRegistrationPhotos",
                column: "SecurityTaskId",
                principalTable: "SecurityTasks",
                principalColumn: "SecurityTaskId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityTasks_SecurityRegistrations_SecurityRegistrationId",
                table: "SecurityTasks",
                column: "SecurityRegistrationId",
                principalTable: "SecurityRegistrations",
                principalColumn: "SecurityRegistrationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
