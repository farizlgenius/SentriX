using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Output.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultPulse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "metadata",
                schema: "output",
                table: "Outputs");

            migrationBuilder.AlterColumn<short>(
                name: "output_no",
                schema: "output",
                table: "Outputs",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<short>(
                name: "default_pulse",
                schema: "output",
                table: "Outputs",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "default_pulse",
                schema: "output",
                table: "Outputs");

            migrationBuilder.AlterColumn<int>(
                name: "output_no",
                schema: "output",
                table: "Outputs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AddColumn<string>(
                name: "metadata",
                schema: "output",
                table: "Outputs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
