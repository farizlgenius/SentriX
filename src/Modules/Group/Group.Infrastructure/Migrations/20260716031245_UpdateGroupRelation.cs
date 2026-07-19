using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Group.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGroupRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupDoorDetails",
                schema: "group");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                schema: "group",
                table: "Groups",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "group",
                table: "Groups",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.AddColumn<Guid>(
                name: "guid",
                schema: "group",
                table: "Groups",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<short>(
                name: "device_component_id",
                schema: "group",
                table: "GroupDoors",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "door_component_id",
                schema: "group",
                table: "GroupDoors",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "timezone_component_id",
                schema: "group",
                table: "GroupDoors",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.UpdateData(
                schema: "group",
                table: "Groups",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "guid", "name" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000000"), "Always" });

            migrationBuilder.UpdateData(
                schema: "group",
                table: "Groups",
                keyColumn: "id",
                keyValue: 2,
                column: "guid",
                value: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "guid",
                schema: "group",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "device_component_id",
                schema: "group",
                table: "GroupDoors");

            migrationBuilder.DropColumn(
                name: "door_component_id",
                schema: "group",
                table: "GroupDoors");

            migrationBuilder.DropColumn(
                name: "timezone_component_id",
                schema: "group",
                table: "GroupDoors");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                schema: "group",
                table: "Groups",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "group",
                table: "Groups",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateTable(
                name: "GroupDoorDetails",
                schema: "group",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    group_door_id = table.Column<int>(type: "integer", nullable: false),
                    door_component_id = table.Column<short>(type: "smallint", nullable: false),
                    timezone_component_id = table.Column<short>(type: "smallint", nullable: false)
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

            migrationBuilder.UpdateData(
                schema: "group",
                table: "Groups",
                keyColumn: "id",
                keyValue: 1,
                column: "name",
                value: "Allow");

            migrationBuilder.CreateIndex(
                name: "IX_GroupDoorDetails_group_door_id",
                schema: "group",
                table: "GroupDoorDetails",
                column: "group_door_id");
        }
    }
}
