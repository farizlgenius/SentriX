using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Operator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "operator",
                table: "operators",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "operator",
                table: "operator_locations",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                schema: "operator",
                table: "operator_locations",
                keyColumn: "id",
                keyValue: 1,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "operator",
                table: "operators",
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
                schema: "operator",
                table: "operators");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "operator",
                table: "operator_locations");
        }
    }
}
