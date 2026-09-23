using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Setting.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddElvlAeroSettingValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "setting",
                table: "AeroDriverSettings",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "max_elalvl", "max_floor_per_acr" },
                values: new object[] { 256, 128 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "setting",
                table: "AeroDriverSettings",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "max_elalvl", "max_floor_per_acr" },
                values: new object[] { 0, 0 });
        }
    }
}
