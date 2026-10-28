using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAreaData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "core",
                table: "Areas",
                columns: new[] { "id", "Deviceid", "altr_external_id", "area_mode", "external_id", "is_active", "is_default", "location_id", "metadata", "multi_occ_mode", "name", "occ_down", "occ_max", "occ_up" },
                values: new object[] { 1, null, null, 0, (short)1, true, true, 1, "", 0, "Outside", (short)0, (short)0, (short)0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "core",
                table: "Areas",
                keyColumn: "id",
                keyValue: 1);
        }
    }
}
