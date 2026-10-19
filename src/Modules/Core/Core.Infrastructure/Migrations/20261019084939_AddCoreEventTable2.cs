using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCoreEventTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alerts_CoreEvents_core_eventid",
                schema: "core",
                table: "Alerts");

            migrationBuilder.DropIndex(
                name: "IX_Alerts_core_eventid",
                schema: "core",
                table: "Alerts");

            migrationBuilder.DropColumn(
                name: "core_eventid",
                schema: "core",
                table: "Alerts");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_core_event_id",
                schema: "core",
                table: "Alerts",
                column: "core_event_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Alerts_CoreEvents_core_event_id",
                schema: "core",
                table: "Alerts",
                column: "core_event_id",
                principalSchema: "core",
                principalTable: "CoreEvents",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alerts_CoreEvents_core_event_id",
                schema: "core",
                table: "Alerts");

            migrationBuilder.DropIndex(
                name: "IX_Alerts_core_event_id",
                schema: "core",
                table: "Alerts");

            migrationBuilder.AddColumn<int>(
                name: "core_eventid",
                schema: "core",
                table: "Alerts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_core_eventid",
                schema: "core",
                table: "Alerts",
                column: "core_eventid");

            migrationBuilder.AddForeignKey(
                name: "FK_Alerts_CoreEvents_core_eventid",
                schema: "core",
                table: "Alerts",
                column: "core_eventid",
                principalSchema: "core",
                principalTable: "CoreEvents",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
