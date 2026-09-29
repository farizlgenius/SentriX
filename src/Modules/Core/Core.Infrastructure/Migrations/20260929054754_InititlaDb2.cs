using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InititlaDb2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doors_Lanes_laneid",
                schema: "core",
                table: "Doors");

            migrationBuilder.DropIndex(
                name: "IX_Doors_guid_id",
                schema: "core",
                table: "Doors");

            migrationBuilder.DropIndex(
                name: "IX_Doors_laneid",
                schema: "core",
                table: "Doors");

            migrationBuilder.DropColumn(
                name: "lane_id",
                schema: "core",
                table: "Doors");

            migrationBuilder.DropColumn(
                name: "laneid",
                schema: "core",
                table: "Doors");

            migrationBuilder.AddColumn<int>(
                name: "device_id",
                schema: "core",
                table: "Doors",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "ComponentMappings",
                keyColumn: "id",
                keyValue: 1,
                column: "entity",
                value: "TimeZone");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "ComponentMappings",
                keyColumn: "id",
                keyValue: 2,
                column: "entity",
                value: "TimeZone");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TimeZones",
                keyColumn: "id",
                keyValue: 1,
                column: "guid",
                value: new Guid("cff24195-43f8-42ec-a03f-e84857cc958f"));

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TimeZones",
                keyColumn: "id",
                keyValue: 2,
                column: "guid",
                value: new Guid("614a766c-1241-4fdc-9d0d-7788291cac2d"));

            migrationBuilder.CreateIndex(
                name: "IX_Doors_device_id",
                schema: "core",
                table: "Doors",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "IX_Doors_guid_id_device_id",
                schema: "core",
                table: "Doors",
                columns: new[] { "guid", "id", "device_id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Doors_Devices_device_id",
                schema: "core",
                table: "Doors",
                column: "device_id",
                principalSchema: "core",
                principalTable: "Devices",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doors_Devices_device_id",
                schema: "core",
                table: "Doors");

            migrationBuilder.DropIndex(
                name: "IX_Doors_device_id",
                schema: "core",
                table: "Doors");

            migrationBuilder.DropIndex(
                name: "IX_Doors_guid_id_device_id",
                schema: "core",
                table: "Doors");

            migrationBuilder.DropColumn(
                name: "device_id",
                schema: "core",
                table: "Doors");

            migrationBuilder.AddColumn<int>(
                name: "lane_id",
                schema: "core",
                table: "Doors",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "laneid",
                schema: "core",
                table: "Doors",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "ComponentMappings",
                keyColumn: "id",
                keyValue: 1,
                column: "entity",
                value: "Timezone");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "ComponentMappings",
                keyColumn: "id",
                keyValue: 2,
                column: "entity",
                value: "Timezone");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TimeZones",
                keyColumn: "id",
                keyValue: 1,
                column: "guid",
                value: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TimeZones",
                keyColumn: "id",
                keyValue: 2,
                column: "guid",
                value: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Doors_guid_id",
                schema: "core",
                table: "Doors",
                columns: new[] { "guid", "id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doors_laneid",
                schema: "core",
                table: "Doors",
                column: "laneid");

            migrationBuilder.AddForeignKey(
                name: "FK_Doors_Lanes_laneid",
                schema: "core",
                table: "Doors",
                column: "laneid",
                principalSchema: "core",
                principalTable: "Lanes",
                principalColumn: "id");
        }
    }
}
