using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodingPlatform.Infrastructure.Migrations
{
    public partial class FixUniqueForeginKeyChallenge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Challenges_Tournaments_TournamentId",
                table: "Challenges");

            migrationBuilder.DropIndex(
                name: "IX_Challenges_TournamentId",
                table: "Challenges");

            migrationBuilder.AlterColumn<long>(
                name: "TournamentId",
                table: "Challenges",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_Challenges_TournamentId",
                table: "Challenges",
                column: "TournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Challenges_Tournaments_TournamentId",
                table: "Challenges",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Challenges_Tournaments_TournamentId",
                table: "Challenges");

            migrationBuilder.DropIndex(
                name: "IX_Challenges_TournamentId",
                table: "Challenges");

            migrationBuilder.AlterColumn<long>(
                name: "TournamentId",
                table: "Challenges",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

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
    }
}
