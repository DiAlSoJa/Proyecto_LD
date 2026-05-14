using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class usersMobile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Auth",
                table: "AppUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "IsActive", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "f78a2f0d-32f4-4d66-9db8-0f69f5d3u101", 0, "00000000-0000-0000-0000-000000000101", "checklist.mobile@ld.com", true, "Checklist Mobile", true, false, null, "CHECKLIST.MOBILE@LD.COM", "CHECKLIST", "AQAAAAIAAYagAAAAELwhYiHkLhnB8GG70zbiuUeHdrzvTuYGbLTFm4kwRZo9h6aUhKdbe49Ka2+WdRbkoA==", null, false, "STATIC-SECURITY-STAMP-CHECKLIST", false, "checklist" },
                    { "f78a2f0d-32f4-4d66-9db8-0f69f5d3u102", 0, "00000000-0000-0000-0000-000000000102", "security.mobile@ld.com", true, "Security Mobile", true, false, null, "SECURITY.MOBILE@LD.COM", "SECURITY", "AQAAAAIAAYagAAAAELwhYiHkLhnB8GG70zbiuUeHdrzvTuYGbLTFm4kwRZo9h6aUhKdbe49Ka2+WdRbkoA==", null, false, "STATIC-SECURITY-STAMP-SECURITY", false, "security" },
                    { "f78a2f0d-32f4-4d66-9db8-0f69f5d3u103", 0, "00000000-0000-0000-0000-000000000103", "controlpatio.mobile@ld.com", true, "Control Patio Mobile", true, false, null, "CONTROLPATIO.MOBILE@LD.COM", "CONTROLPATIO", "AQAAAAIAAYagAAAAELwhYiHkLhnB8GG70zbiuUeHdrzvTuYGbLTFm4kwRZo9h6aUhKdbe49Ka2+WdRbkoA==", null, false, "STATIC-SECURITY-STAMP-CONTROLPATIO", false, "controlpatio" }
                });

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "f78a2f0d-32f4-4d66-9db8-0f69f5d3f101", "1", "ChecklistMobile", "CHECKLISTMOBILE" },
                    { "f78a2f0d-32f4-4d66-9db8-0f69f5d3f102", "1", "SecurityMobile", "SECURITYMOBILE" },
                    { "f78a2f0d-32f4-4d66-9db8-0f69f5d3f103", "1", "ControlPatioMobile", "CONTROLPATIOMOBILE" }
                });

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "IsActive", "LastModifiedAt", "LastModifiedByUserId" },
                values: new object[,]
                {
                    { 23, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f101", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 59, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f101", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 60, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f101", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 61, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f101", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 62, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f101", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 63, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f101", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 43, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f102", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 44, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f102", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 45, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f102", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 46, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f102", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 47, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f102", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 48, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f102", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 49, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f102", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 85, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f102", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null },
                    { 24, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f103", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, true, null, null }
                });

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "f78a2f0d-32f4-4d66-9db8-0f69f5d3f101", "f78a2f0d-32f4-4d66-9db8-0f69f5d3u101" },
                    { "f78a2f0d-32f4-4d66-9db8-0f69f5d3f102", "f78a2f0d-32f4-4d66-9db8-0f69f5d3u102" },
                    { "f78a2f0d-32f4-4d66-9db8-0f69f5d3f103", "f78a2f0d-32f4-4d66-9db8-0f69f5d3u103" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 23, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f101" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 59, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f101" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 60, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f101" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 61, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f101" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 62, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f101" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 63, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f101" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 43, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f102" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 44, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f102" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 45, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f102" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 46, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f102" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 47, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f102" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 48, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f102" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 49, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f102" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 85, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f102" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 24, "f78a2f0d-32f4-4d66-9db8-0f69f5d3f103" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "f78a2f0d-32f4-4d66-9db8-0f69f5d3f101", "f78a2f0d-32f4-4d66-9db8-0f69f5d3u101" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "f78a2f0d-32f4-4d66-9db8-0f69f5d3f102", "f78a2f0d-32f4-4d66-9db8-0f69f5d3u102" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "f78a2f0d-32f4-4d66-9db8-0f69f5d3f103", "f78a2f0d-32f4-4d66-9db8-0f69f5d3u103" });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: "f78a2f0d-32f4-4d66-9db8-0f69f5d3u101");

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: "f78a2f0d-32f4-4d66-9db8-0f69f5d3u102");

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: "f78a2f0d-32f4-4d66-9db8-0f69f5d3u103");

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "f78a2f0d-32f4-4d66-9db8-0f69f5d3f101");

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "f78a2f0d-32f4-4d66-9db8-0f69f5d3f102");

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "f78a2f0d-32f4-4d66-9db8-0f69f5d3f103");
        }
    }
}
