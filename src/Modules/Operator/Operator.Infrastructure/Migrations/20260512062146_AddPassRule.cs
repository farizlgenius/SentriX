using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Operator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPassRule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "password_rules",
                schema: "operator",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    len = table.Column<int>(type: "integer", nullable: false),
                    is_digit = table.Column<bool>(type: "boolean", nullable: false),
                    is_lower = table.Column<bool>(type: "boolean", nullable: false),
                    is_symbol = table.Column<bool>(type: "boolean", nullable: false),
                    is_upper = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_password_rules", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "weak_passwords",
                schema: "operator",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pattern = table.Column<string>(type: "text", nullable: false),
                    password_rule_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weak_passwords", x => x.id);
                    table.ForeignKey(
                        name: "FK_weak_passwords_password_rules_password_rule_id",
                        column: x => x.password_rule_id,
                        principalSchema: "operator",
                        principalTable: "password_rules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "operator",
                table: "password_rules",
                columns: new[] { "id", "is_digit", "is_lower", "is_symbol", "is_upper", "len" },
                values: new object[] { 1, false, false, false, false, 4 });

            migrationBuilder.InsertData(
                schema: "operator",
                table: "weak_passwords",
                columns: new[] { "id", "password_rule_id", "pattern" },
                values: new object[,]
                {
                    { 1, 1, "P@ssw0rd" },
                    { 2, 1, "password" },
                    { 3, 1, "admin" },
                    { 4, 1, "123456" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_weak_passwords_password_rule_id",
                schema: "operator",
                table: "weak_passwords",
                column: "password_rule_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "weak_passwords",
                schema: "operator");

            migrationBuilder.DropTable(
                name: "password_rules",
                schema: "operator");
        }
    }
}
