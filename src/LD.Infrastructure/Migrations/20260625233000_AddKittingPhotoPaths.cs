using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    [DbContext(typeof(LdProyectDbContext))]
    [Migration("20260625233000_AddKittingPhotoPaths")]
    public partial class AddKittingPhotoPaths : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Kittings', 'Photo1Path') IS NULL
    ALTER TABLE dbo.Kittings ADD Photo1Path nvarchar(500) NULL;

IF COL_LENGTH('dbo.Kittings', 'Photo2Path') IS NULL
    ALTER TABLE dbo.Kittings ADD Photo2Path nvarchar(500) NULL;

IF COL_LENGTH('dbo.Kittings', 'Photo3Path') IS NULL
    ALTER TABLE dbo.Kittings ADD Photo3Path nvarchar(500) NULL;

IF COL_LENGTH('dbo.Kittings', 'Photo4Path') IS NULL
    ALTER TABLE dbo.Kittings ADD Photo4Path nvarchar(500) NULL;
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Kittings', 'Photo4Path') IS NOT NULL
    ALTER TABLE dbo.Kittings DROP COLUMN Photo4Path;

IF COL_LENGTH('dbo.Kittings', 'Photo3Path') IS NOT NULL
    ALTER TABLE dbo.Kittings DROP COLUMN Photo3Path;

IF COL_LENGTH('dbo.Kittings', 'Photo2Path') IS NOT NULL
    ALTER TABLE dbo.Kittings DROP COLUMN Photo2Path;

IF COL_LENGTH('dbo.Kittings', 'Photo1Path') IS NOT NULL
    ALTER TABLE dbo.Kittings DROP COLUMN Photo1Path;
");
        }
    }
}
