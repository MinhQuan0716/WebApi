using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class AddAttendanceSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_detailTrainingProgramSyllabuses_Syllabuses_SyllabusId",
                table: "detailTrainingProgramSyllabuses");

            migrationBuilder.DropForeignKey(
                name: "FK_detailTrainingProgramSyllabuses_TrainingPrograms_TrainingProgramId",
                table: "detailTrainingProgramSyllabuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_detailTrainingProgramSyllabuses",
                table: "detailTrainingProgramSyllabuses");

            migrationBuilder.RenameTable(
                name: "detailTrainingProgramSyllabuses",
                newName: "DetailTrainingProgramSyllabus");

            migrationBuilder.RenameIndex(
                name: "IX_detailTrainingProgramSyllabuses_TrainingProgramId",
                table: "DetailTrainingProgramSyllabus",
                newName: "IX_DetailTrainingProgramSyllabus_TrainingProgramId");

            migrationBuilder.RenameIndex(
                name: "IX_detailTrainingProgramSyllabuses_SyllabusId",
                table: "DetailTrainingProgramSyllabus",
                newName: "IX_DetailTrainingProgramSyllabus_SyllabusId");

            migrationBuilder.AddColumn<string>(
                name: "AttendancePermission",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "TrainingClassId",
                table: "Applications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetailTrainingProgramSyllabus",
                table: "DetailTrainingProgramSyllabus",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AuditPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    AuditPlanName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuditLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LectureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModificationBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditPlans_Lectures_LectureId",
                        column: x => x.LectureId,
                        principalTable: "Lectures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModificationBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditQuestions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditSubmissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalGrade = table.Column<double>(type: "float", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuditPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModificationBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditSubmissions_AuditPlans_AuditPlanId",
                        column: x => x.AuditPlanId,
                        principalTable: "AuditPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetailAuditQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    AuditQuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuditPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModificationBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailAuditQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetailAuditQuestions_AuditPlans_AuditPlanId",
                        column: x => x.AuditPlanId,
                        principalTable: "AuditPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetailAuditQuestions_AuditQuestions_AuditQuestionId",
                        column: x => x.AuditQuestionId,
                        principalTable: "AuditQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetailAuditSubmissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    AuditSubmissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DetailAuditQuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModificationBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailAuditSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetailAuditSubmissions_AuditSubmissions_AuditSubmissionId",
                        column: x => x.AuditSubmissionId,
                        principalTable: "AuditSubmissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetailAuditSubmissions_DetailAuditQuestions_DetailAuditQuestionId",
                        column: x => x.DetailAuditQuestionId,
                        principalTable: "DetailAuditQuestions",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                column: "AttendancePermission",
                value: "FullAccess");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "AttendancePermission", "RoleName" },
                values: new object[] { "FullAccess", "Admin" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                column: "AttendancePermission",
                value: "FullAccess");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 4,
                columns: new[] { "AttendancePermission", "RoleName" },
                values: new object[] { "FullAccess", "Trainee" });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_TrainingClassId",
                table: "Applications",
                column: "TrainingClassId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditPlans_LectureId",
                table: "AuditPlans",
                column: "LectureId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditSubmissions_AuditPlanId",
                table: "AuditSubmissions",
                column: "AuditPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailAuditQuestions_AuditPlanId",
                table: "DetailAuditQuestions",
                column: "AuditPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailAuditQuestions_AuditQuestionId",
                table: "DetailAuditQuestions",
                column: "AuditQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailAuditSubmissions_AuditSubmissionId",
                table: "DetailAuditSubmissions",
                column: "AuditSubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailAuditSubmissions_DetailAuditQuestionId",
                table: "DetailAuditSubmissions",
                column: "DetailAuditQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_TrainingClasses_TrainingClassId",
                table: "Applications",
                column: "TrainingClassId",
                principalTable: "TrainingClasses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DetailTrainingProgramSyllabus_Syllabuses_SyllabusId",
                table: "DetailTrainingProgramSyllabus",
                column: "SyllabusId",
                principalTable: "Syllabuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DetailTrainingProgramSyllabus_TrainingPrograms_TrainingProgramId",
                table: "DetailTrainingProgramSyllabus",
                column: "TrainingProgramId",
                principalTable: "TrainingPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_TrainingClasses_TrainingClassId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailTrainingProgramSyllabus_Syllabuses_SyllabusId",
                table: "DetailTrainingProgramSyllabus");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailTrainingProgramSyllabus_TrainingPrograms_TrainingProgramId",
                table: "DetailTrainingProgramSyllabus");

            migrationBuilder.DropTable(
                name: "DetailAuditSubmissions");

            migrationBuilder.DropTable(
                name: "AuditSubmissions");

            migrationBuilder.DropTable(
                name: "DetailAuditQuestions");

            migrationBuilder.DropTable(
                name: "AuditPlans");

            migrationBuilder.DropTable(
                name: "AuditQuestions");

            migrationBuilder.DropIndex(
                name: "IX_Applications_TrainingClassId",
                table: "Applications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DetailTrainingProgramSyllabus",
                table: "DetailTrainingProgramSyllabus");

            migrationBuilder.DropColumn(
                name: "AttendancePermission",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "TrainingClassId",
                table: "Applications");

            migrationBuilder.RenameTable(
                name: "DetailTrainingProgramSyllabus",
                newName: "detailTrainingProgramSyllabuses");

            migrationBuilder.RenameIndex(
                name: "IX_DetailTrainingProgramSyllabus_TrainingProgramId",
                table: "detailTrainingProgramSyllabuses",
                newName: "IX_detailTrainingProgramSyllabuses_TrainingProgramId");

            migrationBuilder.RenameIndex(
                name: "IX_DetailTrainingProgramSyllabus_SyllabusId",
                table: "detailTrainingProgramSyllabuses",
                newName: "IX_detailTrainingProgramSyllabuses_SyllabusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_detailTrainingProgramSyllabuses",
                table: "detailTrainingProgramSyllabuses",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                column: "RoleName",
                value: "ClassAdmin");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 4,
                column: "RoleName",
                value: "Student");

            migrationBuilder.AddForeignKey(
                name: "FK_detailTrainingProgramSyllabuses_Syllabuses_SyllabusId",
                table: "detailTrainingProgramSyllabuses",
                column: "SyllabusId",
                principalTable: "Syllabuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_detailTrainingProgramSyllabuses_TrainingPrograms_TrainingProgramId",
                table: "detailTrainingProgramSyllabuses",
                column: "TrainingProgramId",
                principalTable: "TrainingPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
