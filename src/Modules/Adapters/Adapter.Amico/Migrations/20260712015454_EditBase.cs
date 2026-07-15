using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adapter.Amico.Migrations
{
    /// <inheritdoc />
    public partial class EditBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_active",
                schema: "amico",
                table: "Amicos");

            migrationBuilder.DropColumn(
                name: "is_default",
                schema: "amico",
                table: "Amicos");

            migrationBuilder.DropColumn(
                name: "location_id",
                schema: "amico",
                table: "Amicos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                schema: "amico",
                table: "Amicos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                schema: "amico",
                table: "Amicos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "location_id",
                schema: "amico",
                table: "Amicos",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
