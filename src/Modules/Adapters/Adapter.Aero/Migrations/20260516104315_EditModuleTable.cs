using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Adapter.Aero.Migrations
{
    /// <inheritdoc />
    public partial class EditModuleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "module_id",
                schema: "aero",
                table: "SioPanelConfigurations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ControlPointConfigurations",
                schema: "aero",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    scp_number = table.Column<int>(type: "integer", nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "OutputPointSpecifications",
                schema: "aero",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    scp_id = table.Column<int>(type: "integer", nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "Scps",
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
                    table.PrimaryKey("PK_Scps", x => x.id);
                });

            migrationBuilder.UpdateData(
                schema: "aero",
                table: "SioPanelConfigurations",
                keyColumn: "id",
                keyValue: 1,
                column: "module_id",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ControlPointConfigurations",
                schema: "aero");

            migrationBuilder.DropTable(
                name: "OutputPointSpecifications",
                schema: "aero");

            migrationBuilder.DropTable(
                name: "Scps",
                schema: "aero");

            migrationBuilder.DropColumn(
                name: "module_id",
                schema: "aero",
                table: "SioPanelConfigurations");
        }
    }
}
