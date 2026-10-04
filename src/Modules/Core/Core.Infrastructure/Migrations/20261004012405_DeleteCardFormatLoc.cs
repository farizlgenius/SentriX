using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeleteCardFormatLoc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardFormats_Locations_locationid",
                schema: "core",
                table: "CardFormats");

            migrationBuilder.DropIndex(
                name: "IX_CardFormats_locationid",
                schema: "core",
                table: "CardFormats");

            migrationBuilder.DropColumn(
                name: "location_id",
                schema: "core",
                table: "CardFormats");

            migrationBuilder.DropColumn(
                name: "locationid",
                schema: "core",
                table: "CardFormats");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "location_id",
                schema: "core",
                table: "CardFormats",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "locationid",
                schema: "core",
                table: "CardFormats",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "core",
                table: "CardFormats",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "location_id", "locationid" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "CardFormats",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "location_id", "locationid" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "CardFormats",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "location_id", "locationid" },
                values: new object[] { null, null });

            migrationBuilder.CreateIndex(
                name: "IX_CardFormats_locationid",
                schema: "core",
                table: "CardFormats",
                column: "locationid");

            migrationBuilder.AddForeignKey(
                name: "FK_CardFormats_Locations_locationid",
                schema: "core",
                table: "CardFormats",
                column: "locationid",
                principalSchema: "core",
                principalTable: "Locations",
                principalColumn: "id");
        }
    }
}
