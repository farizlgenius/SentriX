using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Setting.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "setting");

            migrationBuilder.CreateTable(
                name: "AeroDriverSettings",
                schema: "setting",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    n_port = table.Column<int>(type: "integer", nullable: false),
                    n_scps = table.Column<int>(type: "integer", nullable: false),
                    c_type = table.Column<int>(type: "integer", nullable: false),
                    c_port = table.Column<int>(type: "integer", nullable: false),
                    n_msp1_port = table.Column<int>(type: "integer", nullable: false),
                    n_trasaction = table.Column<int>(type: "integer", nullable: false),
                    n_sio = table.Column<int>(type: "integer", nullable: false),
                    n_mp = table.Column<int>(type: "integer", nullable: false),
                    n_cp = table.Column<int>(type: "integer", nullable: false),
                    n_acr = table.Column<int>(type: "integer", nullable: false),
                    n_alvl = table.Column<int>(type: "integer", nullable: false),
                    n_trgr = table.Column<int>(type: "integer", nullable: false),
                    n_proc = table.Column<int>(type: "integer", nullable: false),
                    gmt_offset = table.Column<int>(type: "integer", nullable: false),
                    is_daylight_saving = table.Column<bool>(type: "boolean", nullable: false),
                    n_tz = table.Column<int>(type: "integer", nullable: false),
                    n_hol = table.Column<int>(type: "integer", nullable: false),
                    n_mpg = table.Column<int>(type: "integer", nullable: false),
                    n_tran_limit = table.Column<int>(type: "integer", nullable: false),
                    n_cards = table.Column<int>(type: "integer", nullable: false),
                    n_alvl_per_card = table.Column<int>(type: "integer", nullable: false),
                    pin_duress_mode = table.Column<int>(type: "integer", nullable: false),
                    duress_const_digit = table.Column<int>(type: "integer", nullable: false),
                    card_id_size = table.Column<int>(type: "integer", nullable: false),
                    pin_digit = table.Column<int>(type: "integer", nullable: false),
                    issue_code_bit = table.Column<int>(type: "integer", nullable: false),
                    apb_location = table.Column<bool>(type: "boolean", nullable: false),
                    store_act_date = table.Column<int>(type: "integer", nullable: false),
                    store_deact_date = table.Column<int>(type: "integer", nullable: false),
                    used_limit = table.Column<bool>(type: "boolean", nullable: false),
                    apb_time = table.Column<bool>(type: "boolean", nullable: false),
                    host_timeout = table.Column<int>(type: "integer", nullable: false),
                    escort_timeout = table.Column<int>(type: "integer", nullable: false),
                    multi_card_timeout = table.Column<int>(type: "integer", nullable: false),
                    max_elalvl = table.Column<int>(type: "integer", nullable: false),
                    max_floor_per_acr = table.Column<int>(type: "integer", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AeroDriverSettings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PasswordRules",
                schema: "setting",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    len = table.Column<int>(type: "integer", nullable: false),
                    is_digit = table.Column<bool>(type: "boolean", nullable: false),
                    is_lower = table.Column<bool>(type: "boolean", nullable: false),
                    is_symbol = table.Column<bool>(type: "boolean", nullable: false),
                    is_upper = table.Column<bool>(type: "boolean", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordRules", x => x.id);
                    table.UniqueConstraint("AK_PasswordRules_guid", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "WeakPasswords",
                schema: "setting",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pattern = table.Column<string>(type: "text", nullable: false),
                    password_rule_guid = table.Column<Guid>(type: "uuid", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeakPasswords", x => x.id);
                    table.ForeignKey(
                        name: "FK_WeakPasswords_PasswordRules_password_rule_guid",
                        column: x => x.password_rule_guid,
                        principalSchema: "setting",
                        principalTable: "PasswordRules",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "setting",
                table: "AeroDriverSettings",
                columns: new[] { "id", "apb_location", "apb_time", "c_port", "c_type", "card_id_size", "duress_const_digit", "escort_timeout", "gmt_offset", "host_timeout", "is_active", "is_daylight_saving", "is_default", "issue_code_bit", "max_elalvl", "max_floor_per_acr", "multi_card_timeout", "n_acr", "n_alvl", "n_alvl_per_card", "n_cards", "n_cp", "n_hol", "n_mp", "n_mpg", "n_msp1_port", "n_port", "n_proc", "n_scps", "n_sio", "n_tran_limit", "n_trasaction", "n_trgr", "n_tz", "pin_digit", "pin_duress_mode", "store_act_date", "store_deact_date", "used_limit" },
                values: new object[] { 1, true, true, 3333, 7, 4, 5, 15, -25200, 5, true, false, false, 1, 256, 128, 15, 64, 32000, 8, 200000, 388, 255, 615, 128, 3, 1024, 1024, 1024, 33, 60000, 60000, 1024, 255, 6, 2, 2, 2, true });

            migrationBuilder.InsertData(
                schema: "setting",
                table: "PasswordRules",
                columns: new[] { "id", "guid", "is_active", "is_default", "is_digit", "is_lower", "is_symbol", "is_upper", "len" },
                values: new object[] { 1, new Guid("ae243161-6067-47d0-8bcc-1990388bb6e6"), true, false, false, false, false, false, 4 });

            migrationBuilder.InsertData(
                schema: "setting",
                table: "WeakPasswords",
                columns: new[] { "id", "guid", "is_active", "is_default", "password_rule_guid", "pattern" },
                values: new object[,]
                {
                    { 1, new Guid("f371dff7-fa82-4a0f-95ba-f24954cf73f7"), true, false, new Guid("ae243161-6067-47d0-8bcc-1990388bb6e6"), "P@ssw0rd" },
                    { 2, new Guid("c347ec2d-17e7-4048-82df-9b1b65730669"), true, false, new Guid("ae243161-6067-47d0-8bcc-1990388bb6e6"), "password" },
                    { 3, new Guid("b3124c81-3c54-46b3-bafd-a945854fc946"), true, false, new Guid("ae243161-6067-47d0-8bcc-1990388bb6e6"), "admin" },
                    { 4, new Guid("df75695c-6821-49ad-a857-60e1b0763329"), true, false, new Guid("ae243161-6067-47d0-8bcc-1990388bb6e6"), "123456" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeakPasswords_password_rule_guid",
                schema: "setting",
                table: "WeakPasswords",
                column: "password_rule_guid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AeroDriverSettings",
                schema: "setting");

            migrationBuilder.DropTable(
                name: "WeakPasswords",
                schema: "setting");

            migrationBuilder.DropTable(
                name: "PasswordRules",
                schema: "setting");
        }
    }
}
