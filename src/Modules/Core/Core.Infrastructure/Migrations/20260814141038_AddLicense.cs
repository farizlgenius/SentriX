using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLicense : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "fw",
                schema: "core",
                table: "Modules",
                newName: "firmware");

            migrationBuilder.RenameColumn(
                name: "status",
                schema: "core",
                table: "Devices",
                newName: "firmware");

            migrationBuilder.RenameColumn(
                name: "fw",
                schema: "core",
                table: "Devices",
                newName: "configuration_status");

            migrationBuilder.CreateTable(
                name: "ComponentMappings",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    entity = table.Column<string>(type: "text", nullable: false),
                    internal_guid = table.Column<Guid>(type: "uuid", nullable: false),
                    external_id = table.Column<int>(type: "integer", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    vendor = table.Column<string>(type: "text", nullable: false),
                    location_guid = table.Column<Guid>(type: "uuid", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentMappings", x => x.id);
                    table.ForeignKey(
                        name: "FK_ComponentMappings_Locations_location_guid",
                        column: x => x.location_guid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComponentMappings_location_guid",
                schema: "core",
                table: "ComponentMappings",
                column: "location_guid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComponentMappings",
                schema: "core");

            migrationBuilder.RenameColumn(
                name: "firmware",
                schema: "core",
                table: "Modules",
                newName: "fw");

            migrationBuilder.RenameColumn(
                name: "firmware",
                schema: "core",
                table: "Devices",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "configuration_status",
                schema: "core",
                table: "Devices",
                newName: "fw");
        }
    }
}
