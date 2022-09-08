using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodingPlatform.Infrastructure.Migrations
{
    public partial class RefactorCurrentChallengeAmend : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_CurrentChallenges_ChallengeId",
                table: "Submissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Tips_CurrentChallenges_ChallengeId",
                table: "Tips");

            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_CurrentChallenges_ChallengeId",
                table: "Tournaments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrentChallenges",
                table: "CurrentChallenges");

            migrationBuilder.RenameTable(
                name: "CurrentChallenges",
                newName: "Challenges");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Challenges",
                table: "Challenges",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Challenges_ChallengeId",
                table: "Submissions",
                column: "ChallengeId",
                principalTable: "Challenges",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tips_Challenges_ChallengeId",
                table: "Tips",
                column: "ChallengeId",
                principalTable: "Challenges",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Challenges_ChallengeId",
                table: "Tournaments",
                column: "ChallengeId",
                principalTable: "Challenges",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Challenges_ChallengeId",
                table: "Submissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Tips_Challenges_ChallengeId",
                table: "Tips");

            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Challenges_ChallengeId",
                table: "Tournaments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Challenges",
                table: "Challenges");

            migrationBuilder.RenameTable(
                name: "Challenges",
                newName: "CurrentChallenges");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrentChallenges",
                table: "CurrentChallenges",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_CurrentChallenges_ChallengeId",
                table: "Submissions",
                column: "ChallengeId",
                principalTable: "CurrentChallenges",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tips_CurrentChallenges_ChallengeId",
                table: "Tips",
                column: "ChallengeId",
                principalTable: "CurrentChallenges",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_CurrentChallenges_ChallengeId",
                table: "Tournaments",
                column: "ChallengeId",
                principalTable: "CurrentChallenges",
                principalColumn: "Id");
        }
    }
}
