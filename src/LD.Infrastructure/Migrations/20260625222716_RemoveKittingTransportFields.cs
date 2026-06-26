using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveKittingTransportFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF COL_LENGTH('Kittings', 'Caja') IS NOT NULL
                    ALTER TABLE [Kittings] DROP COLUMN [Caja];
                """);

            migrationBuilder.Sql("""
                IF COL_LENGTH('Kittings', 'Cortina') IS NOT NULL
                    ALTER TABLE [Kittings] DROP COLUMN [Cortina];
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF COL_LENGTH('Kittings', 'Caja') IS NULL
                    ALTER TABLE [Kittings] ADD [Caja] nvarchar(100) NULL;
                """);

            migrationBuilder.Sql("""
                IF COL_LENGTH('Kittings', 'Cortina') IS NULL
                    ALTER TABLE [Kittings] ADD [Cortina] nvarchar(100) NULL;
                """);
        }
    }
}
