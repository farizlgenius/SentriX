using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Door.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOsdpTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OsdpBaudrates",
                schema: "door",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    label = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OsdpBaudrates", x => x.id);
                });

            migrationBuilder.InsertData(
                schema: "door",
                table: "OsdpBaudrates",
                columns: new[] { "id", "component_id", "description", "is_active", "label", "location_id", "value" },
                values: new object[,]
                {
                    { 1, (short)0, "", true, "9600", 0, 9600 },
                    { 2, (short)0, "", true, "19200", 0, 19200 },
                    { 3, (short)0, "", true, "38400", 0, 38400 },
                    { 4, (short)0, "", true, "115200", 0, 115200 },
                    { 5, (short)0, "", true, "57600", 0, 57600 },
                    { 6, (short)0, "", true, "230400", 0, 230400 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OsdpBaudrates",
                schema: "door");
        }
    }
}
