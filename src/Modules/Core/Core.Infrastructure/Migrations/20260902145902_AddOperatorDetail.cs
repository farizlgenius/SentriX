using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOperatorDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operators_Roles_roleid",
                schema: "core",
                table: "Operators");

            migrationBuilder.DropIndex(
                name: "IX_Operators_roleid",
                schema: "core",
                table: "Operators");

            migrationBuilder.DeleteData(
                schema: "core",
                table: "UserLocations",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "Users",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "roleid",
                schema: "core",
                table: "Operators");

            migrationBuilder.AlterColumn<DateTime>(
                name: "expired_date",
                schema: "core",
                table: "Operators",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.InsertData(
                schema: "core",
                table: "Operators",
                columns: new[] { "id", "email", "expired_date", "firstname", "gender", "is_active", "is_default", "joined_date", "lastname", "middlename", "password", "phone", "role_id", "title", "username" },
                values: new object[] { 1, "support@sentrix.com", null, "Administrator", "Male", true, false, new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "", "100000.lG1/4V/VRPZsbhf/Zqc4xw==.6vYcf+wEMSgqcaNhoZEdM9PaPxx2ZUErZhQbeMxo5OY=", "", 1, "Mr", "admin" });

            migrationBuilder.InsertData(
                schema: "core",
                table: "OperatorLocations",
                columns: new[] { "id", "is_active", "is_default", "location_id", "operator_id" },
                values: new object[] { 1, true, false, 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Operators_guid_id_username",
                schema: "core",
                table: "Operators",
                columns: new[] { "guid", "id", "username" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Operators_role_id",
                schema: "core",
                table: "Operators",
                column: "role_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Operators_Roles_role_id",
                schema: "core",
                table: "Operators",
                column: "role_id",
                principalSchema: "core",
                principalTable: "Roles",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operators_Roles_role_id",
                schema: "core",
                table: "Operators");

            migrationBuilder.DropIndex(
                name: "IX_Operators_guid_id_username",
                schema: "core",
                table: "Operators");

            migrationBuilder.DropIndex(
                name: "IX_Operators_role_id",
                schema: "core",
                table: "Operators");

            migrationBuilder.DeleteData(
                schema: "core",
                table: "OperatorLocations",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "Operators",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.AlterColumn<DateTime>(
                name: "expired_date",
                schema: "core",
                table: "Operators",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "roleid",
                schema: "core",
                table: "Operators",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                schema: "core",
                table: "Users",
                columns: new[] { "id", "Locationid", "active_time", "address", "company_id", "date_of_birth", "department_id", "email", "expire_time", "face_id", "firstname", "gender", "guid", "identification", "is_active", "is_default", "is_operator", "is_user", "lastname", "license_plate_id", "middlename", "password", "phone", "pin_id", "position_id", "qr_code_id", "role_id", "title", "user_code", "username" },
                values: new object[] { 1, null, new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", null, new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "support@sentrix.com", new DateTime(9999, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "admin", "Male", new Guid("ed2b5887-9dcb-43bd-a6f8-988330df5181"), "admin", true, true, true, false, "system", null, "", "100000.lG1/4V/VRPZsbhf/Zqc4xw==.6vYcf+wEMSgqcaNhoZEdM9PaPxx2ZUErZhQbeMxo5OY=", "", null, null, null, 1, "Mr", "admin01", "admin" });

            migrationBuilder.InsertData(
                schema: "core",
                table: "UserLocations",
                columns: new[] { "id", "is_active", "is_default", "location_id", "user_id" },
                values: new object[] { 1, true, false, 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Operators_roleid",
                schema: "core",
                table: "Operators",
                column: "roleid");

            migrationBuilder.AddForeignKey(
                name: "FK_Operators_Roles_roleid",
                schema: "core",
                table: "Operators",
                column: "roleid",
                principalSchema: "core",
                principalTable: "Roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
