using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTriggerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TriggerArgs",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    trigger_code_id = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<short>(type: "smallint", nullable: false),
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
                    table.PrimaryKey("PK_TriggerArgs", x => x.id);
                    table.ForeignKey(
                        name: "FK_TriggerArgs_Locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TriggerArgs_TriggerCodes_trigger_code_id",
                        column: x => x.trigger_code_id,
                        principalSchema: "core",
                        principalTable: "TriggerCodes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Triggers",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    command = table.Column<int>(type: "integer", nullable: false),
                    source_component_type = table.Column<int>(type: "integer", nullable: false),
                    src_device_module_id = table.Column<int>(type: "integer", nullable: true),
                    src_input_id = table.Column<int>(type: "integer", nullable: true),
                    src_output_id = table.Column<int>(type: "integer", nullable: true),
                    src_door_id = table.Column<int>(type: "integer", nullable: true),
                    src_timezone_id = table.Column<int>(type: "integer", nullable: true),
                    src_monitor_group_id = table.Column<int>(type: "integer", nullable: true),
                    src_area_id = table.Column<int>(type: "integer", nullable: true),
                    device_id = table.Column<int>(type: "integer", nullable: false),
                    procedure_id = table.Column<int>(type: "integer", nullable: false),
                    trigger_source_id = table.Column<int>(type: "integer", nullable: false),
                    trigger_type_id = table.Column<int>(type: "integer", nullable: false),
                    timezone_id = table.Column<int>(type: "integer", nullable: false),
                    timezoneid = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_Triggers", x => x.id);
                    table.ForeignKey(
                        name: "FK_Triggers_Areas_src_area_id",
                        column: x => x.src_area_id,
                        principalSchema: "core",
                        principalTable: "Areas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Triggers_DeviceModules_src_device_module_id",
                        column: x => x.src_device_module_id,
                        principalSchema: "core",
                        principalTable: "DeviceModules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Triggers_Devices_device_id",
                        column: x => x.device_id,
                        principalSchema: "core",
                        principalTable: "Devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Triggers_Doors_src_door_id",
                        column: x => x.src_door_id,
                        principalSchema: "core",
                        principalTable: "Doors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Triggers_Inputs_src_input_id",
                        column: x => x.src_input_id,
                        principalSchema: "core",
                        principalTable: "Inputs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Triggers_Locations_location_id",
                        column: x => x.location_id,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Triggers_MonitorGroups_src_monitor_group_id",
                        column: x => x.src_monitor_group_id,
                        principalSchema: "core",
                        principalTable: "MonitorGroups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Triggers_Outputs_src_output_id",
                        column: x => x.src_output_id,
                        principalSchema: "core",
                        principalTable: "Outputs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Triggers_Procedures_procedure_id",
                        column: x => x.procedure_id,
                        principalSchema: "core",
                        principalTable: "Procedures",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Triggers_TimeZones_src_timezone_id",
                        column: x => x.src_timezone_id,
                        principalSchema: "core",
                        principalTable: "TimeZones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Triggers_TimeZones_timezoneid",
                        column: x => x.timezoneid,
                        principalSchema: "core",
                        principalTable: "TimeZones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Triggers_TriggerSources_trigger_source_id",
                        column: x => x.trigger_source_id,
                        principalSchema: "core",
                        principalTable: "TriggerSources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Triggers_TriggerTypes_trigger_type_id",
                        column: x => x.trigger_type_id,
                        principalSchema: "core",
                        principalTable: "TriggerTypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TriggerArgMapTrigger",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    trigger_arg_id = table.Column<int>(type: "integer", nullable: false),
                    trigger_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TriggerArgMapTrigger", x => x.id);
                    table.ForeignKey(
                        name: "FK_TriggerArgMapTrigger_TriggerArgs_trigger_arg_id",
                        column: x => x.trigger_arg_id,
                        principalSchema: "core",
                        principalTable: "TriggerArgs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TriggerArgMapTrigger_Triggers_trigger_id",
                        column: x => x.trigger_id,
                        principalSchema: "core",
                        principalTable: "Triggers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TriggerMapTriggerCodes",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    trigger_id = table.Column<int>(type: "integer", nullable: false),
                    trigger_code_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TriggerMapTriggerCodes", x => x.id);
                    table.ForeignKey(
                        name: "FK_TriggerMapTriggerCodes_TriggerCodes_trigger_code_id",
                        column: x => x.trigger_code_id,
                        principalSchema: "core",
                        principalTable: "TriggerCodes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TriggerMapTriggerCodes_Triggers_trigger_id",
                        column: x => x.trigger_id,
                        principalSchema: "core",
                        principalTable: "Triggers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TriggerVariables",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    trigger_id = table.Column<int>(type: "integer", nullable: false),
                    triggerid = table.Column<int>(type: "integer", nullable: false),
                    device_id = table.Column<int>(type: "integer", nullable: false),
                    deviceid = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_TriggerVariables", x => x.id);
                    table.ForeignKey(
                        name: "FK_TriggerVariables_Devices_deviceid",
                        column: x => x.deviceid,
                        principalSchema: "core",
                        principalTable: "Devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TriggerVariables_Locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TriggerVariables_Triggers_triggerid",
                        column: x => x.triggerid,
                        principalSchema: "core",
                        principalTable: "Triggers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TriggerVariableMapTriggers",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    index = table.Column<string>(type: "text", nullable: false),
                    trigger_variable_id = table.Column<int>(type: "integer", nullable: false),
                    trigger_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TriggerVariableMapTriggers", x => x.id);
                    table.ForeignKey(
                        name: "FK_TriggerVariableMapTriggers_TriggerVariables_trigger_variabl~",
                        column: x => x.trigger_variable_id,
                        principalSchema: "core",
                        principalTable: "TriggerVariables",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TriggerVariableMapTriggers_Triggers_trigger_id",
                        column: x => x.trigger_id,
                        principalSchema: "core",
                        principalTable: "Triggers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "core",
                table: "TriggerArgs",
                columns: new[] { "id", "altr_external_id", "external_id", "is_active", "is_default", "location_id", "locationid", "name", "trigger_code_id", "value" },
                values: new object[,]
                {
                    { 1, null, null, true, false, null, null, "Held open pre-alarm", 77, (short)1 },
                    { 2, null, null, true, false, null, null, "Forced open cancelled", 77, (short)2 },
                    { 3, null, null, true, false, null, null, "Held open cancelled", 77, (short)3 },
                    { 4, null, null, true, false, null, null, "Held or force cancelled", 77, (short)4 },
                    { 5, null, null, true, false, null, null, "Door closed", 77, (short)5 },
                    { 6, null, null, true, false, null, null, "Door opened", 77, (short)6 },
                    { 7, null, null, true, false, null, null, "Held open pre-alarm cancel", 77, (short)7 },
                    { 8, null, null, true, false, null, null, "Forced open only", 78, (short)1 },
                    { 9, null, null, true, false, null, null, "Held open after normal", 78, (short)2 },
                    { 10, null, null, true, false, null, null, "Forced and held open", 78, (short)3 },
                    { 11, null, null, true, false, null, null, "Forced open only", 78, (short)4 },
                    { 12, null, null, true, false, null, null, "Forced or held open", 78, (short)5 },
                    { 13, null, null, true, false, null, null, "Reader tamper", 78, (short)6 },
                    { 14, null, null, true, false, null, null, "Inactive", 62, (short)1 },
                    { 15, null, null, true, false, null, null, "Active", 62, (short)2 },
                    { 16, null, null, true, false, null, null, "Alarm", 62, (short)3 },
                    { 17, null, null, true, false, null, null, "Fault", 62, (short)4 },
                    { 18, null, null, true, false, null, null, "Fault or alarm", 62, (short)5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TriggerArgMapTrigger_id_trigger_arg_id_trigger_id",
                schema: "core",
                table: "TriggerArgMapTrigger",
                columns: new[] { "id", "trigger_arg_id", "trigger_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TriggerArgMapTrigger_trigger_arg_id",
                schema: "core",
                table: "TriggerArgMapTrigger",
                column: "trigger_arg_id");

            migrationBuilder.CreateIndex(
                name: "IX_TriggerArgMapTrigger_trigger_id",
                schema: "core",
                table: "TriggerArgMapTrigger",
                column: "trigger_id");

            migrationBuilder.CreateIndex(
                name: "IX_TriggerArgs_guid_id_trigger_code_id",
                schema: "core",
                table: "TriggerArgs",
                columns: new[] { "guid", "id", "trigger_code_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TriggerArgs_locationid",
                schema: "core",
                table: "TriggerArgs",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_TriggerArgs_trigger_code_id",
                schema: "core",
                table: "TriggerArgs",
                column: "trigger_code_id");

            migrationBuilder.CreateIndex(
                name: "IX_TriggerMapTriggerCodes_trigger_code_id",
                schema: "core",
                table: "TriggerMapTriggerCodes",
                column: "trigger_code_id");

            migrationBuilder.CreateIndex(
                name: "IX_TriggerMapTriggerCodes_trigger_id",
                schema: "core",
                table: "TriggerMapTriggerCodes",
                column: "trigger_id");

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_device_id",
                schema: "core",
                table: "Triggers",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_guid_id_trigger_type_id_external_id_trigger_source~",
                schema: "core",
                table: "Triggers",
                columns: new[] { "guid", "id", "trigger_type_id", "external_id", "trigger_source_id", "timezone_id", "src_device_module_id", "src_input_id", "src_output_id", "src_door_id", "src_timezone_id", "src_monitor_group_id", "src_area_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_location_id",
                schema: "core",
                table: "Triggers",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_procedure_id",
                schema: "core",
                table: "Triggers",
                column: "procedure_id");

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_src_area_id",
                schema: "core",
                table: "Triggers",
                column: "src_area_id");

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_src_device_module_id",
                schema: "core",
                table: "Triggers",
                column: "src_device_module_id");

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_src_door_id",
                schema: "core",
                table: "Triggers",
                column: "src_door_id");

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_src_input_id",
                schema: "core",
                table: "Triggers",
                column: "src_input_id");

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_src_monitor_group_id",
                schema: "core",
                table: "Triggers",
                column: "src_monitor_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_src_output_id",
                schema: "core",
                table: "Triggers",
                column: "src_output_id");

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_src_timezone_id",
                schema: "core",
                table: "Triggers",
                column: "src_timezone_id");

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_timezoneid",
                schema: "core",
                table: "Triggers",
                column: "timezoneid");

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_trigger_source_id",
                schema: "core",
                table: "Triggers",
                column: "trigger_source_id");

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_trigger_type_id",
                schema: "core",
                table: "Triggers",
                column: "trigger_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_TriggerVariableMapTriggers_id_trigger_variable_id_trigger_i~",
                schema: "core",
                table: "TriggerVariableMapTriggers",
                columns: new[] { "id", "trigger_variable_id", "trigger_id", "index" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TriggerVariableMapTriggers_trigger_id",
                schema: "core",
                table: "TriggerVariableMapTriggers",
                column: "trigger_id");

            migrationBuilder.CreateIndex(
                name: "IX_TriggerVariableMapTriggers_trigger_variable_id",
                schema: "core",
                table: "TriggerVariableMapTriggers",
                column: "trigger_variable_id");

            migrationBuilder.CreateIndex(
                name: "IX_TriggerVariables_deviceid",
                schema: "core",
                table: "TriggerVariables",
                column: "deviceid");

            migrationBuilder.CreateIndex(
                name: "IX_TriggerVariables_guid_id_external_id",
                schema: "core",
                table: "TriggerVariables",
                columns: new[] { "guid", "id", "external_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TriggerVariables_locationid",
                schema: "core",
                table: "TriggerVariables",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_TriggerVariables_triggerid",
                schema: "core",
                table: "TriggerVariables",
                column: "triggerid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TriggerArgMapTrigger",
                schema: "core");

            migrationBuilder.DropTable(
                name: "TriggerMapTriggerCodes",
                schema: "core");

            migrationBuilder.DropTable(
                name: "TriggerVariableMapTriggers",
                schema: "core");

            migrationBuilder.DropTable(
                name: "TriggerArgs",
                schema: "core");

            migrationBuilder.DropTable(
                name: "TriggerVariables",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Triggers",
                schema: "core");
        }
    }
}
