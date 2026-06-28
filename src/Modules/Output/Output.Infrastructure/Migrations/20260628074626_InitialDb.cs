using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Output.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "output");

            migrationBuilder.CreateTable(
                name: "OutputDriveModes",
                schema: "output",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    label = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<short>(type: "smallint", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutputDriveModes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "OutputModes",
                schema: "output",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    label = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<short>(type: "smallint", nullable: false),
                    drive = table.Column<short>(type: "smallint", nullable: false),
                    offline = table.Column<short>(type: "smallint", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutputModes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "OutputOfflineModes",
                schema: "output",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    label = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<short>(type: "smallint", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutputOfflineModes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Outputs",
                schema: "output",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    module_component_id = table.Column<short>(type: "smallint", nullable: false),
                    device_component_id = table.Column<short>(type: "smallint", nullable: false),
                    output_no = table.Column<short>(type: "smallint", nullable: false),
                    model = table.Column<string>(type: "text", nullable: false),
                    mode = table.Column<short>(type: "smallint", nullable: false),
                    default_pulse = table.Column<short>(type: "smallint", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outputs", x => x.id);
                });

            migrationBuilder.InsertData(
                schema: "output",
                table: "OutputDriveModes",
                columns: new[] { "id", "component_id", "description", "is_active", "label", "location_id", "value" },
                values: new object[,]
                {
                    { 1, (short)0, "", true, "Normal", 0, (short)0 },
                    { 2, (short)0, "", true, "Inverted", 0, (short)1 }
                });

            migrationBuilder.InsertData(
                schema: "output",
                table: "OutputModes",
                columns: new[] { "id", "component_id", "drive", "is_active", "label", "location_id", "offline", "value" },
                values: new object[,]
                {
                    { 1, (short)0, (short)0, true, "", 0, (short)0, (short)0 },
                    { 2, (short)0, (short)1, true, "", 0, (short)0, (short)1 },
                    { 3, (short)0, (short)0, true, "", 0, (short)1, (short)16 },
                    { 4, (short)0, (short)1, true, "", 0, (short)1, (short)17 },
                    { 5, (short)0, (short)0, true, "", 0, (short)2, (short)32 },
                    { 6, (short)0, (short)1, true, "", 0, (short)2, (short)33 }
                });

            migrationBuilder.InsertData(
                schema: "output",
                table: "OutputOfflineModes",
                columns: new[] { "id", "component_id", "description", "is_active", "label", "location_id", "value" },
                values: new object[,]
                {
                    { 1, (short)0, "", true, "No Change", 0, (short)0 },
                    { 2, (short)0, "", true, "Inactive", 0, (short)1 },
                    { 3, (short)0, "", true, "Active", 0, (short)2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutputDriveModes",
                schema: "output");

            migrationBuilder.DropTable(
                name: "OutputModes",
                schema: "output");

            migrationBuilder.DropTable(
                name: "OutputOfflineModes",
                schema: "output");

            migrationBuilder.DropTable(
                name: "Outputs",
                schema: "output");
        }
    }
}
