using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "user",
                table: "Vacations",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "user",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "user",
                table: "UserGroups",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "user",
                table: "UserFlags",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "user",
                table: "UserAdditionals",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "user",
                table: "Positions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "user",
                table: "Departments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "user",
                table: "Credentials",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "user",
                table: "Companies",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                schema: "user",
                table: "UserFlags",
                keyColumn: "id",
                keyValue: 1,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "user",
                table: "UserFlags",
                keyColumn: "id",
                keyValue: 2,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "user",
                table: "UserFlags",
                keyColumn: "id",
                keyValue: 3,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "user",
                table: "UserFlags",
                keyColumn: "id",
                keyValue: 4,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "user",
                table: "UserFlags",
                keyColumn: "id",
                keyValue: 5,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "user",
                table: "UserFlags",
                keyColumn: "id",
                keyValue: 6,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "user",
                table: "UserFlags",
                keyColumn: "id",
                keyValue: 7,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "user",
                table: "UserFlags",
                keyColumn: "id",
                keyValue: 8,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "user",
                table: "UserFlags",
                keyColumn: "id",
                keyValue: 9,
                column: "is_default",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "user",
                table: "Vacations");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "user",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "user",
                table: "UserGroups");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "user",
                table: "UserFlags");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "user",
                table: "UserAdditionals");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "user",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "user",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "user",
                table: "Credentials");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "user",
                table: "Companies");
        }
    }
}
