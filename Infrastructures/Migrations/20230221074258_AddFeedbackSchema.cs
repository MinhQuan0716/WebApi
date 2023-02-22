using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class AddFeedbackSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Feedbakcks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeedbackTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FeedbackLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrainingCLassId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModificationBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbakcks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Feedbakcks_TrainingClasses_TrainingCLassId",
                        column: x => x.TrainingCLassId,
                        principalTable: "TrainingClasses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Feedbakcks_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Feedbakcks_CreatedBy",
                table: "Feedbakcks",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbakcks_TrainingCLassId",
                table: "Feedbakcks",
                column: "TrainingCLassId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feedbakcks");
        }
    }
}
