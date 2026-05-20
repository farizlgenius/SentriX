using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Operator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddComponentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "component_id",
                schema: "operator",
                table: "operators",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "location_id",
                schema: "operator",
                table: "operators",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<short>(
                name: "component_id",
                schema: "operator",
                table: "operator_locations",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.UpdateData(
                schema: "operator",
                table: "operator_locations",
                keyColumn: "id",
                keyValue: 1,
                column: "component_id",
                value: (short)0);

            migrationBuilder.UpdateData(
                schema: "operator",
                table: "operators",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "component_id",
                schema: "operator",
                table: "operators");

            migrationBuilder.DropColumn(
                name: "location_id",
                schema: "operator",
                table: "operators");

            migrationBuilder.DropColumn(
                name: "component_id",
                schema: "operator",
                table: "operator_locations");
        }
    }
}
