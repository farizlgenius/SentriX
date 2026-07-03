using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Input.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "input",
                table: "Inputs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "input",
                table: "InputModes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "input",
                table: "InputLists",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "input",
                table: "InputGroups",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "input",
                table: "InputGroupDetails",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                schema: "input",
                table: "InputModes",
                keyColumn: "id",
                keyValue: 1,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "input",
                table: "InputModes",
                keyColumn: "id",
                keyValue: 2,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "input",
                table: "InputModes",
                keyColumn: "id",
                keyValue: 3,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "input",
                table: "InputModes",
                keyColumn: "id",
                keyValue: 4,
                column: "is_default",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "input",
                table: "Inputs");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "input",
                table: "InputModes");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "input",
                table: "InputLists");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "input",
                table: "InputGroups");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "input",
                table: "InputGroupDetails");
        }
    }
}
