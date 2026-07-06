using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Setting.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "setting");

            migrationBuilder.CreateTable(
                name: "CardFormats",
                schema: "setting",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    fac = table.Column<short>(type: "smallint", nullable: false),
                    offset = table.Column<short>(type: "smallint", nullable: false),
                    function_id = table.Column<short>(type: "smallint", nullable: false),
                    flag = table.Column<short>(type: "smallint", nullable: false),
                    bits = table.Column<short>(type: "smallint", nullable: false),
                    pe_ln = table.Column<short>(type: "smallint", nullable: false),
                    pe_loc = table.Column<short>(type: "smallint", nullable: false),
                    po_ln = table.Column<short>(type: "smallint", nullable: false),
                    po_loc = table.Column<short>(type: "smallint", nullable: false),
                    fc_ln = table.Column<short>(type: "smallint", nullable: false),
                    fc_loc = table.Column<short>(type: "smallint", nullable: false),
                    ch_ln = table.Column<short>(type: "smallint", nullable: false),
                    ch_loc = table.Column<short>(type: "smallint", nullable: false),
                    ic_ln = table.Column<short>(type: "smallint", nullable: false),
                    ic_loc = table.Column<short>(type: "smallint", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardFormats", x => x.id);
                });

            migrationBuilder.InsertData(
                schema: "setting",
                table: "CardFormats",
                columns: new[] { "id", "bits", "ch_ln", "ch_loc", "component_id", "fac", "fc_ln", "fc_loc", "flag", "function_id", "ic_ln", "ic_loc", "is_active", "is_default", "location_id", "name", "offset", "pe_ln", "pe_loc", "po_ln", "po_loc" },
                values: new object[,]
                {
                    { 1, (short)26, (short)26, (short)0, (short)0, (short)-1, (short)0, (short)-1, (short)0, (short)1, (short)0, (short)-1, true, true, 0, "26-bit Wiegand", (short)0, (short)0, (short)-1, (short)0, (short)-1 },
                    { 2, (short)32, (short)32, (short)0, (short)1, (short)-1, (short)0, (short)-1, (short)0, (short)1, (short)0, (short)-1, true, true, 0, "32-bit Wiegand", (short)0, (short)0, (short)-1, (short)0, (short)-1 },
                    { 3, (short)37, (short)37, (short)0, (short)2, (short)-1, (short)0, (short)-1, (short)0, (short)1, (short)0, (short)-1, true, true, 0, "37-bit Wiegand", (short)0, (short)0, (short)-1, (short)0, (short)-1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardFormats",
                schema: "setting");
        }
    }
}
