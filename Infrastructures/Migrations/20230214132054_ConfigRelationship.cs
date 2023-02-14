using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class ConfigRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SyllabusID",
                table: "Units",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 4,
                column: "UserPermission",
                value: "AccessDenied");

            migrationBuilder.CreateIndex(
                name: "IX_Units_SyllabusID",
                table: "Units",
                column: "SyllabusID");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Syllabuses_SyllabusID",
                table: "Units",
                column: "SyllabusID",
                principalTable: "Syllabuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_Syllabuses_SyllabusID",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_SyllabusID",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "SyllabusID",
                table: "Units");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 4,
                column: "UserPermission",
                value: "FullAccess");
        }
    }
}
