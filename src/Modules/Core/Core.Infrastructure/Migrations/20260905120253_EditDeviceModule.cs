using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditDeviceModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubDevices_Devices_device_id",
                schema: "core",
                table: "SubDevices");

            migrationBuilder.DropForeignKey(
                name: "FK_SubDevices_Locations_location_id",
                schema: "core",
                table: "SubDevices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubDevices",
                schema: "core",
                table: "SubDevices");

            migrationBuilder.RenameTable(
                name: "SubDevices",
                schema: "core",
                newName: "DeviceModules",
                newSchema: "core");

            migrationBuilder.RenameIndex(
                name: "IX_SubDevices_location_id",
                schema: "core",
                table: "DeviceModules",
                newName: "IX_DeviceModules_location_id");

            migrationBuilder.RenameIndex(
                name: "IX_SubDevices_device_id_mac_guid_location_id",
                schema: "core",
                table: "DeviceModules",
                newName: "IX_DeviceModules_device_id_mac_guid_location_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeviceModules",
                schema: "core",
                table: "DeviceModules",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceModules_Devices_device_id",
                schema: "core",
                table: "DeviceModules",
                column: "device_id",
                principalSchema: "core",
                principalTable: "Devices",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceModules_Locations_location_id",
                schema: "core",
                table: "DeviceModules",
                column: "location_id",
                principalSchema: "core",
                principalTable: "Locations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceModules_Devices_device_id",
                schema: "core",
                table: "DeviceModules");

            migrationBuilder.DropForeignKey(
                name: "FK_DeviceModules_Locations_location_id",
                schema: "core",
                table: "DeviceModules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeviceModules",
                schema: "core",
                table: "DeviceModules");

            migrationBuilder.RenameTable(
                name: "DeviceModules",
                schema: "core",
                newName: "SubDevices",
                newSchema: "core");

            migrationBuilder.RenameIndex(
                name: "IX_DeviceModules_location_id",
                schema: "core",
                table: "SubDevices",
                newName: "IX_SubDevices_location_id");

            migrationBuilder.RenameIndex(
                name: "IX_DeviceModules_device_id_mac_guid_location_id",
                schema: "core",
                table: "SubDevices",
                newName: "IX_SubDevices_device_id_mac_guid_location_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubDevices",
                schema: "core",
                table: "SubDevices",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubDevices_Devices_device_id",
                schema: "core",
                table: "SubDevices",
                column: "device_id",
                principalSchema: "core",
                principalTable: "Devices",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubDevices_Locations_location_id",
                schema: "core",
                table: "SubDevices",
                column: "location_id",
                principalSchema: "core",
                principalTable: "Locations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
