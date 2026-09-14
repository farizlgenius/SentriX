using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeleteAeroDriverSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AeroDriverSettings",
                schema: "core");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AeroDriverSettings",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    apb_location = table.Column<bool>(type: "boolean", nullable: false),
                    c_port = table.Column<int>(type: "integer", nullable: false),
                    c_type = table.Column<int>(type: "integer", nullable: false),
                    card_id_size = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    duress_const_digit = table.Column<int>(type: "integer", nullable: false),
                    escort_timeout = table.Column<int>(type: "integer", nullable: false),
                    gmt_offset = table.Column<int>(type: "integer", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_daylight_saving = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    issue_code_bit = table.Column<int>(type: "integer", nullable: false),
                    multi_card_timeout = table.Column<int>(type: "integer", nullable: false),
                    n_acr = table.Column<int>(type: "integer", nullable: false),
                    n_alvl = table.Column<int>(type: "integer", nullable: false),
                    n_alvl_per_card = table.Column<int>(type: "integer", nullable: false),
                    n_cards = table.Column<int>(type: "integer", nullable: false),
                    n_cp = table.Column<int>(type: "integer", nullable: false),
                    n_hol = table.Column<int>(type: "integer", nullable: false),
                    n_mp = table.Column<int>(type: "integer", nullable: false),
                    n_mpg = table.Column<int>(type: "integer", nullable: false),
                    n_msp1_port = table.Column<int>(type: "integer", nullable: false),
                    n_port = table.Column<int>(type: "integer", nullable: false),
                    n_proc = table.Column<int>(type: "integer", nullable: false),
                    n_scps = table.Column<int>(type: "integer", nullable: false),
                    n_sio = table.Column<int>(type: "integer", nullable: false),
                    n_tran_limit = table.Column<int>(type: "integer", nullable: false),
                    n_trasaction = table.Column<int>(type: "integer", nullable: false),
                    n_trgr = table.Column<int>(type: "integer", nullable: false),
                    n_tz = table.Column<int>(type: "integer", nullable: false),
                    pin_digit = table.Column<int>(type: "integer", nullable: false),
                    pin_duress_mode = table.Column<int>(type: "integer", nullable: false),
                    store_act_date = table.Column<int>(type: "integer", nullable: false),
                    store_deact_date = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    used_limit = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AeroDriverSettings", x => x.id);
                });

            migrationBuilder.InsertData(
                schema: "core",
                table: "AeroDriverSettings",
                columns: new[] { "id", "apb_location", "c_port", "c_type", "card_id_size", "duress_const_digit", "escort_timeout", "gmt_offset", "is_active", "is_daylight_saving", "is_default", "issue_code_bit", "multi_card_timeout", "n_acr", "n_alvl", "n_alvl_per_card", "n_cards", "n_cp", "n_hol", "n_mp", "n_mpg", "n_msp1_port", "n_port", "n_proc", "n_scps", "n_sio", "n_tran_limit", "n_trasaction", "n_trgr", "n_tz", "pin_digit", "pin_duress_mode", "store_act_date", "store_deact_date", "used_limit" },
                values: new object[] { 1, true, 3333, 7, 0, 5, 15, -25200, true, false, false, 1, 15, 64, 32000, 8, 200000, 388, 255, 615, 128, 3, 1024, 1024, 1024, 33, 60000, 60000, 1024, 255, 6, 2, 2, 2, true });
        }
    }
}
