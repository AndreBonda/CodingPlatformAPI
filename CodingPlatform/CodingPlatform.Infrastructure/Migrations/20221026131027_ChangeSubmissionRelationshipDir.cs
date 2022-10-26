using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodingPlatform.Infrastructure.Migrations
{
    public partial class ChangeSubmissionRelationshipDir : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Users",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Tournaments",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Tips",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Subscriptions",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "DateSubmitted",
                table: "Submissions",
                newName: "SubmitDate");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Submissions",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Challenges",
                newName: "CreateDate");

            migrationBuilder.AddColumn<long>(
                name: "AdminId",
                table: "Submissions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TournamentId",
                table: "Submissions",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_AdminId",
                table: "Submissions",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_TournamentId",
                table: "Submissions",
                column: "TournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Tournaments_TournamentId",
                table: "Submissions",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Users_AdminId",
                table: "Submissions",
                column: "AdminId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Tournaments_TournamentId",
                table: "Submissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Users_AdminId",
                table: "Submissions");

            migrationBuilder.DropIndex(
                name: "IX_Submissions_AdminId",
                table: "Submissions");

            migrationBuilder.DropIndex(
                name: "IX_Submissions_TournamentId",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "TournamentId",
                table: "Submissions");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "Users",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "Tournaments",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "Tips",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "Subscriptions",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "SubmitDate",
                table: "Submissions",
                newName: "DateSubmitted");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "Submissions",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "Challenges",
                newName: "DateCreated");
        }
    }
}
