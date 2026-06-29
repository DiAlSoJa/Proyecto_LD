using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    [DbContext(typeof(LdProyectDbContext))]
    [Migration("20260624120000_AddKittingTransportFlags")]
    public partial class AddKittingTransportFlags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
