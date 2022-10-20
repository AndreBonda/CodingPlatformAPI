using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CodingPlatform.Infrastructure.Migrations
{
    public partial class Refactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTournamentParticipations");

            migrationBuilder.AddColumn<long>(
                name: "TournamentId",
                table: "Users",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tournaments",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Score",
                table: "Submissions",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tournaments",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<byte>(
                name: "Score",
                table: "Submissions",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.CreateTable(
                name: "UserTournamentParticipations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TournamentId = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTournamentParticipations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTournamentParticipations_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserTournamentParticipations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTournamentParticipations_TournamentId",
                table: "UserTournamentParticipations",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTournamentParticipations_UserId",
                table: "UserTournamentParticipations",
                column: "UserId");
        }
    }
}
