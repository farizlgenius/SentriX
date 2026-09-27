using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAuditRelateLoc7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditTrails_Locations_location_id",
                schema: "core",
                table: "AuditTrails");

            migrationBuilder.DropIndex(
                name: "IX_AuditTrails_guid_id_created_at_location_id_action_username_~",
                schema: "core",
                table: "AuditTrails");

            migrationBuilder.RenameColumn(
                name: "location_id",
                schema: "core",
                table: "AuditTrails",
                newName: "Locationid");

            migrationBuilder.RenameIndex(
                name: "IX_AuditTrails_location_id",
                schema: "core",
                table: "AuditTrails",
                newName: "IX_AuditTrails_Locationid");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_guid_id_created_at_action_username_entity",
                schema: "core",
                table: "AuditTrails",
                columns: new[] { "guid", "id", "created_at", "action", "username", "entity" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditTrails_Locations_Locationid",
                schema: "core",
                table: "AuditTrails",
                column: "Locationid",
                principalSchema: "core",
                principalTable: "Locations",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditTrails_Locations_Locationid",
                schema: "core",
                table: "AuditTrails");

            migrationBuilder.DropIndex(
                name: "IX_AuditTrails_guid_id_created_at_action_username_entity",
                schema: "core",
                table: "AuditTrails");

            migrationBuilder.RenameColumn(
                name: "Locationid",
                schema: "core",
                table: "AuditTrails",
                newName: "location_id");

            migrationBuilder.RenameIndex(
                name: "IX_AuditTrails_Locationid",
                schema: "core",
                table: "AuditTrails",
                newName: "IX_AuditTrails_location_id");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_guid_id_created_at_location_id_action_username_~",
                schema: "core",
                table: "AuditTrails",
                columns: new[] { "guid", "id", "created_at", "location_id", "action", "username", "entity" },
                unique: true);

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
