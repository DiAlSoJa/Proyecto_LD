using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPartNumberSystemField2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SystemFields",
                keyColumn: "SystemFieldId",
                keyValue: 7,
                column: "SystemFieldName",
                value: "partnumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SystemFields",
                keyColumn: "SystemFieldId",
                keyValue: 7,
                column: "SystemFieldName",
                value: "part_number");
        }
    }
}
