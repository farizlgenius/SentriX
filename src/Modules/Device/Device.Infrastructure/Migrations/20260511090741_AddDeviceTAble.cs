using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Device.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDeviceTAble : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "device");

            migrationBuilder.CreateTable(
                name: "devices",
                schema: "device",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    component_id = table.Column<int>(type: "integer", nullable: false),
                    serial_number = table.Column<string>(type: "text", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    ip = table.Column<string>(type: "text", nullable: false),
                    port = table.Column<int>(type: "integer", nullable: false),
                    fw = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    synced_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    metadata = table.Column<string>(type: "text", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_devices", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "devices",
                schema: "device");
        }
    }
}
