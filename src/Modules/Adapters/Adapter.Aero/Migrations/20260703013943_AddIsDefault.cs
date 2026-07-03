using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adapter.Aero.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "aero",
                table: "SioPanelConfiguration",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "aero",
                table: "ScpDeviceSpecifications",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "aero",
                table: "OutputPointSpecification",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "aero",
                table: "InputPointSpecification",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "aero",
                table: "ElevatorAccessLevelSpecifications",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "aero",
                table: "DriverConfiguration",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "aero",
                table: "ControlPointConfiguration",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "aero",
                table: "Aeros",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "aero",
                table: "AccessDatabaseSpecifications",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                schema: "aero",
                table: "AccessDatabaseSpecifications",
                keyColumn: "id",
                keyValue: 1,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "aero",
                table: "ElevatorAccessLevelSpecifications",
                keyColumn: "id",
                keyValue: 1,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "aero",
                table: "ScpDeviceSpecifications",
                keyColumn: "id",
                keyValue: 1,
                column: "is_default",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "aero",
                table: "SioPanelConfiguration");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "aero",
                table: "ScpDeviceSpecifications");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "aero",
                table: "OutputPointSpecification");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "aero",
                table: "InputPointSpecification");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "aero",
                table: "ElevatorAccessLevelSpecifications");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "aero",
                table: "DriverConfiguration");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "aero",
                table: "ControlPointConfiguration");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "aero",
                table: "Aeros");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "aero",
                table: "AccessDatabaseSpecifications");
        }
    }
}
