using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Output.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMacToTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "module_id",
                schema: "output",
                table: "Outputs");

            migrationBuilder.AddColumn<short>(
                name: "component_id",
                schema: "output",
                table: "Outputs",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "device_component_id",
                schema: "output",
                table: "Outputs",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<string>(
                name: "mac",
                schema: "output",
                table: "Outputs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<short>(
                name: "mode",
                schema: "output",
                table: "Outputs",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "module_component_id",
                schema: "output",
                table: "Outputs",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<string>(
                name: "type",
                schema: "output",
                table: "Outputs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "component_id",
                schema: "output",
                table: "Outputs");

            migrationBuilder.DropColumn(
                name: "device_component_id",
                schema: "output",
                table: "Outputs");

            migrationBuilder.DropColumn(
                name: "mac",
                schema: "output",
                table: "Outputs");

            migrationBuilder.DropColumn(
                name: "mode",
                schema: "output",
                table: "Outputs");

            migrationBuilder.DropColumn(
                name: "module_component_id",
                schema: "output",
                table: "Outputs");

            migrationBuilder.DropColumn(
                name: "type",
                schema: "output",
                table: "Outputs");

            migrationBuilder.AddColumn<int>(
                name: "module_id",
                schema: "output",
                table: "Outputs",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
