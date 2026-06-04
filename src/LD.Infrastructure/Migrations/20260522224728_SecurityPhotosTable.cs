using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SecurityPhotosTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Firma",
                table: "SecurityRegistrations");

            migrationBuilder.DropColumn(
                name: "LicenciaFoto1",
                table: "SecurityRegistrations");

            migrationBuilder.DropColumn(
                name: "LicenciaFoto2",
                table: "SecurityRegistrations");

            migrationBuilder.DropColumn(
                name: "VehiculoFoto1",
                table: "SecurityRegistrations");

            migrationBuilder.DropColumn(
                name: "VehiculoFoto2",
                table: "SecurityRegistrations");

           

            migrationBuilder.CreateTable(
                name: "SecurityRegistrationPhotos",
                columns: table => new
                {
                    SecurityRegistrationPhotoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SecurityRegistrationId = table.Column<int>(type: "int", nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityRegistrationPhotos", x => x.SecurityRegistrationPhotoId);
                    table.ForeignKey(
                        name: "FK_SecurityRegistrationPhotos_SecurityRegistrations_SecurityRegistrationId",
                        column: x => x.SecurityRegistrationId,
                        principalTable: "SecurityRegistrations",
                        principalColumn: "SecurityRegistrationId",
                        onDelete: ReferentialAction.Restrict);
                });

         

            migrationBuilder.CreateIndex(
                name: "IX_SecurityRegistrationPhotos_SecurityRegistrationId",
                table: "SecurityRegistrationPhotos",
                column: "SecurityRegistrationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
         

            migrationBuilder.DropTable(
                name: "SecurityRegistrationPhotos");

            migrationBuilder.AddColumn<string>(
                name: "Firma",
                table: "SecurityRegistrations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LicenciaFoto1",
                table: "SecurityRegistrations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LicenciaFoto2",
                table: "SecurityRegistrations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehiculoFoto1",
                table: "SecurityRegistrations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehiculoFoto2",
                table: "SecurityRegistrations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
