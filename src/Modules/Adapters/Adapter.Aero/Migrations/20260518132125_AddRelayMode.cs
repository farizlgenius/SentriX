using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Adapter.Aero.Migrations
{
    /// <inheritdoc />
    public partial class AddRelayMode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RelayModes",
                schema: "aero",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    label = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelayModes", x => x.id);
                });

            migrationBuilder.InsertData(
                schema: "aero",
                table: "RelayModes",
                columns: new[] { "id", "label", "value" },
                values: new object[,]
                {
                    { 1, "Normal / No Change", 0 },
                    { 2, "Inverted / No Change", 1 },
                    { 3, "Normal / Inactive", 16 },
                    { 4, "Inverted / Inactive", 17 },
                    { 5, "Normal / Active", 32 },
                    { 6, "Inverted / Active", 33 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RelayModes",
                schema: "aero");
        }
    }
}
