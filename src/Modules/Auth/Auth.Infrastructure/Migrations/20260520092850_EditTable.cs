using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "component_id",
                schema: "auth",
                table: "RefreshTokenAudits",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "location_id",
                schema: "auth",
                table: "RefreshTokenAudits",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<short>(
                name: "component_id",
                schema: "auth",
                table: "ApiKeys",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "location_id",
                schema: "auth",
                table: "ApiKeys",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "component_id",
                schema: "auth",
                table: "RefreshTokenAudits");

            migrationBuilder.DropColumn(
                name: "location_id",
                schema: "auth",
                table: "RefreshTokenAudits");

            migrationBuilder.DropColumn(
                name: "component_id",
                schema: "auth",
                table: "ApiKeys");

            migrationBuilder.DropColumn(
                name: "location_id",
                schema: "auth",
                table: "ApiKeys");
        }
    }
}
