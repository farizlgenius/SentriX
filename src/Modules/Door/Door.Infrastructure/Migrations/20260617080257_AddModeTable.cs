using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Door.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddModeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApbModes",
                schema: "door",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    label = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<short>(type: "smallint", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApbModes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DoorModes",
                schema: "door",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    label = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<short>(type: "smallint", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoorModes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ReaderModes",
                schema: "door",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    label = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<short>(type: "smallint", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReaderModes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "StrikeModes",
                schema: "door",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    label = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<short>(type: "smallint", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StrikeModes", x => x.id);
                });

            migrationBuilder.InsertData(
                schema: "door",
                table: "ApbModes",
                columns: new[] { "id", "component_id", "description", "is_active", "label", "location_id", "value" },
                values: new object[,]
                {
                    { 1, (short)0, "Do not check or alter anti-passback location. No anti-passback rules.", true, "No Apb", 0, (short)0 },
                    { 2, (short)0, "Soft anti-passback: Accept any new location, change the user’s location to current reader, and generate an anti-passback violation for an invalid entry.", true, "Soft", 0, (short)1 },
                    { 3, (short)0, "Hard anti-passback: Check user location, if a valid entry is made, change user’s location to new location. If an invalid entry is attempted, do not grant access.", true, "Hard", 0, (short)2 },
                    { 4, (short)0, "Reader-based anti-passback using the ACR’s last valid user. Verify it’s not the same user within the time parameter specified within apb_delay.", true, "Reader-based Last Valid (s)", 0, (short)3 },
                    { 5, (short)0, "Reader-based anti-passback using the access history from the cardholder database: Check user’s last ACR used, checks for same reader within a specified time (apb_delay). This requires the bSupportTimeApb flag be set in Command 1105: Access Database Specification.", true, "Reader-based Access History (s)", 0, (short)4 },
                    { 6, (short)0, "Area based anti-passback: Check user’s current location, if it does not match the expected location then check the delay time (apb_delay). Change user’s location on entry. This requires the bSupportTimeApb flag be set in Command 1105: Access Database Specification.", true, "Area-based (s)", 0, (short)5 },
                    { 7, (short)0, "Reader-based anti-passback using the ACR’s last valid user. Verify it’s not the same user within the time parameter specified within apb_delay.", true, "Reader-based Last Valid (m)", 0, (short)6 },
                    { 8, (short)0, "Reader-based anti-passback using the access history from the cardholder database: Check user’s last ACR used, checks for same reader within a specified time (apb_delay). This requires the bSupportTimeApb flag be set in Command 1105: Access Database Specification.", true, "Reader-based Access History (s)", 0, (short)7 },
                    { 9, (short)0, "Area based anti-passback: Check user’s current location, if it does not match the expected location then check the delay time (apb_delay). Change user’s location on entry. This requires the bSupportTimeApb flag be set in Command 1105: Access Database Specification.", true, "Area-based (m)", 0, (short)8 }
                });

            migrationBuilder.InsertData(
                schema: "door",
                table: "DoorModes",
                columns: new[] { "id", "component_id", "description", "is_active", "label", "location_id", "value" },
                values: new object[,]
                {
                    { 1, (short)0, "Disable the ACR, no REX", true, "Disable", 0, (short)1 },
                    { 2, (short)0, "Unlock (unlimited access)", true, "Unlock", 0, (short)2 },
                    { 3, (short)0, "Locked (no access,REX active)", true, "Lock", 0, (short)3 },
                    { 4, (short)0, "Facility code only", true, "FAC Only", 0, (short)4 },
                    { 5, (short)0, "Card Only", true, "Card Only", 0, (short)5 },
                    { 6, (short)0, "PIN Only", true, "PIN Only", 0, (short)6 },
                    { 7, (short)0, "Card and PIN required", true, "Card and PIN", 0, (short)7 },
                    { 8, (short)0, "Card or PIN required", true, "Card or PIN", 0, (short)8 }
                });

            migrationBuilder.InsertData(
                schema: "door",
                table: "ReaderModes",
                columns: new[] { "id", "component_id", "description", "is_active", "label", "location_id", "value" },
                values: new object[,]
                {
                    { 1, (short)0, "Single reader, controlling the door", true, "Single", 0, (short)0 },
                    { 2, (short)0, "Paired readers, Primary - this reader controls the door", true, "Dual", 0, (short)1 },
                    { 3, (short)0, "Turnstile Reader", true, "Turnstile", 0, (short)3 },
                    { 4, (short)0, "Elevator, no floor select feedback *", true, "Elevator No Floor", 0, (short)4 },
                    { 5, (short)0, "Elevator with floor select feedback *", true, "Elevator with Floor", 0, (short)5 }
                });

            migrationBuilder.InsertData(
                schema: "door",
                table: "StrikeModes",
                columns: new[] { "id", "component_id", "description", "is_active", "label", "location_id", "value" },
                values: new object[,]
                {
                    { 1, (short)0, "Do not use! This would allow the strike to stay active for the entire strike time allowing the door to be opened multiple times.", true, "No Change", 0, (short)0 },
                    { 2, (short)0, "Deactivate strike when door opens.", true, "Deactivate on open", 0, (short)1 },
                    { 3, (short)0, "Deactivate strike on door close or strike_t_max expires.", true, "Deactivate on close", 0, (short)2 },
                    { 4, (short)0, "Used with ACR_S_OPEN or ACR_S_CLOSE, to select tailgate mode: pulse (strk_sio:strk_number+1) relay for each user expected to enter.", true, "Tailgate", 0, (short)16 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApbModes",
                schema: "door");

            migrationBuilder.DropTable(
                name: "DoorModes",
                schema: "door");

            migrationBuilder.DropTable(
                name: "ReaderModes",
                schema: "door");

            migrationBuilder.DropTable(
                name: "StrikeModes",
                schema: "door");
        }
    }
}
