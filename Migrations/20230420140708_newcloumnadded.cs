using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace anket_kazan.Migrations
{
    /// <inheritdoc />
    public partial class newcloumnadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Answer_Four",
                table: "Question",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Answer_One",
                table: "Question",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Answer_Three",
                table: "Question",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Answer_Two",
                table: "Question",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answer_Four",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "Answer_One",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "Answer_Three",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "Answer_Two",
                table: "Question");
        }
    }
}
