using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCompanyRelateLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Locations_locationid",
                schema: "core",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "location_id",
                schema: "core",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "locationid",
                schema: "core",
                table: "Companies",
                newName: "Locationid");

            migrationBuilder.RenameIndex(
                name: "IX_Companies_locationid",
                schema: "core",
                table: "Companies",
                newName: "IX_Companies_Locationid");

            migrationBuilder.AlterColumn<int>(
                name: "Locationid",
                schema: "core",
                table: "Companies",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Locations_Locationid",
                schema: "core",
                table: "Companies",
                column: "Locationid",
                principalSchema: "core",
                principalTable: "Locations",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Locations_Locationid",
                schema: "core",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "Locationid",
                schema: "core",
                table: "Companies",
                newName: "locationid");

            migrationBuilder.RenameIndex(
                name: "IX_Companies_Locationid",
                schema: "core",
                table: "Companies",
                newName: "IX_Companies_locationid");

            migrationBuilder.AlterColumn<int>(
                name: "locationid",
                schema: "core",
                table: "Companies",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "location_id",
                schema: "core",
                table: "Companies",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Locations_locationid",
                schema: "core",
                table: "Companies",
                column: "locationid",
                principalSchema: "core",
                principalTable: "Locations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
