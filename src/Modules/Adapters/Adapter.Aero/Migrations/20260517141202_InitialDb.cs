using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

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
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
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
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aeros", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "CreateChannels",
                schema: "aero",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    n_channel_id = table.Column<short>(type: "smallint", nullable: false),
                    c_type = table.Column<short>(type: "smallint", nullable: false),
                    c_port = table.Column<short>(type: "smallint", nullable: false),
                    baudrate = table.Column<short>(type: "smallint", nullable: false),
                    timer_1 = table.Column<short>(type: "smallint", nullable: false),
                    timer_2 = table.Column<short>(type: "smallint", nullable: false),
                    c_model_id = table.Column<short>(type: "smallint", nullable: false),
                    c_rts_mode = table.Column<short>(type: "smallint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreateChannels", x => x.id);
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
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElevatorAccessLevelSpecifications", x => x.id);
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
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScpDeviceSpecifications", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SystemLevelSpecifications",
                schema: "aero",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    n_ports = table.Column<short>(type: "smallint", nullable: false),
                    n_scps = table.Column<short>(type: "smallint", nullable: false),
                    n_timezones = table.Column<short>(type: "smallint", nullable: false),
                    n_holidays = table.Column<short>(type: "smallint", nullable: false),
                    b_direct_mode = table.Column<short>(type: "smallint", nullable: false),
                    debug_rq = table.Column<short>(type: "smallint", nullable: false),
                    n_debug_arg = table.Column<short>(type: "smallint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemLevelSpecifications", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "WriterAudits",
                schema: "aero",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mac = table.Column<string>(type: "text", nullable: false),
                    scp_id = table.Column<int>(type: "integer", nullable: false),
                    command_code = table.Column<int>(type: "integer", nullable: false),
                    command = table.Column<string>(type: "text", nullable: false),
                    tag = table.Column<int>(type: "integer", nullable: false),
                    send_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    received_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    body = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    is_nak = table.Column<bool>(type: "boolean", nullable: false),
                    reason = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WriterAudits", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ControlPointConfigurations",
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
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlPointConfigurations", x => x.id);
                    table.ForeignKey(
                        name: "FK_ControlPointConfigurations_Aeros_aero_id",
                        column: x => x.aero_id,
                        principalSchema: "aero",
                        principalTable: "Aeros",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DriverConfigurations",
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
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverConfigurations", x => x.id);
                    table.ForeignKey(
                        name: "FK_DriverConfigurations_Aeros_aero_id",
                        column: x => x.aero_id,
                        principalSchema: "aero",
                        principalTable: "Aeros",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InputPointSpecifications",
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
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InputPointSpecifications", x => x.id);
                    table.ForeignKey(
                        name: "FK_InputPointSpecifications_Aeros_aero_id",
                        column: x => x.aero_id,
                        principalSchema: "aero",
                        principalTable: "Aeros",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OutputPointSpecifications",
                schema: "aero",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    aero_id = table.Column<int>(type: "integer", nullable: false),
                    sio_number = table.Column<int>(type: "integer", nullable: false),
                    output = table.Column<short>(type: "smallint", nullable: false),
                    mode = table.Column<short>(type: "smallint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutputPointSpecifications", x => x.id);
                    table.ForeignKey(
                        name: "FK_OutputPointSpecifications_Aeros_aero_id",
                        column: x => x.aero_id,
                        principalSchema: "aero",
                        principalTable: "Aeros",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SioPanelConfigurations",
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
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SioPanelConfigurations", x => x.id);
                    table.ForeignKey(
                        name: "FK_SioPanelConfigurations_Aeros_aero_id",
                        column: x => x.aero_id,
                        principalSchema: "aero",
                        principalTable: "Aeros",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "aero",
                table: "AccessDatabaseSpecifications",
                columns: new[] { "id", "b_act_date", "b_apb_location", "b_asset_group", "b_deact_date", "b_issue_code", "b_support_time_apb", "b_upgrade_date", "b_use_limit", "b_user_level", "b_vacation_date", "is_active", "mac", "n_alvl", "n_alvl_use4arg", "n_card", "n_escort_timeout", "n_host_response_timeout", "n_multi_card_timeout", "n_pin_digits", "n_tz", "scp_id" },
                values: new object[] { 1, (short)2, (short)1, (short)0, (short)2, (short)1, (short)1, (short)1, (short)1, (short)7, (short)1, true, "", (short)8, (short)0, (short)1000, (short)15, (short)5, (short)15, (short)324, (short)64, (short)0 });

            migrationBuilder.InsertData(
                schema: "aero",
                table: "CreateChannels",
                columns: new[] { "id", "baudrate", "c_model_id", "c_port", "c_rts_mode", "c_type", "is_active", "n_channel_id", "timer_1", "timer_2" },
                values: new object[] { 1, (short)0, (short)0, (short)0, (short)0, (short)7, true, (short)1, (short)3000, (short)0 });

            migrationBuilder.InsertData(
                schema: "aero",
                table: "ElevatorAccessLevelSpecifications",
                columns: new[] { "id", "is_active", "mac", "max_ealvl", "max_floors", "scp_id" },
                values: new object[] { 1, true, "", (short)256, (short)128, (short)0 });

            migrationBuilder.InsertData(
                schema: "aero",
                table: "ScpDeviceSpecifications",
                columns: new[] { "id", "gmt_offset", "is_active", "mac", "n_acr", "n_alvl", "n_cp", "n_dst_id", "n_hol", "n_language", "n_mp", "n_mpg", "n_msp1_port", "n_oper_mode", "n_proc", "n_sio", "n_tran_limit", "n_transcations", "n_trgr", "n_tz", "oper_type", "scp_id" },
                values: new object[] { 1, (short)-25200, true, "", (short)64, (short)32000, (short)388, (short)0, (short)255, (short)0, (short)615, (short)128, (short)3, (short)0, (short)1024, (short)33, 60000, 60000, (short)1024, (short)255, (short)1, (short)0 });

            migrationBuilder.InsertData(
                schema: "aero",
                table: "SystemLevelSpecifications",
                columns: new[] { "id", "b_direct_mode", "debug_rq", "is_active", "n_debug_arg", "n_holidays", "n_ports", "n_scps", "n_timezones" },
                values: new object[] { 1, (short)1, (short)0, true, (short)0, (short)0, (short)1024, (short)1024, (short)0 });

            migrationBuilder.CreateIndex(
                name: "IX_ControlPointConfigurations_aero_id",
                schema: "aero",
                table: "ControlPointConfigurations",
                column: "aero_id");

            migrationBuilder.CreateIndex(
                name: "IX_DriverConfigurations_aero_id",
                schema: "aero",
                table: "DriverConfigurations",
                column: "aero_id");

            migrationBuilder.CreateIndex(
                name: "IX_InputPointSpecifications_aero_id",
                schema: "aero",
                table: "InputPointSpecifications",
                column: "aero_id");

            migrationBuilder.CreateIndex(
                name: "IX_OutputPointSpecifications_aero_id",
                schema: "aero",
                table: "OutputPointSpecifications",
                column: "aero_id");

            migrationBuilder.CreateIndex(
                name: "IX_SioPanelConfigurations_aero_id",
                schema: "aero",
                table: "SioPanelConfigurations",
                column: "aero_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessDatabaseSpecifications",
                schema: "aero");

            migrationBuilder.DropTable(
                name: "ControlPointConfigurations",
                schema: "aero");

            migrationBuilder.DropTable(
                name: "CreateChannels",
                schema: "aero");

            migrationBuilder.DropTable(
                name: "DriverConfigurations",
                schema: "aero");

            migrationBuilder.DropTable(
                name: "ElevatorAccessLevelSpecifications",
                schema: "aero");

            migrationBuilder.DropTable(
                name: "InputPointSpecifications",
                schema: "aero");

            migrationBuilder.DropTable(
                name: "OutputPointSpecifications",
                schema: "aero");

            migrationBuilder.DropTable(
                name: "ScpDeviceSpecifications",
                schema: "aero");

            migrationBuilder.DropTable(
                name: "SioPanelConfigurations",
                schema: "aero");

            migrationBuilder.DropTable(
                name: "SystemLevelSpecifications",
                schema: "aero");

            migrationBuilder.DropTable(
                name: "WriterAudits",
                schema: "aero");

            migrationBuilder.DropTable(
                name: "Aeros",
                schema: "aero");
        }
    }
}
