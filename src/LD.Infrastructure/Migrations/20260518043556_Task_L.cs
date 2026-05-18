using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Task_L : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResolutionObservations",
                table: "OperationalTasks",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResolvedPhoto1Path",
                table: "OperationalTasks",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResolvedPhoto2Path",
                table: "OperationalTasks",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResolvedPhoto3Path",
                table: "OperationalTasks",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResolvedPhoto4Path",
                table: "OperationalTasks",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResolutionObservations",
                table: "OperationalTasks");

            migrationBuilder.DropColumn(
                name: "ResolvedPhoto1Path",
                table: "OperationalTasks");

            migrationBuilder.DropColumn(
                name: "ResolvedPhoto2Path",
                table: "OperationalTasks");

            migrationBuilder.DropColumn(
                name: "ResolvedPhoto3Path",
                table: "OperationalTasks");

            migrationBuilder.DropColumn(
                name: "ResolvedPhoto4Path",
                table: "OperationalTasks");
        }
    }
}
