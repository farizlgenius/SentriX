using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Triggers_TimeZones_timezoneid",
                schema: "core",
                table: "Triggers");

            migrationBuilder.DropIndex(
                name: "IX_Triggers_timezoneid",
                schema: "core",
                table: "Triggers");

            migrationBuilder.DropColumn(
                name: "timezoneid",
                schema: "core",
                table: "Triggers");

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_timezone_id",
                schema: "core",
                table: "Triggers",
                column: "timezone_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Triggers_TimeZones_timezone_id",
                schema: "core",
                table: "Triggers",
                column: "timezone_id",
                principalSchema: "core",
                principalTable: "TimeZones",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Triggers_TimeZones_timezone_id",
                schema: "core",
                table: "Triggers");

            migrationBuilder.DropIndex(
                name: "IX_Triggers_timezone_id",
                schema: "core",
                table: "Triggers");

            migrationBuilder.AddColumn<int>(
                name: "timezoneid",
                schema: "core",
                table: "Triggers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_timezoneid",
                schema: "core",
                table: "Triggers",
                column: "timezoneid");

            migrationBuilder.AddForeignKey(
                name: "FK_Triggers_TimeZones_timezoneid",
                schema: "core",
                table: "Triggers",
                column: "timezoneid",
                principalSchema: "core",
                principalTable: "TimeZones",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
