using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Device.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGUID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inputs_Modules_module_id",
                schema: "device",
                table: "Inputs");

            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Devices_device_id",
                schema: "device",
                table: "Modules");

            migrationBuilder.DropForeignKey(
                name: "FK_Readers_Modules_module_id",
                schema: "device",
                table: "Readers");

            migrationBuilder.DropForeignKey(
                name: "FK_Relays_Modules_module_id",
                schema: "device",
                table: "Relays");

            migrationBuilder.DropIndex(
                name: "IX_Relays_module_id",
                schema: "device",
                table: "Relays");

            migrationBuilder.DropIndex(
                name: "IX_Readers_module_id",
                schema: "device",
                table: "Readers");

            migrationBuilder.DropIndex(
                name: "IX_Modules_device_id",
                schema: "device",
                table: "Modules");

            migrationBuilder.DropIndex(
                name: "IX_Inputs_module_id",
                schema: "device",
                table: "Inputs");

            migrationBuilder.DropColumn(
                name: "component_id",
                schema: "device",
                table: "Relays");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "device",
                table: "Relays");

            migrationBuilder.DropColumn(
                name: "is_active",
                schema: "device",
                table: "Relays");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "device",
                table: "Relays");

            migrationBuilder.DropColumn(
                name: "location_id",
                schema: "device",
                table: "Relays");

            migrationBuilder.DropColumn(
                name: "updated_at",
                schema: "device",
                table: "Relays");

            migrationBuilder.DropColumn(
                name: "component_id",
                schema: "device",
                table: "Readers");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "device",
                table: "Readers");

            migrationBuilder.DropColumn(
                name: "is_active",
                schema: "device",
                table: "Readers");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "device",
                table: "Readers");

            migrationBuilder.DropColumn(
                name: "module_id",
                schema: "device",
                table: "Readers");

            migrationBuilder.DropColumn(
                name: "updated_at",
                schema: "device",
                table: "Readers");

            migrationBuilder.DropColumn(
                name: "device_id",
                schema: "device",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "component_id",
                schema: "device",
                table: "Inputs");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "device",
                table: "Inputs");

            migrationBuilder.DropColumn(
                name: "is_active",
                schema: "device",
                table: "Inputs");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "device",
                table: "Inputs");

            migrationBuilder.DropColumn(
                name: "module_id",
                schema: "device",
                table: "Inputs");

            migrationBuilder.DropColumn(
                name: "updated_at",
                schema: "device",
                table: "Inputs");

            migrationBuilder.RenameColumn(
                name: "module_id",
                schema: "device",
                table: "Relays",
                newName: "LocationId");

            migrationBuilder.AddColumn<Guid>(
                name: "guid",
                schema: "device",
                table: "Relays",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "module_guid",
                schema: "device",
                table: "Relays",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "guid",
                schema: "device",
                table: "Readers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "module_guid",
                schema: "device",
                table: "Readers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "device_guid",
                schema: "device",
                table: "Modules",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "guid",
                schema: "device",
                table: "Modules",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "guid",
                schema: "device",
                table: "Inputs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "module_guid",
                schema: "device",
                table: "Inputs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "guid",
                schema: "device",
                table: "Devices",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Modules_guid",
                schema: "device",
                table: "Modules",
                column: "guid");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Devices_guid",
                schema: "device",
                table: "Devices",
                column: "guid");

            migrationBuilder.CreateIndex(
                name: "IX_Relays_module_guid",
                schema: "device",
                table: "Relays",
                column: "module_guid");

            migrationBuilder.CreateIndex(
                name: "IX_Readers_module_guid",
                schema: "device",
                table: "Readers",
                column: "module_guid");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_device_guid",
                schema: "device",
                table: "Modules",
                column: "device_guid");

            migrationBuilder.CreateIndex(
                name: "IX_Inputs_module_guid",
                schema: "device",
                table: "Inputs",
                column: "module_guid");

            migrationBuilder.AddForeignKey(
                name: "FK_Inputs_Modules_module_guid",
                schema: "device",
                table: "Inputs",
                column: "module_guid",
                principalSchema: "device",
                principalTable: "Modules",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Devices_device_guid",
                schema: "device",
                table: "Modules",
                column: "device_guid",
                principalSchema: "device",
                principalTable: "Devices",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Readers_Modules_module_guid",
                schema: "device",
                table: "Readers",
                column: "module_guid",
                principalSchema: "device",
                principalTable: "Modules",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Relays_Modules_module_guid",
                schema: "device",
                table: "Relays",
                column: "module_guid",
                principalSchema: "device",
                principalTable: "Modules",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inputs_Modules_module_guid",
                schema: "device",
                table: "Inputs");

            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Devices_device_guid",
                schema: "device",
                table: "Modules");

            migrationBuilder.DropForeignKey(
                name: "FK_Readers_Modules_module_guid",
                schema: "device",
                table: "Readers");

            migrationBuilder.DropForeignKey(
                name: "FK_Relays_Modules_module_guid",
                schema: "device",
                table: "Relays");

            migrationBuilder.DropIndex(
                name: "IX_Relays_module_guid",
                schema: "device",
                table: "Relays");

            migrationBuilder.DropIndex(
                name: "IX_Readers_module_guid",
                schema: "device",
                table: "Readers");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Modules_guid",
                schema: "device",
                table: "Modules");

            migrationBuilder.DropIndex(
                name: "IX_Modules_device_guid",
                schema: "device",
                table: "Modules");

            migrationBuilder.DropIndex(
                name: "IX_Inputs_module_guid",
                schema: "device",
                table: "Inputs");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Devices_guid",
                schema: "device",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "guid",
                schema: "device",
                table: "Relays");

            migrationBuilder.DropColumn(
                name: "module_guid",
                schema: "device",
                table: "Relays");

            migrationBuilder.DropColumn(
                name: "guid",
                schema: "device",
                table: "Readers");

            migrationBuilder.DropColumn(
                name: "module_guid",
                schema: "device",
                table: "Readers");

            migrationBuilder.DropColumn(
                name: "device_guid",
                schema: "device",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "guid",
                schema: "device",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "guid",
                schema: "device",
                table: "Inputs");

            migrationBuilder.DropColumn(
                name: "module_guid",
                schema: "device",
                table: "Inputs");

            migrationBuilder.DropColumn(
                name: "guid",
                schema: "device",
                table: "Devices");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                schema: "device",
                table: "Relays",
                newName: "module_id");

            migrationBuilder.AddColumn<short>(
                name: "component_id",
                schema: "device",
                table: "Relays",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "device",
                table: "Relays",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                schema: "device",
                table: "Relays",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "device",
                table: "Relays",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "location_id",
                schema: "device",
                table: "Relays",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                schema: "device",
                table: "Relays",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.AddColumn<short>(
                name: "component_id",
                schema: "device",
                table: "Readers",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "device",
                table: "Readers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                schema: "device",
                table: "Readers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "device",
                table: "Readers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "module_id",
                schema: "device",
                table: "Readers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                schema: "device",
                table: "Readers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.AddColumn<int>(
                name: "device_id",
                schema: "device",
                table: "Modules",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<short>(
                name: "component_id",
                schema: "device",
                table: "Inputs",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "device",
                table: "Inputs",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                schema: "device",
                table: "Inputs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "device",
                table: "Inputs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "module_id",
                schema: "device",
                table: "Inputs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                schema: "device",
                table: "Inputs",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.CreateIndex(
                name: "IX_Relays_module_id",
                schema: "device",
                table: "Relays",
                column: "module_id");

            migrationBuilder.CreateIndex(
                name: "IX_Readers_module_id",
                schema: "device",
                table: "Readers",
                column: "module_id");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_device_id",
                schema: "device",
                table: "Modules",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "IX_Inputs_module_id",
                schema: "device",
                table: "Inputs",
                column: "module_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Inputs_Modules_module_id",
                schema: "device",
                table: "Inputs",
                column: "module_id",
                principalSchema: "device",
                principalTable: "Modules",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Devices_device_id",
                schema: "device",
                table: "Modules",
                column: "device_id",
                principalSchema: "device",
                principalTable: "Devices",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Readers_Modules_module_id",
                schema: "device",
                table: "Readers",
                column: "module_id",
                principalSchema: "device",
                principalTable: "Modules",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Relays_Modules_module_id",
                schema: "device",
                table: "Relays",
                column: "module_id",
                principalSchema: "device",
                principalTable: "Modules",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
