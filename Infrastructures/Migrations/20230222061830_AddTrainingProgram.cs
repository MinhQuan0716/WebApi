using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class AddTrainingProgram : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Syllabuses_Users_userID",
                table: "Syllabuses");

            migrationBuilder.RenameColumn(
                name: "userID",
                table: "Syllabuses",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Syllabuses_userID",
                table: "Syllabuses",
                newName: "IX_Syllabuses_UserId");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AttendanceId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "TrainingProgramId",
                table: "TrainingClasses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "TrainingPrograms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ProgramName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<double>(type: "float", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModificationBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingPrograms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "detailTrainingProgramSyllabuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SyllabusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrainingProgramId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModificationBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detailTrainingProgramSyllabuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_detailTrainingProgramSyllabuses_Syllabuses_SyllabusId",
                        column: x => x.SyllabusId,
                        principalTable: "Syllabuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_detailTrainingProgramSyllabuses_TrainingPrograms_TrainingProgramId",
                        column: x => x.TrainingProgramId,
                        principalTable: "TrainingPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingClasses_TrainingProgramId",
                table: "TrainingClasses",
                column: "TrainingProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_detailTrainingProgramSyllabuses_SyllabusId",
                table: "detailTrainingProgramSyllabuses",
                column: "SyllabusId");

            migrationBuilder.CreateIndex(
                name: "IX_detailTrainingProgramSyllabuses_TrainingProgramId",
                table: "detailTrainingProgramSyllabuses",
                column: "TrainingProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_Syllabuses_Users_UserId",
                table: "Syllabuses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingClasses_TrainingPrograms_TrainingProgramId",
                table: "TrainingClasses",
                column: "TrainingProgramId",
                principalTable: "TrainingPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Syllabuses_Users_UserId",
                table: "Syllabuses");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingClasses_TrainingPrograms_TrainingProgramId",
                table: "TrainingClasses");

            migrationBuilder.DropTable(
                name: "detailTrainingProgramSyllabuses");

            migrationBuilder.DropTable(
                name: "TrainingPrograms");

            migrationBuilder.DropIndex(
                name: "IX_TrainingClasses_TrainingProgramId",
                table: "TrainingClasses");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AttendanceId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TrainingProgramId",
                table: "TrainingClasses");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Syllabuses",
                newName: "userID");

            migrationBuilder.RenameIndex(
                name: "IX_Syllabuses_UserId",
                table: "Syllabuses",
                newName: "IX_Syllabuses_userID");

            migrationBuilder.AddForeignKey(
                name: "FK_Syllabuses_Users_userID",
                table: "Syllabuses",
                column: "userID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
