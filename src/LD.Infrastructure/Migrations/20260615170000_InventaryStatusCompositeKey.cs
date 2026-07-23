using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(LdProyectDbContext))]
    [Migration("20260615170000_InventaryStatusCompositeKey")]
    public partial class InventaryStatusCompositeKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_inventaryStatuses_ClientId'
      AND object_id = OBJECT_ID(N'dbo.inventaryStatuses')
)
BEGIN
    DROP INDEX [IX_inventaryStatuses_ClientId] ON [dbo].[inventaryStatuses];
END
");

            migrationBuilder.Sql(@"
IF EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_inventaryStatuses_ProjectId'
      AND object_id = OBJECT_ID(N'dbo.inventaryStatuses')
)
BEGIN
    DROP INDEX [IX_inventaryStatuses_ProjectId] ON [dbo].[inventaryStatuses];
END
");

            migrationBuilder.DropPrimaryKey(
                name: "PK_inventaryStatuses",
                table: "inventaryStatuses");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "inventaryStatuses",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "inventaryStatuses",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_inventaryStatuses",
                table: "inventaryStatuses",
                columns: new[] { "InventoryStatusIdS", "ClientId", "ProjectId" });

            migrationBuilder.Sql(@"
IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_inventaryStatuses_ClientId'
      AND object_id = OBJECT_ID(N'dbo.inventaryStatuses')
)
BEGIN
    CREATE INDEX [IX_inventaryStatuses_ClientId] ON [dbo].[inventaryStatuses] ([ClientId]);
END
");

            migrationBuilder.Sql(@"
IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_inventaryStatuses_ProjectId'
      AND object_id = OBJECT_ID(N'dbo.inventaryStatuses')
)
BEGIN
    CREATE INDEX [IX_inventaryStatuses_ProjectId] ON [dbo].[inventaryStatuses] ([ProjectId]);
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_inventaryStatuses_ClientId'
      AND object_id = OBJECT_ID(N'dbo.inventaryStatuses')
)
BEGIN
    DROP INDEX [IX_inventaryStatuses_ClientId] ON [dbo].[inventaryStatuses];
END
");

            migrationBuilder.Sql(@"
IF EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_inventaryStatuses_ProjectId'
      AND object_id = OBJECT_ID(N'dbo.inventaryStatuses')
)
BEGIN
    DROP INDEX [IX_inventaryStatuses_ProjectId] ON [dbo].[inventaryStatuses];
END
");

            migrationBuilder.DropPrimaryKey(
                name: "PK_inventaryStatuses",
                table: "inventaryStatuses");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "inventaryStatuses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "inventaryStatuses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_inventaryStatuses",
                table: "inventaryStatuses",
                column: "InventoryStatusIdS");

            migrationBuilder.Sql(@"
IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_inventaryStatuses_ClientId'
      AND object_id = OBJECT_ID(N'dbo.inventaryStatuses')
)
BEGIN
    CREATE INDEX [IX_inventaryStatuses_ClientId] ON [dbo].[inventaryStatuses] ([ClientId]);
END
");

            migrationBuilder.Sql(@"
IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_inventaryStatuses_ProjectId'
      AND object_id = OBJECT_ID(N'dbo.inventaryStatuses')
)
BEGIN
    CREATE INDEX [IX_inventaryStatuses_ProjectId] ON [dbo].[inventaryStatuses] ([ProjectId]);
END
");
        }
    }
}
