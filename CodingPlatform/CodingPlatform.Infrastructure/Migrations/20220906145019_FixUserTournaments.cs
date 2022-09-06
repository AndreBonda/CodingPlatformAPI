using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodingPlatform.Infrastructure.Migrations
{
    public partial class FixUserTournaments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_UserTournamentPartecipations_UserTournamentPart~",
                table: "Tournaments");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserTournamentPartecipations_UserTournamentPartecipat~",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserTournamentPartecipationsId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_UserTournamentPartecipationsId",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "UserTournamentPartecipationsId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserTournamentPartecipationsId",
                table: "Tournaments");

            migrationBuilder.AddColumn<long>(
                name: "TournamentId",
                table: "UserTournamentPartecipations",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "UserTournamentPartecipations",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTournamentPartecipations_TournamentId",
                table: "UserTournamentPartecipations",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTournamentPartecipations_UserId",
                table: "UserTournamentPartecipations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTournamentPartecipations_Tournaments_TournamentId",
                table: "UserTournamentPartecipations",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTournamentPartecipations_Users_UserId",
                table: "UserTournamentPartecipations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTournamentPartecipations_Tournaments_TournamentId",
                table: "UserTournamentPartecipations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTournamentPartecipations_Users_UserId",
                table: "UserTournamentPartecipations");

            migrationBuilder.DropIndex(
                name: "IX_UserTournamentPartecipations_TournamentId",
                table: "UserTournamentPartecipations");

            migrationBuilder.DropIndex(
                name: "IX_UserTournamentPartecipations_UserId",
                table: "UserTournamentPartecipations");

            migrationBuilder.DropColumn(
                name: "TournamentId",
                table: "UserTournamentPartecipations");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserTournamentPartecipations");

            migrationBuilder.AddColumn<long>(
                name: "UserTournamentPartecipationsId",
                table: "Users",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserTournamentPartecipationsId",
                table: "Tournaments",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserTournamentPartecipationsId",
                table: "Users",
                column: "UserTournamentPartecipationsId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_UserTournamentPartecipationsId",
                table: "Tournaments",
                column: "UserTournamentPartecipationsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_UserTournamentPartecipations_UserTournamentPart~",
                table: "Tournaments",
                column: "UserTournamentPartecipationsId",
                principalTable: "UserTournamentPartecipations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserTournamentPartecipations_UserTournamentPartecipat~",
                table: "Users",
                column: "UserTournamentPartecipationsId",
                principalTable: "UserTournamentPartecipations",
                principalColumn: "Id");
        }
    }
}
