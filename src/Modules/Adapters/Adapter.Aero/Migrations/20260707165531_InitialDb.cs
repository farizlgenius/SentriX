using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Adapter.Aero.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "aero");

            migrationBuilder.CreateTable(
                name: "AccessDatabaseSpecifications",
                schema: "aero",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    scp_id = table.Column<short>(type: "smallint", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    n_card = table.Column<short>(type: "smallint", nullable: false),
                    n_alvl = table.Column<short>(type: "smallint", nullable: false),
                    n_pin_digits = table.Column<short>(type: "smallint", nullable: false),
                    b_issue_code = table.Column<short>(type: "smallint", nullable: false),
                    b_apb_location = table.Column<short>(type: "smallint", nullable: false),
                    b_act_date = table.Column<short>(type: "smallint", nullable: false),
                    b_deact_date = table.Column<short>(type: "smallint", nullable: false),
                    b_vacation_date = table.Column<short>(type: "smallint", nullable: false),
                    b_upgrade_date = table.Column<short>(type: "smallint", nullable: false),
                    b_user_level = table.Column<short>(type: "smallint", nullable: false),
                    b_use_limit = table.Column<short>(type: "smallint", nullable: false),
                    b_support_time_apb = table.Column<short>(type: "smallint", nullable: false),
                    n_tz = table.Column<short>(type: "smallint", nullable: false),
                    b_asset_group = table.Column<short>(type: "smallint", nullable: false),
                    n_host_response_timeout = table.Column<short>(type: "smallint", nullable: false),
                    n_alvl_use4arg = table.Column<short>(type: "smallint", nullable: false),
                    n_escort_timeout = table.Column<short>(type: "smallint", nullable: false),
                    n_multi_card_timeout = table.Column<short>(type: "smallint", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessDatabaseSpecifications", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Aeros",
                schema: "aero",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    scp_id = table.Column<int>(type: "integer", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aeros", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DoorModes",
                schema: "aero",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    label = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoorModes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ElevatorAccessLevelSpecifications",
                schema: "aero",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    scp_id = table.Column<short>(type: "smallint", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    max_ealvl = table.Column<short>(type: "smallint", nullable: false),
                    max_floors = table.Column<short>(type: "smallint", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElevatorAccessLevelSpecifications", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "RelayModes",
                schema: "aero",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    label = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelayModes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ScpDeviceSpecifications",
                schema: "aero",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    scp_id = table.Column<short>(type: "smallint", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    n_msp1_port = table.Column<short>(type: "smallint", nullable: false),
                    n_transcations = table.Column<int>(type: "integer", nullable: false),
                    n_sio = table.Column<short>(type: "smallint", nullable: false),
                    n_mp = table.Column<short>(type: "smallint", nullable: false),
                    n_cp = table.Column<short>(type: "smallint", nullable: false),
                    n_acr = table.Column<short>(type: "smallint", nullable: false),
                    n_alvl = table.Column<short>(type: "smallint", nullable: false),
                    n_trgr = table.Column<short>(type: "smallint", nullable: false),
                    n_proc = table.Column<short>(type: "smallint", nullable: false),
                    gmt_offset = table.Column<short>(type: "smallint", nullable: false),
                    n_dst_id = table.Column<short>(type: "smallint", nullable: false),
                    n_tz = table.Column<short>(type: "smallint", nullable: false),
                    n_hol = table.Column<short>(type: "smallint", nullable: false),
                    n_mpg = table.Column<short>(type: "smallint", nullable: false),
                    n_tran_limit = table.Column<int>(type: "integer", nullable: false),
                    n_oper_mode = table.Column<short>(type: "smallint", nullable: false),
                    oper_type = table.Column<short>(type: "smallint", nullable: false),
                    n_language = table.Column<short>(type: "smallint", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScpDeviceSpecifications", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TimezoneModes",
                schema: "aero",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    label = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimezoneModes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ControlPointConfiguration",
                schema: "aero",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    aero_id = table.Column<int>(type: "integer", nullable: false),
                    cp_number = table.Column<short>(type: "smallint", nullable: false),
                    sio_number = table.Column<short>(type: "smallint", nullable: false),
                    op_number = table.Column<short>(type: "smallint", nullable: false),
                    dflt_pulse = table.Column<short>(type: "smallint", nullable: false),
                    output_id = table.Column<int>(type: "integer", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlPointConfiguration", x => x.id);
                    table.ForeignKey(
                        name: "FK_ControlPointConfiguration_Aeros_aero_id",
                        column: x => x.aero_id,
                        principalSchema: "aero",
                        principalTable: "Aeros",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DriverConfiguration",
                schema: "aero",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    msp1_number = table.Column<short>(type: "smallint", nullable: false),
                    port_number = table.Column<short>(type: "smallint", nullable: false),
                    baudrate = table.Column<short>(type: "smallint", nullable: false),
                    reply_time = table.Column<short>(type: "smallint", nullable: false),
                    n_protocol = table.Column<short>(type: "smallint", nullable: false),
                    n_dialect = table.Column<short>(type: "smallint", nullable: false),
                    aero_id = table.Column<int>(type: "integer", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverConfiguration", x => x.id);
                    table.ForeignKey(
                        name: "FK_DriverConfiguration_Aeros_aero_id",
                        column: x => x.aero_id,
                        principalSchema: "aero",
                        principalTable: "Aeros",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InputPointSpecification",
                schema: "aero",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    aero_id = table.Column<int>(type: "integer", nullable: false),
                    sio_number = table.Column<short>(type: "smallint", nullable: false),
                    input_number = table.Column<short>(type: "smallint", nullable: false),
                    icvt_num = table.Column<short>(type: "smallint", nullable: false),
                    debounce = table.Column<short>(type: "smallint", nullable: false),
                    hold_time = table.Column<short>(type: "smallint", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InputPointSpecification", x => x.id);
                    table.ForeignKey(
                        name: "FK_InputPointSpecification_Aeros_aero_id",
                        column: x => x.aero_id,
                        principalSchema: "aero",
                        principalTable: "Aeros",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OutputPointSpecification",
                schema: "aero",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    aero_id = table.Column<int>(type: "integer", nullable: false),
                    sio_number = table.Column<int>(type: "integer", nullable: false),
                    output = table.Column<short>(type: "smallint", nullable: false),
                    mode = table.Column<short>(type: "smallint", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutputPointSpecification", x => x.id);
                    table.ForeignKey(
                        name: "FK_OutputPointSpecification_Aeros_aero_id",
                        column: x => x.aero_id,
                        principalSchema: "aero",
                        principalTable: "Aeros",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SioPanelConfiguration",
                schema: "aero",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    aero_id = table.Column<int>(type: "integer", nullable: false),
                    sio_number = table.Column<short>(type: "smallint", nullable: false),
                    n_inputs = table.Column<short>(type: "smallint", nullable: false),
                    n_outputs = table.Column<short>(type: "smallint", nullable: false),
                    n_readers = table.Column<short>(type: "smallint", nullable: false),
                    model = table.Column<short>(type: "smallint", nullable: false),
                    enable = table.Column<short>(type: "smallint", nullable: false),
                    port = table.Column<short>(type: "smallint", nullable: false),
                    address = table.Column<short>(type: "smallint", nullable: false),
                    emax = table.Column<short>(type: "smallint", nullable: false),
                    flags = table.Column<short>(type: "smallint", nullable: false),
                    n_sio_next_in = table.Column<short>(type: "smallint", nullable: false),
                    n_sio_next_out = table.Column<short>(type: "smallint", nullable: false),
                    n_sio_next_rdr = table.Column<short>(type: "smallint", nullable: false),
                    module_id = table.Column<int>(type: "integer", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SioPanelConfiguration", x => x.id);
                    table.ForeignKey(
                        name: "FK_SioPanelConfiguration_Aeros_aero_id",
                        column: x => x.aero_id,
                        principalSchema: "aero",
                        principalTable: "Aeros",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "aero",
                table: "AccessDatabaseSpecifications",
                columns: new[] { "id", "b_act_date", "b_apb_location", "b_asset_group", "b_deact_date", "b_issue_code", "b_support_time_apb", "b_upgrade_date", "b_use_limit", "b_user_level", "b_vacation_date", "component_id", "is_active", "is_default", "location_id", "mac", "n_alvl", "n_alvl_use4arg", "n_card", "n_escort_timeout", "n_host_response_timeout", "n_multi_card_timeout", "n_pin_digits", "n_tz", "scp_id" },
                values: new object[] { 1, (short)2, (short)1, (short)0, (short)2, (short)1, (short)1, (short)1, (short)1, (short)7, (short)1, (short)0, true, false, 0, "", (short)8, (short)0, (short)1000, (short)15, (short)5, (short)15, (short)324, (short)64, (short)0 });

            migrationBuilder.InsertData(
                schema: "aero",
                table: "DoorModes",
                columns: new[] { "id", "description", "label", "value" },
                values: new object[,]
                {
                    { 1, "Single reader, controlling the door", "Single", 0 },
                    { 2, "In/Out Reader", "Dual", 1 },
                    { 3, "Turnstile Reader", "Turnstile", 3 },
                    { 4, "Elevator, no floor select feedback", "Elevator no floor", 4 },
                    { 5, "Elevator with floor select feedback", "Elevator with floor", 5 }
                });

            migrationBuilder.InsertData(
                schema: "aero",
                table: "ElevatorAccessLevelSpecifications",
                columns: new[] { "id", "component_id", "is_active", "is_default", "location_id", "mac", "max_ealvl", "max_floors", "scp_id" },
                values: new object[] { 1, (short)0, true, false, 0, "", (short)256, (short)128, (short)0 });

            migrationBuilder.InsertData(
                schema: "aero",
                table: "RelayModes",
                columns: new[] { "id", "label", "value" },
                values: new object[,]
                {
                    { 1, "Normal / No Change", 0 },
                    { 2, "Inverted / No Change", 1 },
                    { 3, "Normal / Inactive", 16 },
                    { 4, "Inverted / Inactive", 17 },
                    { 5, "Normal / Active", 32 },
                    { 6, "Inverted / Active", 33 }
                });

            migrationBuilder.InsertData(
                schema: "aero",
                table: "ScpDeviceSpecifications",
                columns: new[] { "id", "component_id", "gmt_offset", "is_active", "is_default", "location_id", "mac", "n_acr", "n_alvl", "n_cp", "n_dst_id", "n_hol", "n_language", "n_mp", "n_mpg", "n_msp1_port", "n_oper_mode", "n_proc", "n_sio", "n_tran_limit", "n_transcations", "n_trgr", "n_tz", "oper_type", "scp_id" },
                values: new object[] { 1, (short)0, (short)-25200, true, false, 0, "", (short)64, (short)32000, (short)388, (short)0, (short)255, (short)0, (short)615, (short)128, (short)3, (short)0, (short)1024, (short)33, 60000, 60000, (short)1024, (short)255, (short)1, (short)0 });

            migrationBuilder.InsertData(
                schema: "aero",
                table: "TimezoneModes",
                columns: new[] { "id", "description", "label", "value" },
                values: new object[,]
                {
                    { 1, "The time zone is always inactive, regardless of the time zone intervals specified or the holidays in effect.", "Off", 0 },
                    { 2, "The time zone is always active, regardless of the time zone intervals specified or the holidays in effect.", "On", 1 },
                    { 3, "The Time Zone state is decided using either the Day Mask or the Holiday Mask. If the current day is specified as a Holiday, the state relies only on whether the Holiday Mask Flag for that Holiday is set (if today is Holiday 1, and the Holiday Mask sets flag H1, then the state is active. If today is Holiday 1, and the Holiday Mask does not have flag H1 set, then the state is inactive). Holidays override the standard accessibility rules. If the current day is not specified as a Holiday, the Time Zone is active or inactive depending on whether the current day/time falls within the Day Mask. If Day Mask is M-F, 8-5, the Time Zone is active during those times, and inactive on the weekend and outside working hours.", "Scan", 2 },
                    { 4, "Scan time zone interval list and apply only if the date string in expTest matches the current date", "One Time Event", 3 },
                    { 5, "This mode is similar to mode 2, but instead of only checking the Holiday Mask if it is a Holiday, and only checking the Day Mask if not, this mode checks both. If it is not a Holiday, this mode functions normally, only checking the Day Mask. If it is a Holiday, this mode performs a logical OR on the Holiday and Day Masks. If either or both are active, the Time Zone is active, otherwise if neither is active, the Time Zone is inactive.", "Scan, Always Honor Day of Week (OR)", 4 },
                    { 6, "This mode is similar to mode 4, but it performs a logical AND instead of a logical OR. If it is not a Holiday, this mode functions normally, only checking the Day Mask. If it is a Holiday, this mode is only active if BOTH the Day Mask and Holiday Mask are active. If either one is inactive, the entire Time Zone is inactive.", "Scan, Always Honor Day of Week (AND) ", 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ControlPointConfiguration_aero_id",
                schema: "aero",
                table: "ControlPointConfiguration",
                column: "aero_id");

            migrationBuilder.CreateIndex(
                name: "IX_DriverConfiguration_aero_id",
                schema: "aero",
                table: "DriverConfiguration",
                column: "aero_id");

            migrationBuilder.CreateIndex(
                name: "IX_InputPointSpecification_aero_id",
                schema: "aero",
                table: "InputPointSpecification",
                column: "aero_id");

            migrationBuilder.CreateIndex(
                name: "IX_OutputPointSpecification_aero_id",
                schema: "aero",
                table: "OutputPointSpecification",
                column: "aero_id");

            migrationBuilder.CreateIndex(
                name: "IX_SioPanelConfiguration_aero_id",
                schema: "aero",
                table: "SioPanelConfiguration",
                column: "aero_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessDatabaseSpecifications",
                schema: "aero");

            migrationBuilder.DropTable(
                name: "ControlPointConfiguration",
                schema: "aero");

            migrationBuilder.DropTable(
                name: "DoorModes",
                schema: "aero");

            migrationBuilder.DropTable(
                name: "DriverConfiguration",
                schema: "aero");

            migrationBuilder.DropTable(
                name: "ElevatorAccessLevelSpecifications",
                schema: "aero");

            migrationBuilder.DropTable(
                name: "InputPointSpecification",
                schema: "aero");

            migrationBuilder.DropTable(
                name: "OutputPointSpecification",
                schema: "aero");

            migrationBuilder.DropTable(
                name: "RelayModes",
                schema: "aero");

            migrationBuilder.DropTable(
                name: "ScpDeviceSpecifications",
                schema: "aero");

            migrationBuilder.DropTable(
                name: "SioPanelConfiguration",
                schema: "aero");

            migrationBuilder.DropTable(
                name: "TimezoneModes",
                schema: "aero");

            migrationBuilder.DropTable(
                name: "Aeros",
                schema: "aero");
        }
    }
}
