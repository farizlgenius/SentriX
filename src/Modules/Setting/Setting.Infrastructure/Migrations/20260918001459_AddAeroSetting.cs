using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Setting.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAeroSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "apb_time",
                schema: "setting",
                table: "AeroDriverSettings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "host_timeout",
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
                columns: new[] { "apb_time", "host_timeout" },
                values: new object[] { true, 5 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "apb_time",
                schema: "setting",
                table: "AeroDriverSettings");

            migrationBuilder.DropColumn(
                name: "host_timeout",
                schema: "setting",
                table: "AeroDriverSettings");
        }
    }
}
