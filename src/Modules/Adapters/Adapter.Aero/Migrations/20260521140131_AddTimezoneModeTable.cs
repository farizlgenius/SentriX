using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Adapter.Aero.Migrations
{
    /// <inheritdoc />
    public partial class AddTimezoneModeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimezoneModes",
                schema: "aero",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    label = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimezoneModes", x => x.id);
                });

            migrationBuilder.InsertData(
                schema: "aero",
                table: "TimezoneModes",
                columns: new[] { "id", "description", "label", "value" },
                values: new object[,]
                {
                    { 1, "The time zone is always inactive, regardless of the time zone intervals specified or the holidays in effect.", "Off", 0 },
                    { 2, "The time zone is always active, regardless of the time zone intervals specified or the holidays in effect.", "On", 1 },
                    { 3, "The Time Zone state is decided using either the Day Mask or the Holiday Mask. If the current day is specified as a Holiday, the state relies only on whether the Holiday Mask Flag for that Holiday is set (if today is Holiday 1, and the Holiday Mask sets flag H1, then the state is active. If today is Holiday 1, and the Holiday Mask does not have flag H1 set, then the state is inactive). Holidays override the standard accessibility rules. If the current day is not specified as a Holiday, the Time Zone is active or inactive depending on whether the current day/time falls within the Day Mask. If Day Mask is M-F, 8-5, the Time Zone is active during those times, and inactive on the weekend and outside working hours.", "Scan", 2 },
                    { 4, "Scan time zone interval list and apply only if the date string in expTest matches the current date", "One Time Event", 3 },
                    { 5, "This mode is similar to mode 2, but instead of only checking the Holiday Mask if it is a Holiday, and only checking the Day Mask if not, this mode checks both. If it is not a Holiday, this mode functions normally, only checking the Day Mask. If it is a Holiday, this mode performs a logical OR on the Holiday and Day Masks. If either or both are active, the Time Zone is active, otherwise if neither is active, the Time Zone is inactive.", "Scan, Always Honor Day of Week (OR)", 4 },
                    { 6, "This mode is similar to mode 4, but it performs a logical AND instead of a logical OR. If it is not a Holiday, this mode functions normally, only checking the Day Mask. If it is a Holiday, this mode is only active if BOTH the Day Mask and Holiday Mask are active. If either one is inactive, the entire Time Zone is inactive.", "Scan, Always Honor Day of Week (AND) ", 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimezoneModes",
                schema: "aero");
        }
    }
}
