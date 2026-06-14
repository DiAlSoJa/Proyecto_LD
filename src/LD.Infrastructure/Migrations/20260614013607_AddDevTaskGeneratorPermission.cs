using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDevTaskGeneratorPermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Auth",
                table: "Permissions",
                columns: new[] { "PermissionId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "IsActive", "Key", "LastModifiedAt", "LastModifiedByUserId", "ModuleId", "PermissionName" },
                values: new object[] { 90, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, "dev.task-generator", null, null, 23, "Generador de tareas demo" });

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "IsActive", "LastModifiedAt", "LastModifiedByUserId" },
                values: new object[] { 90, "87b92599-3be7-4ab5-b19e-9e069e015d4e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 90, "87b92599-3be7-4ab5-b19e-9e069e015d4e" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 90);
        }
    }
}
