using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAuditRelateLoc4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "method",
                schema: "core",
                table: "AuditTrails");

            migrationBuilder.DropColumn(
                name: "path",
                schema: "core",
                table: "AuditTrails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "method",
                schema: "core",
                table: "AuditTrails",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "path",
                schema: "core",
                table: "AuditTrails",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
