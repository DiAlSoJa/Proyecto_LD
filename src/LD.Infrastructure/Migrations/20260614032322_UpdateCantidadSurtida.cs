using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCantidadSurtida : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.KittingDetails', 'CantidadSurtida') IS NULL
BEGIN
    ALTER TABLE dbo.KittingDetails
    ADD CantidadSurtida decimal(18,2) NOT NULL
        CONSTRAINT DF_KittingDetails_CantidadSurtida DEFAULT(0);
END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.KittingDetails', 'CantidadSurtida') IS NOT NULL
BEGIN
    ALTER TABLE dbo.KittingDetails
    DROP CONSTRAINT DF_KittingDetails_CantidadSurtida;

    ALTER TABLE dbo.KittingDetails
    DROP COLUMN CantidadSurtida;
END");
        }
    }
}
