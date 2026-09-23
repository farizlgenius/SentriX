using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Setting.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddElvlAeroSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "max_elalvl",
                schema: "setting",
                table: "AeroDriverSettings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "max_floor_per_acr",
                schema: "setting",
                table: "AeroDriverSettings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "setting",
                table: "AeroDriverSettings",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "max_elalvl", "max_floor_per_acr" },
                values: new object[] { 0, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "max_elalvl",
                schema: "setting",
                table: "AeroDriverSettings");

            migrationBuilder.DropColumn(
                name: "max_floor_per_acr",
                schema: "setting",
                table: "AeroDriverSettings");
        }
    }
}
