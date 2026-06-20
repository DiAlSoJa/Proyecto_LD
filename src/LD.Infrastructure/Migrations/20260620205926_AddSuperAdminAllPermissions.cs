using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSuperAdminAllPermissions : Migration
    {
        private const string SuperAdminRoleId = "87b92599-3be7-4ab5-b19e-9e069e015d4e";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Permisos 86-92 ya existen en la BD desde migraciones previas.
            // Solo asignamos al SuperAdmin los que le faltan: 86, 87, 88, 91, 92
            // (89 y 90 ya los tiene). Usamos IF NOT EXISTS para idempotencia.
            migrationBuilder.Sql($@"
                INSERT INTO Auth.RolePermissions (PermissionId, RoleId, CreatedAt, IsActive)
                SELECT p.PermissionId, '{SuperAdminRoleId}', GETUTCDATE(), 1
                FROM Auth.Permissions p
                WHERE p.PermissionId IN (86, 87, 88, 91, 92)
                  AND NOT EXISTS (
                      SELECT 1 FROM Auth.RolePermissions rp
                      WHERE rp.PermissionId = p.PermissionId
                        AND rp.RoleId = '{SuperAdminRoleId}'
                  )
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                DELETE FROM Auth.RolePermissions
                WHERE RoleId = '{SuperAdminRoleId}'
                  AND PermissionId IN (86, 87, 88, 91, 92)
            ");
        }
    }
}
