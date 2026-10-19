using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReleationAlertLoc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlertEvents_Locations_locationid",
                schema: "core",
                table: "AlertEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_Alerts_Locations_locationid",
                schema: "core",
                table: "Alerts");

            migrationBuilder.DropIndex(
                name: "IX_Alerts_locationid",
                schema: "core",
                table: "Alerts");

            migrationBuilder.DropIndex(
                name: "IX_AlertEvents_locationid",
                schema: "core",
                table: "AlertEvents");

            migrationBuilder.DropColumn(
                name: "locationid",
                schema: "core",
                table: "Alerts");

            migrationBuilder.DropColumn(
                name: "locationid",
                schema: "core",
                table: "AlertEvents");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_location_id",
                schema: "core",
                table: "Alerts",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_AlertEvents_location_id",
                schema: "core",
                table: "AlertEvents",
                column: "location_id");

            migrationBuilder.AddForeignKey(
                name: "FK_AlertEvents_Locations_location_id",
                schema: "core",
                table: "AlertEvents",
                column: "location_id",
                principalSchema: "core",
                principalTable: "Locations",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Alerts_Locations_location_id",
                schema: "core",
                table: "Alerts",
                column: "location_id",
                principalSchema: "core",
                principalTable: "Locations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlertEvents_Locations_location_id",
                schema: "core",
                table: "AlertEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_Alerts_Locations_location_id",
                schema: "core",
                table: "Alerts");

            migrationBuilder.DropIndex(
                name: "IX_Alerts_location_id",
                schema: "core",
                table: "Alerts");

            migrationBuilder.DropIndex(
                name: "IX_AlertEvents_location_id",
                schema: "core",
                table: "AlertEvents");

            migrationBuilder.AddColumn<int>(
                name: "locationid",
                schema: "core",
                table: "Alerts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "locationid",
                schema: "core",
                table: "AlertEvents",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_locationid",
                schema: "core",
                table: "Alerts",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_AlertEvents_locationid",
                schema: "core",
                table: "AlertEvents",
                column: "locationid");

            migrationBuilder.AddForeignKey(
                name: "FK_AlertEvents_Locations_locationid",
                schema: "core",
                table: "AlertEvents",
                column: "locationid",
                principalSchema: "core",
                principalTable: "Locations",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Alerts_Locations_locationid",
                schema: "core",
                table: "Alerts",
                column: "locationid",
                principalSchema: "core",
                principalTable: "Locations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
