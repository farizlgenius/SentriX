using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Location.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "location",
                table: "Locations",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "location",
                table: "Countries",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 1,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 2,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 3,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 4,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 5,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 6,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 7,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 8,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 9,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 10,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 11,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 12,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 13,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 14,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 15,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 16,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 17,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 18,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 19,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 20,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 21,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 22,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 23,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 24,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 25,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 26,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 27,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 28,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 29,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 30,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 31,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 32,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 33,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 34,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 35,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 36,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 37,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 38,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 39,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 40,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 41,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 42,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 43,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 44,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 45,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 46,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 47,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 48,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 49,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 50,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 51,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 52,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 53,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 54,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 55,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 56,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 57,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 58,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 59,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 60,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 61,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 62,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 63,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 64,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 65,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 66,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 67,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 68,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 69,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 70,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 71,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 72,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 73,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 74,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 75,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 76,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 77,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 78,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 79,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 80,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 81,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 82,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 83,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 84,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 85,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 86,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 87,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 88,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 89,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 90,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 91,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 92,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 93,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 94,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 95,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 96,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 97,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 98,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 99,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 100,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 101,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 102,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 103,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 104,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 105,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 106,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 107,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 108,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 109,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 110,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 111,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 112,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 113,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 114,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 115,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 116,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 117,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 118,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 119,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 120,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 121,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 122,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 123,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 124,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 125,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 126,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 127,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 128,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 129,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 130,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 131,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 132,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 133,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 134,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 135,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 136,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 137,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 138,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 139,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 140,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 141,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 142,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 143,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 144,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 145,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 146,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 147,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 148,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 149,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 150,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 151,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 152,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 153,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 154,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 155,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 156,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 157,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 158,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 159,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 160,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 161,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 162,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 163,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 164,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 165,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 166,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 167,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 168,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 169,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 170,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 171,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 172,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 173,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 174,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 175,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 176,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 177,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 178,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Locations",
                keyColumn: "id",
                keyValue: 1,
                column: "is_default",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "location",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "location",
                table: "Countries");
        }
    }
}
