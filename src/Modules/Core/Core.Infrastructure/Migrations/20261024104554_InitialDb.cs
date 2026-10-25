using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "core");

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
                name: "Countries",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DayInWeeks",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sunday = table.Column<bool>(type: "boolean", nullable: false),
                    monday = table.Column<bool>(type: "boolean", nullable: false),
                    tuesday = table.Column<bool>(type: "boolean", nullable: false),
                    wednesday = table.Column<bool>(type: "boolean", nullable: false),
                    thursday = table.Column<bool>(type: "boolean", nullable: false),
                    friday = table.Column<bool>(type: "boolean", nullable: false),
                    saturday = table.Column<bool>(type: "boolean", nullable: false),
                    interval_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayInWeeks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ExceptionEvent",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    timestampe = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    method = table.Column<string>(type: "text", nullable: false),
                    path = table.Column<string>(type: "text", nullable: false),
                    exception = table.Column<string>(type: "text", nullable: false),
                    inner_exception = table.Column<string>(type: "text", nullable: false),
                    stack_trace = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExceptionEvent", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TriggerSources",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TriggerSources", x => x.id);
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

            migrationBuilder.CreateTable(
                name: "Locations",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: false),
                    country_id = table.Column<int>(type: "integer", nullable: false),
                    external_id = table.Column<short>(type: "smallint", nullable: true),
                    name = table.Column<string>(type: "text", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: true),
                    locationid = table.Column<int>(type: "integer", nullable: true),
                    altr_external_id = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.id);
                    table.ForeignKey(
                        name: "FK_Locations_Countries_country_id",
                        column: x => x.country_id,
                        principalSchema: "core",
                        principalTable: "Countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Locations_Locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "TriggerTypes",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<short>(type: "smallint", nullable: false),
                    trigger_source_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TriggerTypes", x => x.id);
                    table.ForeignKey(
                        name: "FK_TriggerTypes_TriggerSources_trigger_source_id",
                        column: x => x.trigger_source_id,
                        principalSchema: "core",
                        principalTable: "TriggerSources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdapterEvents",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mac = table.Column<string>(type: "text", nullable: false),
                    component_id = table.Column<int>(type: "integer", nullable: false),
                    command = table.Column<string>(type: "text", nullable: false),
                    tag = table.Column<int>(type: "integer", nullable: false),
                    send_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    received_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    body = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    reason = table.Column<string>(type: "text", nullable: false),
                    response = table.Column<string>(type: "text", nullable: false),
                    vendor = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_AdapterEvents", x => x.id);
                    table.ForeignKey(
                        name: "FK_AdapterEvents_Locations_location_id",
                        column: x => x.location_id,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "AlertEvents",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    entity = table.Column<int>(type: "integer", nullable: false),
                    base_event = table.Column<int>(type: "integer", nullable: false),
                    alert_action = table.Column<int>(type: "integer", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: true),
                    alert_level = table.Column<string>(type: "text", nullable: false),
                    level_color = table.Column<string>(type: "text", nullable: false),
                    username = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_AlertEvents", x => x.id);
                    table.ForeignKey(
                        name: "FK_AlertEvents_Locations_location_id",
                        column: x => x.location_id,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "AlertLevels",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    color_code = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_AlertLevels", x => x.id);
                    table.ForeignKey(
                        name: "FK_AlertLevels_Locations_location_id",
                        column: x => x.location_id,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "AuditTrails",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    entity = table.Column<string>(type: "text", nullable: false),
                    action = table.Column<string>(type: "text", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    ip = table.Column<string>(type: "text", nullable: false),
                    object_guid = table.Column<Guid>(type: "uuid", nullable: true),
                    object_name = table.Column<string>(type: "text", nullable: true),
                    detail = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_AuditTrails", x => x.id);
                    table.ForeignKey(
                        name: "FK_AuditTrails_Locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "CardFormats",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fac = table.Column<short>(type: "smallint", nullable: false),
                    bits = table.Column<short>(type: "smallint", nullable: false),
                    even_parity_len = table.Column<short>(type: "smallint", nullable: false),
                    even_parity_loc = table.Column<short>(type: "smallint", nullable: false),
                    odd_parity_len = table.Column<short>(type: "smallint", nullable: false),
                    odd_parity_loc = table.Column<short>(type: "smallint", nullable: false),
                    fac_len = table.Column<short>(type: "smallint", nullable: false),
                    fac_loc = table.Column<short>(type: "smallint", nullable: false),
                    card_no_len = table.Column<short>(type: "smallint", nullable: false),
                    card_no_loc = table.Column<short>(type: "smallint", nullable: false),
                    issue_code_len = table.Column<short>(type: "smallint", nullable: false),
                    issue_code_loc = table.Column<short>(type: "smallint", nullable: false),
                    metadata = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_CardFormats", x => x.id);
                    table.ForeignKey(
                        name: "FK_CardFormats_Locations_location_id",
                        column: x => x.location_id,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_Companies", x => x.id);
                    table.ForeignKey(
                        name: "FK_Companies_Locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    serial_number = table.Column<string>(type: "text", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    ip = table.Column<string>(type: "text", nullable: false),
                    port = table.Column<int>(type: "integer", nullable: false),
                    firmware = table.Column<string>(type: "text", nullable: false),
                    vendor = table.Column<string>(type: "text", nullable: false),
                    configuration_status = table.Column<int>(type: "integer", nullable: false),
                    synced_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    metadata = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_Devices", x => x.id);
                    table.ForeignKey(
                        name: "FK_Devices_Locations_location_id",
                        column: x => x.location_id,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    actor = table.Column<string>(type: "text", nullable: false),
                    module = table.Column<string>(type: "text", nullable: false),
                    event_type = table.Column<string>(type: "text", nullable: false),
                    image_name = table.Column<string>(type: "text", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    component_name = table.Column<string>(type: "text", nullable: false),
                    event_code = table.Column<string>(type: "text", nullable: false),
                    remarks = table.Column<string>(type: "text", nullable: false),
                    capture_image_name = table.Column<string>(type: "text", nullable: false),
                    vendor = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_Events", x => x.id);
                    table.ForeignKey(
                        name: "FK_Events_Locations_location_id",
                        column: x => x.location_id,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Faces",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: true),
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
                    table.PrimaryKey("PK_Faces", x => x.id);
                    table.ForeignKey(
                        name: "FK_Faces_Locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                    table.PrimaryKey("PK_Groups", x => x.id);
                    table.ForeignKey(
                        name: "FK_Groups_Locations_location_id",
                        column: x => x.location_id,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Holidays",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    start = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
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
                    table.PrimaryKey("PK_Holidays", x => x.id);
                    table.ForeignKey(
                        name: "FK_Holidays_Locations_location_id",
                        column: x => x.location_id,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Intervals",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    start_time = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    end_time = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    day_id = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_Intervals", x => x.id);
                    table.ForeignKey(
                        name: "FK_Intervals_DayInWeeks_day_id",
                        column: x => x.day_id,
                        principalSchema: "core",
                        principalTable: "DayInWeeks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Intervals_Locations_location_id",
                        column: x => x.location_id,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "License",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MyProperty = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: true),
                    locationid = table.Column<int>(type: "integer", nullable: true),
                    external_id = table.Column<short>(type: "smallint", nullable: true),
                    altr_external_id = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_License", x => x.id);
                    table.ForeignKey(
                        name: "FK_License_Locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "LicenseKey",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: true),
                    locationid = table.Column<int>(type: "integer", nullable: true),
                    external_id = table.Column<short>(type: "smallint", nullable: true),
                    altr_external_id = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseKey", x => x.id);
                    table.ForeignKey(
                        name: "FK_LicenseKey_Locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "LicensePlates",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    license_plate = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: true),
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
                    table.PrimaryKey("PK_LicensePlates", x => x.id);
                    table.ForeignKey(
                        name: "FK_LicensePlates_Locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                    table.PrimaryKey("PK_Modules", x => x.id);
                    table.ForeignKey(
                        name: "FK_Modules_Locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Pins",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pin = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: true),
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
                    table.PrimaryKey("PK_Pins", x => x.id);
                    table.ForeignKey(
                        name: "FK_Pins_Locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "QrCodes",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    qr_code = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_QrCodes", x => x.id);
                    table.ForeignKey(
                        name: "FK_QrCodes_Locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                    table.PrimaryKey("PK_Roles", x => x.id);
                    table.ForeignKey(
                        name: "FK_Roles_Locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "TimeZones",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                    table.PrimaryKey("PK_TimeZones", x => x.id);
                    table.ForeignKey(
                        name: "FK_TimeZones_Locations_location_id",
                        column: x => x.location_id,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Turnstiles",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                    table.PrimaryKey("PK_Turnstiles", x => x.id);
                    table.ForeignKey(
                        name: "FK_Turnstiles_Locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "TriggerCodes",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<short>(type: "smallint", nullable: false),
                    trigger_type_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TriggerCodes", x => x.id);
                    table.ForeignKey(
                        name: "FK_TriggerCodes_TriggerTypes_trigger_type_id",
                        column: x => x.trigger_type_id,
                        principalSchema: "core",
                        principalTable: "TriggerTypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alerts",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    advice = table.Column<string>(type: "text", nullable: false),
                    alert_level_id = table.Column<int>(type: "integer", nullable: false),
                    core_event_id = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_Alerts", x => x.id);
                    table.ForeignKey(
                        name: "FK_Alerts_AlertLevels_alert_level_id",
                        column: x => x.alert_level_id,
                        principalSchema: "core",
                        principalTable: "AlertLevels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alerts_CoreEvents_core_event_id",
                        column: x => x.core_event_id,
                        principalSchema: "core",
                        principalTable: "CoreEvents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alerts_Locations_location_id",
                        column: x => x.location_id,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: false),
                    company_id = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_Departments", x => x.id);
                    table.ForeignKey(
                        name: "FK_Departments_Companies_company_id",
                        column: x => x.company_id,
                        principalSchema: "core",
                        principalTable: "Companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Departments_Locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Action",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<string>(type: "text", nullable: false),
                    metadata = table.Column<string>(type: "text", nullable: false),
                    device_id = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_Action", x => x.id);
                    table.ForeignKey(
                        name: "FK_Action_Devices_device_id",
                        column: x => x.device_id,
                        principalSchema: "core",
                        principalTable: "Devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Action_Locations_location_id",
                        column: x => x.location_id,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceModules",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    serial_number = table.Column<string>(type: "text", nullable: false),
                    firmware = table.Column<string>(type: "text", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    port = table.Column<int>(type: "integer", nullable: false),
                    address = table.Column<int>(type: "integer", nullable: false),
                    model = table.Column<string>(type: "text", nullable: false),
                    reader_slot = table.Column<int>(type: "integer", nullable: false),
                    output_slot = table.Column<int>(type: "integer", nullable: false),
                    input_slot = table.Column<int>(type: "integer", nullable: false),
                    device_id = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_DeviceModules", x => x.id);
                    table.ForeignKey(
                        name: "FK_DeviceModules_Devices_device_id",
                        column: x => x.device_id,
                        principalSchema: "core",
                        principalTable: "Devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeviceModules_Locations_location_id",
                        column: x => x.location_id,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Procedures",
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
                    external_id = table.Column<short>(type: "smallint", nullable: true),
                    altr_external_id = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Procedures", x => x.id);
                    table.ForeignKey(
                        name: "FK_Procedures_Devices_device_id",
                        column: x => x.device_id,
                        principalSchema: "core",
                        principalTable: "Devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Procedures_Locations_location_id",
                        column: x => x.location_id,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Features",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "text", nullable: false),
                    module_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.id);
                    table.ForeignKey(
                        name: "FK_Features_Modules_module_id",
                        column: x => x.module_id,
                        principalSchema: "core",
                        principalTable: "Modules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModulePermissions",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    module_id = table.Column<int>(type: "integer", nullable: false),
                    is_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_ModulePermissions", x => x.id);
                    table.ForeignKey(
                        name: "FK_ModulePermissions_Locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ModulePermissions_Modules_module_id",
                        column: x => x.module_id,
                        principalSchema: "core",
                        principalTable: "Modules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModulePermissions_Roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "core",
                        principalTable: "Roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Operators",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    firstname = table.Column<string>(type: "text", nullable: false),
                    middlename = table.Column<string>(type: "text", nullable: false),
                    lastname = table.Column<string>(type: "text", nullable: false),
                    gender = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    joined_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expired_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    role_id = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_Operators", x => x.id);
                    table.ForeignKey(
                        name: "FK_Operators_Locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Operators_Roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "core",
                        principalTable: "Roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "TimeZoneIntervals",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    timezone_id = table.Column<int>(type: "integer", nullable: false),
                    interval_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeZoneIntervals", x => x.id);
                    table.ForeignKey(
                        name: "FK_TimeZoneIntervals_Intervals_interval_id",
                        column: x => x.interval_id,
                        principalSchema: "core",
                        principalTable: "Intervals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimeZoneIntervals_TimeZones_timezone_id",
                        column: x => x.timezone_id,
                        principalSchema: "core",
                        principalTable: "TimeZones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: false),
                    department_id = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_Positions", x => x.id);
                    table.ForeignKey(
                        name: "FK_Positions_Departments_department_id",
                        column: x => x.department_id,
                        principalSchema: "core",
                        principalTable: "Departments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Positions_Locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Buzzers",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    slot_no = table.Column<int>(type: "integer", nullable: false),
                    metadata = table.Column<string>(type: "text", nullable: false),
                    vendor = table.Column<int>(type: "integer", nullable: false),
                    door_id = table.Column<int>(type: "integer", nullable: false),
                    device_module_id = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_Buzzers", x => x.id);
                    table.ForeignKey(
                        name: "FK_Buzzers_DeviceModules_device_module_id",
                        column: x => x.device_module_id,
                        principalSchema: "core",
                        principalTable: "DeviceModules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Buzzers_Locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Input",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    slot_no = table.Column<int>(type: "integer", nullable: false),
                    vendor = table.Column<string>(type: "text", nullable: false),
                    metadata = table.Column<string>(type: "text", nullable: false),
                    device_module_id = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_Input", x => x.id);
                    table.ForeignKey(
                        name: "FK_Input_DeviceModules_device_module_id",
                        column: x => x.device_module_id,
                        principalSchema: "core",
                        principalTable: "DeviceModules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Input_Locations_location_id",
                        column: x => x.location_id,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Output",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    slot_no = table.Column<int>(type: "integer", nullable: false),
                    vendor = table.Column<string>(type: "text", nullable: false),
                    metadata = table.Column<string>(type: "text", nullable: false),
                    device_module_id = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_Output", x => x.id);
                    table.ForeignKey(
                        name: "FK_Output_DeviceModules_device_module_id",
                        column: x => x.device_module_id,
                        principalSchema: "core",
                        principalTable: "DeviceModules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Output_Locations_location_id",
                        column: x => x.location_id,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcedureActions",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    procedure_id = table.Column<int>(type: "integer", nullable: false),
                    action_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcedureActions", x => x.id);
                    table.ForeignKey(
                        name: "FK_ProcedureActions_Action_action_id",
                        column: x => x.action_id,
                        principalSchema: "core",
                        principalTable: "Action",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcedureActions_Procedures_procedure_id",
                        column: x => x.procedure_id,
                        principalSchema: "core",
                        principalTable: "Procedures",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeaturePermissions",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    feature_id = table.Column<int>(type: "integer", nullable: false),
                    module_permission_id = table.Column<int>(type: "integer", nullable: false),
                    is_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    is_created = table.Column<bool>(type: "boolean", nullable: false),
                    is_updated = table.Column<bool>(type: "boolean", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeaturePermissions", x => x.id);
                    table.ForeignKey(
                        name: "FK_FeaturePermissions_Features_feature_id",
                        column: x => x.feature_id,
                        principalSchema: "core",
                        principalTable: "Features",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeaturePermissions_ModulePermissions_module_permission_id",
                        column: x => x.module_permission_id,
                        principalSchema: "core",
                        principalTable: "ModulePermissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperatorLocations",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    operator_id = table.Column<int>(type: "integer", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperatorLocations", x => x.id);
                    table.ForeignKey(
                        name: "FK_OperatorLocations_Locations_location_id",
                        column: x => x.location_id,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperatorLocations_Operators_operator_id",
                        column: x => x.operator_id,
                        principalSchema: "core",
                        principalTable: "Operators",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    user_code = table.Column<string>(type: "text", nullable: false),
                    identification = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    firstname = table.Column<string>(type: "text", nullable: false),
                    middlename = table.Column<string>(type: "text", nullable: false),
                    lastname = table.Column<string>(type: "text", nullable: false),
                    gender = table.Column<string>(type: "text", nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: true),
                    company_id = table.Column<int>(type: "integer", nullable: true),
                    department_id = table.Column<int>(type: "integer", nullable: true),
                    position_id = table.Column<int>(type: "integer", nullable: true),
                    address = table.Column<string>(type: "text", nullable: false),
                    active_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expire_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    pin_id = table.Column<int>(type: "integer", nullable: true),
                    face_id = table.Column<int>(type: "integer", nullable: true),
                    license_plate_id = table.Column<int>(type: "integer", nullable: true),
                    qr_code_id = table.Column<int>(type: "integer", nullable: true),
                    metadata = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_Users", x => x.id);
                    table.ForeignKey(
                        name: "FK_Users_Companies_company_id",
                        column: x => x.company_id,
                        principalSchema: "core",
                        principalTable: "Companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Departments_department_id",
                        column: x => x.department_id,
                        principalSchema: "core",
                        principalTable: "Departments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Faces_face_id",
                        column: x => x.face_id,
                        principalSchema: "core",
                        principalTable: "Faces",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_LicensePlates_license_plate_id",
                        column: x => x.license_plate_id,
                        principalSchema: "core",
                        principalTable: "LicensePlates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Users_Pins_pin_id",
                        column: x => x.pin_id,
                        principalSchema: "core",
                        principalTable: "Pins",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Positions_position_id",
                        column: x => x.position_id,
                        principalSchema: "core",
                        principalTable: "Positions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_QrCodes_qr_code_id",
                        column: x => x.qr_code_id,
                        principalSchema: "core",
                        principalTable: "QrCodes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "core",
                        principalTable: "Roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Doors",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    vendor = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    metadata = table.Column<string>(type: "text", nullable: false),
                    sensor_id = table.Column<int>(type: "integer", nullable: true),
                    relay_id = table.Column<int>(type: "integer", nullable: true),
                    buzzer_id = table.Column<int>(type: "integer", nullable: true),
                    bg_id = table.Column<int>(type: "integer", nullable: true),
                    device_id = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_Doors", x => x.id);
                    table.ForeignKey(
                        name: "FK_Doors_Buzzers_buzzer_id",
                        column: x => x.buzzer_id,
                        principalSchema: "core",
                        principalTable: "Buzzers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Doors_Devices_device_id",
                        column: x => x.device_id,
                        principalSchema: "core",
                        principalTable: "Devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Doors_Locations_location_id",
                        column: x => x.location_id,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    bits = table.Column<short>(type: "smallint", nullable: false),
                    fac = table.Column<int>(type: "integer", nullable: false),
                    card_number = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: true),
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
                    table.PrimaryKey("PK_Cards", x => x.id);
                    table.ForeignKey(
                        name: "FK_Cards_Locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Cards_Users_user_id",
                        column: x => x.user_id,
                        principalSchema: "core",
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAdditionals",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    additional = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAdditionals", x => x.id);
                    table.ForeignKey(
                        name: "FK_UserAdditionals_Users_user_id",
                        column: x => x.user_id,
                        principalSchema: "core",
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserGroups",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    group_id = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_UserGroups", x => x.id);
                    table.ForeignKey(
                        name: "FK_UserGroups_Groups_group_id",
                        column: x => x.group_id,
                        principalSchema: "core",
                        principalTable: "Groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGroups_Locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_UserGroups_Users_userid",
                        column: x => x.userid,
                        principalSchema: "core",
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLocations",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLocations", x => x.id);
                    table.ForeignKey(
                        name: "FK_UserLocations_Locations_location_id",
                        column: x => x.location_id,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLocations_Users_user_id",
                        column: x => x.user_id,
                        principalSchema: "core",
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlertComponents",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    alert_id = table.Column<int>(type: "integer", nullable: false),
                    door_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertComponents", x => x.id);
                    table.ForeignKey(
                        name: "FK_AlertComponents_Alerts_alert_id",
                        column: x => x.alert_id,
                        principalSchema: "core",
                        principalTable: "Alerts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlertComponents_Doors_door_id",
                        column: x => x.door_id,
                        principalSchema: "core",
                        principalTable: "Doors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BreakGlasses",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    slot_no = table.Column<int>(type: "integer", nullable: false),
                    vendor = table.Column<string>(type: "text", nullable: false),
                    metadata = table.Column<string>(type: "text", nullable: false),
                    door_id = table.Column<int>(type: "integer", nullable: true),
                    device_module_id = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_BreakGlasses", x => x.id);
                    table.ForeignKey(
                        name: "FK_BreakGlasses_DeviceModules_device_module_id",
                        column: x => x.device_module_id,
                        principalSchema: "core",
                        principalTable: "DeviceModules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BreakGlasses_Doors_door_id",
                        column: x => x.door_id,
                        principalSchema: "core",
                        principalTable: "Doors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BreakGlasses_Locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "GroupComponents",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    door_id = table.Column<int>(type: "integer", nullable: false),
                    timezone_id = table.Column<int>(type: "integer", nullable: false),
                    group_id = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_GroupComponents", x => x.id);
                    table.ForeignKey(
                        name: "FK_GroupComponents_Doors_door_id",
                        column: x => x.door_id,
                        principalSchema: "core",
                        principalTable: "Doors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupComponents_Groups_group_id",
                        column: x => x.group_id,
                        principalSchema: "core",
                        principalTable: "Groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupComponents_Locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_GroupComponents_TimeZones_timezone_id",
                        column: x => x.timezone_id,
                        principalSchema: "core",
                        principalTable: "TimeZones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Relays",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    slot_no = table.Column<int>(type: "integer", nullable: false),
                    metadata = table.Column<string>(type: "text", nullable: false),
                    vendor = table.Column<string>(type: "text", nullable: false),
                    door_id = table.Column<int>(type: "integer", nullable: false),
                    device_module_id = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_Relays", x => x.id);
                    table.ForeignKey(
                        name: "FK_Relays_DeviceModules_device_module_id",
                        column: x => x.device_module_id,
                        principalSchema: "core",
                        principalTable: "DeviceModules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Relays_Doors_door_id",
                        column: x => x.door_id,
                        principalSchema: "core",
                        principalTable: "Doors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Relays_Locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Rexes",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    slot_no = table.Column<int>(type: "integer", nullable: false),
                    metadata = table.Column<string>(type: "text", nullable: false),
                    vendor = table.Column<string>(type: "text", nullable: false),
                    door_id = table.Column<int>(type: "integer", nullable: false),
                    device_module_id = table.Column<int>(type: "integer", nullable: false),
                    timezone_id = table.Column<int>(type: "integer", nullable: true),
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
                    table.PrimaryKey("PK_Rexes", x => x.id);
                    table.ForeignKey(
                        name: "FK_Rexes_DeviceModules_device_module_id",
                        column: x => x.device_module_id,
                        principalSchema: "core",
                        principalTable: "DeviceModules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rexes_Doors_door_id",
                        column: x => x.door_id,
                        principalSchema: "core",
                        principalTable: "Doors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rexes_Locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Rexes_TimeZones_timezone_id",
                        column: x => x.timezone_id,
                        principalSchema: "core",
                        principalTable: "TimeZones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Sensors",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    slot_no = table.Column<int>(type: "integer", nullable: false),
                    metadata = table.Column<string>(type: "text", nullable: false),
                    vendor = table.Column<string>(type: "text", nullable: false),
                    door_id = table.Column<int>(type: "integer", nullable: true),
                    lane_id = table.Column<int>(type: "integer", nullable: true),
                    device_module_id = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_Sensors", x => x.id);
                    table.ForeignKey(
                        name: "FK_Sensors_DeviceModules_device_module_id",
                        column: x => x.device_module_id,
                        principalSchema: "core",
                        principalTable: "DeviceModules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sensors_Doors_door_id",
                        column: x => x.door_id,
                        principalSchema: "core",
                        principalTable: "Doors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sensors_Locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Lanes",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    lane_no = table.Column<int>(type: "integer", nullable: false),
                    turnstile_id = table.Column<int>(type: "integer", nullable: false),
                    sensor_id = table.Column<int>(type: "integer", nullable: true),
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
                    table.PrimaryKey("PK_Lanes", x => x.id);
                    table.ForeignKey(
                        name: "FK_Lanes_Locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Lanes_Sensors_sensor_id",
                        column: x => x.sensor_id,
                        principalSchema: "core",
                        principalTable: "Sensors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lanes_Turnstiles_turnstile_id",
                        column: x => x.turnstile_id,
                        principalSchema: "core",
                        principalTable: "Turnstiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Readers",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    slot_no = table.Column<int>(type: "integer", nullable: false),
                    mode = table.Column<string>(type: "text", nullable: false),
                    metadata = table.Column<string>(type: "text", nullable: false),
                    vendor = table.Column<string>(type: "text", nullable: false),
                    reader_direction = table.Column<string>(type: "text", nullable: false),
                    door_id = table.Column<int>(type: "integer", nullable: true),
                    lane_id = table.Column<int>(type: "integer", nullable: true),
                    device_module_id = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_Readers", x => x.id);
                    table.ForeignKey(
                        name: "FK_Readers_DeviceModules_device_module_id",
                        column: x => x.device_module_id,
                        principalSchema: "core",
                        principalTable: "DeviceModules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Readers_Doors_door_id",
                        column: x => x.door_id,
                        principalSchema: "core",
                        principalTable: "Doors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Readers_Lanes_lane_id",
                        column: x => x.lane_id,
                        principalSchema: "core",
                        principalTable: "Lanes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Readers_Locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "id");
                });

            migrationBuilder.InsertData(
                schema: "core",
                table: "AlertLevels",
                columns: new[] { "id", "altr_external_id", "color_code", "external_id", "is_active", "is_default", "location_id", "name" },
                values: new object[,]
                {
                    { 1, null, "#ff1100", null, true, true, null, "Emergency" },
                    { 2, null, "#c2432d", null, true, true, null, "Danger" },
                    { 3, null, "#ff911c", null, true, true, null, "High" },
                    { 4, null, "#ffbd2d", null, true, true, null, "Moderate" },
                    { 5, null, "#989a9c", null, true, true, null, "Low" }
                });

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
                    { 20, "EVENT" },
                    { 21, "ADAPTER_EVENT" },
                    { 22, "INPUT" },
                    { 23, "OUTPUT" },
                    { 24, "PROCEDURE" },
                    { 25, "TRIGGER" },
                    { 26, "MONITOR_GROUP" },
                    { 27, "AREA" },
                    { 28, "WEB" },
                    { 29, "CARD_FORMAT" },
                    { 30, "ACTION" },
                    { 31, "ALERT" },
                    { 32, "ALERT_LEVEL" },
                    { 33, "CORE_EVENT" }
                });

            migrationBuilder.InsertData(
                schema: "core",
                table: "Countries",
                columns: new[] { "id", "code", "name" },
                values: new object[,]
                {
                    { 1, "AD", "Andorra" },
                    { 2, "AE", "United Arab Emirates" },
                    { 3, "AF", "Afghanistan" },
                    { 4, "AG", "Antigua and Barbuda" },
                    { 5, "AI", "Anguilla" },
                    { 6, "AL", "Albania" },
                    { 7, "AM", "Armenia" },
                    { 8, "AN", "Netherlands Antilles" },
                    { 9, "AO", "Angola" },
                    { 10, "AQ", "Antarctica" },
                    { 11, "AR", "Argentina" },
                    { 12, "AS", "American Samoa" },
                    { 13, "AT", "Austria" },
                    { 14, "AU", "Australia" },
                    { 15, "AW", "Aruba" },
                    { 16, "AZ", "Azerbaijan" },
                    { 17, "BA", "Bosnia and Herzegovina" },
                    { 18, "BB", "Barbados" },
                    { 19, "BD", "Bangladesh" },
                    { 20, "BE", "Belgium" },
                    { 21, "BF", "Burkina Faso" },
                    { 22, "BG", "Bulgaria" },
                    { 23, "BH", "Bahrain" },
                    { 24, "BI", "Burundi" },
                    { 25, "BJ", "Benin" },
                    { 26, "BM", "Bermuda" },
                    { 27, "BN", "Brunei" },
                    { 28, "BO", "Bolivia" },
                    { 29, "BR", "Brazil" },
                    { 30, "BS", "Bahamas" },
                    { 31, "BT", "Bhutan" },
                    { 32, "BV", "Bouvet Island" },
                    { 33, "BW", "Botswana" },
                    { 34, "BY", "Belarus" },
                    { 35, "BZ", "Belize" },
                    { 36, "CA", "Canada" },
                    { 37, "CC", "Cocos (Keeling) Islands" },
                    { 38, "CD", "Congo (DRC)" },
                    { 39, "CF", "Central African Republic" },
                    { 40, "CG", "Congo (Republic)" },
                    { 41, "CH", "Switzerland" },
                    { 42, "CI", "Côte d'Ivoire" },
                    { 43, "CK", "Cook Islands" },
                    { 44, "CL", "Chile" },
                    { 45, "CM", "Cameroon" },
                    { 46, "CN", "China" },
                    { 47, "CO", "Colombia" },
                    { 48, "CR", "Costa Rica" },
                    { 49, "CU", "Cuba" },
                    { 50, "CV", "Cape Verde" },
                    { 51, "CX", "Christmas Island" },
                    { 52, "CY", "Cyprus" },
                    { 53, "CZ", "Czech Republic" },
                    { 54, "DE", "Germany" },
                    { 55, "DJ", "Djibouti" },
                    { 56, "DK", "Denmark" },
                    { 57, "DM", "Dominica" },
                    { 58, "DO", "Dominican Republic" },
                    { 59, "DZ", "Algeria" },
                    { 60, "EC", "Ecuador" },
                    { 61, "EE", "Estonia" },
                    { 62, "EG", "Egypt" },
                    { 63, "EH", "Western Sahara" },
                    { 64, "ER", "Eritrea" },
                    { 65, "ES", "Spain" },
                    { 66, "ET", "Ethiopia" },
                    { 67, "FI", "Finland" },
                    { 68, "FJ", "Fiji" },
                    { 69, "FK", "Falkland Islands" },
                    { 70, "FM", "Micronesia" },
                    { 71, "FO", "Faroe Islands" },
                    { 72, "FR", "France" },
                    { 73, "GA", "Gabon" },
                    { 74, "GB", "United Kingdom" },
                    { 75, "GD", "Grenada" },
                    { 76, "GE", "Georgia" },
                    { 77, "GF", "French Guiana" },
                    { 78, "GG", "Guernsey" },
                    { 79, "GH", "Ghana" },
                    { 80, "GI", "Gibraltar" },
                    { 81, "GL", "Greenland" },
                    { 82, "GM", "Gambia" },
                    { 83, "GN", "Guinea" },
                    { 84, "GP", "Guadeloupe" },
                    { 85, "GQ", "Equatorial Guinea" },
                    { 86, "GR", "Greece" },
                    { 87, "GT", "Guatemala" },
                    { 88, "GU", "Guam" },
                    { 89, "GW", "Guinea-Bissau" },
                    { 90, "GY", "Guyana" },
                    { 91, "HK", "Hong Kong" },
                    { 92, "HN", "Honduras" },
                    { 93, "HR", "Croatia" },
                    { 94, "HT", "Haiti" },
                    { 95, "HU", "Hungary" },
                    { 96, "ID", "Indonesia" },
                    { 97, "IE", "Ireland" },
                    { 98, "IL", "Israel" },
                    { 99, "IN", "India" },
                    { 100, "IQ", "Iraq" },
                    { 101, "IR", "Iran" },
                    { 102, "IS", "Iceland" },
                    { 103, "IT", "Italy" },
                    { 104, "JM", "Jamaica" },
                    { 105, "JO", "Jordan" },
                    { 106, "JP", "Japan" },
                    { 107, "KE", "Kenya" },
                    { 108, "KH", "Cambodia" },
                    { 109, "KR", "South Korea" },
                    { 110, "KW", "Kuwait" },
                    { 111, "KZ", "Kazakhstan" },
                    { 112, "LA", "Laos" },
                    { 113, "LB", "Lebanon" },
                    { 114, "LK", "Sri Lanka" },
                    { 115, "LR", "Liberia" },
                    { 116, "LS", "Lesotho" },
                    { 117, "LT", "Lithuania" },
                    { 118, "LU", "Luxembourg" },
                    { 119, "LV", "Latvia" },
                    { 120, "LY", "Libya" },
                    { 121, "MA", "Morocco" },
                    { 122, "MC", "Monaco" },
                    { 123, "MD", "Moldova" },
                    { 124, "ME", "Montenegro" },
                    { 125, "MG", "Madagascar" },
                    { 126, "MV", "Maldives" },
                    { 127, "MX", "Mexico" },
                    { 128, "MY", "Malaysia" },
                    { 129, "MZ", "Mozambique" },
                    { 130, "NA", "Namibia" },
                    { 131, "NG", "Nigeria" },
                    { 132, "NL", "Netherlands" },
                    { 133, "NO", "Norway" },
                    { 134, "NP", "Nepal" },
                    { 135, "NZ", "New Zealand" },
                    { 136, "OM", "Oman" },
                    { 137, "PA", "Panama" },
                    { 138, "PE", "Peru" },
                    { 139, "PH", "Philippines" },
                    { 140, "PK", "Pakistan" },
                    { 141, "PL", "Poland" },
                    { 142, "PT", "Portugal" },
                    { 143, "QA", "Qatar" },
                    { 144, "RO", "Romania" },
                    { 145, "RS", "Serbia" },
                    { 146, "RU", "Russia" },
                    { 147, "RW", "Rwanda" },
                    { 148, "SA", "Saudi Arabia" },
                    { 149, "SE", "Sweden" },
                    { 150, "SG", "Singapore" },
                    { 151, "SI", "Slovenia" },
                    { 152, "SK", "Slovakia" },
                    { 153, "SN", "Senegal" },
                    { 154, "SO", "Somalia" },
                    { 155, "SR", "Suriname" },
                    { 156, "SV", "El Salvador" },
                    { 157, "SY", "Syria" },
                    { 158, "TH", "Thailand" },
                    { 159, "TJ", "Tajikistan" },
                    { 160, "TL", "Timor-Leste" },
                    { 161, "TM", "Turkmenistan" },
                    { 162, "TN", "Tunisia" },
                    { 163, "TR", "Turkey" },
                    { 164, "TW", "Taiwan" },
                    { 165, "TZ", "Tanzania" },
                    { 166, "UA", "Ukraine" },
                    { 167, "UG", "Uganda" },
                    { 168, "US", "United States" },
                    { 169, "UY", "Uruguay" },
                    { 170, "UZ", "Uzbekistan" },
                    { 171, "VA", "Vatican City" },
                    { 172, "VE", "Venezuela" },
                    { 173, "VN", "Vietnam" },
                    { 174, "YE", "Yemen" },
                    { 175, "ZA", "South Africa" },
                    { 176, "ZM", "Zambia" },
                    { 177, "ZW", "Zimbabwe" },
                    { 178, "", "Default" }
                });

            migrationBuilder.InsertData(
                schema: "core",
                table: "Groups",
                columns: new[] { "id", "altr_external_id", "external_id", "guid", "is_active", "is_default", "location_id", "name" },
                values: new object[] { 1, null, (short)1, new Guid("895b0301-3d77-4ec7-8917-43260a839643"), true, true, null, "All Door" });

            migrationBuilder.InsertData(
                schema: "core",
                table: "Modules",
                columns: new[] { "id", "altr_external_id", "external_id", "is_active", "is_default", "location_id", "locationid", "name" },
                values: new object[,]
                {
                    { 1, null, null, true, false, null, null, "Access" },
                    { 2, null, null, true, false, null, null, "Visitor" },
                    { 3, null, null, true, false, null, null, "Time Attendance" }
                });

            migrationBuilder.InsertData(
                schema: "core",
                table: "Roles",
                columns: new[] { "id", "altr_external_id", "external_id", "guid", "is_active", "is_default", "location_id", "locationid", "name" },
                values: new object[] { 1, null, null, new Guid("fe527691-7b13-4294-98b5-cb95181f5453"), true, true, null, null, "Administrator" });

            migrationBuilder.InsertData(
                schema: "core",
                table: "TriggerSources",
                columns: new[] { "id", "name", "value" },
                values: new object[,]
                {
                    { 1, "Device diagnostics", (short)0 },
                    { 2, "Device to HOST communication driver - not defined", (short)1 },
                    { 3, "Device local monitor points (tamper & power fault)", (short)2 },
                    { 4, "Module diagnostics", (short)3 },
                    { 5, "Module communication driver", (short)4 },
                    { 6, "Module cabinet tamper", (short)5 },
                    { 7, "Module power monitor", (short)6 },
                    { 8, "Alarm monitor point", (short)7 },
                    { 9, "Output control point", (short)8 },
                    { 10, "Access Control Reader (ACR)", (short)9 },
                    { 11, "Door: reader tamper monitor", (short)10 },
                    { 12, "Door: door position sensor", (short)11 },
                    { 13, "Door: 1st \"Request to exit\" input", (short)13 },
                    { 14, "Door: 2nd \"Request to exit\" input", (short)14 },
                    { 15, "Time zone", (short)15 },
                    { 16, "Procedure (action list)", (short)16 },
                    { 17, "Trigger", (short)17 },
                    { 18, "Trigger variable", (short)18 },
                    { 19, "Monitor point group", (short)19 },
                    { 20, "Access area", (short)20 },
                    { 21, "Door: the alternate reader's tamper monitor source_number", (short)21 },
                    { 22, "Login Service", (short)24 }
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

            migrationBuilder.InsertData(
                schema: "core",
                table: "Features",
                columns: new[] { "id", "guid", "module_id", "name" },
                values: new object[,]
                {
                    { 1, new Guid("f1f1f528-1025-44de-8512-be5f269417e8"), 1, "dashboard" },
                    { 2, new Guid("62e7ede3-9152-476a-a4df-173cc16a12fe"), 1, "events" },
                    { 3, new Guid("c164d952-6649-49bb-95c9-2543695b8af6"), 1, "location" },
                    { 4, new Guid("14fa8dca-521d-4e1a-a582-0159df91aea9"), 1, "alert" },
                    { 5, new Guid("60239ccd-4cd7-441a-94c4-4a1577c79e38"), 1, "operator" },
                    { 6, new Guid("dc76438d-0e0d-4d60-88bc-0559cb81ce4a"), 1, "device" },
                    { 7, new Guid("77c0545d-ec94-4037-802f-2240bcc9020e"), 1, "control" },
                    { 8, new Guid("2242b3c0-06e7-4e07-be9f-7491584c57c9"), 1, "monitor" },
                    { 9, new Guid("8b5c31bb-706b-4fa5-b0f0-dd246f1e9a2b"), 1, "monitorgroup" },
                    { 10, new Guid("f2143a86-d2f1-47ad-a481-c74ecbdadc83"), 1, "acr" },
                    { 11, new Guid("b753863e-30f4-47aa-81b3-64dda55970da"), 1, "user" },
                    { 12, new Guid("5ab363b5-a921-41e5-949e-5129eb416097"), 1, "group" },
                    { 13, new Guid("4401f4d8-4145-4439-adab-d89cd3e3b2fb"), 1, "area" },
                    { 14, new Guid("f18ef407-cd4d-46e8-a9f5-cbc99b87a0e4"), 1, "time" },
                    { 15, new Guid("5699f80a-aa8c-4325-88cf-8ba31b85f976"), 1, "trigger" },
                    { 16, new Guid("76d4a40a-3fa8-4a9f-a7b5-2b63f57fd26d"), 1, "map" },
                    { 17, new Guid("24ebed55-6686-45a8-95a5-00e4e6516f4f"), 1, "report" },
                    { 18, new Guid("713bcbae-1755-4a94-ab82-0180a856c80a"), 1, "setting" },
                    { 19, new Guid("d57aac0d-1f61-4135-857b-cc1f51288d72"), 1, "tools" }
                });

            migrationBuilder.InsertData(
                schema: "core",
                table: "Features",
                columns: new[] { "id", "module_id", "name" },
                values: new object[,]
                {
                    { 20, 2, "register" },
                    { 21, 2, "appointment" },
                    { 22, 3, "report" }
                });

            migrationBuilder.InsertData(
                schema: "core",
                table: "Locations",
                columns: new[] { "id", "altr_external_id", "country_id", "description", "external_id", "guid", "is_active", "is_default", "location_id", "locationid", "name" },
                values: new object[] { 1, null, 178, "All location", null, new Guid("3a9c9947-d5ca-4bb2-b525-0499a340f1d6"), true, true, null, null, "All" });

            migrationBuilder.InsertData(
                schema: "core",
                table: "ModulePermissions",
                columns: new[] { "id", "altr_external_id", "external_id", "is_active", "is_default", "is_enabled", "location_id", "locationid", "module_id", "name", "role_id" },
                values: new object[,]
                {
                    { 1, null, null, true, true, true, null, null, 1, "", 1 },
                    { 2, null, null, true, true, true, null, null, 2, "", 1 },
                    { 3, null, null, true, true, true, null, null, 3, "", 1 }
                });

            migrationBuilder.InsertData(
                schema: "core",
                table: "Operators",
                columns: new[] { "id", "altr_external_id", "email", "expired_date", "external_id", "firstname", "gender", "is_active", "is_default", "joined_date", "lastname", "location_id", "locationid", "middlename", "name", "password", "phone", "role_id", "title", "username" },
                values: new object[] { 1, null, "support@sentrix.com", null, null, "Administrator", "Male", true, false, new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", null, null, "", "", "100000.lG1/4V/VRPZsbhf/Zqc4xw==.6vYcf+wEMSgqcaNhoZEdM9PaPxx2ZUErZhQbeMxo5OY=", "", 1, "Mr", "admin" });

            migrationBuilder.InsertData(
                schema: "core",
                table: "TriggerTypes",
                columns: new[] { "id", "name", "trigger_source_id", "value" },
                values: new object[,]
                {
                    { 1, "System", 1, (short)1 },
                    { 2, "SIO communication status report", 5, (short)2 },
                    { 3, "Binary card data", 10, (short)3 },
                    { 4, "Card data", 10, (short)4 },
                    { 5, "Formatted card: facility code, card number ID, issue code", 10, (short)5 },
                    { 6, "Formatted card: card number only", 10, (short)6 },
                    { 7, "Change-of-state", 3, (short)7 },
                    { 8, "Exit request", 10, (short)8 },
                    { 9, "Door status monitor change-of-state", 10, (short)9 },
                    { 10, "Procedure (command list) log", 16, (short)10 },
                    { 11, "User command request report", 10, (short)11 },
                    { 12, "Change of state: trigger variable, time zone, & triggers", 15, (short)12 },
                    { 13, "ACR mode change", 10, (short)13 },
                    { 14, "Monitor point group status change", 19, (short)14 },
                    { 15, "Access area", 20, (short)15 },
                    { 16, "Extended user command", 10, (short)18 },
                    { 17, "Use limit report", 10, (short)19 },
                    { 18, "Web activity", 1, (short)20 },
                    { 19, "Specify tranTypeCardFull (0x05) instead", 10, (short)21 },
                    { 20, "Specify tranTypeCardID (0x06) instead", 10, (short)22 },
                    { 21, "Operating mode change", 1, (short)24 },
                    { 22, "Elevator Floor Status CoS", 10, (short)26 },
                    { 23, "File Download Status", 1, (short)27 },
                    { 24, "Elevator Floor Access Transaction", 10, (short)29 },
                    { 25, "Specify tranTypeCardFull (0x05) instead", 10, (short)37 },
                    { 26, "Specify tranTypeCardID (0x06) instead", 10, (short)38 },
                    { 27, "Specify tranTypeCardFull (0x05) instead", 10, (short)53 },
                    { 28, "ACR extended feature stateless transition", 10, (short)64 },
                    { 29, "ACR extended feature change-of-state", 10, (short)65 },
                    { 30, "Change-of-state", 6, (short)7 },
                    { 31, "Change-of-state", 7, (short)7 },
                    { 32, "Change-of-state", 8, (short)7 },
                    { 33, "Change-of-state", 9, (short)7 },
                    { 34, "Change-of-state", 11, (short)7 },
                    { 35, "Change-of-state", 13, (short)7 },
                    { 36, "Change-of-state", 14, (short)7 },
                    { 37, "Change-of-state", 21, (short)7 },
                    { 38, "Change-of-state", 22, (short)7 },
                    { 39, "Change of state: trigger variable, time zone, & triggers", 17, (short)12 },
                    { 40, "Change of state: trigger variable, time zone, & triggers", 18, (short)12 }
                });

            migrationBuilder.InsertData(
                schema: "core",
                table: "CardFormats",
                columns: new[] { "id", "altr_external_id", "bits", "card_no_len", "card_no_loc", "even_parity_len", "even_parity_loc", "external_id", "fac", "fac_len", "fac_loc", "guid", "is_active", "is_default", "issue_code_len", "issue_code_loc", "location_id", "metadata", "name", "odd_parity_len", "odd_parity_loc" },
                values: new object[,]
                {
                    { 1, null, (short)26, (short)26, (short)0, (short)0, (short)-1, (short)0, (short)-1, (short)0, (short)-1, new Guid("356b51d4-5650-4ef9-821b-bf38eb4ec2db"), true, false, (short)0, (short)-1, 1, "", "26-bit Wiegand", (short)0, (short)-1 },
                    { 2, null, (short)32, (short)32, (short)0, (short)0, (short)-1, (short)1, (short)-1, (short)0, (short)-1, new Guid("f04c1839-991d-4f13-be52-4a057d30af68"), true, false, (short)0, (short)-1, 1, "", "32-bit Wiegand", (short)0, (short)-1 },
                    { 3, null, (short)37, (short)37, (short)0, (short)0, (short)-1, (short)2, (short)-1, (short)0, (short)-1, new Guid("2ec6aa6c-dd76-4b3a-98b3-e8b5ecff0d1e"), true, false, (short)0, (short)-1, 1, "", "37-bit Wiegand", (short)0, (short)-1 }
                });

            migrationBuilder.InsertData(
                schema: "core",
                table: "FeaturePermissions",
                columns: new[] { "id", "feature_id", "is_created", "is_deleted", "is_enabled", "is_updated", "module_permission_id" },
                values: new object[,]
                {
                    { 1, 1, true, true, true, true, 1 },
                    { 2, 2, true, true, true, true, 1 },
                    { 3, 3, true, true, true, true, 1 },
                    { 4, 4, true, true, true, true, 1 },
                    { 5, 5, true, true, true, true, 1 },
                    { 6, 6, true, true, true, true, 1 },
                    { 7, 7, true, true, true, true, 1 },
                    { 8, 8, true, true, true, true, 1 },
                    { 9, 9, true, true, true, true, 1 },
                    { 10, 10, true, true, true, true, 1 },
                    { 11, 11, true, true, true, true, 1 },
                    { 12, 12, true, true, true, true, 1 },
                    { 13, 13, true, true, true, true, 1 },
                    { 14, 14, true, true, true, true, 1 },
                    { 15, 15, true, true, true, true, 1 },
                    { 16, 16, true, true, true, true, 1 },
                    { 17, 17, true, true, true, true, 1 },
                    { 18, 18, true, true, true, true, 1 },
                    { 19, 19, true, true, true, true, 1 },
                    { 20, 20, true, true, true, true, 2 },
                    { 21, 21, true, true, true, true, 2 },
                    { 22, 22, true, true, true, true, 3 }
                });

            migrationBuilder.InsertData(
                schema: "core",
                table: "OperatorLocations",
                columns: new[] { "id", "location_id", "operator_id" },
                values: new object[] { 1, 1, 1 });

            migrationBuilder.InsertData(
                schema: "core",
                table: "TimeZones",
                columns: new[] { "id", "altr_external_id", "external_id", "guid", "is_active", "is_default", "location_id", "name" },
                values: new object[,]
                {
                    { 1, null, (short)1, new Guid("cff24195-43f8-42ec-a03f-e84857cc958f"), true, true, 1, "Always" },
                    { 2, null, (short)2, new Guid("614a766c-1241-4fdc-9d0d-7788291cac2d"), true, true, 1, "Never" }
                });

            migrationBuilder.InsertData(
                schema: "core",
                table: "TriggerCodes",
                columns: new[] { "id", "name", "trigger_type_id", "value" },
                values: new object[,]
                {
                    { 1, "SCP Power-up diagnostic", 1, (short)1 },
                    { 2, "Host communication offline", 1, (short)2 },
                    { 3, "Host communication online", 1, (short)3 },
                    { 4, "Transaction count exceed limit", 1, (short)4 },
                    { 5, "Configuration DB save complete", 1, (short)5 },
                    { 6, "Card database save complete", 1, (short)6 },
                    { 7, "Card database cleared due to SRAM buffer overflow", 1, (short)7 },
                    { 8, "Disabled", 2, (short)1 },
                    { 9, "Offline:timeout", 2, (short)2 },
                    { 10, "Offline:invalid identificatiion from module", 2, (short)3 },
                    { 11, "Offline:command too long", 2, (short)4 },
                    { 12, "Online:normal connection", 2, (short)5 },
                    { 13, "hexLoad report: ser_num is address loaded (-1 = last record)", 2, (short)6 },
                    { 14, "Access denied, Invalid card format", 3, (short)1 },
                    { 15, "Access denied, Invalid card format, forward read", 4, (short)1 },
                    { 16, "Access denied, Invalid card format, reverse read", 4, (short)2 },
                    { 17, "Request rejected: access point \"locked\"", 5, (short)1 },
                    { 18, "Request accepted: access point \"unlocked\"", 5, (short)2 },
                    { 19, "Request rejected: invalid facility code", 5, (short)3 },
                    { 20, "Request rejected: invalid facility code extension", 5, (short)4 },
                    { 21, "Request rejected: not in card file", 5, (short)5 },
                    { 22, "Request rejected: invalid issue code", 5, (short)6 },
                    { 23, "Request granted: facility code verified, not used", 5, (short)7 },
                    { 24, "Request granted: facility code verified, door used", 5, (short)8 },
                    { 25, "Access denied - asked for host approval, then timed out", 5, (short)9 },
                    { 26, "Reporting that this card is \"about to get access granted\"", 5, (short)10 },
                    { 27, "Access denied count exceeded", 5, (short)11 },
                    { 28, "Access denied - asked for host approval, then host denied", 5, (short)12 },
                    { 29, "Request rejected: Airlock is busy", 5, (short)13 },
                    { 30, "Request rejected: deactivated card", 6, (short)1 },
                    { 31, "Request rejected: before activation date", 6, (short)2 },
                    { 32, "Request rejected: after expiration date", 6, (short)3 },
                    { 33, "Request rejected: invalid time", 6, (short)4 },
                    { 34, "Request rejected: invalid PIN", 6, (short)5 },
                    { 35, "Request rejected: anti-passback violation", 6, (short)6 },
                    { 36, "Request granted: APB violation, not used", 6, (short)7 },
                    { 37, "Request granted: APB violation, used", 6, (short)8 },
                    { 38, "Request rejected: duress code detected", 6, (short)9 },
                    { 39, "Request granted: duress, used", 6, (short)10 },
                    { 40, "Request granted: duress, not used", 6, (short)11 },
                    { 41, "Request granted: full test, not used", 6, (short)12 },
                    { 42, "Request granted: full test, used", 6, (short)13 },
                    { 43, "Request denied: never allowed at this reader (all Tz's = 0)", 6, (short)14 },
                    { 44, "Request denied: no second card presented", 6, (short)15 },
                    { 45, "Request denied: occupancy limit reached", 6, (short)16 },
                    { 46, "Request denied: the area is NOT enabled", 6, (short)17 },
                    { 47, "Request denied: use limit", 6, (short)18 },
                    { 48, "Granting access: used/not used transaction will follow", 6, (short)21 },
                    { 49, "Request rejected: no escort card presented", 6, (short)24 },
                    { 50, "Reserved", 6, (short)25 },
                    { 51, "Reserved", 6, (short)26 },
                    { 52, "Reserved", 6, (short)27 },
                    { 53, "Request rejected: airlock is busy", 6, (short)29 },
                    { 54, "Request rejected: incomplete CARD & PIN sequence", 6, (short)30 },
                    { 55, "Request granted: double-card event", 6, (short)31 },
                    { 56, "Request granted: double-card event while in uncontrolled state (locked/unlocked)", 6, (short)32 },
                    { 57, "Granting access: requires escort, pending escort card", 6, (short)39 },
                    { 58, "Request rejected: violates minimum occupancy count", 6, (short)40 },
                    { 59, "Request rejected: card pending at another reader", 6, (short)41 },
                    { 60, "Disconnected", 7, (short)1 },
                    { 61, "Unknown", 7, (short)2 },
                    { 62, "Secure", 7, (short)3 },
                    { 63, "Alarm", 7, (short)4 },
                    { 64, "Fault", 7, (short)5 },
                    { 65, "Exit delay in progress", 7, (short)6 },
                    { 66, "Entry delay in progress", 7, (short)7 },
                    { 67, "Online, reader tamper inactive", 7, (short)0 },
                    { 68, "Online, reader tamper active", 7, (short)1 },
                    { 69, "N/A", 7, (short)2 },
                    { 70, "Communication broken (reader offline)", 7, (short)3 },
                    { 71, "Host initiated request: door use not verified", 8, (short)4 },
                    { 72, "Host initiated request: door not used", 8, (short)5 },
                    { 73, "Host initiated request: door used", 8, (short)6 },
                    { 74, "Exit cycle: started", 8, (short)9 },
                    { 75, "Disconnected", 9, (short)1 },
                    { 76, "Unknown", 9, (short)2 },
                    { 77, "Secure", 9, (short)3 },
                    { 78, "Alarm (forced, held open or both)", 9, (short)4 },
                    { 79, "Fault", 9, (short)5 },
                    { 80, "Cancel procedure (abort delay)", 10, (short)1 },
                    { 81, "Execute procedure (start new)", 10, (short)2 },
                    { 82, "Resume procedure, if paused", 10, (short)3 },
                    { 83, "Execute procedure with prefix 256 actions", 10, (short)4 },
                    { 84, "Execute procedure with prefix 512 actions", 10, (short)5 },
                    { 85, "Execute procedure with prefix 1024 actions", 10, (short)6 },
                    { 86, "Resume procedure with prefix 256 actions", 10, (short)7 },
                    { 87, "Resume procedure with prefix 512 actions", 10, (short)8 },
                    { 88, "Resume procedure with prefix 1024 actions", 10, (short)9 },
                    { 89, "Command was issued to procedure with no actions - (NOP)", 10, (short)10 },
                    { 90, "Command entered by the user", 11, (short)1 },
                    { 91, "Became inactive", 12, (short)1 },
                    { 92, "Became active", 12, (short)2 },
                    { 93, "Disabled", 13, (short)1 },
                    { 94, "Unlocked", 13, (short)2 },
                    { 95, "Locked (exit request enabled)", 13, (short)3 },
                    { 96, "Facility code only", 13, (short)4 },
                    { 97, "Card only", 13, (short)5 },
                    { 98, "PIN only", 13, (short)6 },
                    { 99, "Card and PIN", 13, (short)7 },
                    { 100, "PIN or card", 13, (short)8 },
                    { 101, "First disarm command executed (mask_count was 0, all MPs got masked)", 14, (short)1 },
                    { 102, "Subsequent disarm command executed (mask_count incremented, MPs already masked)", 14, (short)2 },
                    { 103, "Override command: armed (mask_count cleared, all points unmasked)", 14, (short)3 },
                    { 104, "Override command: disarmed (mask_count set, unmasked all points)", 14, (short)4 },
                    { 105, "Force arm command, MPG armed, (may have active zones, mask_count is now zero)", 14, (short)5 },
                    { 106, "Force arm command, MPG not armed (mask_count decremented)", 14, (short)6 },
                    { 107, "Standard arm command, MPG armed (did not have active zones, mask_count is now zero)", 14, (short)7 },
                    { 108, "Standard arm command, MPG did not arm, (had active zones, mask_count unchanged)", 14, (short)8 },
                    { 109, "Standard arm command, MPG still armed, (mask_count decremented)", 14, (short)9 },
                    { 110, "Override arm command, MPG armed (mask_count is now zero)", 14, (short)10 },
                    { 111, "Override arm command, MPG did not arm, (mask_count decremented)", 14, (short)11 },
                    { 112, "Area disabled", 15, (short)1 },
                    { 113, "Area enabled", 15, (short)2 },
                    { 114, "Occupancy count reached zero", 15, (short)3 },
                    { 115, "Occupancy count reached the \"downward-limit\"", 15, (short)4 },
                    { 116, "Occupancy count reached the \"upward-limit\"", 15, (short)5 },
                    { 117, "Occupancy count reached the \"max-occupancy-limit\"", 15, (short)6 },
                    { 118, "Multi-occupancy mode changed", 15, (short)7 },
                    { 119, "Use limit changed, reporting new limit", 17, (short)1 },
                    { 120, "Save home notes", 18, (short)1 },
                    { 121, "Save network settings", 18, (short)2 },
                    { 122, "Save host communication settings", 18, (short)3 },
                    { 123, "Add user", 18, (short)4 },
                    { 124, "Delete user", 18, (short)5 },
                    { 125, "Modify user", 18, (short)6 },
                    { 126, "Save password strength and session timer", 18, (short)7 },
                    { 127, "Save web server options", 18, (short)8 },
                    { 128, "Save time server settings", 18, (short)9 },
                    { 129, "Auto save timer settings", 18, (short)10 },
                    { 130, "Load certificate", 18, (short)11 },
                    { 131, "Logged out by link", 18, (short)12 },
                    { 132, "Logged out by timeout", 18, (short)13 },
                    { 133, "Logged out by user", 18, (short)14 },
                    { 134, "Logged out by apply", 18, (short)15 },
                    { 135, "Invalid login", 18, (short)16 },
                    { 136, "Successful login", 18, (short)17 },
                    { 137, "Network diagnostic saved", 18, (short)18 },
                    { 138, "Card DB size saved", 18, (short)19 },
                    { 139, "Diagnostic page saved", 18, (short)21 },
                    { 140, "Security options page saved", 18, (short)22 },
                    { 141, "Add-on package page saved", 18, (short)23 },
                    { 142, "Not used", 18, (short)24 },
                    { 143, "Not used", 18, (short)25 },
                    { 144, "Not used", 18, (short)26 },
                    { 145, "Invalid login limit reached", 18, (short)27 },
                    { 146, "Firmware download initiated", 18, (short)28 },
                    { 147, "Advanced networking routes saved", 18, (short)29 },
                    { 148, "Advanced networking reversion timer started", 18, (short)30 },
                    { 149, "Advanced networking reversion timer elapsed", 18, (short)31 },
                    { 150, "Advanced networking route changes reverted", 18, (short)32 },
                    { 151, "Advanced networking route changes cleared", 18, (short)33 },
                    { 152, "Certificate generation started", 18, (short)34 },
                    { 153, "Operating mode changed to mode 0", 21, (short)1 },
                    { 154, "Operating mode changed to mode 1", 21, (short)2 },
                    { 155, "Operating mode changed to mode 2", 21, (short)3 },
                    { 156, "Operating mode changed to mode 3", 21, (short)4 },
                    { 157, "Operating mode changed to mode 4", 21, (short)5 },
                    { 158, "Operating mode changed to mode 5", 21, (short)6 },
                    { 159, "Operating mode changed to mode 6", 21, (short)7 },
                    { 160, "Operating mode changed to mode 7", 21, (short)8 },
                    { 161, "Floor status is secure", 22, (short)1 },
                    { 162, "Floor status is public", 22, (short)2 },
                    { 163, "Floor status is disabled (override)", 22, (short)3 },
                    { 164, "File transfer success", 23, (short)1 },
                    { 165, "File transfer error", 23, (short)2 },
                    { 166, "File delete successful", 23, (short)3 },
                    { 167, "File delete unsuccessful", 23, (short)4 },
                    { 168, "OSDP file transfer complete (primary ACR) - look at source number for ACR number", 23, (short)5 },
                    { 169, "OSDP file transfer error (primary ACR) - look at source number for ACR number", 23, (short)6 },
                    { 170, "OSDP file transfer complete (alternate ACR) - look at source number for ACR number", 23, (short)7 },
                    { 171, "OSDP file transfer error (alternate ACR) - look at source number for ACR number", 23, (short)8 },
                    { 172, "Elevator access", 24, (short)1 },
                    { 173, "Extended status updated", 28, (short)1 },
                    { 174, "Secure / Inactive", 29, (short)3 },
                    { 175, "Alarm / Active", 29, (short)4 },
                    { 176, "Disconnected", 8, (short)1 },
                    { 177, "Exit cycle: door use not verified", 8, (short)1 },
                    { 178, "Exit cycle: door not used", 8, (short)2 },
                    { 179, "Exit cycle: door used", 8, (short)3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Action_device_id",
                schema: "core",
                table: "Action",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "IX_Action_guid_id_type_device_id_location_id",
                schema: "core",
                table: "Action",
                columns: new[] { "guid", "id", "type", "device_id", "location_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Action_location_id",
                schema: "core",
                table: "Action",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_AdapterEvents_guid_id_location_id_created_at_mac_send_at_re~",
                schema: "core",
                table: "AdapterEvents",
                columns: new[] { "guid", "id", "location_id", "created_at", "mac", "send_at", "received_at", "tag", "vendor" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdapterEvents_location_id",
                schema: "core",
                table: "AdapterEvents",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_AlertComponents_alert_id",
                schema: "core",
                table: "AlertComponents",
                column: "alert_id");

            migrationBuilder.CreateIndex(
                name: "IX_AlertComponents_door_id",
                schema: "core",
                table: "AlertComponents",
                column: "door_id");

            migrationBuilder.CreateIndex(
                name: "IX_AlertEvents_guid_id_location_id",
                schema: "core",
                table: "AlertEvents",
                columns: new[] { "guid", "id", "location_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AlertEvents_location_id",
                schema: "core",
                table: "AlertEvents",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_AlertLevels_guid_id_location_id",
                schema: "core",
                table: "AlertLevels",
                columns: new[] { "guid", "id", "location_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AlertLevels_location_id",
                schema: "core",
                table: "AlertLevels",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_alert_level_id",
                schema: "core",
                table: "Alerts",
                column: "alert_level_id");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_core_event_id",
                schema: "core",
                table: "Alerts",
                column: "core_event_id");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_guid_id_location_id",
                schema: "core",
                table: "Alerts",
                columns: new[] { "guid", "id", "location_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_location_id",
                schema: "core",
                table: "Alerts",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_guid_id_created_at_action_username_entity",
                schema: "core",
                table: "AuditTrails",
                columns: new[] { "guid", "id", "created_at", "action", "username", "entity" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_locationid",
                schema: "core",
                table: "AuditTrails",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_BreakGlasses_device_module_id",
                schema: "core",
                table: "BreakGlasses",
                column: "device_module_id");

            migrationBuilder.CreateIndex(
                name: "IX_BreakGlasses_door_id",
                schema: "core",
                table: "BreakGlasses",
                column: "door_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BreakGlasses_guid_id_door_id_device_module_id_slot_no",
                schema: "core",
                table: "BreakGlasses",
                columns: new[] { "guid", "id", "door_id", "device_module_id", "slot_no" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BreakGlasses_locationid",
                schema: "core",
                table: "BreakGlasses",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_Buzzers_device_module_id",
                schema: "core",
                table: "Buzzers",
                column: "device_module_id");

            migrationBuilder.CreateIndex(
                name: "IX_Buzzers_guid_id_door_id_device_module_id_slot_no",
                schema: "core",
                table: "Buzzers",
                columns: new[] { "guid", "id", "door_id", "device_module_id", "slot_no" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Buzzers_locationid",
                schema: "core",
                table: "Buzzers",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_CardFormats_guid_id_location_id",
                schema: "core",
                table: "CardFormats",
                columns: new[] { "guid", "id", "location_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CardFormats_location_id",
                schema: "core",
                table: "CardFormats",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_guid",
                schema: "core",
                table: "Cards",
                column: "guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_locationid",
                schema: "core",
                table: "Cards",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_user_id",
                schema: "core",
                table: "Cards",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_guid",
                schema: "core",
                table: "Companies",
                column: "guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_locationid",
                schema: "core",
                table: "Companies",
                column: "locationid");

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

            migrationBuilder.CreateIndex(
                name: "IX_Countries_guid",
                schema: "core",
                table: "Countries",
                column: "guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_company_id",
                schema: "core",
                table: "Departments",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_guid",
                schema: "core",
                table: "Departments",
                column: "guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_locationid",
                schema: "core",
                table: "Departments",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceModules_device_id_mac_guid_location_id",
                schema: "core",
                table: "DeviceModules",
                columns: new[] { "device_id", "mac", "guid", "location_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceModules_location_id",
                schema: "core",
                table: "DeviceModules",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_guid_mac_vendor",
                schema: "core",
                table: "Devices",
                columns: new[] { "guid", "mac", "vendor" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_location_id",
                schema: "core",
                table: "Devices",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_Doors_buzzer_id",
                schema: "core",
                table: "Doors",
                column: "buzzer_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doors_device_id",
                schema: "core",
                table: "Doors",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "IX_Doors_guid_id_device_id",
                schema: "core",
                table: "Doors",
                columns: new[] { "guid", "id", "device_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doors_location_id",
                schema: "core",
                table: "Doors",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_Events_guid_id_location_id_created_at_timestamp_mac_vendor",
                schema: "core",
                table: "Events",
                columns: new[] { "guid", "id", "location_id", "created_at", "timestamp", "mac", "vendor" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_location_id",
                schema: "core",
                table: "Events",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_Faces_guid",
                schema: "core",
                table: "Faces",
                column: "guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Faces_locationid",
                schema: "core",
                table: "Faces",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_FeaturePermissions_feature_id",
                schema: "core",
                table: "FeaturePermissions",
                column: "feature_id");

            migrationBuilder.CreateIndex(
                name: "IX_FeaturePermissions_guid",
                schema: "core",
                table: "FeaturePermissions",
                column: "guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeaturePermissions_module_permission_id",
                schema: "core",
                table: "FeaturePermissions",
                column: "module_permission_id");

            migrationBuilder.CreateIndex(
                name: "IX_Features_guid",
                schema: "core",
                table: "Features",
                column: "guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Features_module_id",
                schema: "core",
                table: "Features",
                column: "module_id");

            migrationBuilder.CreateIndex(
                name: "IX_GroupComponents_door_id_id_timezone_id_guid",
                schema: "core",
                table: "GroupComponents",
                columns: new[] { "door_id", "id", "timezone_id", "guid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupComponents_group_id",
                schema: "core",
                table: "GroupComponents",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_GroupComponents_locationid",
                schema: "core",
                table: "GroupComponents",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_GroupComponents_timezone_id",
                schema: "core",
                table: "GroupComponents",
                column: "timezone_id");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_guid_id",
                schema: "core",
                table: "Groups",
                columns: new[] { "guid", "id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_location_id",
                schema: "core",
                table: "Groups",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_Holidays_location_id",
                schema: "core",
                table: "Holidays",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_Input_device_module_id",
                schema: "core",
                table: "Input",
                column: "device_module_id");

            migrationBuilder.CreateIndex(
                name: "IX_Input_guid_id_device_module_id_slot_no",
                schema: "core",
                table: "Input",
                columns: new[] { "guid", "id", "device_module_id", "slot_no" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Input_location_id",
                schema: "core",
                table: "Input",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_Intervals_day_id",
                schema: "core",
                table: "Intervals",
                column: "day_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Intervals_location_id",
                schema: "core",
                table: "Intervals",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_Lanes_locationid",
                schema: "core",
                table: "Lanes",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_Lanes_sensor_id",
                schema: "core",
                table: "Lanes",
                column: "sensor_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lanes_turnstile_id",
                schema: "core",
                table: "Lanes",
                column: "turnstile_id");

            migrationBuilder.CreateIndex(
                name: "IX_License_guid",
                schema: "core",
                table: "License",
                column: "guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_License_locationid",
                schema: "core",
                table: "License",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseKey_guid",
                schema: "core",
                table: "LicenseKey",
                column: "guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LicenseKey_locationid",
                schema: "core",
                table: "LicenseKey",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_LicensePlates_guid",
                schema: "core",
                table: "LicensePlates",
                column: "guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LicensePlates_locationid",
                schema: "core",
                table: "LicensePlates",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_country_id",
                schema: "core",
                table: "Locations",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_guid",
                schema: "core",
                table: "Locations",
                column: "guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_locationid",
                schema: "core",
                table: "Locations",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_ModulePermissions_locationid",
                schema: "core",
                table: "ModulePermissions",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_ModulePermissions_module_id",
                schema: "core",
                table: "ModulePermissions",
                column: "module_id");

            migrationBuilder.CreateIndex(
                name: "IX_ModulePermissions_role_id",
                schema: "core",
                table: "ModulePermissions",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_locationid",
                schema: "core",
                table: "Modules",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_OperatorLocations_location_id",
                schema: "core",
                table: "OperatorLocations",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_OperatorLocations_operator_id",
                schema: "core",
                table: "OperatorLocations",
                column: "operator_id");

            migrationBuilder.CreateIndex(
                name: "IX_Operators_guid_id_username",
                schema: "core",
                table: "Operators",
                columns: new[] { "guid", "id", "username" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Operators_locationid",
                schema: "core",
                table: "Operators",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_Operators_role_id",
                schema: "core",
                table: "Operators",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_Output_device_module_id",
                schema: "core",
                table: "Output",
                column: "device_module_id");

            migrationBuilder.CreateIndex(
                name: "IX_Output_guid_id_device_module_id_slot_no",
                schema: "core",
                table: "Output",
                columns: new[] { "guid", "id", "device_module_id", "slot_no" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Output_location_id",
                schema: "core",
                table: "Output",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_Pins_guid",
                schema: "core",
                table: "Pins",
                column: "guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pins_locationid",
                schema: "core",
                table: "Pins",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_department_id",
                schema: "core",
                table: "Positions",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_guid",
                schema: "core",
                table: "Positions",
                column: "guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Positions_locationid",
                schema: "core",
                table: "Positions",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureActions_action_id",
                schema: "core",
                table: "ProcedureActions",
                column: "action_id");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureActions_procedure_id_action_id",
                schema: "core",
                table: "ProcedureActions",
                columns: new[] { "procedure_id", "action_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Procedures_device_id",
                schema: "core",
                table: "Procedures",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "IX_Procedures_guid_id_device_id_location_id",
                schema: "core",
                table: "Procedures",
                columns: new[] { "guid", "id", "device_id", "location_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Procedures_location_id",
                schema: "core",
                table: "Procedures",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_QrCodes_guid",
                schema: "core",
                table: "QrCodes",
                column: "guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QrCodes_locationid",
                schema: "core",
                table: "QrCodes",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_Readers_device_module_id",
                schema: "core",
                table: "Readers",
                column: "device_module_id");

            migrationBuilder.CreateIndex(
                name: "IX_Readers_door_id",
                schema: "core",
                table: "Readers",
                column: "door_id");

            migrationBuilder.CreateIndex(
                name: "IX_Readers_guid_id_door_id_device_module_id_slot_no",
                schema: "core",
                table: "Readers",
                columns: new[] { "guid", "id", "door_id", "device_module_id", "slot_no" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Readers_lane_id",
                schema: "core",
                table: "Readers",
                column: "lane_id");

            migrationBuilder.CreateIndex(
                name: "IX_Readers_locationid",
                schema: "core",
                table: "Readers",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_Relays_device_module_id",
                schema: "core",
                table: "Relays",
                column: "device_module_id");

            migrationBuilder.CreateIndex(
                name: "IX_Relays_door_id",
                schema: "core",
                table: "Relays",
                column: "door_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Relays_guid_id_door_id_device_module_id_slot_no",
                schema: "core",
                table: "Relays",
                columns: new[] { "guid", "id", "door_id", "device_module_id", "slot_no" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Relays_locationid",
                schema: "core",
                table: "Relays",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_Rexes_device_module_id",
                schema: "core",
                table: "Rexes",
                column: "device_module_id");

            migrationBuilder.CreateIndex(
                name: "IX_Rexes_door_id",
                schema: "core",
                table: "Rexes",
                column: "door_id");

            migrationBuilder.CreateIndex(
                name: "IX_Rexes_guid_id_door_id_device_module_id_slot_no",
                schema: "core",
                table: "Rexes",
                columns: new[] { "guid", "id", "door_id", "device_module_id", "slot_no" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rexes_locationid",
                schema: "core",
                table: "Rexes",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_Rexes_timezone_id",
                schema: "core",
                table: "Rexes",
                column: "timezone_id");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_guid",
                schema: "core",
                table: "Roles",
                column: "guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_locationid",
                schema: "core",
                table: "Roles",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_device_module_id",
                schema: "core",
                table: "Sensors",
                column: "device_module_id");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_door_id",
                schema: "core",
                table: "Sensors",
                column: "door_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_guid_id_door_id_device_module_id_slot_no",
                schema: "core",
                table: "Sensors",
                columns: new[] { "guid", "id", "door_id", "device_module_id", "slot_no" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_locationid",
                schema: "core",
                table: "Sensors",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_TimeZoneIntervals_interval_id",
                schema: "core",
                table: "TimeZoneIntervals",
                column: "interval_id");

            migrationBuilder.CreateIndex(
                name: "IX_TimeZoneIntervals_timezone_id",
                schema: "core",
                table: "TimeZoneIntervals",
                column: "timezone_id");

            migrationBuilder.CreateIndex(
                name: "IX_TimeZones_location_id_guid_name",
                schema: "core",
                table: "TimeZones",
                columns: new[] { "location_id", "guid", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TriggerCodes_guid_id",
                schema: "core",
                table: "TriggerCodes",
                columns: new[] { "guid", "id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TriggerCodes_trigger_type_id",
                schema: "core",
                table: "TriggerCodes",
                column: "trigger_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_TriggerSources_guid_id",
                schema: "core",
                table: "TriggerSources",
                columns: new[] { "guid", "id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TriggerTypes_guid_id",
                schema: "core",
                table: "TriggerTypes",
                columns: new[] { "guid", "id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TriggerTypes_trigger_source_id",
                schema: "core",
                table: "TriggerTypes",
                column: "trigger_source_id");

            migrationBuilder.CreateIndex(
                name: "IX_Turnstiles_locationid",
                schema: "core",
                table: "Turnstiles",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_UserAdditionals_user_id",
                schema: "core",
                table: "UserAdditionals",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_group_id",
                schema: "core",
                table: "UserGroups",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_locationid",
                schema: "core",
                table: "UserGroups",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_userid",
                schema: "core",
                table: "UserGroups",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_UserLocations_location_id",
                schema: "core",
                table: "UserLocations",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserLocations_user_id_location_id",
                schema: "core",
                table: "UserLocations",
                columns: new[] { "user_id", "location_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_company_id",
                schema: "core",
                table: "Users",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_department_id",
                schema: "core",
                table: "Users",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_face_id",
                schema: "core",
                table: "Users",
                column: "face_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_guid_username_identification_firstname_lastname_is_de~",
                schema: "core",
                table: "Users",
                columns: new[] { "guid", "username", "identification", "firstname", "lastname", "is_default" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_license_plate_id",
                schema: "core",
                table: "Users",
                column: "license_plate_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_locationid",
                schema: "core",
                table: "Users",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_Users_pin_id",
                schema: "core",
                table: "Users",
                column: "pin_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_position_id",
                schema: "core",
                table: "Users",
                column: "position_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_qr_code_id",
                schema: "core",
                table: "Users",
                column: "qr_code_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_role_id",
                schema: "core",
                table: "Users",
                column: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdapterEvents",
                schema: "core");

            migrationBuilder.DropTable(
                name: "AlertComponents",
                schema: "core");

            migrationBuilder.DropTable(
                name: "AlertEvents",
                schema: "core");

            migrationBuilder.DropTable(
                name: "AuditTrails",
                schema: "core");

            migrationBuilder.DropTable(
                name: "BreakGlasses",
                schema: "core");

            migrationBuilder.DropTable(
                name: "CardFormats",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Cards",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Events",
                schema: "core");

            migrationBuilder.DropTable(
                name: "ExceptionEvent",
                schema: "core");

            migrationBuilder.DropTable(
                name: "FeaturePermissions",
                schema: "core");

            migrationBuilder.DropTable(
                name: "GroupComponents",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Holidays",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Input",
                schema: "core");

            migrationBuilder.DropTable(
                name: "License",
                schema: "core");

            migrationBuilder.DropTable(
                name: "LicenseKey",
                schema: "core");

            migrationBuilder.DropTable(
                name: "OperatorLocations",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Output",
                schema: "core");

            migrationBuilder.DropTable(
                name: "ProcedureActions",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Readers",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Relays",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Rexes",
                schema: "core");

            migrationBuilder.DropTable(
                name: "TimeZoneIntervals",
                schema: "core");

            migrationBuilder.DropTable(
                name: "TriggerCodes",
                schema: "core");

            migrationBuilder.DropTable(
                name: "UserAdditionals",
                schema: "core");

            migrationBuilder.DropTable(
                name: "UserGroups",
                schema: "core");

            migrationBuilder.DropTable(
                name: "UserLocations",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Alerts",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Features",
                schema: "core");

            migrationBuilder.DropTable(
                name: "ModulePermissions",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Operators",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Action",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Procedures",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Lanes",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Intervals",
                schema: "core");

            migrationBuilder.DropTable(
                name: "TimeZones",
                schema: "core");

            migrationBuilder.DropTable(
                name: "TriggerTypes",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Groups",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "core");

            migrationBuilder.DropTable(
                name: "AlertLevels",
                schema: "core");

            migrationBuilder.DropTable(
                name: "CoreEvents",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Modules",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Sensors",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Turnstiles",
                schema: "core");

            migrationBuilder.DropTable(
                name: "DayInWeeks",
                schema: "core");

            migrationBuilder.DropTable(
                name: "TriggerSources",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Faces",
                schema: "core");

            migrationBuilder.DropTable(
                name: "LicensePlates",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Pins",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Positions",
                schema: "core");

            migrationBuilder.DropTable(
                name: "QrCodes",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "core");

            migrationBuilder.DropTable(
                name: "CoreEntities",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Doors",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Departments",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Buzzers",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Companies",
                schema: "core");

            migrationBuilder.DropTable(
                name: "DeviceModules",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Devices",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Locations",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Countries",
                schema: "core");
        }
    }
}
