using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Checklist_Feature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Checklists",
                columns: table => new
                {
                    ChecklistId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    EquipmentTypeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Turno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checklists", x => x.ChecklistId);
                    table.ForeignKey(
                        name: "FK_Checklists_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "EquipmentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChecklistAnswers",
                columns: table => new
                {
                    ChecklistAnswerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChecklistId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    QuestionTextSnapshot = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AnswerText = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsOk = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistAnswers", x => x.ChecklistAnswerId);
                    table.ForeignKey(
                        name: "FK_ChecklistAnswers_Checklists_ChecklistId",
                        column: x => x.ChecklistId,
                        principalTable: "Checklists",
                        principalColumn: "ChecklistId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChecklistDefectMarks",
                columns: table => new
                {
                    ChecklistDefectMarkId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChecklistId = table.Column<int>(type: "int", nullable: false),
                    Side = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    XPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    YPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistDefectMarks", x => x.ChecklistDefectMarkId);
                    table.ForeignKey(
                        name: "FK_ChecklistDefectMarks_Checklists_ChecklistId",
                        column: x => x.ChecklistId,
                        principalTable: "Checklists",
                        principalColumn: "ChecklistId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChecklistPhotos",
                columns: table => new
                {
                    ChecklistPhotoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChecklistId = table.Column<int>(type: "int", nullable: false),
                    RelativePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Side = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistPhotos", x => x.ChecklistPhotoId);
                    table.ForeignKey(
                        name: "FK_ChecklistPhotos_Checklists_ChecklistId",
                        column: x => x.ChecklistId,
                        principalTable: "Checklists",
                        principalColumn: "ChecklistId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistAnswers_ChecklistId",
                table: "ChecklistAnswers",
                column: "ChecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistDefectMarks_ChecklistId",
                table: "ChecklistDefectMarks",
                column: "ChecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistPhotos_ChecklistId",
                table: "ChecklistPhotos",
                column: "ChecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_Checklists_EquipmentId",
                table: "Checklists",
                column: "EquipmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChecklistAnswers");

            migrationBuilder.DropTable(
                name: "ChecklistDefectMarks");

            migrationBuilder.DropTable(
                name: "ChecklistPhotos");

            migrationBuilder.DropTable(
                name: "Checklists");
        }
    }
}
