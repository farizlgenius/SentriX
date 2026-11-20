using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTriggerSource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "entity_Type",
                schema: "core",
                table: "TriggerSources",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerSources",
                keyColumn: "id",
                keyValue: 1,
                column: "entity_Type",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerSources",
                keyColumn: "id",
                keyValue: 2,
                column: "entity_Type",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerSources",
                keyColumn: "id",
                keyValue: 3,
                column: "entity_Type",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerSources",
                keyColumn: "id",
                keyValue: 4,
                column: "entity_Type",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerSources",
                keyColumn: "id",
                keyValue: 5,
                column: "entity_Type",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerSources",
                keyColumn: "id",
                keyValue: 6,
                column: "entity_Type",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerSources",
                keyColumn: "id",
                keyValue: 7,
                column: "entity_Type",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerSources",
                keyColumn: "id",
                keyValue: 8,
                column: "entity_Type",
                value: 22);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerSources",
                keyColumn: "id",
                keyValue: 9,
                column: "entity_Type",
                value: 23);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerSources",
                keyColumn: "id",
                keyValue: 10,
                column: "entity_Type",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerSources",
                keyColumn: "id",
                keyValue: 11,
                column: "entity_Type",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerSources",
                keyColumn: "id",
                keyValue: 12,
                column: "entity_Type",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerSources",
                keyColumn: "id",
                keyValue: 13,
                column: "entity_Type",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerSources",
                keyColumn: "id",
                keyValue: 14,
                column: "entity_Type",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerSources",
                keyColumn: "id",
                keyValue: 15,
                column: "entity_Type",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerSources",
                keyColumn: "id",
                keyValue: 16,
                column: "entity_Type",
                value: 24);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerSources",
                keyColumn: "id",
                keyValue: 17,
                column: "entity_Type",
                value: 25);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerSources",
                keyColumn: "id",
                keyValue: 18,
                column: "entity_Type",
                value: 26);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerSources",
                keyColumn: "id",
                keyValue: 19,
                column: "entity_Type",
                value: 27);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerSources",
                keyColumn: "id",
                keyValue: 20,
                column: "entity_Type",
                value: 28);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerSources",
                keyColumn: "id",
                keyValue: 21,
                column: "entity_Type",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "TriggerSources",
                keyColumn: "id",
                keyValue: 22,
                column: "entity_Type",
                value: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "entity_Type",
                schema: "core",
                table: "TriggerSources");
        }
    }
}
