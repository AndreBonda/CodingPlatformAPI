using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CodingPlatform.Infrastructure.Migrations
{
    public partial class AddCurrentChallengeAndTip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CurrentChallengeId",
                table: "Tournaments",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CurrentChallenges",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentChallenges", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_CurrentChallengeId",
                table: "Tournaments",
                column: "CurrentChallengeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_CurrentChallenges_CurrentChallengeId",
                table: "Tournaments",
                column: "CurrentChallengeId",
                principalTable: "CurrentChallenges",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_CurrentChallenges_CurrentChallengeId",
                table: "Tournaments");

            migrationBuilder.DropTable(
                name: "CurrentChallenges");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_CurrentChallengeId",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "CurrentChallengeId",
                table: "Tournaments");
        }
    }
}
