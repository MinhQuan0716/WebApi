using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTrainingClassSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingClasses_Locations_LocationID",
                table: "TrainingClasses");

            migrationBuilder.AlterColumn<Guid>(
                name: "LocationID",
                table: "TrainingClasses",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingClasses_Locations_LocationID",
                table: "TrainingClasses",
                column: "LocationID",
                principalTable: "Locations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingClasses_Locations_LocationID",
                table: "TrainingClasses");

            migrationBuilder.AlterColumn<Guid>(
                name: "LocationID",
                table: "TrainingClasses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingClasses_Locations_LocationID",
                table: "TrainingClasses",
                column: "LocationID",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
