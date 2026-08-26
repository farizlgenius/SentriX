using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditRoleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                schema: "core",
                table: "ModulePermissions");

            migrationBuilder.DropColumn(
                name: "name",
                schema: "core",
                table: "FeaturePermissions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "name",
                schema: "core",
                table: "ModulePermissions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "name",
                schema: "core",
                table: "FeaturePermissions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "FeaturePermissions",
                keyColumn: "id",
                keyValue: 1,
                column: "name",
                value: "");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "FeaturePermissions",
                keyColumn: "id",
                keyValue: 2,
                column: "name",
                value: "");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "FeaturePermissions",
                keyColumn: "id",
                keyValue: 3,
                column: "name",
                value: "");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "FeaturePermissions",
                keyColumn: "id",
                keyValue: 4,
                column: "name",
                value: "");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "FeaturePermissions",
                keyColumn: "id",
                keyValue: 5,
                column: "name",
                value: "");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "FeaturePermissions",
                keyColumn: "id",
                keyValue: 6,
                column: "name",
                value: "");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "FeaturePermissions",
                keyColumn: "id",
                keyValue: 7,
                column: "name",
                value: "");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "FeaturePermissions",
                keyColumn: "id",
                keyValue: 8,
                column: "name",
                value: "");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "FeaturePermissions",
                keyColumn: "id",
                keyValue: 9,
                column: "name",
                value: "");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "FeaturePermissions",
                keyColumn: "id",
                keyValue: 10,
                column: "name",
                value: "");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "FeaturePermissions",
                keyColumn: "id",
                keyValue: 11,
                column: "name",
                value: "");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "FeaturePermissions",
                keyColumn: "id",
                keyValue: 12,
                column: "name",
                value: "");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "FeaturePermissions",
                keyColumn: "id",
                keyValue: 13,
                column: "name",
                value: "");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "FeaturePermissions",
                keyColumn: "id",
                keyValue: 14,
                column: "name",
                value: "");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "FeaturePermissions",
                keyColumn: "id",
                keyValue: 15,
                column: "name",
                value: "");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "FeaturePermissions",
                keyColumn: "id",
                keyValue: 16,
                column: "name",
                value: "");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "FeaturePermissions",
                keyColumn: "id",
                keyValue: 17,
                column: "name",
                value: "");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "FeaturePermissions",
                keyColumn: "id",
                keyValue: 18,
                column: "name",
                value: "");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "FeaturePermissions",
                keyColumn: "id",
                keyValue: 19,
                column: "name",
                value: "");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "FeaturePermissions",
                keyColumn: "id",
                keyValue: 20,
                column: "name",
                value: "");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "FeaturePermissions",
                keyColumn: "id",
                keyValue: 21,
                column: "name",
                value: "");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "FeaturePermissions",
                keyColumn: "id",
                keyValue: 22,
                column: "name",
                value: "");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "ModulePermissions",
                keyColumn: "id",
                keyValue: 1,
                column: "name",
                value: "");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "ModulePermissions",
                keyColumn: "id",
                keyValue: 2,
                column: "name",
                value: "");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "ModulePermissions",
                keyColumn: "id",
                keyValue: 3,
                column: "name",
                value: "");
        }
    }
}
