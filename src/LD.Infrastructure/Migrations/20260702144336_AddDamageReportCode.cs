using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations;

[DbContext(typeof(LdProyectDbContext))]
[Migration("20260702144336_AddDamageReportCode")]
public partial class AddDamageReportCode : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "DamageReportCode",
            table: "DamageReports",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.Sql(
            """
            UPDATE [dbo].[DamageReports]
            SET [DamageReportCode] = CONCAT(
                CONVERT(char(8), [ReportDate], 112),
                '-',
                LEFT(REPLACE(CONVERT(varchar(36), NEWID()), '-', ''), 12))
            WHERE [DamageReportCode] IS NULL;
            """);

        migrationBuilder.CreateIndex(
            name: "IX_DamageReports_DamageReportCode",
            table: "DamageReports",
            column: "DamageReportCode",
            unique: true,
            filter: "[DamageReportCode] IS NOT NULL");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_DamageReports_DamageReportCode",
            table: "DamageReports");

        migrationBuilder.DropColumn(
            name: "DamageReportCode",
            table: "DamageReports");
    }
}
