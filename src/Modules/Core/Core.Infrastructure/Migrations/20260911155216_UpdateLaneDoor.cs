using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLaneDoor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doors_DeviceModules_device_module_id",
                schema: "core",
                table: "Doors");

            migrationBuilder.DropForeignKey(
                name: "FK_Doors_Lanes_lane_id",
                schema: "core",
                table: "Doors");

            migrationBuilder.DropIndex(
                name: "IX_Sensors_guid_id_door_id",
                schema: "core",
                table: "Sensors");

            migrationBuilder.DropIndex(
                name: "IX_Rexes_guid_id_door_id",
                schema: "core",
                table: "Rexes");

            migrationBuilder.DropIndex(
                name: "IX_Relays_guid_id_door_id",
                schema: "core",
                table: "Relays");

            migrationBuilder.DropIndex(
                name: "IX_Readers_guid_id_door_id",
                schema: "core",
                table: "Readers");

            migrationBuilder.DropIndex(
                name: "IX_Doors_device_module_id",
                schema: "core",
                table: "Doors");

            migrationBuilder.DropIndex(
                name: "IX_Doors_guid_id_device_module_id",
                schema: "core",
                table: "Doors");

            migrationBuilder.DropIndex(
                name: "IX_Doors_lane_id",
                schema: "core",
                table: "Doors");

            migrationBuilder.DropIndex(
                name: "IX_Buzzers_guid_id_door_id",
                schema: "core",
                table: "Buzzers");

            migrationBuilder.DropColumn(
                name: "device_module_id",
                schema: "core",
                table: "Doors");

            migrationBuilder.AlterColumn<int>(
                name: "door_id",
                schema: "core",
                table: "Sensors",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "device_module_id",
                schema: "core",
                table: "Sensors",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "lane_id",
                schema: "core",
                table: "Sensors",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "device_module_id",
                schema: "core",
                table: "Rexes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "device_module_id",
                schema: "core",
                table: "Relays",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "door_id",
                schema: "core",
                table: "Readers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "device_module_id",
                schema: "core",
                table: "Readers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "lane_id",
                schema: "core",
                table: "Readers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "sensor_id",
                schema: "core",
                table: "Lanes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "laneid",
                schema: "core",
                table: "Doors",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "device_module_id",
                schema: "core",
                table: "Buzzers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_device_module_id",
                schema: "core",
                table: "Sensors",
                column: "device_module_id");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_guid_id_door_id_device_module_id",
                schema: "core",
                table: "Sensors",
                columns: new[] { "guid", "id", "door_id", "device_module_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rexes_device_module_id",
                schema: "core",
                table: "Rexes",
                column: "device_module_id");

            migrationBuilder.CreateIndex(
                name: "IX_Rexes_guid_id_door_id_device_module_id",
                schema: "core",
                table: "Rexes",
                columns: new[] { "guid", "id", "door_id", "device_module_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Relays_device_module_id",
                schema: "core",
                table: "Relays",
                column: "device_module_id");

            migrationBuilder.CreateIndex(
                name: "IX_Relays_guid_id_door_id_device_module_id",
                schema: "core",
                table: "Relays",
                columns: new[] { "guid", "id", "door_id", "device_module_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Readers_device_module_id",
                schema: "core",
                table: "Readers",
                column: "device_module_id");

            migrationBuilder.CreateIndex(
                name: "IX_Readers_guid_id_door_id_device_module_id",
                schema: "core",
                table: "Readers",
                columns: new[] { "guid", "id", "door_id", "device_module_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Readers_lane_id",
                schema: "core",
                table: "Readers",
                column: "lane_id");

            migrationBuilder.CreateIndex(
                name: "IX_Lanes_sensor_id",
                schema: "core",
                table: "Lanes",
                column: "sensor_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doors_guid_id",
                schema: "core",
                table: "Doors",
                columns: new[] { "guid", "id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doors_laneid",
                schema: "core",
                table: "Doors",
                column: "laneid");

            migrationBuilder.CreateIndex(
                name: "IX_Buzzers_device_module_id",
                schema: "core",
                table: "Buzzers",
                column: "device_module_id");

            migrationBuilder.CreateIndex(
                name: "IX_Buzzers_guid_id_door_id_device_module_id",
                schema: "core",
                table: "Buzzers",
                columns: new[] { "guid", "id", "door_id", "device_module_id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Buzzers_DeviceModules_device_module_id",
                schema: "core",
                table: "Buzzers",
                column: "device_module_id",
                principalSchema: "core",
                principalTable: "DeviceModules",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Doors_Lanes_laneid",
                schema: "core",
                table: "Doors",
                column: "laneid",
                principalSchema: "core",
                principalTable: "Lanes",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lanes_Sensors_sensor_id",
                schema: "core",
                table: "Lanes",
                column: "sensor_id",
                principalSchema: "core",
                principalTable: "Sensors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Readers_DeviceModules_device_module_id",
                schema: "core",
                table: "Readers",
                column: "device_module_id",
                principalSchema: "core",
                principalTable: "DeviceModules",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Readers_Lanes_lane_id",
                schema: "core",
                table: "Readers",
                column: "lane_id",
                principalSchema: "core",
                principalTable: "Lanes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Relays_DeviceModules_device_module_id",
                schema: "core",
                table: "Relays",
                column: "device_module_id",
                principalSchema: "core",
                principalTable: "DeviceModules",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rexes_DeviceModules_device_module_id",
                schema: "core",
                table: "Rexes",
                column: "device_module_id",
                principalSchema: "core",
                principalTable: "DeviceModules",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_DeviceModules_device_module_id",
                schema: "core",
                table: "Sensors",
                column: "device_module_id",
                principalSchema: "core",
                principalTable: "DeviceModules",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buzzers_DeviceModules_device_module_id",
                schema: "core",
                table: "Buzzers");

            migrationBuilder.DropForeignKey(
                name: "FK_Doors_Lanes_laneid",
                schema: "core",
                table: "Doors");

            migrationBuilder.DropForeignKey(
                name: "FK_Lanes_Sensors_sensor_id",
                schema: "core",
                table: "Lanes");

            migrationBuilder.DropForeignKey(
                name: "FK_Readers_DeviceModules_device_module_id",
                schema: "core",
                table: "Readers");

            migrationBuilder.DropForeignKey(
                name: "FK_Readers_Lanes_lane_id",
                schema: "core",
                table: "Readers");

            migrationBuilder.DropForeignKey(
                name: "FK_Relays_DeviceModules_device_module_id",
                schema: "core",
                table: "Relays");

            migrationBuilder.DropForeignKey(
                name: "FK_Rexes_DeviceModules_device_module_id",
                schema: "core",
                table: "Rexes");

            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_DeviceModules_device_module_id",
                schema: "core",
                table: "Sensors");

            migrationBuilder.DropIndex(
                name: "IX_Sensors_device_module_id",
                schema: "core",
                table: "Sensors");

            migrationBuilder.DropIndex(
                name: "IX_Sensors_guid_id_door_id_device_module_id",
                schema: "core",
                table: "Sensors");

            migrationBuilder.DropIndex(
                name: "IX_Rexes_device_module_id",
                schema: "core",
                table: "Rexes");

            migrationBuilder.DropIndex(
                name: "IX_Rexes_guid_id_door_id_device_module_id",
                schema: "core",
                table: "Rexes");

            migrationBuilder.DropIndex(
                name: "IX_Relays_device_module_id",
                schema: "core",
                table: "Relays");

            migrationBuilder.DropIndex(
                name: "IX_Relays_guid_id_door_id_device_module_id",
                schema: "core",
                table: "Relays");

            migrationBuilder.DropIndex(
                name: "IX_Readers_device_module_id",
                schema: "core",
                table: "Readers");

            migrationBuilder.DropIndex(
                name: "IX_Readers_guid_id_door_id_device_module_id",
                schema: "core",
                table: "Readers");

            migrationBuilder.DropIndex(
                name: "IX_Readers_lane_id",
                schema: "core",
                table: "Readers");

            migrationBuilder.DropIndex(
                name: "IX_Lanes_sensor_id",
                schema: "core",
                table: "Lanes");

            migrationBuilder.DropIndex(
                name: "IX_Doors_guid_id",
                schema: "core",
                table: "Doors");

            migrationBuilder.DropIndex(
                name: "IX_Doors_laneid",
                schema: "core",
                table: "Doors");

            migrationBuilder.DropIndex(
                name: "IX_Buzzers_device_module_id",
                schema: "core",
                table: "Buzzers");

            migrationBuilder.DropIndex(
                name: "IX_Buzzers_guid_id_door_id_device_module_id",
                schema: "core",
                table: "Buzzers");

            migrationBuilder.DropColumn(
                name: "device_module_id",
                schema: "core",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "lane_id",
                schema: "core",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "device_module_id",
                schema: "core",
                table: "Rexes");

            migrationBuilder.DropColumn(
                name: "device_module_id",
                schema: "core",
                table: "Relays");

            migrationBuilder.DropColumn(
                name: "device_module_id",
                schema: "core",
                table: "Readers");

            migrationBuilder.DropColumn(
                name: "lane_id",
                schema: "core",
                table: "Readers");

            migrationBuilder.DropColumn(
                name: "sensor_id",
                schema: "core",
                table: "Lanes");

            migrationBuilder.DropColumn(
                name: "laneid",
                schema: "core",
                table: "Doors");

            migrationBuilder.DropColumn(
                name: "device_module_id",
                schema: "core",
                table: "Buzzers");

            migrationBuilder.AlterColumn<int>(
                name: "door_id",
                schema: "core",
                table: "Sensors",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "door_id",
                schema: "core",
                table: "Readers",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "device_module_id",
                schema: "core",
                table: "Doors",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_guid_id_door_id",
                schema: "core",
                table: "Sensors",
                columns: new[] { "guid", "id", "door_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rexes_guid_id_door_id",
                schema: "core",
                table: "Rexes",
                columns: new[] { "guid", "id", "door_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Relays_guid_id_door_id",
                schema: "core",
                table: "Relays",
                columns: new[] { "guid", "id", "door_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Readers_guid_id_door_id",
                schema: "core",
                table: "Readers",
                columns: new[] { "guid", "id", "door_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doors_device_module_id",
                schema: "core",
                table: "Doors",
                column: "device_module_id");

            migrationBuilder.CreateIndex(
                name: "IX_Doors_guid_id_device_module_id",
                schema: "core",
                table: "Doors",
                columns: new[] { "guid", "id", "device_module_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doors_lane_id",
                schema: "core",
                table: "Doors",
                column: "lane_id");

            migrationBuilder.CreateIndex(
                name: "IX_Buzzers_guid_id_door_id",
                schema: "core",
                table: "Buzzers",
                columns: new[] { "guid", "id", "door_id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Doors_DeviceModules_device_module_id",
                schema: "core",
                table: "Doors",
                column: "device_module_id",
                principalSchema: "core",
                principalTable: "DeviceModules",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Doors_Lanes_lane_id",
                schema: "core",
                table: "Doors",
                column: "lane_id",
                principalSchema: "core",
                principalTable: "Lanes",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
