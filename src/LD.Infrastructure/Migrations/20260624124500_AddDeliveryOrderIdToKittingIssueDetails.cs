using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    [DbContext(typeof(LdProyectDbContext))]
    [Migration("20260624124500_AddDeliveryOrderIdToKittingIssueDetails")]
    public partial class AddDeliveryOrderIdToKittingIssueDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeliveryOrderId",
                table: "KittingIssueDetails",
                type: "int",
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE kid
                SET kid.DeliveryOrderId = dok.DeliveryOrderId
                FROM [KittingIssueDetails] AS kid
                INNER JOIN [KittingDetails] AS kd
                    ON kd.[KittingDetailId] = kid.[KittingDetailId]
                INNER JOIN [DeliveryOrderKittings] AS dok
                    ON dok.[KittingId] = kd.[KittingId]
                WHERE kid.[DeliveryOrderId] IS NULL;
                """);

            migrationBuilder.CreateIndex(
                name: "IX_KittingIssueDetails_DeliveryOrderId",
                table: "KittingIssueDetails",
                column: "DeliveryOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_KittingIssueDetails_DeliveryOrders_DeliveryOrderId",
                table: "KittingIssueDetails",
                column: "DeliveryOrderId",
                principalTable: "DeliveryOrders",
                principalColumn: "DeliveryOrderId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KittingIssueDetails_DeliveryOrders_DeliveryOrderId",
                table: "KittingIssueDetails");

            migrationBuilder.DropIndex(
                name: "IX_KittingIssueDetails_DeliveryOrderId",
                table: "KittingIssueDetails");

            migrationBuilder.DropColumn(
                name: "DeliveryOrderId",
                table: "KittingIssueDetails");
        }
    }
}
