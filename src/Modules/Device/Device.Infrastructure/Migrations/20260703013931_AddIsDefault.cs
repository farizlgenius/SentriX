using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Device.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "device",
                table: "Relays",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "device",
                table: "Readers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "device",
                table: "Modules",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "device",
                table: "Inputs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "device",
                table: "Devices",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "device",
                table: "Relays");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "device",
                table: "Readers");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "device",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "device",
                table: "Inputs");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "device",
                table: "Devices");
        }
    }
}
