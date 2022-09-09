using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodingPlatform.Infrastructure.Migrations
{
    public partial class AddSubmissionScore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Score",
                table: "Submissions",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "Submissions");
        }
    }
}
