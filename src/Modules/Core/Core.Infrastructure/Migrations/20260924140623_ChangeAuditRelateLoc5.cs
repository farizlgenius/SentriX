using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAuditRelateLoc5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditTrails_Locations_location_id",
                schema: "core",
                table: "AuditTrails");

            migrationBuilder.DropIndex(
                name: "IX_AuditTrails_guid_id_location_id_created_at_action_username_~",
                schema: "core",
                table: "AuditTrails");

            migrationBuilder.DropIndex(
                name: "IX_AuditTrails_location_id",
                schema: "core",
                table: "AuditTrails");

            migrationBuilder.DropColumn(
                name: "location_id",
                schema: "core",
                table: "AuditTrails");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_guid_id_created_at_action_username_entity",
                schema: "core",
                table: "AuditTrails",
                columns: new[] { "guid", "id", "created_at", "action", "username", "entity" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AuditTrails_guid_id_created_at_action_username_entity",
                schema: "core",
                table: "AuditTrails");

            migrationBuilder.AddColumn<int>(
                name: "location_id",
                schema: "core",
                table: "AuditTrails",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_guid_id_location_id_created_at_action_username_~",
                schema: "core",
                table: "AuditTrails",
                columns: new[] { "guid", "id", "location_id", "created_at", "action", "username", "entity" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_location_id",
                schema: "core",
                table: "AuditTrails",
                column: "location_id");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditTrails_Locations_location_id",
                schema: "core",
                table: "AuditTrails",
                column: "location_id",
                principalSchema: "core",
                principalTable: "Locations",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
