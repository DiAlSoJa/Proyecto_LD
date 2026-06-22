using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    [Migration("20260622183000_RenameKittingStatuses")]
    public partial class RenameKittingStatuses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE [Kittings]
                SET [Status] = N'Validación'
                WHERE [Status] = N'Surtido';

                UPDATE [Kittings]
                SET [Status] = N'Cargando'
                WHERE [Status] = N'Validado';

                UPDATE [KittingIssueDetails]
                SET [SupplyStatus] = N'Validación'
                WHERE [SupplyStatus] = N'Surtido';

                UPDATE [KittingIssueDetails]
                SET [SupplyStatus] = N'Cargando'
                WHERE [SupplyStatus] = N'Validado';
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE [Kittings]
                SET [Status] = N'Surtido'
                WHERE [Status] = N'Validación';

                UPDATE [Kittings]
                SET [Status] = N'Validado'
                WHERE [Status] = N'Cargando';

                UPDATE [KittingIssueDetails]
                SET [SupplyStatus] = N'Surtido'
                WHERE [SupplyStatus] = N'Validación';

                UPDATE [KittingIssueDetails]
                SET [SupplyStatus] = N'Validado'
                WHERE [SupplyStatus] = N'Cargando';
                """);
        }
    }
}
