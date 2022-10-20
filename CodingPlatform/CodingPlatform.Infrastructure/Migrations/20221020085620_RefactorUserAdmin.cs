using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodingPlatform.Infrastructure.Migrations
{
    public partial class RefactorUserAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Users_AdminId",
                table: "Tournaments");

            migrationBuilder.AlterColumn<long>(
                name: "AdminId",
                table: "Tournaments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Users_AdminId",
                table: "Tournaments",
                column: "AdminId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Users_AdminId",
                table: "Tournaments");

            migrationBuilder.AlterColumn<long>(
                name: "AdminId",
                table: "Tournaments",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Users_AdminId",
                table: "Tournaments",
                column: "AdminId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
