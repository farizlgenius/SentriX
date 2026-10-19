using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCoreEventTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alerts_BaseEvents_base_event_id",
                schema: "core",
                table: "Alerts");

            migrationBuilder.DropTable(
                name: "BaseEvents",
                schema: "core");

            migrationBuilder.DropColumn(
                name: "acknowledge",
                schema: "core",
                table: "AlertEvents");

            migrationBuilder.DropColumn(
                name: "postpone",
                schema: "core",
                table: "AlertEvents");

            migrationBuilder.RenameColumn(
                name: "base_event_id",
                schema: "core",
                table: "Alerts",
                newName: "core_eventid");

            migrationBuilder.RenameIndex(
                name: "IX_Alerts_base_event_id",
                schema: "core",
                table: "Alerts",
                newName: "IX_Alerts_core_eventid");

            migrationBuilder.AddColumn<int>(
                name: "core_event_id",
                schema: "core",
                table: "Alerts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "comment",
                schema: "core",
                table: "AlertEvents",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "alert_action",
                schema: "core",
                table: "AlertEvents",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "username",
                schema: "core",
                table: "AlertEvents",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CoreEntities",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    entity = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoreEntities", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "CoreEvents",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    @event = table.Column<string>(name: "event", type: "text", nullable: false),
                    entity_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoreEvents", x => x.id);
                    table.ForeignKey(
                        name: "FK_CoreEvents_CoreEntities_entity_id",
                        column: x => x.entity_id,
                        principalSchema: "core",
                        principalTable: "CoreEntities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "ComponentMappings",
                keyColumn: "id",
                keyValue: 1,
                column: "entity",
                value: "TIMEZONE");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "ComponentMappings",
                keyColumn: "id",
                keyValue: 2,
                column: "entity",
                value: "TIMEZONE");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "ComponentMappings",
                keyColumn: "id",
                keyValue: 3,
                column: "entity",
                value: "CARD_FORMAT");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "ComponentMappings",
                keyColumn: "id",
                keyValue: 4,
                column: "entity",
                value: "CARD_FORMAT");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "ComponentMappings",
                keyColumn: "id",
                keyValue: 5,
                column: "entity",
                value: "CARD_FORMAT");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "ComponentMappings",
                keyColumn: "id",
                keyValue: 6,
                column: "entity",
                value: "GROUP");

            migrationBuilder.InsertData(
                schema: "core",
                table: "CoreEntities",
                columns: new[] { "id", "entity" },
                values: new object[,]
                {
                    { 1, "AUTH" },
                    { 2, "DEVICE" },
                    { 3, "TEMP_DEVICE" },
                    { 4, "DEVICE_MODULE" },
                    { 5, "LOCATION" },
                    { 6, "COMPANY" },
                    { 7, "DEPARTMENT" },
                    { 8, "POSITION" },
                    { 9, "USER" },
                    { 10, "ROLE" },
                    { 11, "OPERATOR" },
                    { 12, "PASSWORD_RULE" },
                    { 13, "WEAK_PASSWORD" },
                    { 14, "TIMEZONE" },
                    { 15, "HOLIDAY" },
                    { 16, "INTERVAL" },
                    { 17, "DOOR" },
                    { 18, "TURNSTILE" },
                    { 19, "GROUP" },
                    { 20, "COMPONENT_MAPPING" },
                    { 21, "EVENT" },
                    { 22, "ADAPTER_EVENT" },
                    { 23, "INPUT" },
                    { 24, "OUTPUT" },
                    { 25, "PROCEDURE" },
                    { 26, "TRIGGER" },
                    { 27, "MONITOR_GROUP" },
                    { 28, "AREA" },
                    { 29, "WEB" },
                    { 30, "CARD_FORMAT" },
                    { 31, "ACTION" },
                    { 32, "ALERT" },
                    { 33, "ALERT_LEVEL" },
                    { 34, "CORE_EVENT" }
                });

            migrationBuilder.InsertData(
                schema: "core",
                table: "CoreEvents",
                columns: new[] { "id", "entity_id", "event" },
                values: new object[,]
                {
                    { 1, 2, "ONLINE" },
                    { 2, 2, "OFFLINE" },
                    { 3, 2, "TRANSACTION_EXCEED" },
                    { 4, 4, "DISABLED" },
                    { 5, 4, "ONLINE" },
                    { 6, 4, "OFFLINE" },
                    { 7, 4, "TIMEOUT" },
                    { 8, 17, "INVALID_CARD_FORMAT" },
                    { 9, 17, "ACCESS_DENIED" },
                    { 10, 17, "ACCESS_GRANT" },
                    { 11, 17, "CARD_NOT_FOUND" },
                    { 12, 17, "COUNT_EXCEED" },
                    { 13, 17, "INTERLOCK" },
                    { 14, 17, "ANTIPASSBACK_VIOLATION" },
                    { 15, 17, "OCCUPANCY_REACHED_ZERO" },
                    { 16, 17, "DOUBLE_CARD_EVENT" },
                    { 17, 17, "TAMPER" },
                    { 18, 17, "HELD_OPEN" },
                    { 19, 17, "FORCE_OPEN" },
                    { 20, 23, "DISCONNECTED" },
                    { 21, 23, "UNKNOWN" },
                    { 22, 23, "OPEN" },
                    { 23, 23, "CLOSE" },
                    { 24, 23, "FAULT" },
                    { 25, 24, "DISCONNECTED" },
                    { 26, 24, "UNKNOWN" },
                    { 27, 24, "ACTIVE" },
                    { 28, 24, "DEACTIVE" },
                    { 29, 24, "FAULT" },
                    { 30, 28, "OCCUPANCY_LIMIT_REACHED" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoreEntities_guid_id",
                schema: "core",
                table: "CoreEntities",
                columns: new[] { "guid", "id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoreEvents_entity_id",
                schema: "core",
                table: "CoreEvents",
                column: "entity_id");

            migrationBuilder.CreateIndex(
                name: "IX_CoreEvents_guid_id_entity_id",
                schema: "core",
                table: "CoreEvents",
                columns: new[] { "guid", "id", "entity_id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Alerts_CoreEvents_core_eventid",
                schema: "core",
                table: "Alerts",
                column: "core_eventid",
                principalSchema: "core",
                principalTable: "CoreEvents",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alerts_CoreEvents_core_eventid",
                schema: "core",
                table: "Alerts");

            migrationBuilder.DropTable(
                name: "CoreEvents",
                schema: "core");

            migrationBuilder.DropTable(
                name: "CoreEntities",
                schema: "core");

            migrationBuilder.DropColumn(
                name: "core_event_id",
                schema: "core",
                table: "Alerts");

            migrationBuilder.DropColumn(
                name: "alert_action",
                schema: "core",
                table: "AlertEvents");

            migrationBuilder.DropColumn(
                name: "username",
                schema: "core",
                table: "AlertEvents");

            migrationBuilder.RenameColumn(
                name: "core_eventid",
                schema: "core",
                table: "Alerts",
                newName: "base_event_id");

            migrationBuilder.RenameIndex(
                name: "IX_Alerts_core_eventid",
                schema: "core",
                table: "Alerts",
                newName: "IX_Alerts_base_event_id");

            migrationBuilder.AlterColumn<string>(
                name: "comment",
                schema: "core",
                table: "AlertEvents",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "acknowledge",
                schema: "core",
                table: "AlertEvents",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "postpone",
                schema: "core",
                table: "AlertEvents",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "BaseEvents",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    detail = table.Column<string>(type: "text", nullable: false),
                    entity = table.Column<string>(type: "text", nullable: false),
                    event_detail = table.Column<string>(type: "text", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseEvents", x => x.id);
                });

            migrationBuilder.InsertData(
                schema: "core",
                table: "BaseEvents",
                columns: new[] { "id", "detail", "entity", "event_detail", "is_active", "is_default" },
                values: new object[,]
                {
                    { 1, "Online", "Device", "Online", true, false },
                    { 2, "Offline", "Device", "Offline", true, false },
                    { 5, "Online", "DeviceModule", "Online", true, false },
                    { 6, "Offline", "DeviceModule", "Offline", true, false },
                    { 7, "Disabled", "DeviceModule", "Disabled", true, false },
                    { 8, "Reader online", "Door", "Online", true, false },
                    { 9, "Reader offline", "Door", "Offline", true, false },
                    { 10, "Reader tamper", "Door", "Tamper", true, false },
                    { 11, "Card not found", "Door", "CardNoFound", true, false },
                    { 12, "Grant", "Door", "Grant", true, false },
                    { 13, "Rejected", "Door", "Rejected", true, false },
                    { 14, "Denied", "Door", "Denied", true, false },
                    { 15, "Duress", "Door", "Duress", true, false },
                    { 16, "Held open", "Door", "HeldOpen", true, false },
                    { 17, "Force open", "Door", "ForcedOpen", true, false },
                    { 18, "Double card", "Door", "DoubleCardEvent", true, false },
                    { 19, "Active", "Input", "Active", true, false },
                    { 20, "Inactive", "Input", "Inactive", true, false },
                    { 21, "Disconnected", "Input", "Disconnect", true, false },
                    { 22, "Active", "Output", "Active", true, false },
                    { 23, "Inactive", "Output", "Inactive", true, false },
                    { 24, "Disconnected", "Output", "Disconnect", true, false },
                    { 25, "Antipassback violate", "Door", "APB", true, false },
                    { 26, "Occupancy reached limit", "Door", "OccLimit", true, false }
                });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "ComponentMappings",
                keyColumn: "id",
                keyValue: 1,
                column: "entity",
                value: "TimeZone");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "ComponentMappings",
                keyColumn: "id",
                keyValue: 2,
                column: "entity",
                value: "TimeZone");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "ComponentMappings",
                keyColumn: "id",
                keyValue: 3,
                column: "entity",
                value: "CardFormat");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "ComponentMappings",
                keyColumn: "id",
                keyValue: 4,
                column: "entity",
                value: "CardFormat");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "ComponentMappings",
                keyColumn: "id",
                keyValue: 5,
                column: "entity",
                value: "CardFormat");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "ComponentMappings",
                keyColumn: "id",
                keyValue: 6,
                column: "entity",
                value: "Group");

            migrationBuilder.AddForeignKey(
                name: "FK_Alerts_BaseEvents_base_event_id",
                schema: "core",
                table: "Alerts",
                column: "base_event_id",
                principalSchema: "core",
                principalTable: "BaseEvents",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
