using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Input.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "input");

            migrationBuilder.CreateTable(
                name: "InputGroups",
                schema: "input",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InputGroups", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "InputModes",
                schema: "input",
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
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InputModes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Inputs",
                schema: "input",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    device_component_id = table.Column<short>(type: "smallint", nullable: false),
                    module_component_id = table.Column<short>(type: "smallint", nullable: false),
                    input_no = table.Column<short>(type: "smallint", nullable: false),
                    sensor_mode = table.Column<short>(type: "smallint", nullable: false),
                    debounce = table.Column<short>(type: "smallint", nullable: false),
                    hold_time = table.Column<short>(type: "smallint", nullable: false),
                    log_function = table.Column<short>(type: "smallint", nullable: false),
                    latch_mode = table.Column<short>(type: "smallint", nullable: false),
                    delay_entry = table.Column<short>(type: "smallint", nullable: false),
                    delay_exit = table.Column<short>(type: "smallint", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inputs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "InputGroupDetails",
                schema: "input",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mac = table.Column<string>(type: "text", nullable: false),
                    device_component_id = table.Column<short>(type: "smallint", nullable: false),
                    input_group_id = table.Column<int>(type: "integer", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InputGroupDetails", x => x.id);
                    table.ForeignKey(
                        name: "FK_InputGroupDetails_InputGroups_input_group_id",
                        column: x => x.input_group_id,
                        principalSchema: "input",
                        principalTable: "InputGroups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InputLists",
                schema: "input",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    input_component_id = table.Column<short>(type: "smallint", nullable: false),
                    input_type = table.Column<short>(type: "smallint", nullable: false),
                    input_group_detail_id = table.Column<int>(type: "integer", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InputLists", x => x.id);
                    table.ForeignKey(
                        name: "FK_InputLists_InputGroupDetails_input_group_detail_id",
                        column: x => x.input_group_detail_id,
                        principalSchema: "input",
                        principalTable: "InputGroupDetails",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "input",
                table: "InputModes",
                columns: new[] { "id", "component_id", "description", "is_active", "is_default", "label", "location_id", "value" },
                values: new object[,]
                {
                    { 1, (short)0, "", true, false, "Normally closed", 0, (short)0 },
                    { 2, (short)0, "", true, false, "Normally open", 0, (short)1 },
                    { 3, (short)0, "", true, false, "EOL: 1 kΩ normal, 2 kΩ active", 0, (short)2 },
                    { 4, (short)0, "", true, false, "EOL: 2 kΩ normal, 1 kΩ active", 0, (short)3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_InputGroupDetails_input_group_id",
                schema: "input",
                table: "InputGroupDetails",
                column: "input_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_InputLists_input_group_detail_id",
                schema: "input",
                table: "InputLists",
                column: "input_group_detail_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InputLists",
                schema: "input");

            migrationBuilder.DropTable(
                name: "InputModes",
                schema: "input");

            migrationBuilder.DropTable(
                name: "Inputs",
                schema: "input");

            migrationBuilder.DropTable(
                name: "InputGroupDetails",
                schema: "input");

            migrationBuilder.DropTable(
                name: "InputGroups",
                schema: "input");
        }
    }
}
