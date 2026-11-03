using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMPGRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MonitorGroupComponents_door_id",
                schema: "core",
                table: "MonitorGroupComponents");

            migrationBuilder.DropIndex(
                name: "IX_MonitorGroupComponents_input_id",
                schema: "core",
                table: "MonitorGroupComponents");

            migrationBuilder.CreateIndex(
                name: "IX_MonitorGroupComponents_door_id",
                schema: "core",
                table: "MonitorGroupComponents",
                column: "door_id");

            migrationBuilder.CreateIndex(
                name: "IX_MonitorGroupComponents_input_id",
                schema: "core",
                table: "MonitorGroupComponents",
                column: "input_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MonitorGroupComponents_door_id",
                schema: "core",
                table: "MonitorGroupComponents");

            migrationBuilder.DropIndex(
                name: "IX_MonitorGroupComponents_input_id",
                schema: "core",
                table: "MonitorGroupComponents");

            migrationBuilder.CreateIndex(
                name: "IX_MonitorGroupComponents_door_id",
                schema: "core",
                table: "MonitorGroupComponents",
                column: "door_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonitorGroupComponents_input_id",
                schema: "core",
                table: "MonitorGroupComponents",
                column: "input_id",
                unique: true);
        }
    }
}
