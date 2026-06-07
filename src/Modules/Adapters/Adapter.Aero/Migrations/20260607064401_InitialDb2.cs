using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Adapter.Aero.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DoorModes",
                schema: "aero",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    label = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoorModes", x => x.id);
                });

            migrationBuilder.InsertData(
                schema: "aero",
                table: "DoorModes",
                columns: new[] { "id", "description", "label", "value" },
                values: new object[,]
                {
                    { 1, "Single reader, controlling the door", "Single", 0 },
                    { 2, "In/Out Reader", "Dual", 1 },
                    { 3, "Turnstile Reader", "Turnstile", 3 },
                    { 4, "Elevator, no floor select feedback", "Elevator no floor", 4 },
                    { 5, "Elevator with floor select feedback", "Elevator with floor", 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoorModes",
                schema: "aero");
        }
    }
}
