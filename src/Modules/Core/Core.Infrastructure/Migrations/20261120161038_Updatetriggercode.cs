using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Updatetriggercode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 67,
                column: "trigger_type_id",
                value: 34);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 68,
                column: "trigger_type_id",
                value: 34);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 69,
                column: "trigger_type_id",
                value: 34);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 70,
                column: "trigger_type_id",
                value: 34);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 71,
                columns: new[] { "name", "value" },
                values: new object[] { "Disconnected", (short)1 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 72,
                columns: new[] { "name", "value" },
                values: new object[] { "Exit cycle: door use not verified", (short)1 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 73,
                columns: new[] { "name", "value" },
                values: new object[] { "Exit cycle: door not used", (short)2 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 74,
                columns: new[] { "name", "value" },
                values: new object[] { "Exit cycle: door used", (short)3 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 75,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Host initiated request: door use not verified", 8, (short)4 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 76,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Host initiated request: door not used", 8, (short)5 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 77,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Host initiated request: door used", 8, (short)6 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 78,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Exit cycle: started", 8, (short)9 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 79,
                columns: new[] { "name", "value" },
                values: new object[] { "Disconnected", (short)1 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 80,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Unknown", 9, (short)2 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 81,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Secure", 9, (short)3 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 82,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Alarm (forced, held open or both)", 9, (short)4 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 83,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Fault", 9, (short)5 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 84,
                columns: new[] { "name", "value" },
                values: new object[] { "Cancel procedure (abort delay)", (short)1 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 85,
                columns: new[] { "name", "value" },
                values: new object[] { "Execute procedure (start new)", (short)2 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 86,
                columns: new[] { "name", "value" },
                values: new object[] { "Resume procedure, if paused", (short)3 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 87,
                columns: new[] { "name", "value" },
                values: new object[] { "Execute procedure with prefix 256 actions", (short)4 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 88,
                columns: new[] { "name", "value" },
                values: new object[] { "Execute procedure with prefix 512 actions", (short)5 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 89,
                columns: new[] { "name", "value" },
                values: new object[] { "Execute procedure with prefix 1024 actions", (short)6 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 90,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Resume procedure with prefix 256 actions", 10, (short)7 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 91,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Resume procedure with prefix 512 actions", 10, (short)8 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 92,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Resume procedure with prefix 1024 actions", 10, (short)9 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 93,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Command was issued to procedure with no actions - (NOP)", 10, (short)10 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 94,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Command entered by the user", 11, (short)1 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 95,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Became inactive", 12, (short)1 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 96,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Became active", 12, (short)2 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 97,
                columns: new[] { "name", "value" },
                values: new object[] { "Disabled", (short)1 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 98,
                columns: new[] { "name", "value" },
                values: new object[] { "Unlocked", (short)2 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 99,
                columns: new[] { "name", "value" },
                values: new object[] { "Locked (exit request enabled)", (short)3 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 100,
                columns: new[] { "name", "value" },
                values: new object[] { "Facility code only", (short)4 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 101,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Card only", 13, (short)5 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 102,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "PIN only", 13, (short)6 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 103,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Card and PIN", 13, (short)7 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 104,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "PIN or card", 13, (short)8 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 105,
                columns: new[] { "name", "value" },
                values: new object[] { "First disarm command executed (mask_count was 0, all MPs got masked)", (short)1 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 106,
                columns: new[] { "name", "value" },
                values: new object[] { "Subsequent disarm command executed (mask_count incremented, MPs already masked)", (short)2 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 107,
                columns: new[] { "name", "value" },
                values: new object[] { "Override command: armed (mask_count cleared, all points unmasked)", (short)3 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 108,
                columns: new[] { "name", "value" },
                values: new object[] { "Override command: disarmed (mask_count set, unmasked all points)", (short)4 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 109,
                columns: new[] { "name", "value" },
                values: new object[] { "Force arm command, MPG armed, (may have active zones, mask_count is now zero)", (short)5 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 110,
                columns: new[] { "name", "value" },
                values: new object[] { "Force arm command, MPG not armed (mask_count decremented)", (short)6 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 111,
                columns: new[] { "name", "value" },
                values: new object[] { "Standard arm command, MPG armed (did not have active zones, mask_count is now zero)", (short)7 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 112,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Standard arm command, MPG did not arm, (had active zones, mask_count unchanged)", 14, (short)8 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 113,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Standard arm command, MPG still armed, (mask_count decremented)", 14, (short)9 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 114,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Override arm command, MPG armed (mask_count is now zero)", 14, (short)10 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 115,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Override arm command, MPG did not arm, (mask_count decremented)", 14, (short)11 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 116,
                columns: new[] { "name", "value" },
                values: new object[] { "Area disabled", (short)1 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 117,
                columns: new[] { "name", "value" },
                values: new object[] { "Area enabled", (short)2 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 118,
                columns: new[] { "name", "value" },
                values: new object[] { "Occupancy count reached zero", (short)3 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 119,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Occupancy count reached the \"downward-limit\"", 15, (short)4 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 120,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Occupancy count reached the \"upward-limit\"", 15, (short)5 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 121,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Occupancy count reached the \"max-occupancy-limit\"", 15, (short)6 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 122,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Multi-occupancy mode changed", 15, (short)7 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 123,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Use limit changed, reporting new limit", 17, (short)1 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 124,
                columns: new[] { "name", "value" },
                values: new object[] { "Save home notes", (short)1 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 125,
                columns: new[] { "name", "value" },
                values: new object[] { "Save network settings", (short)2 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 126,
                columns: new[] { "name", "value" },
                values: new object[] { "Save host communication settings", (short)3 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 127,
                columns: new[] { "name", "value" },
                values: new object[] { "Add user", (short)4 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 128,
                columns: new[] { "name", "value" },
                values: new object[] { "Delete user", (short)5 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 129,
                columns: new[] { "name", "value" },
                values: new object[] { "Modify user", (short)6 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 130,
                columns: new[] { "name", "value" },
                values: new object[] { "Save password strength and session timer", (short)7 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 131,
                columns: new[] { "name", "value" },
                values: new object[] { "Save web server options", (short)8 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 132,
                columns: new[] { "name", "value" },
                values: new object[] { "Save time server settings", (short)9 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 133,
                columns: new[] { "name", "value" },
                values: new object[] { "Auto save timer settings", (short)10 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 134,
                columns: new[] { "name", "value" },
                values: new object[] { "Load certificate", (short)11 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 135,
                columns: new[] { "name", "value" },
                values: new object[] { "Logged out by link", (short)12 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 136,
                columns: new[] { "name", "value" },
                values: new object[] { "Logged out by timeout", (short)13 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 137,
                columns: new[] { "name", "value" },
                values: new object[] { "Logged out by user", (short)14 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 138,
                columns: new[] { "name", "value" },
                values: new object[] { "Logged out by apply", (short)15 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 139,
                columns: new[] { "name", "value" },
                values: new object[] { "Invalid login", (short)16 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 140,
                columns: new[] { "name", "value" },
                values: new object[] { "Successful login", (short)17 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 141,
                columns: new[] { "name", "value" },
                values: new object[] { "Network diagnostic saved", (short)18 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 142,
                columns: new[] { "name", "value" },
                values: new object[] { "Card DB size saved", (short)19 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 143,
                columns: new[] { "name", "value" },
                values: new object[] { "Diagnostic page saved", (short)21 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 144,
                columns: new[] { "name", "value" },
                values: new object[] { "Security options page saved", (short)22 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 145,
                columns: new[] { "name", "value" },
                values: new object[] { "Add-on package page saved", (short)23 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 146,
                columns: new[] { "name", "value" },
                values: new object[] { "Not used", (short)24 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 147,
                columns: new[] { "name", "value" },
                values: new object[] { "Not used", (short)25 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 148,
                columns: new[] { "name", "value" },
                values: new object[] { "Not used", (short)26 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 149,
                columns: new[] { "name", "value" },
                values: new object[] { "Invalid login limit reached", (short)27 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 150,
                columns: new[] { "name", "value" },
                values: new object[] { "Firmware download initiated", (short)28 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 151,
                columns: new[] { "name", "value" },
                values: new object[] { "Advanced networking routes saved", (short)29 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 152,
                columns: new[] { "name", "value" },
                values: new object[] { "Advanced networking reversion timer started", (short)30 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 153,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Advanced networking reversion timer elapsed", 18, (short)31 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 154,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Advanced networking route changes reverted", 18, (short)32 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 155,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Advanced networking route changes cleared", 18, (short)33 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 156,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Certificate generation started", 18, (short)34 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 157,
                columns: new[] { "name", "value" },
                values: new object[] { "Operating mode changed to mode 0", (short)1 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 158,
                columns: new[] { "name", "value" },
                values: new object[] { "Operating mode changed to mode 1", (short)2 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 159,
                columns: new[] { "name", "value" },
                values: new object[] { "Operating mode changed to mode 2", (short)3 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 160,
                columns: new[] { "name", "value" },
                values: new object[] { "Operating mode changed to mode 3", (short)4 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 161,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Operating mode changed to mode 4", 21, (short)5 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 162,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Operating mode changed to mode 5", 21, (short)6 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 163,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Operating mode changed to mode 6", 21, (short)7 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 164,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Operating mode changed to mode 7", 21, (short)8 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 165,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Floor status is secure", 22, (short)1 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 166,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Floor status is public", 22, (short)2 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 167,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Floor status is disabled (override)", 22, (short)3 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 168,
                columns: new[] { "name", "value" },
                values: new object[] { "File transfer success", (short)1 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 169,
                columns: new[] { "name", "value" },
                values: new object[] { "File transfer error", (short)2 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 170,
                columns: new[] { "name", "value" },
                values: new object[] { "File delete successful", (short)3 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 171,
                columns: new[] { "name", "value" },
                values: new object[] { "File delete unsuccessful", (short)4 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 172,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "OSDP file transfer complete (primary ACR) - look at source number for ACR number", 23, (short)5 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 173,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "OSDP file transfer error (primary ACR) - look at source number for ACR number", 23, (short)6 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 174,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "OSDP file transfer complete (alternate ACR) - look at source number for ACR number", 23, (short)7 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 175,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "OSDP file transfer error (alternate ACR) - look at source number for ACR number", 23, (short)8 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 176,
                columns: new[] { "name", "trigger_type_id" },
                values: new object[] { "Elevator access", 24 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 177,
                columns: new[] { "name", "trigger_type_id" },
                values: new object[] { "Extended status updated", 28 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 178,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Secure / Inactive", 29, (short)3 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 179,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Alarm / Active", 29, (short)4 });

            migrationBuilder.InsertData(
                schema: "core",
                table: "TriggerCodes",
                columns: new[] { "id", "name", "trigger_type_id", "value" },
                values: new object[,]
                {
                    { 180, "Disconnected", 30, (short)1 },
                    { 181, "Unknown", 30, (short)2 },
                    { 182, "Secure", 30, (short)3 },
                    { 183, "Alarm", 30, (short)4 },
                    { 184, "Fault", 30, (short)5 },
                    { 185, "Exit delay in progress", 30, (short)6 },
                    { 186, "Entry delay in progress", 30, (short)7 },
                    { 187, "Disconnected", 31, (short)1 },
                    { 188, "Unknown", 31, (short)2 },
                    { 189, "Secure", 31, (short)3 },
                    { 190, "Alarm", 31, (short)4 },
                    { 191, "Fault", 31, (short)5 },
                    { 192, "Exit delay in progress", 31, (short)6 },
                    { 193, "Entry delay in progress", 31, (short)7 },
                    { 194, "Disconnected", 32, (short)1 },
                    { 195, "Unknown", 32, (short)2 },
                    { 196, "Secure", 32, (short)3 },
                    { 197, "Alarm", 32, (short)4 },
                    { 198, "Fault", 32, (short)5 },
                    { 199, "Exit delay in progress", 32, (short)6 },
                    { 200, "Entry delay in progress", 32, (short)7 },
                    { 201, "Disconnected", 33, (short)1 },
                    { 202, "Unknown", 33, (short)2 },
                    { 203, "Secure", 33, (short)3 },
                    { 204, "Alarm", 33, (short)4 },
                    { 205, "Fault", 33, (short)5 },
                    { 206, "Exit delay in progress", 33, (short)6 },
                    { 207, "Entry delay in progress", 33, (short)7 },
                    { 208, "Disconnected", 34, (short)1 },
                    { 209, "Unknown", 34, (short)2 },
                    { 210, "Secure", 34, (short)3 },
                    { 211, "Alarm", 34, (short)4 },
                    { 212, "Fault", 34, (short)5 },
                    { 213, "Exit delay in progress", 34, (short)6 },
                    { 214, "Entry delay in progress", 34, (short)7 },
                    { 215, "Disconnected", 35, (short)1 },
                    { 216, "Unknown", 35, (short)2 },
                    { 217, "Secure", 35, (short)3 },
                    { 218, "Alarm", 35, (short)4 },
                    { 219, "Fault", 35, (short)5 },
                    { 220, "Exit delay in progress", 35, (short)6 },
                    { 221, "Entry delay in progress", 35, (short)7 },
                    { 222, "Disconnected", 36, (short)1 },
                    { 223, "Unknown", 36, (short)2 },
                    { 224, "Secure", 36, (short)3 },
                    { 225, "Alarm", 36, (short)4 },
                    { 226, "Fault", 36, (short)5 },
                    { 227, "Exit delay in progress", 36, (short)6 },
                    { 228, "Entry delay in progress", 36, (short)7 },
                    { 229, "Disconnected", 37, (short)1 },
                    { 230, "Unknown", 37, (short)2 },
                    { 231, "Secure", 37, (short)3 },
                    { 232, "Alarm", 37, (short)4 },
                    { 233, "Fault", 37, (short)5 },
                    { 234, "Exit delay in progress", 37, (short)6 },
                    { 235, "Entry delay in progress", 37, (short)7 },
                    { 236, "Disconnected", 38, (short)1 },
                    { 237, "Unknown", 38, (short)2 },
                    { 238, "Secure", 38, (short)3 },
                    { 239, "Alarm", 38, (short)4 },
                    { 240, "Fault", 38, (short)5 },
                    { 241, "Exit delay in progress", 38, (short)6 },
                    { 242, "Entry delay in progress", 38, (short)7 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 180);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 181);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 182);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 183);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 184);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 185);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 186);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 187);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 188);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 189);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 190);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 191);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 192);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 193);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 194);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 195);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 196);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 197);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 198);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 199);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 200);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 201);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 202);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 203);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 204);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 205);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 206);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 207);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 208);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 209);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 210);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 211);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 212);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 213);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 214);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 215);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 216);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 217);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 218);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 219);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 220);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 221);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 222);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 223);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 224);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 225);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 226);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 227);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 228);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 229);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 230);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 231);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 232);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 233);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 234);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 235);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 236);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 237);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 238);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 239);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 240);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 241);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 242);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 67,
                column: "trigger_type_id",
                value: 7);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 68,
                column: "trigger_type_id",
                value: 7);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 69,
                column: "trigger_type_id",
                value: 7);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 70,
                column: "trigger_type_id",
                value: 7);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 71,
                columns: new[] { "name", "value" },
                values: new object[] { "Host initiated request: door use not verified", (short)4 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 72,
                columns: new[] { "name", "value" },
                values: new object[] { "Host initiated request: door not used", (short)5 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 73,
                columns: new[] { "name", "value" },
                values: new object[] { "Host initiated request: door used", (short)6 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 74,
                columns: new[] { "name", "value" },
                values: new object[] { "Exit cycle: started", (short)9 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 75,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Disconnected", 9, (short)1 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 76,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Unknown", 9, (short)2 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 77,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Secure", 9, (short)3 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 78,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Alarm (forced, held open or both)", 9, (short)4 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 79,
                columns: new[] { "name", "value" },
                values: new object[] { "Fault", (short)5 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 80,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Cancel procedure (abort delay)", 10, (short)1 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 81,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Execute procedure (start new)", 10, (short)2 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 82,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Resume procedure, if paused", 10, (short)3 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 83,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Execute procedure with prefix 256 actions", 10, (short)4 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 84,
                columns: new[] { "name", "value" },
                values: new object[] { "Execute procedure with prefix 512 actions", (short)5 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 85,
                columns: new[] { "name", "value" },
                values: new object[] { "Execute procedure with prefix 1024 actions", (short)6 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 86,
                columns: new[] { "name", "value" },
                values: new object[] { "Resume procedure with prefix 256 actions", (short)7 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 87,
                columns: new[] { "name", "value" },
                values: new object[] { "Resume procedure with prefix 512 actions", (short)8 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 88,
                columns: new[] { "name", "value" },
                values: new object[] { "Resume procedure with prefix 1024 actions", (short)9 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 89,
                columns: new[] { "name", "value" },
                values: new object[] { "Command was issued to procedure with no actions - (NOP)", (short)10 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 90,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Command entered by the user", 11, (short)1 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 91,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Became inactive", 12, (short)1 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 92,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Became active", 12, (short)2 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 93,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Disabled", 13, (short)1 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 94,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Unlocked", 13, (short)2 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 95,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Locked (exit request enabled)", 13, (short)3 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 96,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Facility code only", 13, (short)4 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 97,
                columns: new[] { "name", "value" },
                values: new object[] { "Card only", (short)5 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 98,
                columns: new[] { "name", "value" },
                values: new object[] { "PIN only", (short)6 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 99,
                columns: new[] { "name", "value" },
                values: new object[] { "Card and PIN", (short)7 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 100,
                columns: new[] { "name", "value" },
                values: new object[] { "PIN or card", (short)8 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 101,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "First disarm command executed (mask_count was 0, all MPs got masked)", 14, (short)1 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 102,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Subsequent disarm command executed (mask_count incremented, MPs already masked)", 14, (short)2 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 103,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Override command: armed (mask_count cleared, all points unmasked)", 14, (short)3 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 104,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Override command: disarmed (mask_count set, unmasked all points)", 14, (short)4 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 105,
                columns: new[] { "name", "value" },
                values: new object[] { "Force arm command, MPG armed, (may have active zones, mask_count is now zero)", (short)5 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 106,
                columns: new[] { "name", "value" },
                values: new object[] { "Force arm command, MPG not armed (mask_count decremented)", (short)6 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 107,
                columns: new[] { "name", "value" },
                values: new object[] { "Standard arm command, MPG armed (did not have active zones, mask_count is now zero)", (short)7 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 108,
                columns: new[] { "name", "value" },
                values: new object[] { "Standard arm command, MPG did not arm, (had active zones, mask_count unchanged)", (short)8 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 109,
                columns: new[] { "name", "value" },
                values: new object[] { "Standard arm command, MPG still armed, (mask_count decremented)", (short)9 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 110,
                columns: new[] { "name", "value" },
                values: new object[] { "Override arm command, MPG armed (mask_count is now zero)", (short)10 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 111,
                columns: new[] { "name", "value" },
                values: new object[] { "Override arm command, MPG did not arm, (mask_count decremented)", (short)11 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 112,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Area disabled", 15, (short)1 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 113,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Area enabled", 15, (short)2 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 114,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Occupancy count reached zero", 15, (short)3 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 115,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Occupancy count reached the \"downward-limit\"", 15, (short)4 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 116,
                columns: new[] { "name", "value" },
                values: new object[] { "Occupancy count reached the \"upward-limit\"", (short)5 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 117,
                columns: new[] { "name", "value" },
                values: new object[] { "Occupancy count reached the \"max-occupancy-limit\"", (short)6 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 118,
                columns: new[] { "name", "value" },
                values: new object[] { "Multi-occupancy mode changed", (short)7 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 119,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Use limit changed, reporting new limit", 17, (short)1 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 120,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Save home notes", 18, (short)1 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 121,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Save network settings", 18, (short)2 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 122,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Save host communication settings", 18, (short)3 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 123,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Add user", 18, (short)4 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 124,
                columns: new[] { "name", "value" },
                values: new object[] { "Delete user", (short)5 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 125,
                columns: new[] { "name", "value" },
                values: new object[] { "Modify user", (short)6 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 126,
                columns: new[] { "name", "value" },
                values: new object[] { "Save password strength and session timer", (short)7 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 127,
                columns: new[] { "name", "value" },
                values: new object[] { "Save web server options", (short)8 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 128,
                columns: new[] { "name", "value" },
                values: new object[] { "Save time server settings", (short)9 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 129,
                columns: new[] { "name", "value" },
                values: new object[] { "Auto save timer settings", (short)10 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 130,
                columns: new[] { "name", "value" },
                values: new object[] { "Load certificate", (short)11 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 131,
                columns: new[] { "name", "value" },
                values: new object[] { "Logged out by link", (short)12 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 132,
                columns: new[] { "name", "value" },
                values: new object[] { "Logged out by timeout", (short)13 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 133,
                columns: new[] { "name", "value" },
                values: new object[] { "Logged out by user", (short)14 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 134,
                columns: new[] { "name", "value" },
                values: new object[] { "Logged out by apply", (short)15 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 135,
                columns: new[] { "name", "value" },
                values: new object[] { "Invalid login", (short)16 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 136,
                columns: new[] { "name", "value" },
                values: new object[] { "Successful login", (short)17 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 137,
                columns: new[] { "name", "value" },
                values: new object[] { "Network diagnostic saved", (short)18 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 138,
                columns: new[] { "name", "value" },
                values: new object[] { "Card DB size saved", (short)19 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 139,
                columns: new[] { "name", "value" },
                values: new object[] { "Diagnostic page saved", (short)21 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 140,
                columns: new[] { "name", "value" },
                values: new object[] { "Security options page saved", (short)22 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 141,
                columns: new[] { "name", "value" },
                values: new object[] { "Add-on package page saved", (short)23 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 142,
                columns: new[] { "name", "value" },
                values: new object[] { "Not used", (short)24 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 143,
                columns: new[] { "name", "value" },
                values: new object[] { "Not used", (short)25 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 144,
                columns: new[] { "name", "value" },
                values: new object[] { "Not used", (short)26 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 145,
                columns: new[] { "name", "value" },
                values: new object[] { "Invalid login limit reached", (short)27 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 146,
                columns: new[] { "name", "value" },
                values: new object[] { "Firmware download initiated", (short)28 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 147,
                columns: new[] { "name", "value" },
                values: new object[] { "Advanced networking routes saved", (short)29 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 148,
                columns: new[] { "name", "value" },
                values: new object[] { "Advanced networking reversion timer started", (short)30 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 149,
                columns: new[] { "name", "value" },
                values: new object[] { "Advanced networking reversion timer elapsed", (short)31 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 150,
                columns: new[] { "name", "value" },
                values: new object[] { "Advanced networking route changes reverted", (short)32 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 151,
                columns: new[] { "name", "value" },
                values: new object[] { "Advanced networking route changes cleared", (short)33 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 152,
                columns: new[] { "name", "value" },
                values: new object[] { "Certificate generation started", (short)34 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 153,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Operating mode changed to mode 0", 21, (short)1 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 154,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Operating mode changed to mode 1", 21, (short)2 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 155,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Operating mode changed to mode 2", 21, (short)3 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 156,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Operating mode changed to mode 3", 21, (short)4 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 157,
                columns: new[] { "name", "value" },
                values: new object[] { "Operating mode changed to mode 4", (short)5 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 158,
                columns: new[] { "name", "value" },
                values: new object[] { "Operating mode changed to mode 5", (short)6 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 159,
                columns: new[] { "name", "value" },
                values: new object[] { "Operating mode changed to mode 6", (short)7 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 160,
                columns: new[] { "name", "value" },
                values: new object[] { "Operating mode changed to mode 7", (short)8 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 161,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Floor status is secure", 22, (short)1 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 162,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Floor status is public", 22, (short)2 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 163,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Floor status is disabled (override)", 22, (short)3 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 164,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "File transfer success", 23, (short)1 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 165,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "File transfer error", 23, (short)2 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 166,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "File delete successful", 23, (short)3 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 167,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "File delete unsuccessful", 23, (short)4 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 168,
                columns: new[] { "name", "value" },
                values: new object[] { "OSDP file transfer complete (primary ACR) - look at source number for ACR number", (short)5 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 169,
                columns: new[] { "name", "value" },
                values: new object[] { "OSDP file transfer error (primary ACR) - look at source number for ACR number", (short)6 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 170,
                columns: new[] { "name", "value" },
                values: new object[] { "OSDP file transfer complete (alternate ACR) - look at source number for ACR number", (short)7 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 171,
                columns: new[] { "name", "value" },
                values: new object[] { "OSDP file transfer error (alternate ACR) - look at source number for ACR number", (short)8 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 172,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Elevator access", 24, (short)1 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 173,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Extended status updated", 28, (short)1 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 174,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Secure / Inactive", 29, (short)3 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 175,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Alarm / Active", 29, (short)4 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 176,
                columns: new[] { "name", "trigger_type_id" },
                values: new object[] { "Disconnected", 8 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 177,
                columns: new[] { "name", "trigger_type_id" },
                values: new object[] { "Exit cycle: door use not verified", 8 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 178,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Exit cycle: door not used", 8, (short)2 });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerCodes",
                keyColumn: "id",
                keyValue: 179,
                columns: new[] { "name", "trigger_type_id", "value" },
                values: new object[] { "Exit cycle: door used", 8, (short)3 });
        }
    }
}
