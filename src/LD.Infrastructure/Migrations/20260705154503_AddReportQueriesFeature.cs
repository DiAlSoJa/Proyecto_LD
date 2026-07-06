using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReportQueriesFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReportQueries",
                columns: table => new
                {
                    ReportQueryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SqlQuery = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_ReportQueries", x => x.ReportQueryId);
                });

            // Use key-based idempotent SQL because live databases may already have other permissions
            // occupying identity values 86/87 from runtime seeding.
            migrationBuilder.Sql(@"
DECLARE @ManagePermissionId INT;
SELECT @ManagePermissionId = PermissionId
FROM Auth.Permissions
WHERE [Key] = N'queries.manage';

IF @ManagePermissionId IS NULL
BEGIN
    INSERT INTO Auth.Permissions
        (PermissionName, [Key], ModuleId, CreatedAt, IsActive)
    VALUES
        (N'Administrar consultas', N'queries.manage', 26, '0001-01-01T00:00:00', 1);

    SET @ManagePermissionId = CONVERT(INT, SCOPE_IDENTITY());
END;

IF NOT EXISTS (
    SELECT 1
    FROM Auth.RolePermissions
    WHERE RoleId = '87b92599-3be7-4ab5-b19e-9e069e015d4e'
      AND PermissionId = @ManagePermissionId
)
BEGIN
    INSERT INTO Auth.RolePermissions
        (PermissionId, RoleId, CreatedAt, IsActive)
    VALUES
        (@ManagePermissionId, '87b92599-3be7-4ab5-b19e-9e069e015d4e', '0001-01-01T00:00:00', 1);
END;

DECLARE @ExecutePermissionId INT;
SELECT @ExecutePermissionId = PermissionId
FROM Auth.Permissions
WHERE [Key] = N'queries.execute';

IF @ExecutePermissionId IS NULL
BEGIN
    INSERT INTO Auth.Permissions
        (PermissionName, [Key], ModuleId, CreatedAt, IsActive)
    VALUES
        (N'Ejecutar consultas', N'queries.execute', 26, '0001-01-01T00:00:00', 1);

    SET @ExecutePermissionId = CONVERT(INT, SCOPE_IDENTITY());
END;

IF NOT EXISTS (
    SELECT 1
    FROM Auth.RolePermissions
    WHERE RoleId = '87b92599-3be7-4ab5-b19e-9e069e015d4e'
      AND PermissionId = @ExecutePermissionId
)
BEGIN
    INSERT INTO Auth.RolePermissions
        (PermissionId, RoleId, CreatedAt, IsActive)
    VALUES
        (@ExecutePermissionId, '87b92599-3be7-4ab5-b19e-9e069e015d4e', '0001-01-01T00:00:00', 1);
END;
");

            migrationBuilder.CreateIndex(
                name: "IX_ReportQueries_Name",
                table: "ReportQueries",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportQueries");

            migrationBuilder.Sql(@"
DECLARE @ManagePermissionId INT;
SELECT @ManagePermissionId = PermissionId
FROM Auth.Permissions
WHERE [Key] = N'queries.manage';

IF @ManagePermissionId IS NOT NULL
BEGIN
    DELETE FROM Auth.RolePermissions
    WHERE RoleId = '87b92599-3be7-4ab5-b19e-9e069e015d4e'
      AND PermissionId = @ManagePermissionId;

    DELETE FROM Auth.Permissions
    WHERE PermissionId = @ManagePermissionId;
END;

DECLARE @ExecutePermissionId INT;
SELECT @ExecutePermissionId = PermissionId
FROM Auth.Permissions
WHERE [Key] = N'queries.execute';

IF @ExecutePermissionId IS NOT NULL
BEGIN
    DELETE FROM Auth.RolePermissions
    WHERE RoleId = '87b92599-3be7-4ab5-b19e-9e069e015d4e'
      AND PermissionId = @ExecutePermissionId;

    DELETE FROM Auth.Permissions
    WHERE PermissionId = @ExecutePermissionId;
END;
");
        }
    }
}
