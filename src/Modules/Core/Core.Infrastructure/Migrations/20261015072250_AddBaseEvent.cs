using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBaseEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AlertEvents",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    entity = table.Column<int>(type: "integer", nullable: false),
                    base_event = table.Column<int>(type: "integer", nullable: false),
                    acknowledge = table.Column<bool>(type: "boolean", nullable: false),
                    postpone = table.Column<bool>(type: "boolean", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: false),
                    alert_level = table.Column<string>(type: "text", nullable: false),
                    level_color = table.Column<string>(type: "text", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: true),
                    locationid = table.Column<int>(type: "integer", nullable: true),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertEvents", x => x.id);
                    table.ForeignKey(
                        name: "FK_AlertEvents_Locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "AlertLevel",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    color_code = table.Column<string>(type: "text", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertLevel", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "BaseEvents",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    entity = table.Column<string>(type: "text", nullable: false),
                    event_detail = table.Column<string>(type: "text", nullable: false),
                    detail = table.Column<string>(type: "text", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseEvents", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Alerts",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    advice = table.Column<string>(type: "text", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    locationid = table.Column<int>(type: "integer", nullable: false),
                    alert_level_id = table.Column<int>(type: "integer", nullable: false),
                    alert_levelid = table.Column<int>(type: "integer", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alerts", x => x.id);
                    table.ForeignKey(
                        name: "FK_Alerts_AlertLevel_alert_levelid",
                        column: x => x.alert_levelid,
                        principalSchema: "core",
                        principalTable: "AlertLevel",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alerts_Locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlertBaseEvents",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    alert_id = table.Column<int>(type: "integer", nullable: false),
                    event_detail_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertBaseEvents", x => x.id);
                    table.ForeignKey(
                        name: "FK_AlertBaseEvents_Alerts_alert_id",
                        column: x => x.alert_id,
                        principalSchema: "core",
                        principalTable: "Alerts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlertBaseEvents_BaseEvents_event_detail_id",
                        column: x => x.event_detail_id,
                        principalSchema: "core",
                        principalTable: "BaseEvents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "core",
                table: "AlertLevel",
                columns: new[] { "id", "color_code", "is_active", "is_default", "name" },
                values: new object[,]
                {
                    { 1, "#ff1100", true, false, "Emergency" },
                    { 2, "#c2432d", true, false, "Danger" },
                    { 3, "#ff911c", true, false, "High" },
                    { 4, "#ffbd2d", true, false, "Moderate" },
                    { 5, "#989a9c", true, false, "Low" }
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

            migrationBuilder.CreateIndex(
                name: "IX_AlertBaseEvents_alert_id",
                schema: "core",
                table: "AlertBaseEvents",
                column: "alert_id");

            migrationBuilder.CreateIndex(
                name: "IX_AlertBaseEvents_event_detail_id",
                schema: "core",
                table: "AlertBaseEvents",
                column: "event_detail_id");

            migrationBuilder.CreateIndex(
                name: "IX_AlertEvents_locationid",
                schema: "core",
                table: "AlertEvents",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_alert_levelid",
                schema: "core",
                table: "Alerts",
                column: "alert_levelid");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_locationid",
                schema: "core",
                table: "Alerts",
                column: "locationid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlertBaseEvents",
                schema: "core");

            migrationBuilder.DropTable(
                name: "AlertEvents",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Alerts",
                schema: "core");

            migrationBuilder.DropTable(
                name: "BaseEvents",
                schema: "core");

            migrationBuilder.DropTable(
                name: "AlertLevel",
                schema: "core");
        }
    }
}
