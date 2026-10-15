using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAlrtLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alerts_AlertLevel_alert_levelid",
                schema: "core",
                table: "Alerts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AlertLevel",
                schema: "core",
                table: "AlertLevel");

            migrationBuilder.RenameTable(
                name: "AlertLevel",
                schema: "core",
                newName: "AlertLevels",
                newSchema: "core");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AlertLevels",
                schema: "core",
                table: "AlertLevels",
                column: "id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alerts_AlertLevels_alert_levelid",
                schema: "core",
                table: "Alerts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AlertLevels",
                schema: "core",
                table: "AlertLevels");

            migrationBuilder.RenameTable(
                name: "AlertLevels",
                schema: "core",
                newName: "AlertLevel",
                newSchema: "core");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AlertLevel",
                schema: "core",
                table: "AlertLevel",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Alerts_AlertLevel_alert_levelid",
                schema: "core",
                table: "Alerts",
                column: "alert_levelid",
                principalSchema: "core",
                principalTable: "AlertLevel",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
