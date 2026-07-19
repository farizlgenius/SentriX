using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Group.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "group",
                table: "Groups",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "location_id", "name" },
                values: new object[] { -1, "Default" });

            migrationBuilder.UpdateData(
                schema: "group",
                table: "Groups",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "component_id", "name" },
                values: new object[] { (short)1, "Always" });

            migrationBuilder.InsertData(
                schema: "group",
                table: "Groups",
                columns: new[] { "id", "component_id", "created_at", "guid", "is_active", "is_default", "location_id", "name", "updated_at" },
                values: new object[] { 3, (short)2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), true, true, 0, "Never", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "group",
                table: "Groups",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                schema: "group",
                table: "Groups",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "location_id", "name" },
                values: new object[] { 0, "Always" });

            migrationBuilder.UpdateData(
                schema: "group",
                table: "Groups",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "component_id", "name" },
                values: new object[] { (short)2, "Never" });
        }
    }
}
