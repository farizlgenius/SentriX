using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Device.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "port",
                schema: "device",
                table: "Modules",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<short>(
                name: "address",
                schema: "device",
                table: "Modules",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<short>(
                name: "component_id",
                schema: "device",
                table: "Modules",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "location_id",
                schema: "device",
                table: "Modules",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "type",
                schema: "device",
                table: "Modules",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<short>(
                name: "component_id",
                schema: "device",
                table: "Devices",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "component_id",
                schema: "device",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "location_id",
                schema: "device",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "type",
                schema: "device",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "component_id",
                schema: "device",
                table: "Devices");

            migrationBuilder.AlterColumn<int>(
                name: "port",
                schema: "device",
                table: "Modules",
                type: "integer",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<int>(
                name: "address",
                schema: "device",
                table: "Modules",
                type: "integer",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");
        }
    }
}
