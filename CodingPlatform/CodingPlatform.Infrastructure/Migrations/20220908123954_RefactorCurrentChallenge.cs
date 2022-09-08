using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CodingPlatform.Infrastructure.Migrations
{
    public partial class RefactorCurrentChallenge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tips_CurrentChallenges_CurrentChallengeId",
                table: "Tips");

            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_CurrentChallenges_CurrentChallengeId",
                table: "Tournaments");

            migrationBuilder.RenameColumn(
                name: "CurrentChallengeId",
                table: "Tournaments",
                newName: "ChallengeId");

            migrationBuilder.RenameIndex(
                name: "IX_Tournaments_CurrentChallengeId",
                table: "Tournaments",
                newName: "IX_Tournaments_ChallengeId");

            migrationBuilder.RenameColumn(
                name: "CurrentChallengeId",
                table: "Tips",
                newName: "ChallengeId");

            migrationBuilder.RenameIndex(
                name: "IX_Tips_CurrentChallengeId",
                table: "Tips",
                newName: "IX_Tips_ChallengeId");

            migrationBuilder.CreateTable(
                name: "Submissions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TipsNumber = table.Column<byte>(type: "smallint", nullable: false),
                    DateSubmitted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChallengeId = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Submissions_CurrentChallenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "CurrentChallenges",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Submissions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_ChallengeId",
                table: "Submissions",
                column: "ChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_UserId",
                table: "Submissions",
                column: "UserId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tips_CurrentChallenges_ChallengeId",
                table: "Tips");

            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_CurrentChallenges_ChallengeId",
                table: "Tournaments");

            migrationBuilder.DropTable(
                name: "Submissions");

            migrationBuilder.RenameColumn(
                name: "ChallengeId",
                table: "Tournaments",
                newName: "CurrentChallengeId");

            migrationBuilder.RenameIndex(
                name: "IX_Tournaments_ChallengeId",
                table: "Tournaments",
                newName: "IX_Tournaments_CurrentChallengeId");

            migrationBuilder.RenameColumn(
                name: "ChallengeId",
                table: "Tips",
                newName: "CurrentChallengeId");

            migrationBuilder.RenameIndex(
                name: "IX_Tips_ChallengeId",
                table: "Tips",
                newName: "IX_Tips_CurrentChallengeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tips_CurrentChallenges_CurrentChallengeId",
                table: "Tips",
                column: "CurrentChallengeId",
                principalTable: "CurrentChallenges",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_CurrentChallenges_CurrentChallengeId",
                table: "Tournaments",
                column: "CurrentChallengeId",
                principalTable: "CurrentChallenges",
                principalColumn: "Id");
        }
    }
}
