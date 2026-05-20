using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Role.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddComponentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "component_id",
                schema: "role",
                table: "roles",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "component_id",
                schema: "role",
                table: "role_operators",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "location_id",
                schema: "role",
                table: "role_operators",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<short>(
                name: "component_id",
                schema: "role",
                table: "permissions",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "location_id",
                schema: "role",
                table: "permissions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<short>(
                name: "component_id",
                schema: "role",
                table: "features",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "location_id",
                schema: "role",
                table: "features",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 10,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 11,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 12,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 13,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 14,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 15,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 16,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 17,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 18,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "features",
                keyColumn: "id",
                keyValue: 19,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 10,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 11,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 12,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 13,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 14,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 15,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 16,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 17,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 18,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "permissions",
                keyColumn: "id",
                keyValue: 19,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "role",
                table: "roles",
                keyColumn: "id",
                keyValue: 1,
                column: "component_id",
                value: (short)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "component_id",
                schema: "role",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "component_id",
                schema: "role",
                table: "role_operators");

            migrationBuilder.DropColumn(
                name: "location_id",
                schema: "role",
                table: "role_operators");

            migrationBuilder.DropColumn(
                name: "component_id",
                schema: "role",
                table: "permissions");

            migrationBuilder.DropColumn(
                name: "location_id",
                schema: "role",
                table: "permissions");

            migrationBuilder.DropColumn(
                name: "component_id",
                schema: "role",
                table: "features");

            migrationBuilder.DropColumn(
                name: "location_id",
                schema: "role",
                table: "features");
        }
    }
}
