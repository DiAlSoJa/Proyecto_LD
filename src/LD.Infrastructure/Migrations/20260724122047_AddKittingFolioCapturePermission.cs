using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddKittingFolioCapturePermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DECLARE @PermissionId INT;

                SELECT @PermissionId = PermissionId
                FROM Auth.Permissions
                WHERE [Key] = N'kitting-folio-capture.access';

                IF @PermissionId IS NULL
                BEGIN
                    INSERT INTO Auth.Permissions
                        (PermissionName, [Key], ModuleId, CreatedAt, IsActive)
                    VALUES
                        (N'Captura de folios Kitting', N'kitting-folio-capture.access', 17, GETUTCDATE(), 1);

                    SET @PermissionId = CONVERT(INT, SCOPE_IDENTITY());
                END;

                IF NOT EXISTS (
                    SELECT 1
                    FROM Auth.RolePermissions
                    WHERE RoleId = '87b92599-3be7-4ab5-b19e-9e069e015d4e'
                      AND PermissionId = @PermissionId
                )
                BEGIN
                    INSERT INTO Auth.RolePermissions
                        (PermissionId, RoleId, CreatedAt, IsActive)
                    VALUES
                        (@PermissionId, '87b92599-3be7-4ab5-b19e-9e069e015d4e', GETUTCDATE(), 1);
                END;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DECLARE @PermissionId INT;

                SELECT @PermissionId = PermissionId
                FROM Auth.Permissions
                WHERE [Key] = N'kitting-folio-capture.access';

                IF @PermissionId IS NOT NULL
                BEGIN
                    DELETE FROM Auth.RolePermissions
                    WHERE PermissionId = @PermissionId;

                    DELETE FROM Auth.Permissions
                    WHERE PermissionId = @PermissionId;
                END;
                """);
        }
    }
}
