using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    public partial class BackfillKittingIssueSd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE kid
                SET kid.SD = kd.SD
                FROM [KittingIssueDetails] AS kid
                INNER JOIN [KittingDetails] AS kd
                    ON kd.[KittingDetailId] = kid.[KittingDetailId]
                WHERE (kid.SD IS NULL OR LTRIM(RTRIM(kid.SD)) = '')
                  AND kd.SD IS NOT NULL
                  AND LTRIM(RTRIM(kd.SD)) <> '';
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
