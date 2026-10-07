using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "location_id",
                schema: "core",
                table: "Groups",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.InsertData(
                schema: "core",
                table: "ComponentMappings",
                columns: new[] { "id", "entity", "external_id", "guid", "is_active", "is_default", "location_id", "mac", "vendor" },
                values: new object[] { 6, "Group", 1, new Guid("895b0301-3d77-4ec7-8917-43260a839643"), true, false, null, null, null });

            migrationBuilder.InsertData(
                schema: "core",
                table: "Groups",
                columns: new[] { "id", "guid", "is_active", "is_default", "location_id", "name" },
                values: new object[] { 1, new Guid("895b0301-3d77-4ec7-8917-43260a839643"), true, true, null, "All Door" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "core",
                table: "ComponentMappings",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "Groups",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "location_id",
                schema: "core",
                table: "Groups",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
