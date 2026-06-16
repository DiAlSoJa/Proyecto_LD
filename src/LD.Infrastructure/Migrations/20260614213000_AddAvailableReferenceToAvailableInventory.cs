using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations;

[DbContext(typeof(LdProyectDbContext))]
[Migration("20260614213000_AddAvailableReferenceToAvailableInventory")]
public partial class AddAvailableReferenceToAvailableInventory : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "AvailableReference",
            table: "AvailableInventories",
            type: "nvarchar(30)",
            maxLength: 30,
            nullable: true);

        migrationBuilder.Sql(
            """
            UPDATE [dbo].[AvailableInventories]
            SET [AvailableReference] = LEFT([DocumentId], 30)
            WHERE [DocumentId] IS NOT NULL;
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "AvailableReference",
            table: "AvailableInventories");
    }
}
