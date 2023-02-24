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
                name: "TrainingProgramId",
                table: "TrainingClasses");

            migrationBuilder.CreateTable(
                name: "Bank",
                columns: table => new
                {
                    TempId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.UniqueConstraint("AK_Bank_TempId1", x => x.TempId1);
                });

            migrationBuilder.CreateTable(
                name: "QuizTest",
                columns: table => new
                {
                    SyllabusIDId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TempId = table.Column<int>(type: "int", nullable: false),
                    TempId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.UniqueConstraint("AK_QuizTest_TempId", x => x.TempId);
                    table.UniqueConstraint("AK_QuizTest_TempId1", x => x.TempId1);
                    table.ForeignKey(
                        name: "FK_QuizTest_Syllabuses_SyllabusIDId",
                        column: x => x.SyllabusIDId,
                        principalTable: "Syllabuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TypeQuestion",
                columns: table => new
                {
                    TempId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.UniqueConstraint("AK_TypeQuestion_TempId", x => x.TempId);
                });

            migrationBuilder.CreateTable(
                name: "QuestionDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizTestIDId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeleteBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModificationBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Point = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionDetail_QuizTest_QuizTestIDId",
                        column: x => x.QuizTestIDId,
                        principalTable: "QuizTest",
                        principalColumn: "TempId1",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizResult",
                columns: table => new
                {
                    QuizTestIdId = table.Column<int>(type: "int", nullable: true),
                    UserIDId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_QuizResult_QuizTest_QuizTestIdId",
                        column: x => x.QuizTestIdId,
                        principalTable: "QuizTest",
                        principalColumn: "TempId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizResult_Users_UserIDId",
                        column: x => x.UserIDId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BankId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeID1 = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentQuestion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeleteBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModificationBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    QuestionDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Question_Bank_BankId",
                        column: x => x.BankId,
                        principalTable: "Bank",
                        principalColumn: "TempId1",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Question_QuestionDetail_QuestionDetailId",
                        column: x => x.QuestionDetailId,
                        principalTable: "QuestionDetail",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Question_TypeQuestion_TypeID1",
                        column: x => x.TypeID1,
                        principalTable: "TypeQuestion",
                        principalColumn: "TempId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Question_BankId",
                table: "Question",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_QuestionDetailId",
                table: "Question",
                column: "QuestionDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_TypeID1",
                table: "Question",
                column: "TypeID1");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionDetail_QuizTestIDId",
                table: "QuestionDetail",
                column: "QuizTestIDId");
        }
    }
}
