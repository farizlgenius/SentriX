using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Output.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "output",
                table: "Outputs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "output",
                table: "OutputOfflineModes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "output",
                table: "OutputModes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "output",
                table: "OutputDriveModes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                schema: "output",
                table: "OutputDriveModes",
                keyColumn: "id",
                keyValue: 1,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "output",
                table: "OutputDriveModes",
                keyColumn: "id",
                keyValue: 2,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "output",
                table: "OutputModes",
                keyColumn: "id",
                keyValue: 1,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "output",
                table: "OutputModes",
                keyColumn: "id",
                keyValue: 2,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "output",
                table: "OutputModes",
                keyColumn: "id",
                keyValue: 3,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "output",
                table: "OutputModes",
                keyColumn: "id",
                keyValue: 4,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "output",
                table: "OutputModes",
                keyColumn: "id",
                keyValue: 5,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "output",
                table: "OutputModes",
                keyColumn: "id",
                keyValue: 6,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "output",
                table: "OutputOfflineModes",
                keyColumn: "id",
                keyValue: 1,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "output",
                table: "OutputOfflineModes",
                keyColumn: "id",
                keyValue: 2,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "output",
                table: "OutputOfflineModes",
                keyColumn: "id",
                keyValue: 3,
                column: "is_default",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "output",
                table: "Outputs");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "output",
                table: "OutputOfflineModes");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "output",
                table: "OutputModes");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "output",
                table: "OutputDriveModes");
        }
    }
}
