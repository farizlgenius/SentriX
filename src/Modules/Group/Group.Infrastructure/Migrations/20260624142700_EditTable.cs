using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Group.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "metadata",
                schema: "door",
                table: "Groups");

            migrationBuilder.EnsureSchema(
                name: "group");

            migrationBuilder.RenameTable(
                name: "Groups",
                schema: "door",
                newName: "Groups",
                newSchema: "group");

            migrationBuilder.CreateTable(
                name: "GroupDoors",
                schema: "group",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mac = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    group_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupDoors", x => x.id);
                    table.ForeignKey(
                        name: "FK_GroupDoors_Groups_group_id",
                        column: x => x.group_id,
                        principalSchema: "group",
                        principalTable: "Groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupDoorDetails",
                schema: "group",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    door_component_id = table.Column<short>(type: "smallint", nullable: false),
                    timezone_component_id = table.Column<short>(type: "smallint", nullable: false),
                    group_door_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupDoorDetails", x => x.id);
                    table.ForeignKey(
                        name: "FK_GroupDoorDetails_GroupDoors_group_door_id",
                        column: x => x.group_door_id,
                        principalSchema: "group",
                        principalTable: "GroupDoors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupDoorDetails_group_door_id",
                schema: "group",
                table: "GroupDoorDetails",
                column: "group_door_id");

            migrationBuilder.CreateIndex(
                name: "IX_GroupDoors_group_id",
                schema: "group",
                table: "GroupDoors",
                column: "group_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupDoorDetails",
                schema: "group");

            migrationBuilder.DropTable(
                name: "GroupDoors",
                schema: "group");

            migrationBuilder.EnsureSchema(
                name: "door");

            migrationBuilder.RenameTable(
                name: "Groups",
                schema: "group",
                newName: "Groups",
                newSchema: "door");

            migrationBuilder.AddColumn<string>(
                name: "metadata",
                schema: "door",
                table: "Groups",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
