using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Time.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "time");

            migrationBuilder.CreateTable(
                name: "Holidays",
                schema: "time",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    start = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holidays", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Timezones",
                schema: "time",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timezones", x => x.id);
                    table.UniqueConstraint("AK_Timezones_guid", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "Intervals",
                schema: "time",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guid = table.Column<Guid>(type: "uuid", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    start = table.Column<string>(type: "text", nullable: false),
                    end = table.Column<string>(type: "text", nullable: false),
                    timezone_guid = table.Column<Guid>(type: "uuid", nullable: false),
                    day_in_week_guid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Intervals", x => x.id);
                    table.UniqueConstraint("AK_Intervals_guid", x => x.guid);
                    table.ForeignKey(
                        name: "FK_Intervals_Timezones_timezone_guid",
                        column: x => x.timezone_guid,
                        principalSchema: "time",
                        principalTable: "Timezones",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DayInWeeks",
                schema: "time",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guid = table.Column<Guid>(type: "uuid", nullable: false),
                    sunday = table.Column<bool>(type: "boolean", nullable: false),
                    monday = table.Column<bool>(type: "boolean", nullable: false),
                    tuesday = table.Column<bool>(type: "boolean", nullable: false),
                    wednesday = table.Column<bool>(type: "boolean", nullable: false),
                    thursday = table.Column<bool>(type: "boolean", nullable: false),
                    friday = table.Column<bool>(type: "boolean", nullable: false),
                    saturday = table.Column<bool>(type: "boolean", nullable: false),
                    interval_guid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayInWeeks", x => x.id);
                    table.ForeignKey(
                        name: "FK_DayInWeeks_Intervals_interval_guid",
                        column: x => x.interval_guid,
                        principalSchema: "time",
                        principalTable: "Intervals",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "time",
                table: "Timezones",
                columns: new[] { "id", "component_id", "guid", "is_active", "is_default", "location_id", "name" },
                values: new object[,]
                {
                    { 1, (short)1, new Guid("65dd168c-e53f-4f42-a8c0-d83487bfb321"), true, true, -1, "Default" },
                    { 2, (short)2, new Guid("9b6e1f89-6f6e-4c5d-a0a5-c9d6f5d18e7b"), true, true, 0, "Always" },
                    { 3, (short)3, new Guid("6ce6a36f-a898-4f14-a198-1b85aa43834e"), true, true, 0, "Never" }
                });

            migrationBuilder.InsertData(
                schema: "time",
                table: "Intervals",
                columns: new[] { "id", "component_id", "day_in_week_guid", "end", "guid", "start", "timezone_guid" },
                values: new object[,]
                {
                    { 1, (short)1, new Guid("00000000-0000-0000-0000-000000000000"), "23:00", new Guid("65364114-fd3b-43f4-8710-ce62655fb44d"), "00:00", new Guid("65dd168c-e53f-4f42-a8c0-d83487bfb321") },
                    { 2, (short)2, new Guid("4e7a2d90-3b8f-4fd8-9c57-2a1e6b9d8f43"), "23:00", new Guid("f2d4c8b3-91aa-4b4c-8e1d-73c1f9b2a6d4"), "00:00", new Guid("9b6e1f89-6f6e-4c5d-a0a5-c9d6f5d18e7b") }
                });

            migrationBuilder.InsertData(
                schema: "time",
                table: "DayInWeeks",
                columns: new[] { "id", "friday", "guid", "interval_guid", "monday", "saturday", "sunday", "thursday", "tuesday", "wednesday" },
                values: new object[] { 1, true, new Guid("4e7a2d90-3b8f-4fd8-9c57-2a1e6b9d8f43"), new Guid("f2d4c8b3-91aa-4b4c-8e1d-73c1f9b2a6d4"), true, true, true, true, true, true });

            migrationBuilder.CreateIndex(
                name: "IX_DayInWeeks_interval_guid",
                schema: "time",
                table: "DayInWeeks",
                column: "interval_guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Intervals_timezone_guid",
                schema: "time",
                table: "Intervals",
                column: "timezone_guid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DayInWeeks",
                schema: "time");

            migrationBuilder.DropTable(
                name: "Holidays",
                schema: "time");

            migrationBuilder.DropTable(
                name: "Intervals",
                schema: "time");

            migrationBuilder.DropTable(
                name: "Timezones",
                schema: "time");
        }
    }
}
