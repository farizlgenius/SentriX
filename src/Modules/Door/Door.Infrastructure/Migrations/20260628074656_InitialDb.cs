using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Door.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "door");

            migrationBuilder.CreateTable(
                name: "AccessControlFlags",
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
                    table.PrimaryKey("PK_AccessControlFlags", x => x.id);
                });

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
                name: "Doors",
                schema: "door",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    device_component_id = table.Column<short>(type: "smallint", nullable: false),
                    second_component_id = table.Column<short>(type: "smallint", nullable: false),
                    door_type = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    metadata = table.Column<string>(type: "text", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doors", x => x.id);
                });

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
                name: "SpareFlags",
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
                    table.PrimaryKey("PK_SpareFlags", x => x.id);
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
                table: "AccessControlFlags",
                columns: new[] { "id", "component_id", "description", "is_active", "label", "location_id", "value" },
                values: new object[,]
                {
                    { 1, (short)0, "Decrement use limits on access", true, "Decrement Use Limit", 0, 1 },
                    { 2, (short)0, "Require use limit to be non-zero", true, "Require use limit", 0, 2 },
                    { 3, (short)0, "Set to deny a duress request. The default behavior is to grant access under duress and log event. ", true, "Deny duress", 0, 4 },
                    { 4, (short)0, "Do not wait for door to open. Assume that the door was used and log all access requests as used as soon as the request is granted.", true, "Not wait door open", 0, 8 },
                    { 5, (short)0, "Do not pulse the door strike on REX cycle. Used for “quiet” exit.", true, "Quiet REX", 0, 16 },
                    { 6, (short)0, "Filter Change-of-state Door transactions. This flag is normally set,unless detailed door sequence notifications are required.", true, "Filter door transaction", 0, 32 },
                    { 7, (short)0, "Require two-card control at this reader.", true, "2 Card require", 0, 64 },
                    { 8, (short)0, "If online, check with HOST before GRANTING access.", true, "Require host confirm", 0, 1024 },
                    { 9, (short)0, "If HOST is not available (offline or timeout) proceed with GRANT.", true, "Always grant if offline", 0, 2048 },
                    { 10, (short)0, "Enable cipher mode (if user command fits a card format then use it as card). Allows user to enter digits through the keypad as card number.", true, "Cipher mode", 0, 4096 },
                    { 11, (short)0, "If set, log access grant transaction right away, then log used/not-used. This feature disabled when the ACR_F_ALLUSED (0x0008) access control flag is set.", true, "Log early", 0, 16384 },
                    { 12, (short)0, "If set, show “wait” pattern on “card not in file” instead of “denied” response. See Command 122: Reader LED/Buzzer Function Specs “wait” state.", true, "Wait pattern", 0, 32768 }
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
                table: "SpareFlags",
                columns: new[] { "id", "component_id", "description", "is_active", "label", "location_id", "value" },
                values: new object[,]
                {
                    { 1, (short)0, "On a new access grant, do not resume the extended door held open timer", true, "No extend held timer", 0, 1 },
                    { 2, (short)0, "Card and PIN reader mode: Do not accept PIN followed by CARD. Forces CARD to be read first.", true, "Force card before PIN", 0, 2 },
                    { 3, (short)0, "Enable “Door Forced Open Filter”. Opening door within 3 seconds of door closed will not report a door forced open.", true, "Door Forced Filter", 0, 8 },
                    { 4, (short)0, "Do not process any access request. Reports all access requests as “Access Denied, Door Locked”.", true, "No request", 0, 16 },
                    { 5, (short)0, "Relay #(strike_rly+1) becomes the 'shunt relay'. On door unlocked, the shunt relay is activated 5 ms before the strike relay. The shunt relay is deactivated 1 second after the door is closed or the held open timer expires. The dc_held field must be greater than 1 for the shunt relay to function correctly.", true, "Shunt relay", 0, 32 },
                    { 6, (short)0, "Enables “output selection tracking” feature when reader is configured for elevator type 1 and the reader is also in Card and PIN mode. Instead of entering a PIN code at the reader, the floor/output number would be entered instead.", true, "Output Selection Tracking", 0, 64 },
                    { 7, (short)0, "Enables “output selection tracking” feature when reader is configured for elevator type 1 and the reader is also in Card and PIN mode. Instead of entering a PIN code at the reader, the floor/output number would be entered instead.", true, "Link mode", 0, 128 },
                    { 8, (short)0, "Flag that enables the ability to use the double card functionality at this ACR. Presenting a valid card that has rights at the ACR twice within 5 seconds will generate a double card transaction.", true, "Double Card", 0, 256 },
                    { 9, (short)0, "Flag that allows for override credentials to gain access to this ACR even when in locked state. Override credentials are configured using Free Form Field type FFRM_FLD_ACCESSFLGS.", true, "Override Credential", 0, 1024 },
                    { 10, (short)0, "Flag indicating if this ACR allows the disabling of elevator floors via the offline_mode field. Applies only to Type 1 and Type 2 elevators.", true, "Disable Elevator Floor", 0, 2048 },
                    { 11, (short)0, "Flag that indicates if ACR is in linking mode for alternate reader, acr_mode = 32 will start linking mode and acr_mode = 33 can abort linking mode or once reader is linked or timeout reached this flag will clear.", true, "Link mode Alt", 0, 4096 },
                    { 12, (short)0, "lag to enable extending REX 'grant time' while REX input is active", true, "Extend REX", 0, 8192 },
                    { 13, (short)0, "ACR_F_HOST_CBG must also be enabled for this flag to take effect. When both flags are active, the controller bypasses its local database check and for a grant decision. The host can respond with a grant or deny, which will be processed by the controller. If the host does not respond in time, the process times out, and the controller performs a secondary check using the local controller database. During a timeout, if the card is present in the local controller database and valid for the ACR/time, a grant is locally issued by the controller. Otherwisea deny is issued. This mode works with PIN codes if the ACR is configured into the Card and PIN reader mode.", true, "Controller Bypass", 0, 16384 },
                    { 14, (short)0, "Flag to enable generating a transaction at the start of the REX cycle.", true, "Early REX", 0, 32768 }
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
                name: "AccessControlFlags",
                schema: "door");

            migrationBuilder.DropTable(
                name: "ApbModes",
                schema: "door");

            migrationBuilder.DropTable(
                name: "DoorModes",
                schema: "door");

            migrationBuilder.DropTable(
                name: "Doors",
                schema: "door");

            migrationBuilder.DropTable(
                name: "OsdpBaudrates",
                schema: "door");

            migrationBuilder.DropTable(
                name: "ReaderModes",
                schema: "door");

            migrationBuilder.DropTable(
                name: "SpareFlags",
                schema: "door");

            migrationBuilder.DropTable(
                name: "StrikeModes",
                schema: "door");
        }
    }
}
