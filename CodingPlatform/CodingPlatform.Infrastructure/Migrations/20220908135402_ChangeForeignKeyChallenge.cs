using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodingPlatform.Infrastructure.Migrations
{
    public partial class ChangeForeignKeyChallenge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Challenges_ChallengeId",
                table: "Tournaments");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_ChallengeId",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "ChallengeId",
                table: "Tournaments");

            migrationBuilder.AddColumn<long>(
                name: "TournamentId",
                table: "Challenges",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Challenges_TournamentId",
                table: "Challenges",
                column: "TournamentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Challenges_Tournaments_TournamentId",
                table: "Challenges",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Challenges_Tournaments_TournamentId",
                table: "Challenges");

            migrationBuilder.DropIndex(
                name: "IX_Challenges_TournamentId",
                table: "Challenges");

            migrationBuilder.DropColumn(
                name: "TournamentId",
                table: "Challenges");

            migrationBuilder.AddColumn<long>(
                name: "ChallengeId",
                table: "Tournaments",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_ChallengeId",
                table: "Tournaments",
                column: "ChallengeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Challenges_ChallengeId",
                table: "Tournaments",
                column: "ChallengeId",
                principalTable: "Challenges",
                principalColumn: "Id");
        }
    }
}
