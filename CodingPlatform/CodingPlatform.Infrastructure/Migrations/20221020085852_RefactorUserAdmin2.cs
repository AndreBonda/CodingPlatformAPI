using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodingPlatform.Infrastructure.Migrations
{
    public partial class RefactorUserAdmin2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tournaments_TournamentId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TournamentId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TournamentId",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TournamentId",
                table: "Users",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_TournamentId",
                table: "Users",
                column: "TournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Tournaments_TournamentId",
                table: "Users",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id");
        }
    }
}
