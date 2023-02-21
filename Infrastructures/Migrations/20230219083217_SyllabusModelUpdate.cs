using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class SyllabusModelUpdate : Migration
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

            migrationBuilder.AddForeignKey(
                name: "FK_Syllabuses_Users_UserId",
                table: "Syllabuses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Syllabuses_Users_UserId",
                table: "Syllabuses");

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
