using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace anket_kazan.Migrations
{
    /// <inheritdoc />
    public partial class errorfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "survey_id",
                table: "Question");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "survey_id",
                table: "Question",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
