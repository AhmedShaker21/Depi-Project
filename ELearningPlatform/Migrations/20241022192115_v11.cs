using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearningPlatform.Migrations
{
    /// <inheritdoc />
    public partial class v11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Codes_Course_CourseId",
                table: "Codes");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "Codes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Codes_Course_CourseId",
                table: "Codes",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Codes_Course_CourseId",
                table: "Codes");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "Codes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Codes_Course_CourseId",
                table: "Codes",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id");
        }
    }
}
