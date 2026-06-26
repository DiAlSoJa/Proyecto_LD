using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddKittingCortinaCaja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF COL_LENGTH('Asns', 'Caja') IS NOT NULL
                    ALTER TABLE [Asns] DROP COLUMN [Caja];
                """);

            migrationBuilder.Sql("""
                IF COL_LENGTH('Asns', 'Cortina') IS NOT NULL
                    ALTER TABLE [Asns] DROP COLUMN [Cortina];
                """);

            migrationBuilder.Sql("""
                IF COL_LENGTH('Kittings', 'Caja') IS NULL
                    ALTER TABLE [Kittings] ADD [Caja] nvarchar(100) NULL;
                """);

            migrationBuilder.Sql("""
                IF COL_LENGTH('Kittings', 'Cortina') IS NULL
                    ALTER TABLE [Kittings] ADD [Cortina] nvarchar(100) NULL;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF COL_LENGTH('Kittings', 'Caja') IS NOT NULL
                    ALTER TABLE [Kittings] DROP COLUMN [Caja];
                """);

            migrationBuilder.Sql("""
                IF COL_LENGTH('Kittings', 'Cortina') IS NOT NULL
                    ALTER TABLE [Kittings] DROP COLUMN [Cortina];
                """);

            migrationBuilder.Sql("""
                IF COL_LENGTH('Asns', 'Caja') IS NULL
                    ALTER TABLE [Asns] ADD [Caja] nvarchar(100) NULL;
                """);

            migrationBuilder.Sql("""
                IF COL_LENGTH('Asns', 'Cortina') IS NULL
                    ALTER TABLE [Asns] ADD [Cortina] nvarchar(100) NULL;
                """);
        }
    }
}
