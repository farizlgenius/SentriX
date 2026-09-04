using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDeviceModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SubDevices_device_id",
                schema: "core",
                table: "SubDevices");

            migrationBuilder.DropIndex(
                name: "IX_SubDevices_guid_location_id",
                schema: "core",
                table: "SubDevices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_guid",
                schema: "core",
                table: "Devices");

            migrationBuilder.CreateIndex(
                name: "IX_SubDevices_device_id_mac_guid_location_id",
                schema: "core",
                table: "SubDevices",
                columns: new[] { "device_id", "mac", "guid", "location_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_guid_mac_vendor",
                schema: "core",
                table: "Devices",
                columns: new[] { "guid", "mac", "vendor" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SubDevices_device_id_mac_guid_location_id",
                schema: "core",
                table: "SubDevices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_guid_mac_vendor",
                schema: "core",
                table: "Devices");

            migrationBuilder.CreateIndex(
                name: "IX_SubDevices_device_id",
                schema: "core",
                table: "SubDevices",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "IX_SubDevices_guid_location_id",
                schema: "core",
                table: "SubDevices",
                columns: new[] { "guid", "location_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_guid",
                schema: "core",
                table: "Devices",
                column: "guid",
                unique: true);
        }
    }
}
