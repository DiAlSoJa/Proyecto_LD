using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations;

[DbContext(typeof(LdProyectDbContext))]
[Migration("20260614220000_AddSupplyAndFinalAvailableToAvailableInventory")]
public partial class AddSupplyAndFinalAvailableToAvailableInventory : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<decimal>(
            name: "Supply",
            table: "AvailableInventories",
            type: "decimal(18,2)",
            nullable: false,
            defaultValue: 0m);

        migrationBuilder.AddColumn<decimal>(
            name: "FinalAvailable",
            table: "AvailableInventories",
            type: "decimal(18,2)",
            nullable: false,
            defaultValue: 0m);

        migrationBuilder.Sql(
            """
            UPDATE [dbo].[AvailableInventories]
            SET [Supply] = 0,
                [FinalAvailable] = ISNULL([Qty], 0);
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Supply",
            table: "AvailableInventories");

        migrationBuilder.DropColumn(
            name: "FinalAvailable",
            table: "AvailableInventories");
    }
}
