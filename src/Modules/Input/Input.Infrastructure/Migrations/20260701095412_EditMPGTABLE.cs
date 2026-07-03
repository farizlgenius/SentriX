using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Input.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditMPGTABLE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "device_component_id",
                schema: "input",
                table: "InputGroups");

            migrationBuilder.DropColumn(
                name: "mac",
                schema: "input",
                table: "InputGroups");

            migrationBuilder.DropColumn(
                name: "metadata",
                schema: "input",
                table: "InputGroups");

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
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
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
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
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
                name: "InputGroupDetails",
                schema: "input");

            migrationBuilder.AddColumn<short>(
                name: "device_component_id",
                schema: "input",
                table: "InputGroups",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<string>(
                name: "mac",
                schema: "input",
                table: "InputGroups",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "metadata",
                schema: "input",
                table: "InputGroups",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
