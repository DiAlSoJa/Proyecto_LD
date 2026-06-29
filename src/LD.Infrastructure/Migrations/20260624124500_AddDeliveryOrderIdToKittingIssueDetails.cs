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
            migrationBuilder.Sql("""
                IF COL_LENGTH('KittingIssueDetails', 'DeliveryOrderId') IS NULL
                    ALTER TABLE [KittingIssueDetails] ADD [DeliveryOrderId] int NULL;
                """);

            migrationBuilder.Sql("""
                IF OBJECT_ID(N'[DeliveryOrderKittings]') IS NOT NULL
                BEGIN
                    UPDATE kid
                    SET kid.DeliveryOrderId = dok.DeliveryOrderId
                    FROM [KittingIssueDetails] AS kid
                    INNER JOIN [KittingDetails] AS kd
                        ON kd.[KittingDetailId] = kid.[KittingDetailId]
                    INNER JOIN [DeliveryOrderKittings] AS dok
                        ON dok.[KittingId] = kd.[KittingId]
                    WHERE kid.[DeliveryOrderId] IS NULL;
                END
                """);

            migrationBuilder.Sql("""
                IF NOT EXISTS (
                    SELECT 1
                    FROM sys.indexes
                    WHERE name = 'IX_KittingIssueDetails_DeliveryOrderId'
                      AND object_id = OBJECT_ID(N'[KittingIssueDetails]')
                )
                BEGIN
                    CREATE INDEX [IX_KittingIssueDetails_DeliveryOrderId]
                    ON [KittingIssueDetails] ([DeliveryOrderId]);
                END
                """);

            migrationBuilder.Sql("""
                IF OBJECT_ID(N'[DeliveryOrders]') IS NOT NULL
                   AND OBJECT_ID(N'[FK_KittingIssueDetails_DeliveryOrders_DeliveryOrderId]') IS NULL
                BEGIN
                    ALTER TABLE [KittingIssueDetails]
                    ADD CONSTRAINT [FK_KittingIssueDetails_DeliveryOrders_DeliveryOrderId]
                    FOREIGN KEY ([DeliveryOrderId]) REFERENCES [DeliveryOrders] ([DeliveryOrderId])
                    ON DELETE NO ACTION;
                END
                """);
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
