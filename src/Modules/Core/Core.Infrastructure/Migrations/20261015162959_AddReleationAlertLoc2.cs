using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReleationAlertLoc2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alerts_AlertLevels_alert_levelid",
                schema: "core",
                table: "Alerts");

            migrationBuilder.DropIndex(
                name: "IX_Alerts_alert_levelid",
                schema: "core",
                table: "Alerts");

            migrationBuilder.DropColumn(
                name: "alert_levelid",
                schema: "core",
                table: "Alerts");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_alert_level_id",
                schema: "core",
                table: "Alerts",
                column: "alert_level_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Alerts_AlertLevels_alert_level_id",
                schema: "core",
                table: "Alerts",
                column: "alert_level_id",
                principalSchema: "core",
                principalTable: "AlertLevels",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alerts_AlertLevels_alert_level_id",
                schema: "core",
                table: "Alerts");

            migrationBuilder.DropIndex(
                name: "IX_Alerts_alert_level_id",
                schema: "core",
                table: "Alerts");

            migrationBuilder.AddColumn<int>(
                name: "alert_levelid",
                schema: "core",
                table: "Alerts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_alert_levelid",
                schema: "core",
                table: "Alerts",
                column: "alert_levelid");

            migrationBuilder.AddForeignKey(
                name: "FK_Alerts_AlertLevels_alert_levelid",
                schema: "core",
                table: "Alerts",
                column: "alert_levelid",
                principalSchema: "core",
                principalTable: "AlertLevels",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
