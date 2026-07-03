using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Output.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditRelayMode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "mode",
                schema: "output",
                table: "Outputs",
                newName: "offline_mode");

            migrationBuilder.AddColumn<short>(
                name: "drive_mode",
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
                name: "drive_mode",
                schema: "output",
                table: "Outputs");

            migrationBuilder.RenameColumn(
                name: "offline_mode",
                schema: "output",
                table: "Outputs",
                newName: "mode");
        }
    }
}
