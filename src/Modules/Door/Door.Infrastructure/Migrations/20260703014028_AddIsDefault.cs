using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Door.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "door",
                table: "StrikeModes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "door",
                table: "SpareFlags",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "door",
                table: "ReaderModes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "door",
                table: "OsdpBaudrates",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "door",
                table: "Doors",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "door",
                table: "DoorModes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "door",
                table: "ApbModes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "door",
                table: "AccessControlFlags",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "AccessControlFlags",
                keyColumn: "id",
                keyValue: 1,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "AccessControlFlags",
                keyColumn: "id",
                keyValue: 2,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "AccessControlFlags",
                keyColumn: "id",
                keyValue: 3,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "AccessControlFlags",
                keyColumn: "id",
                keyValue: 4,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "AccessControlFlags",
                keyColumn: "id",
                keyValue: 5,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "AccessControlFlags",
                keyColumn: "id",
                keyValue: 6,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "AccessControlFlags",
                keyColumn: "id",
                keyValue: 7,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "AccessControlFlags",
                keyColumn: "id",
                keyValue: 8,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "AccessControlFlags",
                keyColumn: "id",
                keyValue: 9,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "AccessControlFlags",
                keyColumn: "id",
                keyValue: 10,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "AccessControlFlags",
                keyColumn: "id",
                keyValue: 11,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "AccessControlFlags",
                keyColumn: "id",
                keyValue: 12,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "ApbModes",
                keyColumn: "id",
                keyValue: 1,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "ApbModes",
                keyColumn: "id",
                keyValue: 2,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "ApbModes",
                keyColumn: "id",
                keyValue: 3,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "ApbModes",
                keyColumn: "id",
                keyValue: 4,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "ApbModes",
                keyColumn: "id",
                keyValue: 5,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "ApbModes",
                keyColumn: "id",
                keyValue: 6,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "ApbModes",
                keyColumn: "id",
                keyValue: 7,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "ApbModes",
                keyColumn: "id",
                keyValue: 8,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "ApbModes",
                keyColumn: "id",
                keyValue: 9,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "DoorModes",
                keyColumn: "id",
                keyValue: 1,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "DoorModes",
                keyColumn: "id",
                keyValue: 2,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "DoorModes",
                keyColumn: "id",
                keyValue: 3,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "DoorModes",
                keyColumn: "id",
                keyValue: 4,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "DoorModes",
                keyColumn: "id",
                keyValue: 5,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "DoorModes",
                keyColumn: "id",
                keyValue: 6,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "DoorModes",
                keyColumn: "id",
                keyValue: 7,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "DoorModes",
                keyColumn: "id",
                keyValue: 8,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "OsdpBaudrates",
                keyColumn: "id",
                keyValue: 1,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "OsdpBaudrates",
                keyColumn: "id",
                keyValue: 2,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "OsdpBaudrates",
                keyColumn: "id",
                keyValue: 3,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "OsdpBaudrates",
                keyColumn: "id",
                keyValue: 4,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "OsdpBaudrates",
                keyColumn: "id",
                keyValue: 5,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "OsdpBaudrates",
                keyColumn: "id",
                keyValue: 6,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "ReaderModes",
                keyColumn: "id",
                keyValue: 1,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "ReaderModes",
                keyColumn: "id",
                keyValue: 2,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "ReaderModes",
                keyColumn: "id",
                keyValue: 3,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "ReaderModes",
                keyColumn: "id",
                keyValue: 4,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "ReaderModes",
                keyColumn: "id",
                keyValue: 5,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "SpareFlags",
                keyColumn: "id",
                keyValue: 1,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "SpareFlags",
                keyColumn: "id",
                keyValue: 2,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "SpareFlags",
                keyColumn: "id",
                keyValue: 3,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "SpareFlags",
                keyColumn: "id",
                keyValue: 4,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "SpareFlags",
                keyColumn: "id",
                keyValue: 5,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "SpareFlags",
                keyColumn: "id",
                keyValue: 6,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "SpareFlags",
                keyColumn: "id",
                keyValue: 7,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "SpareFlags",
                keyColumn: "id",
                keyValue: 8,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "SpareFlags",
                keyColumn: "id",
                keyValue: 9,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "SpareFlags",
                keyColumn: "id",
                keyValue: 10,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "SpareFlags",
                keyColumn: "id",
                keyValue: 11,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "SpareFlags",
                keyColumn: "id",
                keyValue: 12,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "SpareFlags",
                keyColumn: "id",
                keyValue: 13,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "SpareFlags",
                keyColumn: "id",
                keyValue: 14,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "StrikeModes",
                keyColumn: "id",
                keyValue: 1,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "StrikeModes",
                keyColumn: "id",
                keyValue: 2,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "StrikeModes",
                keyColumn: "id",
                keyValue: 3,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "door",
                table: "StrikeModes",
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
                schema: "door",
                table: "StrikeModes");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "door",
                table: "SpareFlags");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "door",
                table: "ReaderModes");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "door",
                table: "OsdpBaudrates");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "door",
                table: "Doors");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "door",
                table: "DoorModes");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "door",
                table: "ApbModes");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "door",
                table: "AccessControlFlags");
        }
    }
}
