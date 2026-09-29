using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBG : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Outputs_DeviceModules_device_module_id",
                schema: "core",
                table: "Outputs");

            migrationBuilder.DropForeignKey(
                name: "FK_Outputs_Locations_location_id",
                schema: "core",
                table: "Outputs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Outputs",
                schema: "core",
                table: "Outputs");

            migrationBuilder.RenameTable(
                name: "Outputs",
                schema: "core",
                newName: "Output",
                newSchema: "core");

            migrationBuilder.RenameIndex(
                name: "IX_Outputs_location_id",
                schema: "core",
                table: "Output",
                newName: "IX_Output_location_id");

            migrationBuilder.RenameIndex(
                name: "IX_Outputs_guid_id_device_module_id_slot_no",
                schema: "core",
                table: "Output",
                newName: "IX_Output_guid_id_device_module_id_slot_no");

            migrationBuilder.RenameIndex(
                name: "IX_Outputs_device_module_id",
                schema: "core",
                table: "Output",
                newName: "IX_Output_device_module_id");

            migrationBuilder.AddColumn<int>(
                name: "bg_id",
                schema: "core",
                table: "Doors",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Output",
                schema: "core",
                table: "Output",
                column: "id");

            migrationBuilder.CreateTable(
                name: "BreakGlasses",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    slot_no = table.Column<int>(type: "integer", nullable: false),
                    vendor = table.Column<string>(type: "text", nullable: false),
                    door_id = table.Column<int>(type: "integer", nullable: true),
                    device_module_id = table.Column<int>(type: "integer", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "Input",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    slot_no = table.Column<int>(type: "integer", nullable: false),
                    vendor = table.Column<string>(type: "text", nullable: false),
                    mode = table.Column<string>(type: "text", nullable: false),
                    metadata = table.Column<string>(type: "text", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    device_module_id = table.Column<int>(type: "integer", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_Doors_bg_id",
                schema: "core",
                table: "Doors",
                column: "bg_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BreakGlasses_device_module_id",
                schema: "core",
                table: "BreakGlasses",
                column: "device_module_id");

            migrationBuilder.CreateIndex(
                name: "IX_BreakGlasses_guid_id_door_id_device_module_id_slot_no",
                schema: "core",
                table: "BreakGlasses",
                columns: new[] { "guid", "id", "door_id", "device_module_id", "slot_no" },
                unique: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Doors_BreakGlasses_bg_id",
                schema: "core",
                table: "Doors",
                column: "bg_id",
                principalSchema: "core",
                principalTable: "BreakGlasses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Output_DeviceModules_device_module_id",
                schema: "core",
                table: "Output",
                column: "device_module_id",
                principalSchema: "core",
                principalTable: "DeviceModules",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Output_Locations_location_id",
                schema: "core",
                table: "Output",
                column: "location_id",
                principalSchema: "core",
                principalTable: "Locations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doors_BreakGlasses_bg_id",
                schema: "core",
                table: "Doors");

            migrationBuilder.DropForeignKey(
                name: "FK_Output_DeviceModules_device_module_id",
                schema: "core",
                table: "Output");

            migrationBuilder.DropForeignKey(
                name: "FK_Output_Locations_location_id",
                schema: "core",
                table: "Output");

            migrationBuilder.DropTable(
                name: "BreakGlasses",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Input",
                schema: "core");

            migrationBuilder.DropIndex(
                name: "IX_Doors_bg_id",
                schema: "core",
                table: "Doors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Output",
                schema: "core",
                table: "Output");

            migrationBuilder.DropColumn(
                name: "bg_id",
                schema: "core",
                table: "Doors");

            migrationBuilder.RenameTable(
                name: "Output",
                schema: "core",
                newName: "Outputs",
                newSchema: "core");

            migrationBuilder.RenameIndex(
                name: "IX_Output_location_id",
                schema: "core",
                table: "Outputs",
                newName: "IX_Outputs_location_id");

            migrationBuilder.RenameIndex(
                name: "IX_Output_guid_id_device_module_id_slot_no",
                schema: "core",
                table: "Outputs",
                newName: "IX_Outputs_guid_id_device_module_id_slot_no");

            migrationBuilder.RenameIndex(
                name: "IX_Output_device_module_id",
                schema: "core",
                table: "Outputs",
                newName: "IX_Outputs_device_module_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Outputs",
                schema: "core",
                table: "Outputs",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Outputs_DeviceModules_device_module_id",
                schema: "core",
                table: "Outputs",
                column: "device_module_id",
                principalSchema: "core",
                principalTable: "DeviceModules",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Outputs_Locations_location_id",
                schema: "core",
                table: "Outputs",
                column: "location_id",
                principalSchema: "core",
                principalTable: "Locations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
