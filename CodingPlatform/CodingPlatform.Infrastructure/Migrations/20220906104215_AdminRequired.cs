using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodingPlatform.Infrastructure.Migrations
{
    public partial class AdminRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournament_Users_AdminId",
                table: "Tournament");

            migrationBuilder.AlterColumn<long>(
                name: "AdminId",
                table: "Tournament",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tournament_Users_AdminId",
                table: "Tournament",
                column: "AdminId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournament_Users_AdminId",
                table: "Tournament");

            migrationBuilder.AlterColumn<long>(
                name: "AdminId",
                table: "Tournament",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournament_Users_AdminId",
                table: "Tournament",
                column: "AdminId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
