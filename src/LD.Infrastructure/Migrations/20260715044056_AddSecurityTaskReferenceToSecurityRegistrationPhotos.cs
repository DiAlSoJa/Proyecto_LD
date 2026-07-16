using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSecurityTaskReferenceToSecurityRegistrationPhotos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RealizadaPor",
                table: "SecurityRegistrationPhotos",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SecurityTaskId",
                table: "SecurityRegistrationPhotos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SecurityRegistrationPhotos_SecurityTaskId",
                table: "SecurityRegistrationPhotos",
                column: "SecurityTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityRegistrationPhotos_SecurityTasks_SecurityTaskId",
                table: "SecurityRegistrationPhotos",
                column: "SecurityTaskId",
                principalTable: "SecurityTasks",
                principalColumn: "SecurityTaskId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SecurityRegistrationPhotos_SecurityTasks_SecurityTaskId",
                table: "SecurityRegistrationPhotos");

            migrationBuilder.DropIndex(
                name: "IX_SecurityRegistrationPhotos_SecurityTaskId",
                table: "SecurityRegistrationPhotos");

            migrationBuilder.DropColumn(
                name: "RealizadaPor",
                table: "SecurityRegistrationPhotos");

            migrationBuilder.DropColumn(
                name: "SecurityTaskId",
                table: "SecurityRegistrationPhotos");
        }
    }
}
