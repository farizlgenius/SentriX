using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace User.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserFlags",
                schema: "user",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    label = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFlags", x => x.id);
                });

            migrationBuilder.InsertData(
                schema: "user",
                table: "UserFlags",
                columns: new[] { "id", "component_id", "description", "is_active", "label", "location_id", "value" },
                values: new object[,]
                {
                    { 1, (short)0, "Active cardholder record", true, "Active", 0, 1 },
                    { 2, (short)0, "Allow one free anti-passback pass", true, "One free APB", 0, 2 },
                    { 3, (short)0, "Anti-passback exempt", true, "APB Exempt", 0, 4 },
                    { 4, (short)0, "Use timing parameters for the disabled (ADA)", true, "ADA", 0, 8 },
                    { 5, (short)0, "Use timing parameters for the disabled (ADA)", true, "ADA", 0, 8 },
                    { 6, (short)0, "PIN Exempt for 'Card & PIN' ACR mode", true, "PIN Exempt", 0, 16 },
                    { 7, (short)0, "Do not change apb_loc", true, "No Change APB Location", 0, 32 },
                    { 8, (short)0, "Do not alter either the 'original' or the 'current' use count values", true, "No Change Use Limit", 0, 64 },
                    { 9, (short)0, "Do not alter the 'current' use count but change the original use limit stored in the cardholder database", true, "No Change Current", 0, 128 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFlags",
                schema: "user");
        }
    }
}
