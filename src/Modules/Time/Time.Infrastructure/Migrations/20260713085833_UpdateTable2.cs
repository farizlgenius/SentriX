using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Time.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "time",
                table: "Timezones",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.InsertData(
                schema: "time",
                table: "Timezones",
                columns: new[] { "id", "active", "component_id", "deactive", "guid", "is_active", "is_default", "location_id", "mode", "name" },
                values: new object[] { 1, "", (short)1, "", new Guid("9b6e1f89-6f6e-4c5d-a0a5-c9d6f5d18e7b"), true, true, 0, (short)1, "Always" });

            migrationBuilder.InsertData(
                schema: "time",
                table: "Intervals",
                columns: new[] { "id", "component_id", "day_in_week_guid", "days_detail", "end", "guid", "start", "timezone_guid" },
                values: new object[] { 1, (short)1, new Guid("00000000-0000-0000-0000-000000000000"), "", "23:00", new Guid("f2d4c8b3-91aa-4b4c-8e1d-73c1f9b2a6d4"), "00:00", new Guid("9b6e1f89-6f6e-4c5d-a0a5-c9d6f5d18e7b") });

            migrationBuilder.InsertData(
                schema: "time",
                table: "DayInWeeks",
                columns: new[] { "id", "friday", "guid", "interval_guid", "monday", "saturday", "sunday", "thursday", "tuesday", "wednesday" },
                values: new object[] { 1, true, new Guid("4e7a2d90-3b8f-4fd8-9c57-2a1e6b9d8f43"), new Guid("f2d4c8b3-91aa-4b4c-8e1d-73c1f9b2a6d4"), true, true, true, true, true, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "time",
                table: "DayInWeeks",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "time",
                table: "Intervals",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.UpdateData(
                schema: "time",
                table: "Timezones",
                keyColumn: "id",
                keyValue: 1,
                column: "guid",
                value: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
