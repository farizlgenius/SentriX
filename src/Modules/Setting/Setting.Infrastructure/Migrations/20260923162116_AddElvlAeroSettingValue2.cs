using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Setting.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddElvlAeroSettingValue2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "setting",
                table: "AeroDriverSettings",
                keyColumn: "id",
                keyValue: 1,
                column: "card_id_size",
                value: 4);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "setting",
                table: "AeroDriverSettings",
                keyColumn: "id",
                keyValue: 1,
                column: "card_id_size",
                value: 0);
        }
    }
}
