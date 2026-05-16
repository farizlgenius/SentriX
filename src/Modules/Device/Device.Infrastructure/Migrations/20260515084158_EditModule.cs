using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Device.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modules_devices_device_id",
                schema: "device",
                table: "Modules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_devices",
                schema: "device",
                table: "devices");

            migrationBuilder.RenameTable(
                name: "devices",
                schema: "device",
                newName: "Devices",
                newSchema: "device");

            migrationBuilder.AddColumn<int>(
                name: "address",
                schema: "device",
                table: "Modules",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "component_id",
                schema: "device",
                table: "Modules",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "mac",
                schema: "device",
                table: "Modules",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "port",
                schema: "device",
                table: "Modules",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Devices",
                schema: "device",
                table: "Devices",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Devices_device_id",
                schema: "device",
                table: "Modules",
                column: "device_id",
                principalSchema: "device",
                principalTable: "Devices",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Devices_device_id",
                schema: "device",
                table: "Modules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Devices",
                schema: "device",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "address",
                schema: "device",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "component_id",
                schema: "device",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "mac",
                schema: "device",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "port",
                schema: "device",
                table: "Modules");

            migrationBuilder.RenameTable(
                name: "Devices",
                schema: "device",
                newName: "devices",
                newSchema: "device");

            migrationBuilder.AddPrimaryKey(
                name: "PK_devices",
                schema: "device",
                table: "devices",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_devices_device_id",
                schema: "device",
                table: "Modules",
                column: "device_id",
                principalSchema: "device",
                principalTable: "devices",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
