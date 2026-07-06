using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRelate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGroups_Users_userid",
                schema: "user",
                table: "UserGroups");

            migrationBuilder.DropIndex(
                name: "IX_UserGroups_userid",
                schema: "user",
                table: "UserGroups");

            migrationBuilder.DropColumn(
                name: "userid",
                schema: "user",
                table: "UserGroups");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_user_id",
                schema: "user",
                table: "UserGroups",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroups_Users_user_id",
                schema: "user",
                table: "UserGroups",
                column: "user_id",
                principalSchema: "user",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGroups_Users_user_id",
                schema: "user",
                table: "UserGroups");

            migrationBuilder.DropIndex(
                name: "IX_UserGroups_user_id",
                schema: "user",
                table: "UserGroups");

            migrationBuilder.AddColumn<int>(
                name: "userid",
                schema: "user",
                table: "UserGroups",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_userid",
                schema: "user",
                table: "UserGroups",
                column: "userid");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroups_Users_userid",
                schema: "user",
                table: "UserGroups",
                column: "userid",
                principalSchema: "user",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
