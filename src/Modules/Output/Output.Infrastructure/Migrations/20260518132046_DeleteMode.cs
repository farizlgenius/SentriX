using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Output.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeleteMode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RelayModes",
                schema: "output");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RelayModes",
                schema: "output",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mode = table.Column<string>(type: "text", nullable: false),
                    offline = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelayModes", x => x.id);
                });

            migrationBuilder.InsertData(
                schema: "output",
                table: "RelayModes",
                columns: new[] { "id", "mode", "offline", "value" },
                values: new object[,]
                {
                    { 1, "Normal", "No Change", 0 },
                    { 2, "Normal", "Inactive", 16 },
                    { 3, "Normal", "Active", 32 },
                    { 4, "Inverted", "No Change", 1 },
                    { 5, "Inverted", "Inactive", 16 },
                    { 6, "Inverted", "Active", 33 }
                });
        }
    }
}
