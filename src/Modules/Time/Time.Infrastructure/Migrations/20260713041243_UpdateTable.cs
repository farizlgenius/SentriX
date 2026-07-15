using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Time.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "day",
                schema: "time",
                table: "Holidays");

            migrationBuilder.DropColumn(
                name: "metadata",
                schema: "time",
                table: "Holidays");

            migrationBuilder.DropColumn(
                name: "month",
                schema: "time",
                table: "Holidays");

            migrationBuilder.DropColumn(
                name: "year",
                schema: "time",
                table: "Holidays");

            migrationBuilder.AddColumn<short>(
                name: "component_id",
                schema: "time",
                table: "Intervals",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<DateTime>(
                name: "end",
                schema: "time",
                table: "Holidays",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "start",
                schema: "time",
                table: "Holidays",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "component_id",
                schema: "time",
                table: "Intervals");

            migrationBuilder.DropColumn(
                name: "end",
                schema: "time",
                table: "Holidays");

            migrationBuilder.DropColumn(
                name: "start",
                schema: "time",
                table: "Holidays");

            migrationBuilder.AddColumn<short>(
                name: "day",
                schema: "time",
                table: "Holidays",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<string>(
                name: "metadata",
                schema: "time",
                table: "Holidays",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<short>(
                name: "month",
                schema: "time",
                table: "Holidays",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "year",
                schema: "time",
                table: "Holidays",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }
    }
}
