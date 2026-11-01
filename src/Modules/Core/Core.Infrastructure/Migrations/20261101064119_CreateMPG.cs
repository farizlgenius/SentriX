using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateMPG : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonitorGroup_Devices_device_id",
                schema: "core",
                table: "MonitorGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_MonitorGroup_Locations_locationid",
                schema: "core",
                table: "MonitorGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_MonitorGroupComponent_Doors_door_id",
                schema: "core",
                table: "MonitorGroupComponent");

            migrationBuilder.DropForeignKey(
                name: "FK_MonitorGroupComponent_Inputs_input_id",
                schema: "core",
                table: "MonitorGroupComponent");

            migrationBuilder.DropForeignKey(
                name: "FK_MonitorGroupComponent_MonitorGroup_monitor_group_id",
                schema: "core",
                table: "MonitorGroupComponent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MonitorGroupComponent",
                schema: "core",
                table: "MonitorGroupComponent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MonitorGroup",
                schema: "core",
                table: "MonitorGroup");

            migrationBuilder.RenameTable(
                name: "MonitorGroupComponent",
                schema: "core",
                newName: "MonitorGroupComponents",
                newSchema: "core");

            migrationBuilder.RenameTable(
                name: "MonitorGroup",
                schema: "core",
                newName: "MonitorGroups",
                newSchema: "core");

            migrationBuilder.RenameIndex(
                name: "IX_MonitorGroupComponent_monitor_group_id",
                schema: "core",
                table: "MonitorGroupComponents",
                newName: "IX_MonitorGroupComponents_monitor_group_id");

            migrationBuilder.RenameIndex(
                name: "IX_MonitorGroupComponent_input_id",
                schema: "core",
                table: "MonitorGroupComponents",
                newName: "IX_MonitorGroupComponents_input_id");

            migrationBuilder.RenameIndex(
                name: "IX_MonitorGroupComponent_door_id",
                schema: "core",
                table: "MonitorGroupComponents",
                newName: "IX_MonitorGroupComponents_door_id");

            migrationBuilder.RenameIndex(
                name: "IX_MonitorGroup_locationid",
                schema: "core",
                table: "MonitorGroups",
                newName: "IX_MonitorGroups_locationid");

            migrationBuilder.RenameIndex(
                name: "IX_MonitorGroup_device_id",
                schema: "core",
                table: "MonitorGroups",
                newName: "IX_MonitorGroups_device_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MonitorGroupComponents",
                schema: "core",
                table: "MonitorGroupComponents",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MonitorGroups",
                schema: "core",
                table: "MonitorGroups",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_MonitorGroupComponents_id_door_id_monitor_group_id_input_id",
                schema: "core",
                table: "MonitorGroupComponents",
                columns: new[] { "id", "door_id", "monitor_group_id", "input_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonitorGroups_guid_id_location_id",
                schema: "core",
                table: "MonitorGroups",
                columns: new[] { "guid", "id", "location_id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MonitorGroupComponents_Doors_door_id",
                schema: "core",
                table: "MonitorGroupComponents",
                column: "door_id",
                principalSchema: "core",
                principalTable: "Doors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MonitorGroupComponents_Inputs_input_id",
                schema: "core",
                table: "MonitorGroupComponents",
                column: "input_id",
                principalSchema: "core",
                principalTable: "Inputs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MonitorGroupComponents_MonitorGroups_monitor_group_id",
                schema: "core",
                table: "MonitorGroupComponents",
                column: "monitor_group_id",
                principalSchema: "core",
                principalTable: "MonitorGroups",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MonitorGroups_Devices_device_id",
                schema: "core",
                table: "MonitorGroups",
                column: "device_id",
                principalSchema: "core",
                principalTable: "Devices",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MonitorGroups_Locations_locationid",
                schema: "core",
                table: "MonitorGroups",
                column: "locationid",
                principalSchema: "core",
                principalTable: "Locations",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonitorGroupComponents_Doors_door_id",
                schema: "core",
                table: "MonitorGroupComponents");

            migrationBuilder.DropForeignKey(
                name: "FK_MonitorGroupComponents_Inputs_input_id",
                schema: "core",
                table: "MonitorGroupComponents");

            migrationBuilder.DropForeignKey(
                name: "FK_MonitorGroupComponents_MonitorGroups_monitor_group_id",
                schema: "core",
                table: "MonitorGroupComponents");

            migrationBuilder.DropForeignKey(
                name: "FK_MonitorGroups_Devices_device_id",
                schema: "core",
                table: "MonitorGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_MonitorGroups_Locations_locationid",
                schema: "core",
                table: "MonitorGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MonitorGroups",
                schema: "core",
                table: "MonitorGroups");

            migrationBuilder.DropIndex(
                name: "IX_MonitorGroups_guid_id_location_id",
                schema: "core",
                table: "MonitorGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MonitorGroupComponents",
                schema: "core",
                table: "MonitorGroupComponents");

            migrationBuilder.DropIndex(
                name: "IX_MonitorGroupComponents_id_door_id_monitor_group_id_input_id",
                schema: "core",
                table: "MonitorGroupComponents");

            migrationBuilder.RenameTable(
                name: "MonitorGroups",
                schema: "core",
                newName: "MonitorGroup",
                newSchema: "core");

            migrationBuilder.RenameTable(
                name: "MonitorGroupComponents",
                schema: "core",
                newName: "MonitorGroupComponent",
                newSchema: "core");

            migrationBuilder.RenameIndex(
                name: "IX_MonitorGroups_locationid",
                schema: "core",
                table: "MonitorGroup",
                newName: "IX_MonitorGroup_locationid");

            migrationBuilder.RenameIndex(
                name: "IX_MonitorGroups_device_id",
                schema: "core",
                table: "MonitorGroup",
                newName: "IX_MonitorGroup_device_id");

            migrationBuilder.RenameIndex(
                name: "IX_MonitorGroupComponents_monitor_group_id",
                schema: "core",
                table: "MonitorGroupComponent",
                newName: "IX_MonitorGroupComponent_monitor_group_id");

            migrationBuilder.RenameIndex(
                name: "IX_MonitorGroupComponents_input_id",
                schema: "core",
                table: "MonitorGroupComponent",
                newName: "IX_MonitorGroupComponent_input_id");

            migrationBuilder.RenameIndex(
                name: "IX_MonitorGroupComponents_door_id",
                schema: "core",
                table: "MonitorGroupComponent",
                newName: "IX_MonitorGroupComponent_door_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MonitorGroup",
                schema: "core",
                table: "MonitorGroup",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MonitorGroupComponent",
                schema: "core",
                table: "MonitorGroupComponent",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_MonitorGroup_Devices_device_id",
                schema: "core",
                table: "MonitorGroup",
                column: "device_id",
                principalSchema: "core",
                principalTable: "Devices",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MonitorGroup_Locations_locationid",
                schema: "core",
                table: "MonitorGroup",
                column: "locationid",
                principalSchema: "core",
                principalTable: "Locations",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_MonitorGroupComponent_Doors_door_id",
                schema: "core",
                table: "MonitorGroupComponent",
                column: "door_id",
                principalSchema: "core",
                principalTable: "Doors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MonitorGroupComponent_Inputs_input_id",
                schema: "core",
                table: "MonitorGroupComponent",
                column: "input_id",
                principalSchema: "core",
                principalTable: "Inputs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MonitorGroupComponent_MonitorGroup_monitor_group_id",
                schema: "core",
                table: "MonitorGroupComponent",
                column: "monitor_group_id",
                principalSchema: "core",
                principalTable: "MonitorGroup",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
