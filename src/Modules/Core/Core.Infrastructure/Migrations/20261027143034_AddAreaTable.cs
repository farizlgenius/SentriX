using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAreaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Devices_guid_mac_vendor",
                schema: "core",
                table: "Devices");

            migrationBuilder.AddColumn<int>(
                name: "antipassback_from",
                schema: "core",
                table: "Doors",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "antipassback_to",
                schema: "core",
                table: "Doors",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "from_area_id",
                schema: "core",
                table: "Doors",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "to_area_id",
                schema: "core",
                table: "Doors",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Areas",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    multi_occ_mode = table.Column<int>(type: "integer", nullable: false),
                    area_mode = table.Column<int>(type: "integer", nullable: false),
                    occ_max = table.Column<short>(type: "smallint", nullable: false),
                    occ_down = table.Column<short>(type: "smallint", nullable: false),
                    occ_up = table.Column<short>(type: "smallint", nullable: false),
                    metadata = table.Column<string>(type: "text", nullable: false),
                    Deviceid = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "text", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: true),
                    external_id = table.Column<short>(type: "smallint", nullable: true),
                    altr_external_id = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.id);
                    table.ForeignKey(
                        name: "FK_Areas_Devices_Deviceid",
                        column: x => x.Deviceid,
                        principalSchema: "core",
                        principalTable: "Devices",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Areas_Locations_location_id",
                        column: x => x.location_id,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonitorGroup",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    device_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: true),
                    locationid = table.Column<int>(type: "integer", nullable: true),
                    external_id = table.Column<short>(type: "smallint", nullable: true),
                    altr_external_id = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitorGroup", x => x.id);
                    table.ForeignKey(
                        name: "FK_MonitorGroup_Devices_device_id",
                        column: x => x.device_id,
                        principalSchema: "core",
                        principalTable: "Devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonitorGroup_Locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "MonitorGroupComponent",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<int>(type: "integer", nullable: false),
                    input_id = table.Column<int>(type: "integer", nullable: true),
                    door_id = table.Column<int>(type: "integer", nullable: true),
                    monitor_group_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitorGroupComponent", x => x.id);
                    table.ForeignKey(
                        name: "FK_MonitorGroupComponent_Doors_door_id",
                        column: x => x.door_id,
                        principalSchema: "core",
                        principalTable: "Doors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonitorGroupComponent_Input_input_id",
                        column: x => x.input_id,
                        principalSchema: "core",
                        principalTable: "Input",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonitorGroupComponent_MonitorGroup_monitor_group_id",
                        column: x => x.monitor_group_id,
                        principalSchema: "core",
                        principalTable: "MonitorGroup",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Doors_from_area_id",
                schema: "core",
                table: "Doors",
                column: "from_area_id");

            migrationBuilder.CreateIndex(
                name: "IX_Doors_to_area_id",
                schema: "core",
                table: "Doors",
                column: "to_area_id");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_guid_mac_vendor_location_id",
                schema: "core",
                table: "Devices",
                columns: new[] { "guid", "mac", "vendor", "location_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Areas_Deviceid",
                schema: "core",
                table: "Areas",
                column: "Deviceid");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_guid_id_location_id",
                schema: "core",
                table: "Areas",
                columns: new[] { "guid", "id", "location_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Areas_location_id",
                schema: "core",
                table: "Areas",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_MonitorGroup_device_id",
                schema: "core",
                table: "MonitorGroup",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "IX_MonitorGroup_locationid",
                schema: "core",
                table: "MonitorGroup",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_MonitorGroupComponent_door_id",
                schema: "core",
                table: "MonitorGroupComponent",
                column: "door_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonitorGroupComponent_input_id",
                schema: "core",
                table: "MonitorGroupComponent",
                column: "input_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonitorGroupComponent_monitor_group_id",
                schema: "core",
                table: "MonitorGroupComponent",
                column: "monitor_group_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Doors_Areas_from_area_id",
                schema: "core",
                table: "Doors",
                column: "from_area_id",
                principalSchema: "core",
                principalTable: "Areas",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Doors_Areas_to_area_id",
                schema: "core",
                table: "Doors",
                column: "to_area_id",
                principalSchema: "core",
                principalTable: "Areas",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doors_Areas_from_area_id",
                schema: "core",
                table: "Doors");

            migrationBuilder.DropForeignKey(
                name: "FK_Doors_Areas_to_area_id",
                schema: "core",
                table: "Doors");

            migrationBuilder.DropTable(
                name: "Areas",
                schema: "core");

            migrationBuilder.DropTable(
                name: "MonitorGroupComponent",
                schema: "core");

            migrationBuilder.DropTable(
                name: "MonitorGroup",
                schema: "core");

            migrationBuilder.DropIndex(
                name: "IX_Doors_from_area_id",
                schema: "core",
                table: "Doors");

            migrationBuilder.DropIndex(
                name: "IX_Doors_to_area_id",
                schema: "core",
                table: "Doors");

            migrationBuilder.DropIndex(
                name: "IX_Devices_guid_mac_vendor_location_id",
                schema: "core",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "antipassback_from",
                schema: "core",
                table: "Doors");

            migrationBuilder.DropColumn(
                name: "antipassback_to",
                schema: "core",
                table: "Doors");

            migrationBuilder.DropColumn(
                name: "from_area_id",
                schema: "core",
                table: "Doors");

            migrationBuilder.DropColumn(
                name: "to_area_id",
                schema: "core",
                table: "Doors");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_guid_mac_vendor",
                schema: "core",
                table: "Devices",
                columns: new[] { "guid", "mac", "vendor" },
                unique: true);
        }
    }
}
