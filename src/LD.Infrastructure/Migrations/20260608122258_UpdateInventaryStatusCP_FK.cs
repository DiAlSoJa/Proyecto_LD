using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInventaryStatusCP_FK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH(N'dbo.inventaryStatuses', N'ClientId') IS NULL
BEGIN
    ALTER TABLE [dbo].[inventaryStatuses] ADD [ClientId] int NULL;
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH(N'dbo.inventaryStatuses', N'ProjectId') IS NULL
BEGIN
    ALTER TABLE [dbo].[inventaryStatuses] ADD [ProjectId] int NULL;
END
");

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

            migrationBuilder.Sql(@"
IF NOT EXISTS (
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = N'FK_inventaryStatuses_Clients_ClientId'
      AND parent_object_id = OBJECT_ID(N'dbo.inventaryStatuses')
)
BEGIN
    ALTER TABLE [dbo].[inventaryStatuses] WITH CHECK
    ADD CONSTRAINT [FK_inventaryStatuses_Clients_ClientId]
    FOREIGN KEY ([ClientId]) REFERENCES [dbo].[Clients] ([ClientId]);
END
");

            migrationBuilder.Sql(@"
IF NOT EXISTS (
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = N'FK_inventaryStatuses_Projects_ProjectId'
      AND parent_object_id = OBJECT_ID(N'dbo.inventaryStatuses')
)
BEGIN
    ALTER TABLE [dbo].[inventaryStatuses] WITH CHECK
    ADD CONSTRAINT [FK_inventaryStatuses_Projects_ProjectId]
    FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Projects] ([ProjectId]);
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF EXISTS (
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = N'FK_inventaryStatuses_Clients_ClientId'
      AND parent_object_id = OBJECT_ID(N'dbo.inventaryStatuses')
)
BEGIN
    ALTER TABLE [dbo].[inventaryStatuses] DROP CONSTRAINT [FK_inventaryStatuses_Clients_ClientId];
END
");

            migrationBuilder.Sql(@"
IF EXISTS (
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = N'FK_inventaryStatuses_Projects_ProjectId'
      AND parent_object_id = OBJECT_ID(N'dbo.inventaryStatuses')
)
BEGIN
    ALTER TABLE [dbo].[inventaryStatuses] DROP CONSTRAINT [FK_inventaryStatuses_Projects_ProjectId];
END
");

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

            migrationBuilder.Sql(@"
IF COL_LENGTH(N'dbo.inventaryStatuses', N'ClientId') IS NOT NULL
BEGIN
    ALTER TABLE [dbo].[inventaryStatuses] DROP COLUMN [ClientId];
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH(N'dbo.inventaryStatuses', N'ProjectId') IS NOT NULL
BEGIN
    ALTER TABLE [dbo].[inventaryStatuses] DROP COLUMN [ProjectId];
END
");
        }
    }
}
