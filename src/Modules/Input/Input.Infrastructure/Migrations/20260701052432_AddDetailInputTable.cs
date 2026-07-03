using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Input.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDetailInputTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "metadata",
                schema: "input",
                table: "Inputs");

            migrationBuilder.AddColumn<short>(
                name: "debounce",
                schema: "input",
                table: "Inputs",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "delay_entry",
                schema: "input",
                table: "Inputs",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "delay_exit",
                schema: "input",
                table: "Inputs",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "hold_time",
                schema: "input",
                table: "Inputs",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "latch_mode",
                schema: "input",
                table: "Inputs",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "log_function",
                schema: "input",
                table: "Inputs",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "sensor_mode",
                schema: "input",
                table: "Inputs",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "debounce",
                schema: "input",
                table: "Inputs");

            migrationBuilder.DropColumn(
                name: "delay_entry",
                schema: "input",
                table: "Inputs");

            migrationBuilder.DropColumn(
                name: "delay_exit",
                schema: "input",
                table: "Inputs");

            migrationBuilder.DropColumn(
                name: "hold_time",
                schema: "input",
                table: "Inputs");

            migrationBuilder.DropColumn(
                name: "latch_mode",
                schema: "input",
                table: "Inputs");

            migrationBuilder.DropColumn(
                name: "log_function",
                schema: "input",
                table: "Inputs");

            migrationBuilder.DropColumn(
                name: "sensor_mode",
                schema: "input",
                table: "Inputs");

            migrationBuilder.AddColumn<string>(
                name: "metadata",
                schema: "input",
                table: "Inputs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
