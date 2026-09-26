using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAuditRelateLoc6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AuditTrails_guid_id_created_at_action_username_entity",
                schema: "core",
                table: "AuditTrails");

            migrationBuilder.DropColumn(
                name: "after",
                schema: "core",
                table: "AuditTrails");

            migrationBuilder.RenameColumn(
                name: "diff",
                schema: "core",
                table: "AuditTrails",
                newName: "object_name");

            migrationBuilder.RenameColumn(
                name: "before",
                schema: "core",
                table: "AuditTrails",
                newName: "detail");

            migrationBuilder.AddColumn<int>(
                name: "location_id",
                schema: "core",
                table: "AuditTrails",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "object_guid",
                schema: "core",
                table: "AuditTrails",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_guid_id_created_at_location_id_action_username_~",
                schema: "core",
                table: "AuditTrails",
                columns: new[] { "guid", "id", "created_at", "location_id", "action", "username", "entity" },
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditTrails_Locations_location_id",
                schema: "core",
                table: "AuditTrails");

            migrationBuilder.DropIndex(
                name: "IX_AuditTrails_guid_id_created_at_location_id_action_username_~",
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

            migrationBuilder.DropColumn(
                name: "object_guid",
                schema: "core",
                table: "AuditTrails");

            migrationBuilder.RenameColumn(
                name: "object_name",
                schema: "core",
                table: "AuditTrails",
                newName: "diff");

            migrationBuilder.RenameColumn(
                name: "detail",
                schema: "core",
                table: "AuditTrails",
                newName: "before");

            migrationBuilder.AddColumn<string>(
                name: "after",
                schema: "core",
                table: "AuditTrails",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_guid_id_created_at_action_username_entity",
                schema: "core",
                table: "AuditTrails",
                columns: new[] { "guid", "id", "created_at", "action", "username", "entity" },
                unique: true);
        }
    }
}
