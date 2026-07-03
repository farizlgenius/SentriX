using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Role.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "role",
                table: "roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "role",
                table: "role_operators",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "role",
                table: "permissions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "role",
                table: "features",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 1,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 2,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 3,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 4,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 5,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 6,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 7,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 8,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 9,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 10,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 11,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 12,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 13,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 14,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 15,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 16,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 17,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 18,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 19,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 1,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 2,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 3,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 4,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 5,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 6,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 7,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 8,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 9,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 10,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 11,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 12,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 13,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 14,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 15,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 16,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 17,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 18,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 19,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "roles",
                keyColumn: "id",
                keyValue: 1,
                column: "is_default",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "role",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "role",
                table: "role_operators");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "role",
                table: "permissions");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "role",
                table: "features");
        }
    }
}
