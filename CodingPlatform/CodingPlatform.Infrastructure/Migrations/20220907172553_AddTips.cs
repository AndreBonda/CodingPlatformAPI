using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CodingPlatform.Infrastructure.Migrations
{
    public partial class AddTips : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tips",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CurrentChallengeId = table.Column<long>(type: "bigint", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tips_CurrentChallenges_CurrentChallengeId",
                        column: x => x.CurrentChallengeId,
                        principalTable: "CurrentChallenges",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tips_CurrentChallengeId",
                table: "Tips",
                column: "CurrentChallengeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tips");
        }
    }
}
