using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Location.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddComponentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "component_id",
                schema: "location",
                table: "Locations",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "location_id",
                schema: "location",
                table: "Locations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<short>(
                name: "component_id",
                schema: "location",
                table: "Countries",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "location_id",
                schema: "location",
                table: "Countries",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 10,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 11,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 12,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 13,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 14,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 15,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 16,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 17,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 18,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 19,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 20,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 21,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 22,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 23,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 24,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 25,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 26,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 27,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 28,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 29,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 30,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 31,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 32,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 33,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 34,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 35,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 36,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 37,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 38,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 39,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 40,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 41,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 42,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 43,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 44,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 45,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 46,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 47,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 48,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 49,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 50,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 51,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 52,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 53,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 54,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 55,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 56,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 57,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 58,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 59,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 60,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 61,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 62,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 63,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 64,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 65,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 66,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 67,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 68,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 69,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 70,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 71,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 72,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 73,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 74,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 75,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 76,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 77,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 78,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 79,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 80,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 81,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 82,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 83,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 84,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 85,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 86,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 87,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 88,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 89,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 90,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 91,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 92,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 93,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 94,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 95,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 96,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 97,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 98,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 99,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 100,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 101,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 102,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 103,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 104,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 105,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 106,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 107,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 108,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 109,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 110,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 111,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 112,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 113,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 114,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 115,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 116,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 117,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 118,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 119,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 120,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 121,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 122,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 123,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 124,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 125,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 126,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 127,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 128,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 129,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 130,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 131,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 132,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 133,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 134,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 135,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 136,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 137,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 138,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 139,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 140,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 141,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 142,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 143,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 144,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 145,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 146,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 147,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 148,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 149,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 150,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 151,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 152,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 153,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 154,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 155,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 156,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 157,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 158,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 159,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 160,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 161,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 162,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 163,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 164,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 165,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 166,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 167,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 168,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 169,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 170,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 171,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 172,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 173,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 174,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 175,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 176,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 177,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Countries",
                keyColumn: "id",
                keyValue: 178,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "location",
                table: "Locations",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "component_id",
                schema: "location",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "location_id",
                schema: "location",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "component_id",
                schema: "location",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "location_id",
                schema: "location",
                table: "Countries");
        }
    }
}
