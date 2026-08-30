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
                name: "PasswordRules",
                schema: "setting",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    len = table.Column<int>(type: "integer", nullable: false),
                    is_digit = table.Column<bool>(type: "boolean", nullable: false),
                    is_lower = table.Column<bool>(type: "boolean", nullable: false),
                    is_symbol = table.Column<bool>(type: "boolean", nullable: false),
                    is_upper = table.Column<bool>(type: "boolean", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordRules", x => x.id);
                    table.UniqueConstraint("AK_PasswordRules_guid", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "WeakPasswords",
                schema: "setting",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pattern = table.Column<string>(type: "text", nullable: false),
                    password_rule_guid = table.Column<Guid>(type: "uuid", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeakPasswords", x => x.id);
                    table.ForeignKey(
                        name: "FK_WeakPasswords_PasswordRules_password_rule_guid",
                        column: x => x.password_rule_guid,
                        principalSchema: "setting",
                        principalTable: "PasswordRules",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "setting",
                table: "PasswordRules",
                columns: new[] { "id", "guid", "is_digit", "is_lower", "is_symbol", "is_upper", "len" },
                values: new object[] { 1, new Guid("ae243161-6067-47d0-8bcc-1990388bb6e6"), false, false, false, false, 4 });

            migrationBuilder.InsertData(
                schema: "setting",
                table: "WeakPasswords",
                columns: new[] { "id", "guid", "password_rule_guid", "pattern" },
                values: new object[,]
                {
                    { 1, new Guid("f371dff7-fa82-4a0f-95ba-f24954cf73f7"), new Guid("ae243161-6067-47d0-8bcc-1990388bb6e6"), "P@ssw0rd" },
                    { 2, new Guid("c347ec2d-17e7-4048-82df-9b1b65730669"), new Guid("ae243161-6067-47d0-8bcc-1990388bb6e6"), "password" },
                    { 3, new Guid("b3124c81-3c54-46b3-bafd-a945854fc946"), new Guid("ae243161-6067-47d0-8bcc-1990388bb6e6"), "admin" },
                    { 4, new Guid("df75695c-6821-49ad-a857-60e1b0763329"), new Guid("ae243161-6067-47d0-8bcc-1990388bb6e6"), "123456" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeakPasswords_password_rule_guid",
                schema: "setting",
                table: "WeakPasswords",
                column: "password_rule_guid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeakPasswords",
                schema: "setting");

            migrationBuilder.DropTable(
                name: "PasswordRules",
                schema: "setting");
        }
    }
}
