using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAlertComponent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AlertComponents",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    alert_id = table.Column<int>(type: "integer", nullable: false),
                    door_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertComponents", x => x.id);
                    table.ForeignKey(
                        name: "FK_AlertComponents_Alerts_alert_id",
                        column: x => x.alert_id,
                        principalSchema: "core",
                        principalTable: "Alerts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlertComponents_Doors_door_id",
                        column: x => x.door_id,
                        principalSchema: "core",
                        principalTable: "Doors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlertComponents_alert_id",
                schema: "core",
                table: "AlertComponents",
                column: "alert_id");

            migrationBuilder.CreateIndex(
                name: "IX_AlertComponents_door_id",
                schema: "core",
                table: "AlertComponents",
                column: "door_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlertComponents",
                schema: "core");
        }
    }
}
