using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditAlertRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlertBaseEvents",
                schema: "core");

            migrationBuilder.AddColumn<int>(
                name: "base_event_id",
                schema: "core",
                table: "Alerts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "AlertLevels",
                keyColumn: "id",
                keyValue: 1,
                column: "is_default",
                value: true);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "AlertLevels",
                keyColumn: "id",
                keyValue: 2,
                column: "is_default",
                value: true);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "AlertLevels",
                keyColumn: "id",
                keyValue: 3,
                column: "is_default",
                value: true);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "AlertLevels",
                keyColumn: "id",
                keyValue: 4,
                column: "is_default",
                value: true);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "AlertLevels",
                keyColumn: "id",
                keyValue: 5,
                column: "is_default",
                value: true);

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_base_event_id",
                schema: "core",
                table: "Alerts",
                column: "base_event_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Alerts_BaseEvents_base_event_id",
                schema: "core",
                table: "Alerts",
                column: "base_event_id",
                principalSchema: "core",
                principalTable: "BaseEvents",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alerts_BaseEvents_base_event_id",
                schema: "core",
                table: "Alerts");

            migrationBuilder.DropIndex(
                name: "IX_Alerts_base_event_id",
                schema: "core",
                table: "Alerts");

            migrationBuilder.DropColumn(
                name: "base_event_id",
                schema: "core",
                table: "Alerts");

            migrationBuilder.CreateTable(
                name: "AlertBaseEvents",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    alert_id = table.Column<int>(type: "integer", nullable: false),
                    event_detail_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertBaseEvents", x => x.id);
                    table.ForeignKey(
                        name: "FK_AlertBaseEvents_Alerts_alert_id",
                        column: x => x.alert_id,
                        principalSchema: "core",
                        principalTable: "Alerts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlertBaseEvents_BaseEvents_event_detail_id",
                        column: x => x.event_detail_id,
                        principalSchema: "core",
                        principalTable: "BaseEvents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "AlertLevels",
                keyColumn: "id",
                keyValue: 1,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "AlertLevels",
                keyColumn: "id",
                keyValue: 2,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "AlertLevels",
                keyColumn: "id",
                keyValue: 3,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "AlertLevels",
                keyColumn: "id",
                keyValue: 4,
                column: "is_default",
                value: false);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "AlertLevels",
                keyColumn: "id",
                keyValue: 5,
                column: "is_default",
                value: false);

            migrationBuilder.CreateIndex(
                name: "IX_AlertBaseEvents_alert_id",
                schema: "core",
                table: "AlertBaseEvents",
                column: "alert_id");

            migrationBuilder.CreateIndex(
                name: "IX_AlertBaseEvents_event_detail_id",
                schema: "core",
                table: "AlertBaseEvents",
                column: "event_detail_id");
        }
    }
}
