using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Time.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DayInWeeks_Intervals_interval_id",
                schema: "time",
                table: "DayInWeeks");

            migrationBuilder.DropForeignKey(
                name: "FK_Intervals_Timezones_timezone_id",
                schema: "time",
                table: "Intervals");

            migrationBuilder.DropIndex(
                name: "IX_Intervals_timezone_id",
                schema: "time",
                table: "Intervals");

            migrationBuilder.DropIndex(
                name: "IX_DayInWeeks_interval_id",
                schema: "time",
                table: "DayInWeeks");

            migrationBuilder.DeleteData(
                schema: "time",
                table: "Timezones",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "time",
                table: "Timezones",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "component_id",
                schema: "time",
                table: "Intervals");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "time",
                table: "Intervals");

            migrationBuilder.DropColumn(
                name: "day_in_week_id",
                schema: "time",
                table: "Intervals");

            migrationBuilder.DropColumn(
                name: "is_active",
                schema: "time",
                table: "Intervals");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "time",
                table: "Intervals");

            migrationBuilder.DropColumn(
                name: "location_id",
                schema: "time",
                table: "Intervals");

            migrationBuilder.DropColumn(
                name: "timezone_id",
                schema: "time",
                table: "Intervals");

            migrationBuilder.DropColumn(
                name: "updated_at",
                schema: "time",
                table: "Intervals");

            migrationBuilder.DropColumn(
                name: "component_id",
                schema: "time",
                table: "DayInWeeks");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "time",
                table: "DayInWeeks");

            migrationBuilder.DropColumn(
                name: "interval_id",
                schema: "time",
                table: "DayInWeeks");

            migrationBuilder.DropColumn(
                name: "is_active",
                schema: "time",
                table: "DayInWeeks");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "time",
                table: "DayInWeeks");

            migrationBuilder.DropColumn(
                name: "location_id",
                schema: "time",
                table: "DayInWeeks");

            migrationBuilder.DropColumn(
                name: "updated_at",
                schema: "time",
                table: "DayInWeeks");

            migrationBuilder.AddColumn<Guid>(
                name: "guid",
                schema: "time",
                table: "Timezones",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()");

            migrationBuilder.AddColumn<Guid>(
                name: "day_in_week_guid",
                schema: "time",
                table: "Intervals",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "guid",
                schema: "time",
                table: "Intervals",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "timezone_guid",
                schema: "time",
                table: "Intervals",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "guid",
                schema: "time",
                table: "Holidays",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()");

            migrationBuilder.AddColumn<Guid>(
                name: "guid",
                schema: "time",
                table: "DayInWeeks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "interval_guid",
                schema: "time",
                table: "DayInWeeks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Timezones_guid",
                schema: "time",
                table: "Timezones",
                column: "guid");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Intervals_guid",
                schema: "time",
                table: "Intervals",
                column: "guid");

            migrationBuilder.InsertData(
                schema: "time",
                table: "Timezones",
                columns: new[] { "id", "active", "component_id", "deactive", "is_active", "is_default", "location_id", "mode", "name" },
                values: new object[,]
                {
                    { 1, "", (short)1, "", true, true, 0, (short)1, "Always" },
                    { 2, "", (short)2, "", true, true, 0, (short)0, "Never" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Intervals_timezone_guid",
                schema: "time",
                table: "Intervals",
                column: "timezone_guid");

            migrationBuilder.CreateIndex(
                name: "IX_DayInWeeks_interval_guid",
                schema: "time",
                table: "DayInWeeks",
                column: "interval_guid",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DayInWeeks_Intervals_interval_guid",
                schema: "time",
                table: "DayInWeeks",
                column: "interval_guid",
                principalSchema: "time",
                principalTable: "Intervals",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Intervals_Timezones_timezone_guid",
                schema: "time",
                table: "Intervals",
                column: "timezone_guid",
                principalSchema: "time",
                principalTable: "Timezones",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DayInWeeks_Intervals_interval_guid",
                schema: "time",
                table: "DayInWeeks");

            migrationBuilder.DropForeignKey(
                name: "FK_Intervals_Timezones_timezone_guid",
                schema: "time",
                table: "Intervals");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Timezones_guid",
                schema: "time",
                table: "Timezones");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Intervals_guid",
                schema: "time",
                table: "Intervals");

            migrationBuilder.DropIndex(
                name: "IX_Intervals_timezone_guid",
                schema: "time",
                table: "Intervals");

            migrationBuilder.DropIndex(
                name: "IX_DayInWeeks_interval_guid",
                schema: "time",
                table: "DayInWeeks");

            migrationBuilder.DropColumn(
                name: "guid",
                schema: "time",
                table: "Timezones");

            migrationBuilder.DropColumn(
                name: "day_in_week_guid",
                schema: "time",
                table: "Intervals");

            migrationBuilder.DropColumn(
                name: "guid",
                schema: "time",
                table: "Intervals");

            migrationBuilder.DropColumn(
                name: "timezone_guid",
                schema: "time",
                table: "Intervals");

            migrationBuilder.DropColumn(
                name: "guid",
                schema: "time",
                table: "Holidays");

            migrationBuilder.DropColumn(
                name: "guid",
                schema: "time",
                table: "DayInWeeks");

            migrationBuilder.DropColumn(
                name: "interval_guid",
                schema: "time",
                table: "DayInWeeks");

            migrationBuilder.AddColumn<short>(
                name: "component_id",
                schema: "time",
                table: "Intervals",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "time",
                table: "Intervals",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.AddColumn<int>(
                name: "day_in_week_id",
                schema: "time",
                table: "Intervals",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                schema: "time",
                table: "Intervals",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "time",
                table: "Intervals",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "location_id",
                schema: "time",
                table: "Intervals",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "timezone_id",
                schema: "time",
                table: "Intervals",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                schema: "time",
                table: "Intervals",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.AddColumn<short>(
                name: "component_id",
                schema: "time",
                table: "DayInWeeks",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "time",
                table: "DayInWeeks",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.AddColumn<int>(
                name: "interval_id",
                schema: "time",
                table: "DayInWeeks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                schema: "time",
                table: "DayInWeeks",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "time",
                table: "DayInWeeks",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "location_id",
                schema: "time",
                table: "DayInWeeks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                schema: "time",
                table: "DayInWeeks",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.CreateIndex(
                name: "IX_Intervals_timezone_id",
                schema: "time",
                table: "Intervals",
                column: "timezone_id");

            migrationBuilder.CreateIndex(
                name: "IX_DayInWeeks_interval_id",
                schema: "time",
                table: "DayInWeeks",
                column: "interval_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DayInWeeks_Intervals_interval_id",
                schema: "time",
                table: "DayInWeeks",
                column: "interval_id",
                principalSchema: "time",
                principalTable: "Intervals",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Intervals_Timezones_timezone_id",
                schema: "time",
                table: "Intervals",
                column: "timezone_id",
                principalSchema: "time",
                principalTable: "Timezones",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
