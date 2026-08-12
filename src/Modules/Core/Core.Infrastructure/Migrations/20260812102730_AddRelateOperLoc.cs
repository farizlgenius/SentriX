using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRelateOperLoc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OperatorLocation_Locations_location_guid",
                schema: "core",
                table: "OperatorLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_OperatorLocation_Operators_operator_guid",
                schema: "core",
                table: "OperatorLocation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OperatorLocation",
                schema: "core",
                table: "OperatorLocation");

            migrationBuilder.DeleteData(
                schema: "core",
                table: "Operators",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.RenameTable(
                name: "OperatorLocation",
                schema: "core",
                newName: "OperatorLocations",
                newSchema: "core");

            migrationBuilder.RenameIndex(
                name: "IX_OperatorLocation_operator_guid",
                schema: "core",
                table: "OperatorLocations",
                newName: "IX_OperatorLocations_operator_guid");

            migrationBuilder.RenameIndex(
                name: "IX_OperatorLocation_location_guid",
                schema: "core",
                table: "OperatorLocations",
                newName: "IX_OperatorLocations_location_guid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OperatorLocations",
                schema: "core",
                table: "OperatorLocations",
                column: "id");

            migrationBuilder.InsertData(
                schema: "core",
                table: "Operators",
                columns: new[] { "id", "active_time", "email", "expire_time", "guid", "is_active", "is_default", "password", "phone", "role_guid", "username" },
                values: new object[] { 1, new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "support@sentrix.com", new DateTime(9999, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("ed2b5887-9dcb-43bd-a6f8-988330df5181"), true, true, "100000.lG1/4V/VRPZsbhf/Zqc4xw==.6vYcf+wEMSgqcaNhoZEdM9PaPxx2ZUErZhQbeMxo5OY=", "", new Guid("fe527691-7b13-4294-98b5-cb95181f5453"), "admin" });

            migrationBuilder.InsertData(
                schema: "core",
                table: "OperatorLocations",
                columns: new[] { "id", "guid", "is_active", "is_default", "location_guid", "operator_guid" },
                values: new object[] { 1, new Guid("88f16b53-b5b1-4c21-9324-968d58584b06"), true, false, new Guid("3a9c9947-d5ca-4bb2-b525-0499a340f1d6"), new Guid("ed2b5887-9dcb-43bd-a6f8-988330df5181") });

            migrationBuilder.AddForeignKey(
                name: "FK_OperatorLocations_Locations_location_guid",
                schema: "core",
                table: "OperatorLocations",
                column: "location_guid",
                principalSchema: "core",
                principalTable: "Locations",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OperatorLocations_Operators_operator_guid",
                schema: "core",
                table: "OperatorLocations",
                column: "operator_guid",
                principalSchema: "core",
                principalTable: "Operators",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OperatorLocations_Locations_location_guid",
                schema: "core",
                table: "OperatorLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_OperatorLocations_Operators_operator_guid",
                schema: "core",
                table: "OperatorLocations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OperatorLocations",
                schema: "core",
                table: "OperatorLocations");

            migrationBuilder.DeleteData(
                schema: "core",
                table: "OperatorLocations",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.RenameTable(
                name: "OperatorLocations",
                schema: "core",
                newName: "OperatorLocation",
                newSchema: "core");

            migrationBuilder.RenameIndex(
                name: "IX_OperatorLocations_operator_guid",
                schema: "core",
                table: "OperatorLocation",
                newName: "IX_OperatorLocation_operator_guid");

            migrationBuilder.RenameIndex(
                name: "IX_OperatorLocations_location_guid",
                schema: "core",
                table: "OperatorLocation",
                newName: "IX_OperatorLocation_location_guid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OperatorLocation",
                schema: "core",
                table: "OperatorLocation",
                column: "id");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "Operators",
                keyColumn: "id",
                keyValue: 1,
                column: "guid",
                value: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_OperatorLocation_Locations_location_guid",
                schema: "core",
                table: "OperatorLocation",
                column: "location_guid",
                principalSchema: "core",
                principalTable: "Locations",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OperatorLocation_Operators_operator_guid",
                schema: "core",
                table: "OperatorLocation",
                column: "operator_guid",
                principalSchema: "core",
                principalTable: "Operators",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
