using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CodingPlatform.Infrastructure.Migrations
{
    public partial class AddUserTournamentPartecipations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournament_Users_AdminId",
                table: "Tournament");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tournament",
                table: "Tournament");

            migrationBuilder.RenameTable(
                name: "Tournament",
                newName: "Tournaments");

            migrationBuilder.RenameIndex(
                name: "IX_Tournament_AdminId",
                table: "Tournaments",
                newName: "IX_Tournaments_AdminId");

            migrationBuilder.AddColumn<long>(
                name: "UserTournamentPartecipationsId",
                table: "Users",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "AdminId",
                table: "Tournaments",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "UserTournamentPartecipationsId",
                table: "Tournaments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tournaments",
                table: "Tournaments",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserTournamentPartecipations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTournamentPartecipations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserTournamentPartecipationsId",
                table: "Users",
                column: "UserTournamentPartecipationsId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_UserTournamentPartecipationsId",
                table: "Tournaments",
                column: "UserTournamentPartecipationsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Users_AdminId",
                table: "Tournaments",
                column: "AdminId",
                principalTable: "Users",
                principalColumn: "Id");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Users_AdminId",
                table: "Tournaments");

            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_UserTournamentPartecipations_UserTournamentPart~",
                table: "Tournaments");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserTournamentPartecipations_UserTournamentPartecipat~",
                table: "Users");

            migrationBuilder.DropTable(
                name: "UserTournamentPartecipations");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserTournamentPartecipationsId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tournaments",
                table: "Tournaments");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_UserTournamentPartecipationsId",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "UserTournamentPartecipationsId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserTournamentPartecipationsId",
                table: "Tournaments");

            migrationBuilder.RenameTable(
                name: "Tournaments",
                newName: "Tournament");

            migrationBuilder.RenameIndex(
                name: "IX_Tournaments_AdminId",
                table: "Tournament",
                newName: "IX_Tournament_AdminId");

            migrationBuilder.AlterColumn<long>(
                name: "AdminId",
                table: "Tournament",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tournament",
                table: "Tournament",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournament_Users_AdminId",
                table: "Tournament",
                column: "AdminId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
