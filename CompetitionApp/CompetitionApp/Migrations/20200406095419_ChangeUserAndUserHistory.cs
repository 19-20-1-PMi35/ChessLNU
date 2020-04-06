using Microsoft.EntityFrameworkCore.Migrations;

namespace CompetitionApp.Migrations
{
    public partial class ChangeUserAndUserHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserHistories_Events_EventId",
                table: "UserHistories");

            migrationBuilder.DropIndex(
                name: "IX_UserHistories_UserId",
                table: "UserHistories");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "UserHistories",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserHistories_UserId",
                table: "UserHistories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserHistories_Events_EventId",
                table: "UserHistories",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserHistories_Events_EventId",
                table: "UserHistories");

            migrationBuilder.DropIndex(
                name: "IX_UserHistories_UserId",
                table: "UserHistories");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "UserHistories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_UserHistories_UserId",
                table: "UserHistories",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_UserHistories_Events_EventId",
                table: "UserHistories",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
